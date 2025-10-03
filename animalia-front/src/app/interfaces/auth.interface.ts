export interface SignupFormData {
  firstName: string;
  lastName: string;
  email: string;
  username: string;
  password: string;
  confirmPassword: string;
  phone?: string;
  acceptTerms: boolean;
}

export interface UserData {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  username: string;
}

export interface PasswordStrengthValidation {
  hasNumber: boolean;
  hasUpper: boolean;
  hasLower: boolean;
  hasSpecial: boolean;
}

export interface PasswordValidationError {
  passwordStrength: PasswordStrengthValidation;
}