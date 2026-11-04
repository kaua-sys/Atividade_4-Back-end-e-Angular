import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
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
import { LoginComponent } from './pages/login/login.component';

export const routes: Routes = [
  {
    path: '', redirectTo: 'login', pathMatch: 'full'
  },
  {
    path: 'funcionarios', component: FuncionariosComponent,
    canActivate: [authGuard], data: { roles: ['admin','funcionario'] }
  },
  {
    path: 'clientes', component: ClientesComponent,
    canActivate: [authGuard], data: { roles: ['admin'] }
  },
  {
    path: 'catalogo', component: CatalogoComponent,
    canActivate: [authGuard], data: { roles: ['cliente'] }
  },
  {
    path: 'carrinho', component: CarrinhoComponent,
    canActivate: [authGuard], data: { roles: ['cliente'] }
  },
  {
    path: 'pedidos', component: MeusPedidosComponent,
    canActivate: [authGuard], data: { roles: ['cliente'] }
  },
  {
    path: 'gerenciar', component: GerenciarPedidosComponent,
    canActivate: [authGuard], data: { roles: ['admin', 'funcionario'] }
  },
  {
    path: 'produtos', component: GerenciarItensComponent,
    canActivate: [authGuard], data: { roles: ['admin'] }
  },
  {
    path: 'categoria', component: CategoriaComponent,
    canActivate: [authGuard], data: { roles: ['admin'] }
  },
  {
    path: 'estoque', component: EstoqueComponent,
    canActivate: [authGuard], data: { roles: ['admin', 'funcionario'] }
  },
  {
    path: 'admin', component: DashboardComponent,
    canActivate: [authGuard], data: { roles: ['admin'] }
  },
  {
    path: 'login', component: LoginComponent
  }
];