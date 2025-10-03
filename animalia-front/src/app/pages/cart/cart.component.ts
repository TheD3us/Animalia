import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { CartService } from '../../services/cart.service';
import { AuthService } from '../../services/auth.service';
import { PurchaseService } from '../../services/purchase.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})
export class CartComponent implements OnInit {
  cartItems: any[] = [];
  isLoggedIn = false;

  constructor(
    private cartService: CartService,
    private authService: AuthService,
    private purchaseService: PurchaseService,
    private router: Router
  ) {}

  ngOnInit() {
    this.cartItems = this.cartService.getItems();
    
    this.authService.isLoggedIn$.subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
    });
  }

  removeItem(index: number) {
    this.cartService.removeItem(index);
    this.cartItems = this.cartService.getItems();
  }

  clearCart() {
    if (confirm('Êtes-vous sûr de vouloir vider votre panier ?')) {
      this.cartService.clear();
      this.cartItems = [];
    }
  }

  proceedToCheckout() {
    if (!this.authService.isLoggedIn()) {
      alert('Vous devez être connecté pour valider votre sélection. Vous serez redirigé vers la page de connexion.');
      this.router.navigate(['/login']);
      return;
    }

    if (this.cartItems.length > 0) {
      this.purchaseService.purchaseWorkouts(this.cartItems);
      
      this.cartService.clear();
      this.cartItems = [];
      
      alert('Félicitations ! Vos entraînements ont été achetés avec succès. Vous allez être redirigé vers vos entraînements.');
      this.router.navigate(['/my-purchases']);
    }
  }
}