import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FuncionarioService } from '../../services/funcionario.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-funcionarios',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './funcionarios.component.html',
  styleUrl: './funcionarios.component.css'
})
export class FuncionariosComponent implements OnInit {

  funcionarios: any[] = [];

  novoFuncionario: any = {
    nome: '',
    cpf: '',
    email: '',
    cargo: '',
    dataAdmissao: ''
  };

  constructor(private funcionarioService: FuncionarioService) {}

  ngOnInit(): void {
    this.funcionarioService.listar().subscribe({
      next: (dados) => this.funcionarios = dados,
      error: (erro) => console.error(erro)
    });
  }

  excluir(idPessoa: number): void {
    if (!confirm('Deseja excluir este funcionário?')) return;

    this.funcionarioService.excluir(idPessoa).subscribe({
      next: () => {
        this.funcionarios = this.funcionarios.filter(
          f => f.idPessoa !== idPessoa
        );
        alert('Funcionário excluído!');
      },
      error: () => alert('Erro ao excluir funcionário.')
    });
  }
}