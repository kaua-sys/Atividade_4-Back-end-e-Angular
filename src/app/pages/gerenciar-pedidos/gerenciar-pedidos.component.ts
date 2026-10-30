import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PedidoService } from '../../services/pedido.service';

@Component({
  selector: 'app-gerenciar-pedidos',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './gerenciar-pedidos.component.html',
  styleUrl: './gerenciar-pedidos.component.css'
})
export class GerenciarPedidosComponent implements OnInit {

  pedidos: any[] = [];

  constructor(private pedidoService: PedidoService) {}

  ngOnInit(): void {
    this.carregarPedidos();
  }

  carregarPedidos(): void {
    this.pedidoService.listarTodos().subscribe({
      next: dados => this.pedidos = dados,
      error: () => alert('Erro ao carregar os pedidos.')
    });
  }

  alterarStatus(id: number, status: string): void {
    this.pedidoService.alterarStatus(id, status).subscribe({
      next: () => {
        alert('Situação do pedido alterada!');
        this.carregarPedidos();
      },
      error: () => alert('Erro ao alterar a situação do pedido.')
    });
  }

  calcularTotal(pedido: any): number {
    return pedido.itens.reduce(
      (total: number, item: any) =>
        total + item.valorUnitario * item.quantidade, 0
    );
  }

  nomeStatus(status: string): string {
    switch (status) {
      case 'A': return 'Aberto';
      case 'P': return 'Em processamento';
      case 'C': return 'Concluído';
      case 'X': return 'Cancelado';
      default: return status;
    }
  }
}