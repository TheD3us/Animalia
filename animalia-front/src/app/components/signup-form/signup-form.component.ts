import { Component, signal, output } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { UserData, PasswordStrengthValidation } from '../../interfaces/auth.interface';

@Component({
  selector: 'app-signup-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './signup-form.component.html',
  styleUrl: './signup-form.component.scss'
})
export class SignupFormComponent {
  private readonly fb = new FormBuilder();
  
  signupSuccess = output<UserData>();
  switchToLogin = output<void>();

  protected readonly signupForm = this.fb.group({
    firstName: ['', [Validators.required, Validators.minLength(2)]],
    lastName: ['', [Validators.required, Validators.minLength(2)]],
    email: ['', [Validators.required, Validators.email]],
    username: ['', [Validators.required, Validators.minLength(3)]],
    password: ['', [Validators.required, Validators.minLength(8), this.passwordValidator]],
    confirmPassword: ['', [Validators.required]],
    phone: ['', [Validators.pattern(/^(\+33|0)[1-9](\d{8})$/)]],
    acceptTerms: [false, [Validators.requiredTrue]]
  }, { validators: this.passwordMatchValidator });

  protected readonly isLoading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly showPassword = signal(false);
  protected readonly showConfirmPassword = signal(false);

  private passwordValidator(control: AbstractControl) {
    const value = control.value;
    if (!value) return null;
    
    const hasNumber = /[0-9]/.test(value);
    const hasUpper = /[A-Z]/.test(value);
    const hasLower = /[a-z]/.test(value);
    const hasSpecial = /[#?!@$%^&*-]/.test(value);
    
    const valid = hasNumber && hasUpper && hasLower && hasSpecial;
    
    if (!valid) {
      return { 
        passwordStrength: {
          hasNumber,
          hasUpper, 
          hasLower,
          hasSpecial
        } as PasswordStrengthValidation
      };
    }
    
    return null;
  }

  private passwordMatchValidator(control: AbstractControl) {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;
    
    if (password && confirmPassword && password !== confirmPassword) {
      return { passwordMismatch: true };
    }
    
    return null;
  }

  onSignup() {
    if (this.signupForm.valid) {
      this.isLoading.set(true);
      this.error.set(null);
      
      const formData = this.signupForm.value;
      console.log('Création de compte:', formData);
      
      setTimeout(() => {
        this.isLoading.set(false);
        
        const userData: UserData = {
          id: Date.now(),
          firstName: formData.firstName!,
          lastName: formData.lastName!,
          email: formData.email!,
          username: formData.username!
        };
        
        this.signupSuccess.emit(userData);
      }, 2000);
    } else {
      this.error.set('Veuillez corriger les erreurs dans le formulaire');
      this.markAllFieldsAsTouched();
    }
  }

  onSwitchToLogin() {
    this.switchToLogin.emit();
  }

  togglePasswordVisibility() {
    this.showPassword.set(!this.showPassword());
  }

  toggleConfirmPasswordVisibility() {
    this.showConfirmPassword.set(!this.showConfirmPassword());
  }

  private markAllFieldsAsTouched() {
    Object.keys(this.signupForm.controls).forEach(key => {
      this.signupForm.get(key)?.markAsTouched();
    });
  }

  get firstName() { return this.signupForm.get('firstName'); }
  get lastName() { return this.signupForm.get('lastName'); }
  get email() { return this.signupForm.get('email'); }
  get username() { return this.signupForm.get('username'); }
  get password() { return this.signupForm.get('password'); }
  get confirmPassword() { return this.signupForm.get('confirmPassword'); }
  get phone() { return this.signupForm.get('phone'); }
  get acceptTerms() { return this.signupForm.get('acceptTerms'); }
}