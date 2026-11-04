import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  clientes = 0;
  produtos = 0;
  servicos = 0;
  pedidos = 0;
  estoque = 0;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.carregarDados();
  }

  carregarDados() {
    this.http.get<any[]>('https://localhost:7032/api/Pessoa').subscribe({
      next: dados => this.clientes = dados.length
    });

    this.http.get<any[]>('https://localhost:7032/api/ProdutoItem').subscribe({
      next: dados => {
        this.produtos = dados.filter(i => i.item?.tipoItem === 'P').length;
        this.servicos = dados.filter(i => i.item?.tipoItem === 'S').length;
      }
    });

    this.http.get<any[]>('https://localhost:7032/api/Atendimento').subscribe({
      next: dados => this.pedidos = dados.length
    });

    this.http.get<any[]>('https://localhost:7032/api/Estoque').subscribe({
      next: dados => {
        this.estoque = dados.reduce((total, item) => total + item.quantidade, 0);
      }
    });
  }
}