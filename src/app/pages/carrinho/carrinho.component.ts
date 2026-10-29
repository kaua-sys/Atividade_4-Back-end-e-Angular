
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-carrinho',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './carrinho.component.html',
  styleUrl: './carrinho.component.css'
})
export class CarrinhoComponent implements OnInit {
  carrinho: any[] = [];

  ngOnInit(): void {
    this.carregarCarrinho();
  }

  carregarCarrinho(): void {
    this.carrinho = JSON.parse(
      sessionStorage.getItem('carrinho') || '[]'
    );
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

  limpar(): void {
    this.carrinho = [];
    this.salvar();
  }

  remover(item: any): void {
    this.carrinho = this.carrinho.filter(i => i !== item);
    this.salvar();
  }


  calcularTotal(): number {
    return this.carrinho.reduce(
      (total, item) => total + item.preco * item.quantidade,
      0
    );
  }
}
