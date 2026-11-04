import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const usuario = auth.obterUsuario();

  if (!usuario) {
    router.navigate(['/login']);
    return false;
  }

  const permissoes = route.data['roles'];

  if (permissoes && !permissoes.includes(usuario.tipoUsuario)) {
    router.navigate(['/catalogo']);
    return false;
  }

  return true;
};