import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FuncionarioService } from '../../services/funcionario.service';

@Component({
  selector: 'app-funcionarios',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './funcionarios.component.html',
  styleUrl: './funcionarios.component.css'
})
export class FuncionariosComponent implements OnInit {

  funcionarios: any[] = [];

  constructor(private funcionarioService: FuncionarioService) {}

  ngOnInit(): void {
    this.funcionarioService.listar().subscribe({
      next: (dados) => {
        this.funcionarios = dados;
        console.log(dados);
      },
      error: (erro) => {
        console.error('Erro ao buscar funcionários:', erro);
      }
    });
  }


excluir(idPessoa: number): void {
  const confirmar = confirm('Deseja realmente excluir este funcionário?');

  if (!confirmar) {
    return;
  }

  // Vamos implementar a chamada à API no próximo passo.
}

}
