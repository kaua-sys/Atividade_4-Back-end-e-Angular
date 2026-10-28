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
  if (this.cliente.idPessoa) {
    // Atualizar cliente existente
    this.service.atualizar(this.cliente.idPessoa, this.cliente).subscribe(() => {
        alert('Cliente atualizado!');
        this.cliente = {};
        this.carregar();
      });
  } else {
    // Cadastrar novo cliente
    this.service.cadastrar({
      ...this.cliente,
      enderecos: []
    }).subscribe(() => {
      alert('Cliente cadastrado!');
      this.cliente = {};
      this.carregar();
    });
  }
}


excluir(id: number) {
  if (confirm('Deseja excluir este cliente?')) {
    this.service.excluir(id).subscribe(() => {
      alert('Cliente excluído!');
      this.carregar();
    });
  }
}

editar(cliente: any) {
  this.cliente = { ...cliente };
}

}