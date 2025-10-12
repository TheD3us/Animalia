import {
  Component,
  Input,
  Output,
  EventEmitter,
  ChangeDetectionStrategy,
  HostBinding,
  OnInit,
  ElementRef,
  Renderer2,
  forwardRef
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-star-rating',
  templateUrl: './star-rating.component.html',
  styleUrls: ['./star-rating.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [CommonModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => StarRatingComponent),
      multi: true
    }
  ]
})
export class StarRatingComponent implements OnInit {
  /** Valeur actuelle (1..maxStars) */
  @Input() value = 0;

  /** Nombre d'étoiles (par défaut 5) */
  @Input() maxStars = 5;

  /** Si true, le composant est lecture seule */
  @Input() readonly = false;

  /** Taille CSS (ex: '24px') */
  @Input() size = '28px';

  /** Evénement quand la note change (nombre entier) */
  @Output() valueChange = new EventEmitter<number>();

  /** étoile survolée actuellement (prévisualisation) */
  hoverValue = 0;

  /** index focus (1..maxStars) pour clavier */
  focusedIndex = 0;

  stars: number[] = [];
  onChange = (value: number) => {};
  onTouched = () => {};

  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngOnInit(): void {
    this.maxStars = Math.max(1, Math.floor(this.maxStars || 5));
    this.stars = Array.from({ length: this.maxStars }, (_, i) => i + 1);
    this.value = this.clampValue(this.value);
    // expose tabindex if not readonly for accessibility
    if (!this.readonly) {
      this.renderer.setAttribute(this.el.nativeElement, 'tabindex', '0');
      this.renderer.setAttribute(this.el.nativeElement, 'role', 'radiogroup');
    } else {
      this.renderer.setAttribute(this.el.nativeElement, 'aria-hidden', 'false');
    }
    console.log('Stars:', this.stars);
  }

  clampValue(v: number) {
    if (!Number.isFinite(v)) return 0;
    return Math.max(0, Math.min(this.maxStars, Math.floor(v)));
  }

  // Hover events (desktop)
  onMouseEnter(index: number) {
    if (this.readonly) return;
    this.hoverValue = index;
  }
  onMouseLeave() {
    if (this.readonly) return;
    this.hoverValue = 0;
  }

  // Click / touch to set
  setValue(index: number) {
    if (this.readonly) return;
    this.value = index;
    this.onChange(this.value); // ✅ Angular reçoit le changement ici
    this.onTouched();
    this.valueChange.emit(this.value);
}
  

  

  // Used by template to know if star should be filled
  isFilled(index: number) {
    return index <= (this.hoverValue || this.value);
  }

  writeValue(value: number): void {
    this.value = value || 0;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
}