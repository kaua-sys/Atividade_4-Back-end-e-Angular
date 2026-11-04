import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  email = '';
  senha = '';

  constructor(
    private authService: AuthService,
  ) {}

  entrar() {
    this.authService.login({
      email: this.email,
      senha: this.senha
    }).subscribe({
      next: usuario => {
        this.authService.salvarUsuario(usuario);
      },
      error: () => {
        alert('E-mail ou senha incorretos.');
      }
    });
  }
}