
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

  constructor(private pedidoService: PedidoService) {}

  ngOnInit(): void {
    this.carrinho = JSON.parse(sessionStorage.getItem('carrinho') || '[]');
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
    if (this.carrinho.length === 0) {
      alert('Carrinho vazio.');
      return;
    }

    const atendimento = {
      idPessoa: 1,
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
      error: () => alert('Erro ao registrar o pedido.')
    });
  }
}
