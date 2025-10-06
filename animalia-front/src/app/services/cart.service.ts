import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CartService {
  private items: any[] = [];
  private cartCountSubject = new BehaviorSubject<number>(0);
  cartCount$ = this.cartCountSubject.asObservable();
  private readonly CART_STORAGE_KEY = 'animalia_cart';

  constructor() {
    this.loadCartFromStorage();
  }

  addItem(item: any) {
    this.items.push(item);
    this.saveCartToStorage();
    this.cartCountSubject.next(this.items.length);
  }

  removeItem(index: number) {
    this.items.splice(index, 1);
    this.saveCartToStorage();
    this.cartCountSubject.next(this.items.length);
  }

  getItems() {
    return this.items;
  }

  getCount() {
    return this.items.length;
  }

  clear() {
    this.items = [];
    this.saveCartToStorage();
    this.cartCountSubject.next(0);
  }

  private saveCartToStorage() {
    try {
      localStorage.setItem(this.CART_STORAGE_KEY, JSON.stringify(this.items));
    } catch (error) {
      console.warn('Impossible de sauvegarder le panier dans le localStorage:', error);
    }
  }

  private loadCartFromStorage() {
    try {
      const savedCart = localStorage.getItem(this.CART_STORAGE_KEY);
      if (savedCart) {
        this.items = JSON.parse(savedCart);
        this.cartCountSubject.next(this.items.length);
      }
    } catch (error) {
      console.warn('Impossible de charger le panier depuis le localStorage:', error);
      this.items = [];
    }
  }
}
