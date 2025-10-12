import { Component, signal } from '@angular/core';
import { Testimonial } from '../../interfaces/testimonial';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { StarRatingComponent } from "../../components/star-rating.component/star-rating.component";
import { TestimonialService } from '../../services/testimonial-service';

@Component({
  selector: 'app-testimonial-form',
  imports: [StarRatingComponent, ReactiveFormsModule],
  templateUrl: './testimonial-form.html',
  styleUrl: './testimonial-form.scss'
})
export class TestimonialForm {

  constructor(
    private testimonialService: TestimonialService
  ) {}  

  isSubmittingTestimonial = signal(false);
  private readonly fb = new FormBuilder();
    testimonial: Testimonial = {
      Id: 0,
      AuthorName: '',
      Text: '',
      Rating: 0,
      CreatedAt: new Date()
    };

  protected readonly testimonialFormFb = this.fb.group({

    AuthorName: ['', [Validators.required, Validators.minLength(3)]],
    Text: ['', [Validators.required, Validators.minLength(5)]],
    Rating: [1, []],
  });

  onSubmit() {
    if (this.testimonialFormFb.valid) {
      this.isSubmittingTestimonial.set(true);
      
    

      const newTestimonial: Testimonial = {
        Id: 0,
        AuthorName: this.testimonialFormFb.value.AuthorName || '',
        Text: this.testimonialFormFb.value.Text || '',
        Rating: this.testimonialFormFb.value.Rating || 0,
        CreatedAt: new Date()
      };

      this.testimonialService.post(newTestimonial);
      window.location.href = '/sports-program'; // Redirection vers la page d'accueil
    }
  }

  
}