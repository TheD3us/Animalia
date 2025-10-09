import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { PurchaseService } from '../../services/purchase.service';
import { CartService } from '../../services/cart.service';
import { Training } from '../../interfaces/training';
import { TrainingService } from '../../services/training-service';

interface WorkoutData {
  id: string;
  title: string;
  description: string;
  image?: string;
  type?: string;
}

@Component({
  selector: 'app-workout-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './workout-detail.component.html',
  styleUrl: './workout-detail.component.scss'
})
export class WorkoutDetailComponent implements OnInit {
  workout: WorkoutData | null = null;
  workoutContent: any = {};
  isPurchased = false;
  workoutPrice = '';
  trains: Training[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private purchaseService: PurchaseService,
    private cartService: CartService,
    private trainingService: TrainingService
  ) {}

  ngOnInit() {
    const workoutId = this.route.snapshot.paramMap.get('id');
    if (workoutId) {

      this.trainingService.get(+workoutId).subscribe({
        next: (res: any) => {
          this.workout = {
            id: res.Id.toString(),
            title: res.Title,
            description: res.Description,
            type: 'training'
          };
        }
      });

    }

  }

  addToCart() {

    if (this.workout) {
      this.cartService.addItem({
        id: this.workout.id,
        title: this.workout.title,
        description: this.workout.description,
        type: 'training',
        addedAt: new Date()
      });
      alert(`"${this.workout.title}" a été ajouté à votre panier !`);
    }
  }

  goBack() {
    this.location.back();
  }
}
