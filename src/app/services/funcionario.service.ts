import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class FuncionarioService {
  private apiUrl = 'https://localhost:7032/api/Funcionario';

  constructor(private http: HttpClient) {}

  listar() {
    return this.http.get<any[]>(this.apiUrl);
  }

  excluir(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  cadastrar(funcionario: any) {
    return this.http.post(this.apiUrl,funcionario,{responseType: 'text' });
  }

  atualizar(id: number, funcionario: any) {
    return this.http.put(`${this.apiUrl}/${id}`, funcionario, { responseType: 'text' });
  }
}
