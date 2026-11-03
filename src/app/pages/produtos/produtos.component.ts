import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProdutoService } from '../../services/produto.service';

@Component({
  selector: 'app-gerenciar-itens',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './produtos.component.html',
  styleUrl: './produtos.component.css'
})
export class GerenciarItensComponent implements OnInit {

  itens: any[] = [];
  editandoId: number | null = null;

  novoItem: any = {
    nome: '',
    descricao: '',
    tipo: 'P',
    preco: 0,
    categoria: 1
  };

  constructor(private produtoService: ProdutoService) {}

  ngOnInit(): void {
    this.carregarItens();
  }

  carregarItens(): void {
    this.produtoService.listar().subscribe(dados => {
      this.itens = dados;
    });
  }

  salvar(): void {
    const item = {
      item: {
        item1: this.novoItem.nome,
        descricaoItem: this.novoItem.descricao,
        tipoItem: this.novoItem.tipo,
        ativoProdutoServico: 'S',
        categoriaIdcategoria: this.novoItem.categoria
      },
      precoVenda: this.novoItem.preco
    };

    if (this.editandoId) {
      this.produtoService.editar(this.editandoId, item).subscribe(() => {
        this.carregarItens();
        this.cancelar();
      });
    } else {
      this.produtoService.cadastrar(item).subscribe(() => {
        this.carregarItens();
        this.cancelar();
      });
    }
  }

  editar(item: any): void {
    this.editandoId = item.iditem;

    this.novoItem = {
      nome: item.item?.item1,
      descricao: item.item?.descricaoItem,
      tipo: item.item?.tipoItem,
      preco: item.precoVenda,
      categoria: item.item?.categoriaIdcategoria
    };
  }

  desativar(id: number): void {
    this.produtoService.desativar(id).subscribe(() => {
      this.carregarItens();
    });
  }

  cancelar(): void {
    this.editandoId = null;

    this.novoItem = {
      nome: '',
      descricao: '',
      tipo: 'P',
      preco: 0,
      categoria: 1
    };
  }
}