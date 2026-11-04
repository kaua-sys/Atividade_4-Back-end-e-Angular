import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-entrada-produto',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './entrada-produto.component.html',
  styleUrl: './entrada-produto.component.css'
})
export class EntradaProdutoComponent implements OnInit {

  produtos: any[] = [];

  entrada = {
    iditem: 0,
    quantidadeEntrada: 0,
    dataEntradaProduto: '',
    valorUnitarioEntrada: 0,
    unidade: 'UN'
  };

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.carregarProdutos();
  }

  carregarProdutos(): void {
    this.http.get<any[]>('https://localhost:7032/api/ProdutoItem')
      .subscribe({
        next: dados => {
          this.produtos = dados.filter(
            p => p.item?.tipoItem === 'P'
          );
        },
        error: () => alert('Erro ao carregar os produtos.')
      });
  }

  salvar(): void {
    this.http.post(
      'https://localhost:7032/api/EntradaProduto',
      this.entrada
    ).subscribe({
      next: () => {
        alert('Entrada registrada com sucesso!');

        this.entrada = {
          iditem: 0,
          quantidadeEntrada: 0,
          dataEntradaProduto: '',
          valorUnitarioEntrada: 0,
          unidade: 'UN'
        };
      },
      error: () => alert('Erro ao registrar a entrada.')
    });
  }
}