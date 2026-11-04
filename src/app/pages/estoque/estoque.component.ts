import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-estoque',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './estoque.component.html',
  styleUrl: './estoque.component.css'
})
export class EstoqueComponent implements OnInit {

  estoque: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.carregarEstoque();
  }

  carregarEstoque() {
    this.http.get<any[]>('https://localhost:7032/api/Estoque')
      .subscribe({
        next: dados => this.estoque = dados,
        error: erro => console.log(erro)
      });
  }
}