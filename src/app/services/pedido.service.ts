import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class PedidoService {
  private apiUrl = 'https://localhost:7032/api';

  constructor(private http: HttpClient) {}

  // CPF + e-mail -> devolve { idPessoa, nome }
  identificar(cpf: number, email: string) {
    return this.http.post<{ idPessoa: number; nome: string }>(
      `${this.apiUrl}/Pessoa/identificar`,
      { cpf, email }
    );
  }

  // Grava o pedido: { idCliente, itens: [{ idItem, quantidade }] }
  criar(pedido: any) {
    return this.http.post<any>(`${this.apiUrl}/Pedido`, pedido);
  }

  // "Meus pedidos"
  listarDoCliente(idCliente: number) {
    return this.http.get<any[]>(`${this.apiUrl}/Pessoa/${idCliente}/pedidos`);
  }
}