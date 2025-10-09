import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { PurchaseService } from '../../services/purchase.service';
import { CartService } from '../../services/cart.service';
import { Training } from '../../interfaces/training';
import { TrainingService } from '../../services/training-service';
import { ProgramModelService } from '../../services/program-model';

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
  trainings: Training[] = []; 

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private purchaseService: PurchaseService,
    private cartService: CartService,
    private trainingService: TrainingService,
    private programService: ProgramModelService
  ) {}

  ngOnInit() {
    const programId = this.route.snapshot.paramMap.get('id');
    if (programId) {

      this.programService.getTrainingByProgram(+programId).subscribe({
        next: (res: any[]) => {
          console.log(res);

          // On mappe les propriétés du backend vers notre interface Training
          this.trainings = res.map(item => ({
            id: item.Id,
            title: item.Title,
            description: item.Description,
            durationMinutes: item.DurationMinutes,
            equipment: item.Equipment,
            level: item.Level

          } as Training));

          if (this.trainings.length > 0) {
            const t = this.trainings[0];
            this.workout = {
              id: t.id.toString(),
              title: t.title,
              description: `Durée : ${t.durationMinutes} min | Niveau : ${t.level} | Matériel : ${t.equipment}`,
              type: 'training'
            };
          }
        },
        error: (err) => console.error("Erreur lors du chargement des trainings du programme", err)
      });


    }

  }

  addToCart(training: Training) {
    this.cartService.addItem({
      id: training.id.toString(),
      title: training.title,
      description: `Durée : ${training.durationMinutes} min | Niveau : ${training.level} | Matériel : ${training.equipment}`,
      type: 'training',
      addedAt: new Date()
    });
    alert(`"${training.title}" a été ajouté à votre panier !`);
  }

  addAllToCart() {
    if (this.trainings && this.trainings.length > 0) {
      this.trainings.forEach(t => {
        this.cartService.addItem({
          id: t.id.toString(),
          title: t.title,
          description: `Durée : ${t.durationMinutes} min | Niveau : ${t.level} | Matériel : ${t.equipment}`,
          type: 'training',
          addedAt: new Date()
        });
      });
      alert(`${this.trainings.length} entraînement(s) ont été ajoutés à votre panier !`);
    }
  }


  goBack() {
    this.location.back();
  }
}
