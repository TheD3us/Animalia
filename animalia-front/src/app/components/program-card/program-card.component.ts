import { Component, input, output } from '@angular/core';
import { CartService } from '../../services/cart.service';

export interface ProgramCard {
  id: string;
  title: string;
  description: string;
  image?: string;
  price?: string;
  difficulty: string;
  features?: string[];
  buttonText?: string;
  buttonClass?: string;
  buttonIcon?: string;
  type?: 'workout' | 'pack'; // Type pour différencier les entraînements des packs
}

@Component({
  selector: 'app-program-card',
  standalone: true,
  imports: [],
  templateUrl: './program-card.component.html',
  styleUrl: './program-card.component.scss'
})
export class ProgramCardComponent {
  readonly program = input.required<ProgramCard>();
  readonly showFeatures = input<boolean>(false);
  
  readonly cardClick = output<string>();

  constructor(private cartService: CartService) {}

  onCardClick() {
    this.cardClick.emit(this.program().id);
  }

  onAddToCart() {
    this.cartService.addItem({
      id: this.program().id,
      title: this.program().title,
      description: this.program().description,
      type: 'workout',
      addedAt: new Date()
    });
  }
}