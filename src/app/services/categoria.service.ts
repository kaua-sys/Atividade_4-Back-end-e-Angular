import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CategoriaService {

  private url = 'https://localhost:7032/api/Categorium';

  constructor(private http: HttpClient) {}

  listar() {
    return this.http.get<any[]>(this.url);
  }

  cadastrar(categoria: any) {
    return this.http.post(this.url, categoria);
  }

  editar(id: number, categoria: any) {
    return this.http.put(`${this.url}/${id}`, categoria);
  }

  excluir(id: number) {
    return this.http.delete(`${this.url}/${id}`);
  }
}