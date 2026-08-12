import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'clientes' },
  {
    path: 'clientes',
    loadComponent: () =>
      import('./features/clientes/clientes-list/clientes-list').then(m => m.ClientesList)
  },
  {
    path: 'clientes/nuevo',
    loadComponent: () =>
      import('./features/clientes/cliente-form/cliente-form').then(m => m.ClienteForm)
  },
  {
    path: 'clientes/:id/editar',
    loadComponent: () =>
      import('./features/clientes/cliente-form/cliente-form').then(m => m.ClienteForm)
  },
  { path: '**', redirectTo: 'clientes' }
];
