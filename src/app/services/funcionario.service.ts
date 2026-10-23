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
}
