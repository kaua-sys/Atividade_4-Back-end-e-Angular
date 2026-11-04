import { Routes } from '@angular/router';
import { FuncionariosComponent } from './pages/funcionarios/funcionarios.component';
import { ClientesComponent } from './pages/clientes/clientes.component';
import { CatalogoComponent } from './pages/catalogo/catalogo.component';
import { CarrinhoComponent } from './pages/carrinho/carrinho.component';
import { MeusPedidosComponent } from './pages/meus-pedidos/meus-pedidos.component';
import { GerenciarPedidosComponent } from './pages/gerenciar-pedidos/gerenciar-pedidos.component';
import { GerenciarItensComponent } from './pages/produtos/produtos.component';
import { CategoriaComponent } from './pages/categoria/categoria.component';
import { EstoqueComponent } from './pages/estoque/estoque.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';

export const routes: Routes = [
  {
   path: '', redirectTo: 'funcionarios', pathMatch: 'full' 
  },
  { 
   path: 'funcionarios',component: FuncionariosComponent 
  },
  { 
   path: 'clientes',component: ClientesComponent 
  },
  {
   path: 'catalogo',component: CatalogoComponent
  },
  {
   path: 'carrinho',component: CarrinhoComponent
  },
  {
    path: 'pedidos',component: MeusPedidosComponent
  },
  {
    path: 'gerenciar',component:GerenciarPedidosComponent
  },
  {
    path: 'produtos',component:GerenciarItensComponent
  },
  {
    path: 'categoria',component:CategoriaComponent
  },
  {
    path: 'estoque',component:EstoqueComponent
  },
  {
    path:'admin', component:DashboardComponent
  },
];