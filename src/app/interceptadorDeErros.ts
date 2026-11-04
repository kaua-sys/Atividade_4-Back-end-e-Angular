import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const erroInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((erro) => {
      let mensagem = 'Ocorreu um erro na requisição.';

      if (erro.status === 0) {
        mensagem = 'Não foi possível conectar ao servidor.';
      } else if (erro.status === 404) {
        mensagem = 'Registro não encontrado.';
      } else if (erro.status === 400) {
        mensagem = erro.error?.mensagem || 'Dados inválidos.';
      } else if (erro.status >= 500) {
        mensagem = 'Erro interno do servidor.';
      }

      alert(mensagem);

      return throwError(() => erro);
    })
  );
};