import { Routes } from '@angular/router';
import { FuncionariosComponent } from './pages/funcionarios/funcionarios.component';

export const routes: Routes = [
  {
     path: '', redirectTo: 'funcionarios', pathMatch: 'full' 
  },

  {
     path: 'funcionarios', component: FuncionariosComponent 
  }
];