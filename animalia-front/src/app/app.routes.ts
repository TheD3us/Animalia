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
    path: 'cart', 
    loadComponent: () => import('./pages/cart/cart.component').then(m => m.CartComponent) 
  },
  { 
    path: 'my-purchases', 
    loadComponent: () => import('./pages/my-purchases/my-purchases.component').then(m => m.MyPurchasesComponent) 
  },
  { 
    path: 'workout/:id', 
    loadComponent: () => import('./pages/workout-detail/workout-detail.component').then(m => m.WorkoutDetailComponent) 
  },
  { 
    path: '**', 
    redirectTo: '' 
  }
];
