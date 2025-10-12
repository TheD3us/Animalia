import { Component, input, signal } from '@angular/core';
import { StarRatingComponent } from '../star-rating.component/star-rating.component';

export interface CarouselItem {
  id: string;
  image?: string;
  title?: string;
  content: string;
  author?: string;
  rating?: number; // Note de 1 à 5
}

@Component({
  selector: 'app-carousel',
  standalone: true,
  imports: [StarRatingComponent],
  templateUrl: './carousel.component.html',
  styleUrl: './carousel.component.scss'
})
export class CarouselComponent {
  // Angular 20 input signals
  readonly items = input<CarouselItem[]>([]);
  readonly carouselId = input<string>('defaultCarousel');
  readonly isImageCarousel = input<boolean>(true);
  readonly autoSlide = input<boolean>(true);
  readonly interval = input<number>(5000);

  protected readonly showControls = signal(true);
}