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
  cliente: { idPessoa: number; nome: string } | null = null;
  pedidos: any[] = [];
  pedidoAberto: number | null = null;

  constructor(private pedidoService: PedidoService) {}

  entrar(): void {
    const cpf = Number(String(this.cpf).replace(/\D/g, ''));

    this.pedidoService.identificar(cpf, this.email).subscribe({
      next: cliente => {
        this.cliente = cliente;
        this.carregar();
      },
      error: erro => alert(this.mensagemDe(erro, 'Erro ao identificar o cliente.'))
    });
  }

  carregar(): void {
    this.pedidoService.listarDoCliente(this.cliente!.idPessoa).subscribe({
      next: dados => this.pedidos = dados,
      error: erro => alert(this.mensagemDe(erro, 'Erro ao carregar os pedidos.'))
    });
  }

  alternar(idPedido: number): void {
    this.pedidoAberto = this.pedidoAberto === idPedido ? null : idPedido;
  }

  sair(): void {
    this.cliente = null;
    this.pedidos = [];
    this.pedidoAberto = null;
    this.cpf = '';
    this.email = '';
  }

  private mensagemDe(erro: any, padrao: string): string {
    return typeof erro.error === 'string' && erro.error ? erro.error : padrao;
  }
}