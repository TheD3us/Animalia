import { Component, signal, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SignupFormComponent } from '../../components/signup-form/signup-form.component';
import { UserData } from '../../interfaces/auth.interface';
import { AuthService } from '../../services/auth.service';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, SignupFormComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  private readonly fb = new FormBuilder();
  
  protected readonly loginForm = this.fb.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    password: ['', [Validators.required, Validators.minLength(8)]]
  });

  protected readonly isLoading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly showSignupForm = signal(false);
  protected readonly hasItemsInCart = signal(false);
  protected readonly cartItemsCount = signal(0);

  constructor(
    private authService: AuthService,
    private router: Router,
    private cartService: CartService
  ) {}

  ngOnInit() {
    const count = this.cartService.getCount();
    this.hasItemsInCart.set(count > 0);
    this.cartItemsCount.set(count);
  }

  onLogin() {
    if (this.loginForm.valid) {
      this.isLoading.set(true);
      this.error.set(null);
      
      const formData = this.loginForm.value;
      console.log('Tentative de connexion:', formData);
      
      setTimeout(() => {
        const success = this.authService.login(formData);
        this.isLoading.set(false);
        
        if (success) {
          const hasItemsInCart = this.cartService.getCount() > 0;
          
          if (hasItemsInCart) {
            this.router.navigate(['/cart']);
          } else {
            this.router.navigate(['/']);
          }
        } else {
          this.error.set('Nom d\'utilisateur ou mot de passe incorrect');
        }
      }, 2000);
    } else {
      this.error.set('Veuillez remplir tous les champs correctement');
    }
  }

  onCreateAccount() {
    this.showSignupForm.set(true);
  }

  onBackToLogin() {
    this.showSignupForm.set(false);
  }

  onSignupSuccess(userData: UserData) {
    console.log('Compte créé avec succès:', userData);
    this.showSignupForm.set(false);
    this.loginForm.patchValue({ username: userData.username });
  }
}