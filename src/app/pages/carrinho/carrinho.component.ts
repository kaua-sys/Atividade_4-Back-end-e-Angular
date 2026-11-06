import { FuncionarioService } from '../../services/funcionario.service';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { PedidoService } from '../../services/pedido.service';

@Component({
  selector: 'app-carrinho',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './carrinho.component.html',
  styleUrl: './carrinho.component.css'
})
export class CarrinhoComponent implements OnInit {

  carrinho: any[] = [];
  funcionarios: any[] = [];
  idFuncionario: number | null = null;

  constructor(private pedidoService: PedidoService, private funcionarioService: FuncionarioService) {}

  ngOnInit(): void {
  this.carrinho = JSON.parse(sessionStorage.getItem('carrinho') || '[]');

  this.funcionarioService.listar().subscribe({
    next: (dados) => {
      this.funcionarios = dados.filter(
        f => f.pessoa?.ativoPessoa === 'A'
      );
    },
    error: (erro) => {
      console.log(erro);
      alert('Erro ao carregar os profissionais.');
    }
  });
}

  salvar(): void {
    sessionStorage.setItem('carrinho', JSON.stringify(this.carrinho));
  }

  aumentar(item: any): void {
    if (item.estoque !== undefined && item.quantidade >= item.estoque) {
      alert('Estoque insuficiente.');
      return;
    }

    item.quantidade++;
    this.salvar();
  }

  diminuir(item: any): void {
    if (item.quantidade > 1) {
      item.quantidade--;
      this.salvar();
    }
  }

  remover(item: any): void {
    this.carrinho = this.carrinho.filter(i => i !== item);
    this.salvar();
  }

  limpar(): void {
    this.carrinho = [];
    this.salvar();
  }

  calcularTotal(): number {
    return this.carrinho.reduce((total, item) => total + item.preco * item.quantidade, 0);
  }

  confirmar(): void {
    if (this.idFuncionario === null) {
  alert('Selecione um profissional.');
  return;
}

    if (this.carrinho.length === 0) {
      alert('Carrinho vazio.');
      return;
    }

    const cpf = Number(prompt('Digite seu CPF:'));
    const email = prompt('Digite seu e-mail:');

    if (!cpf || !email) {
      alert('Informe seu CPF e e-mail.');
      return;
    }

    this.pedidoService.identificar(cpf, email).subscribe({
      next: (pessoa) => {
        const atendimento = {
          idFuncionario: this.idFuncionario,
          idPessoa: pessoa.idPessoa,
          statusAtendimento: 'A',
          itens: this.carrinho.map(item => ({
            idItem: item.iditem,
            quantidade: item.quantidade
          }))
        };

        this.pedidoService.criar(atendimento).subscribe({
          next: () => {
            alert('Pedido registrado com sucesso!');
            this.limpar();
          },
          error: (erro) => {
            console.log(erro);
            alert(erro.error?.mensagem || 'Erro ao registrar o pedido.');
          }
        });
      },
      error: (erro) => {
        console.log(erro);
        alert(erro.error?.mensagem || 'Cliente não encontrado.');
      }
    });
  }
}