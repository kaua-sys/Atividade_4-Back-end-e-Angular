
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProdutoService {

  private apiUrl = 'https://localhost:7032/api/ProdutoItem';

  constructor(private http: HttpClient) {}

  listar() {
    return this.http.get<any[]>(this.apiUrl);
  }

  cadastrar(item: any) {
    return this.http.post(this.apiUrl, item);
  }

  editar(id: number, item: any) {
    return this.http.put(`${this.apiUrl}/${id}`, item);
  }

  excluir(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
  
  desativar(id: number) {
  return this.http.put(`${this.apiUrl}/desativar/${id}`, {});
}
}
