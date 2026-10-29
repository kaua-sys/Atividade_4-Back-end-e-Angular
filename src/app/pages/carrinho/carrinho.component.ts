
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

  calcularTotal(): number {
    return this.carrinho.reduce(
      (total, item) => total + item.preco * item.quantidade,
      0
    );
  }
}
