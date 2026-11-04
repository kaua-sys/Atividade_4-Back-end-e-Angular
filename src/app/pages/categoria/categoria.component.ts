import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CategoriaService } from '../../services/categoria.service';

@Component({
  selector: 'app-categoria',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './categoria.component.html',
  styleUrl: './categoria.component.css'
})
export class CategoriaComponent implements OnInit {

  categorias: any[] = [];
  categoria: any = { categoria: '' };
  editando = false;

  constructor(private service: CategoriaService) {}

  ngOnInit() {
    this.listar();
  }

  listar() {
    this.service.listar().subscribe(dados => {
      this.categorias = dados;
    });
  }

  salvar() {
    if (this.editando) {
      this.service.editar(this.categoria.idcategoria, this.categoria)
        .subscribe(() => {
          this.limpar();
          this.listar();
        });
    } else {
      this.service.cadastrar(this.categoria)
        .subscribe(() => {
          this.limpar();
          this.listar();
        });
    }
  }

  selecionar(categoria: any) {
    this.categoria = { ...categoria };
    this.editando = true;
  }

  excluir(id: number) {
    if (confirm('Deseja excluir?')) {
      this.service.excluir(id).subscribe(() => {
        this.listar();
      });
    }
  }

  limpar() {
    this.categoria = { categoria: '' };
    this.editando = false;
  }
}