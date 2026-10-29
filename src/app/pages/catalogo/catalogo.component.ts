import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProdutoService } from '../../services/produto.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-catalogo',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './catalogo.component.html',
  styleUrl: './catalogo.component.css'
})
export class CatalogoComponent implements OnInit {

  produtos: any[] = [];
  mensagemErro = '';
  carrinho: any[] = [];

  constructor(private produtoService: ProdutoService) {}

  ngOnInit(): void {
    this.carrinho = JSON.parse( sessionStorage.getItem('carrinho') || '[]');
  
    this.carregarProdutos();
  }

  
  pesquisa = '';
  produtosFiltrados: any[] = [];
  tipoSelecionado = 'T';

  filtrarProdutos(): void {
    this.produtosFiltrados = this.produtos.filter(produto => { const nome = produto.item?.item1 || '';
  
    const nomeCorresponde = nome .toLowerCase() .includes(this.pesquisa.toLowerCase());
  
      const tipoCorresponde = this.tipoSelecionado === 'T' || produto.item?.tipoItem === this.tipoSelecionado;
  
      return nomeCorresponde && tipoCorresponde;
    });
  }

  carregarProdutos(): void {
    this.produtoService.listar().subscribe({
      next: (dados) => {
        this.produtos = dados.filter(produto => produto.item?.ativoProdutoServico === 'S' || produto.item?.ativoProdutoServico === '1');
      
        this.filtrarProdutos();
      },
      error: (erro) => {console.error('Erro ao carregar produtos:', erro);this.mensagemErro = 'Não foi possível carregar o catálogo.';
      }
    });
  }

  adicionarAoCarrinho(produto: any): void {
    const itemExistente = this.carrinho.find(item => item.iditem === produto.iditem);
  
    if (itemExistente) {
      itemExistente.quantidade++;
    } else {
      this.carrinho.push({iditem: produto.iditem,nome: produto.item1, preco: produto.preco, quantidade: 1});
    }
  
    sessionStorage.setItem('carrinho',JSON.stringify(this.carrinho));
  
    alert('Item adicionado ao carrinho!');
  }
  
}
