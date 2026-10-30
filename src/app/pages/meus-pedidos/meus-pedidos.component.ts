
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PedidoService } from '../../services/pedido.service';

@Component({
  selector: 'app-meus-pedidos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './meus-pedidos.component.html',
  styleUrl: './meus-pedidos.component.css'
})
export class MeusPedidosComponent {
  cpf = '';
  email = '';
  cliente: any = null;
  pedidos: any[] = [];
  pedidoAberto: number | null = null;

  constructor(private pedidoService: PedidoService) {}

  entrar(): void {
    const cpf = Number(this.cpf.replace(/\D/g, ''));

    this.pedidoService.identificar(cpf, this.email).subscribe({
      next: cliente => {
        this.cliente = cliente;
        this.carregar();
      },
      error: () => alert('Erro ao identificar o cliente.')
    });
  }

  carregar(): void {
    this.pedidoService.listarDoCliente(this.cliente.idPessoa).subscribe({
      next: dados => this.pedidos = dados,
      error: () => alert('Erro ao carregar os pedidos.')
    });
  }

  calcularTotal(pedido: any): number {
    return pedido.itens.reduce(
      (total: number, item: any) =>
        total + item.valorUnitario * item.quantidade, 0
    );
  }

  alternar(id: number): void {
    this.pedidoAberto = this.pedidoAberto === id ? null : id;
  }

  sair(): void {
    this.cliente = null;
    this.pedidos = [];
    this.pedidoAberto = null;
    this.cpf = '';
    this.email = '';
  }
}
