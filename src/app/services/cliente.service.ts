import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ClienteService {
  private apiUrl = 'https://localhost:7032/api/PessoaEndereco';

  constructor(private http: HttpClient) {}

  listar() {
    return this.http.get<any[]>(this.apiUrl);
  }

  cadastrar(cliente: any) {
    return this.http.post(this.apiUrl, cliente);
  }

  atualizar(id: number, cliente: any) {
    return this.http.put(`${this.apiUrl}/${id}`, cliente);
  }

  excluir(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}