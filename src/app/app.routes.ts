import { Routes } from '@angular/router';
import { FuncionariosComponent } from './pages/funcionarios/funcionarios.component';
import { ClientesComponent } from './pages/clientes/clientes.component';
import { CatalogoComponent } from './pages/catalogo/catalogo.component';
import { CarrinhoComponent } from './pages/carrinho/carrinho.component';
import { MeusPedidosComponent } from './pages/meus-pedidos/meus-pedidos.component';

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
  }
];