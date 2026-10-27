import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgForm } from '@angular/forms';
import { FuncionarioService } from '../../services/funcionario.service';

@Component({
  selector: 'app-funcionarios',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './funcionarios.component.html',
  styleUrl: './funcionarios.component.css'
})
export class FuncionariosComponent implements OnInit {

  funcionarios: any[] = [];
  novoFuncionario = this.vazio();

  constructor(private service: FuncionarioService) {}


  editandoId: number | null = null;

  ngOnInit() {
    this.carregar();
  }

  vazio() {
    return {
      cargo: '',
      dataAdmissao: '',
      pessoa: {
        nome: '',
         cpf: '', 
         email: '',
        telefone: '',
        sexo: '',
        dataNascimento: '',
        enderecos: []
      }
    };
  }

  carregar() {
    this.service.listar().subscribe(dados => this.funcionarios = dados);
  }

  salvar() {
    const requisicao = this.editandoId
      ? this.service.atualizar(this.editandoId, this.novoFuncionario)
      : this.service.cadastrar(this.novoFuncionario);
  
    requisicao.subscribe({
      next: () => {
        alert(this.editandoId ? 'Funcionário atualizado!' : 'Funcionário cadastrado!');
        this.novoFuncionario = this.vazio();
        this.editandoId = null;
        this.carregar();
      },
      error: (erro) => {
        alert(erro.error || 'Erro ao salvar funcionário.');
      }
    });
  }

  editar(f: any) {
    this.editandoId = f.idPessoa;
    this.novoFuncionario = {
      cargo: f.cargo,
      dataAdmissao: (f.dataAdmissao ?? '').substring(0, 10),
      pessoa: { ...f.pessoa,
        dataNascimento: (f.pessoa.dataNascimento ?? '').substring(0, 10)
      }
    };
  }

  excluir(idPessoa: number) {
    if (!confirm('Deseja excluir este funcionário?')) return;
    this.service.excluir(idPessoa).subscribe(() => this.carregar());
  }
}