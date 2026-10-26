import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FuncionariosComponent } from './pages/funcionarios/funcionarios.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FuncionariosComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Atividade_4';
}
