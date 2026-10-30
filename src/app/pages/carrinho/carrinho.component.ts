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

  confirmando = false;
  enviando = false;
  cpf = '';
  email = '';

  constructor(private pedidoService: PedidoService) {}

  ngOnInit(): void {
    this.carrinho = JSON.parse(sessionStorage.getItem('carrinho') || '[]');
  }

  salvar(): void {
    sessionStorage.setItem('carrinho', JSON.stringify(this.carrinho));
  }

  aumentar(item: any): void {
    if (item.estoque !== undefined && item.quantidade >= item.estoque) {
      alert('Quantidade máxima em estoque atingida.');
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
    return this.carrinho.reduce((t, i) => t + i.preco * i.quantidade, 0);
  }

 
confirmar(): void {
  if (this.carrinho.length === 0) {
    alert('Seu carrinho está vazio.');
    return;
  }

  const atendimento = {
    idPessoa: 1,
    statusAtendimento: 'A',
    itens: this.carrinho.map(i => ({
      idItem: i.iditem,
      quantidade: i.quantidade
    }))
  };

  this.pedidoService.criar(atendimento).subscribe({
    next: resposta => {
      alert(`Pedido nº ${resposta.id} registrado com sucesso!`);
      this.limpar();
    },
    error: erro => {
      alert(this.mensagemDe(erro, 'Erro ao registrar o pedido.'));
    }
  });
}

  private mensagemDe(erro: any, padrao: string): string {
    return typeof erro.error === 'string' && erro.error ? erro.error : padrao;
  }
}