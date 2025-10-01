import { Component, input, output } from '@angular/core';

export interface ProgramCard {
  id: string;
  title: string;
  description: string;
  image?: string;
  price?: string;
  features?: string[];
  buttonText?: string;
  buttonClass?: string;
  buttonIcon?: string;
}

@Component({
  selector: 'app-program-card',
  standalone: true,
  imports: [],
  templateUrl: './program-card.component.html',
  styleUrl: './program-card.component.scss'
})
export class ProgramCardComponent {
  // Angular 20 input signals
  readonly program = input.required<ProgramCard>();
  readonly showFeatures = input<boolean>(false);
  
  // Angular 20 output signal
  readonly cardClick = output<string>();

  onCardClick() {
    // Émetteur d'événement pour gérer les clics
    this.cardClick.emit(this.program().id);
  }
}