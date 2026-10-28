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

  constructor(private produtoService: ProdutoService) {}

  ngOnInit(): void {
    this.carregarProdutos();
  }

  pesquisa = '';
  produtosFiltrados: any[] = [];
  
  filtrarProdutos(): void {
    this.produtosFiltrados = this.produtos.filter(produto =>
      produto.item1
        .toLowerCase()
        .includes(this.pesquisa.toLowerCase())
    );
  }

  carregarProdutos(): void {
    this.produtoService.listar().subscribe({
      next: (dados) => {
        this.produtos = dados.filter(
          produto =>
            produto.ativoProdutoServico === 'S' ||
            produto.ativoProdutoServico === '1'
        );
      
        this.filtrarProdutos();
      },
      error: (erro) => {
        console.error('Erro ao carregar produtos:', erro);
        this.mensagemErro = 'Não foi possível carregar o catálogo.';
      }
    });
  }
  
}
