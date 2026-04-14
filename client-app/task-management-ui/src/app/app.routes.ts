import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'auth/login',
    loadComponent: () =>
      import('./features/auth/login/login').then(m => m.Login)
  },
  {
    path: 'auth/register',
    loadComponent: () =>
      import('./features/auth/register/register').then(m => m.Register)
  },
  {
    path: 'tasks',
    loadComponent: () =>
      import('./features/tasks/task-list/task-list').then(m => m.TaskList)
  },
  {
    path: 'tasks/create',   
    loadComponent: () =>
      import('./features/tasks/task-form/task-form').then(m => m.TaskForm)
  },
  {
  path: 'tasks/edit/:id',
  loadComponent: () =>
    import('./features/tasks/task-form/task-form').then(m => m.TaskForm),
  data: { renderMode: 'client' } 
},
  {
    path: '',
    redirectTo: 'auth/login',
    pathMatch: 'full'
  }
];