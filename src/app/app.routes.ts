import { Routes } from '@angular/router';
import { FuncionariosComponent } from './pages/funcionarios/funcionarios.component';
import { ClientesComponent } from './pages/clientes/clientes.component';

export const routes: Routes = [
  {
    path: '', redirectTo: 'funcionarios', pathMatch: 'full' 
  },

  { 
   path: 'funcionarios', component: FuncionariosComponent 
  },
  { 
   path: 'clientes', component: ClientesComponent 
  }
];