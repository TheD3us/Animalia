import { Routes } from '@angular/router';

export const routes: Routes = [
  { 
    path: '', 
    loadComponent: () => import('./pages/home/home.component').then(m => m.HomeComponent) 
  },
  { 
    path: 'login', 
    loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent) 
  },
  { 
    path: 'offers', 
    loadComponent: () => import('./pages/offers/offers.component').then(m => m.OffersComponent) 
  },
  { 
    path: 'programs', 
    loadComponent: () => import('./pages/sports-program/sports-program.component').then(m => m.SportsProgramComponent) 
  },
  { 
    path: '**', 
    redirectTo: '' 
  }
];
