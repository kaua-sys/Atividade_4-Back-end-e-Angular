import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClienteService } from '../../services/cliente.service';

@Component({
  selector: 'app-clientes',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './clientes.component.html',
  styleUrl: './clientes.component.css'
})
export class ClientesComponent implements OnInit {

  clientes: any[] = [];
  cliente: any = {};

  constructor(private service: ClienteService) {}

  ngOnInit() {
    this.carregar();
  }

  carregar() {
    this.service.listar().subscribe(dados => this.clientes = dados);
  }

  cadastrar() {
    this.service.cadastrar({ ...this.cliente, enderecos: [] }).subscribe({
      next: () => {
        alert('Cliente cadastrado!');
        this.cliente = {};
        this.carregar();
      },
      error: erro => alert(erro.error || 'Erro ao cadastrar cliente.')
    });
  }
}