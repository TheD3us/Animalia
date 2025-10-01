import { Component, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { SignupFormComponent } from '../../components/signup-form/signup-form.component';
import { UserData } from '../../interfaces/auth.interface';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, SignupFormComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private readonly fb = new FormBuilder();
  
  // Angular 20 reactive form with signals
  protected readonly loginForm = this.fb.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    password: ['', [Validators.required, Validators.minLength(8)]]
  });

  protected readonly isLoading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly showSignupForm = signal(false);

  onLogin() {
    if (this.loginForm.valid) {
      this.isLoading.set(true);
      this.error.set(null);
      
      const formData = this.loginForm.value;
      console.log('Tentative de connexion:', formData);
      
      // Simulation d'une connexion
      setTimeout(() => {
        this.isLoading.set(false);
        // Logique de connexion ici
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