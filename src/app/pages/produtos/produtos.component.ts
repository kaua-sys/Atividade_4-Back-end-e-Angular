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

    if (
  !this.novoItem.nome.trim() ||
  !this.novoItem.descricao.trim() ||
  !this.novoItem.tipo ||
  this.novoItem.preco <= 0 ||
  this.novoItem.categoria <= 0
  ) {
  alert('Preencha todos os campos corretamente.');
  return;
   }
    const item = {
      idCategoria: this.novoItem.categoria,
      item: this.novoItem.nome,
      descricaoItem: this.novoItem.descricao,
      preco: this.novoItem.preco,
      tipoItem: this.novoItem.tipo,
      ativoProdutoServico: 'S',
      codigoPreco: 1,
      precoVenda: this.novoItem.preco,
      unidade: 'UN'
    };
  
    const requisicao = this.editandoId
      ? this.produtoService.editar(this.editandoId, item)
      : this.produtoService.cadastrar(item);
  
    requisicao.subscribe(() => {
      this.carregarItens();
      this.cancelar();
      alert('Salvo com sucesso!');
    });
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

  
excluir(id: number): void {
  if (confirm('Deseja realmente excluir este item?')) {
    this.produtoService.excluir(id).subscribe({
      next: () => {
        this.carregarItens();
        alert('Item excluído com sucesso!');
      },
      error: (erro) => {
        console.log(erro);
        alert(erro.error?.mensagem || 'Erro ao excluir item.');
      }
    });
  }
}

desativar(id: number): void {
  if (confirm('Deseja realmente desativar este item?')) {
    this.produtoService.desativar(id).subscribe({
      next: () => {
        this.carregarItens();
        alert('Item desativado com sucesso!');
      },
      error: (erro) => {
        console.log(erro);
        alert('Erro ao desativar item.');
      }
    });
  }
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