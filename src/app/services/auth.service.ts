import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7032/api/Pessoa';

  constructor(private http: HttpClient) {}

  login(dados: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, dados);
  }

  salvarUsuario(usuario: any) {
    sessionStorage.setItem('usuario', JSON.stringify(usuario));
  }

  obterUsuario() {
    const usuario = sessionStorage.getItem('usuario');
    return usuario ? JSON.parse(usuario) : null;
  }

  sair() {
    sessionStorage.removeItem('usuario');
  }

  estaLogado(): boolean {
    return this.obterUsuario() !== null;
  }
}