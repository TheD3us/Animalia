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

  // Base de données des entraînements (normalement viendrait d'un service)
  private workoutsData: { [key: string]: WorkoutData } = {
    
  };

  

  ngOnInit() {
    const workoutId = this.route.snapshot.paramMap.get('id');
    if (workoutId) {
      this.loadWorkout(workoutId);
    }

    this.trainingService.getListe().subscribe({
      next: (res : Training[]) => {
          
          this.FillWorkoutsData(res);
          console.log(res);
        },
        error: (err) => console.error(err)
    })
  }

  private FillWorkoutsData(res: Training[]){
    res.forEach(item => {
      const workout: WorkoutData = {
      id : item.id.toString(),
      title : item.title,
      description : ''
      
    }
    })
    
  }

  private loadWorkout(workoutId: string) {
    // Charger les données de base de l'entraînement
    this.workout = this.workoutsData[workoutId] || null;
    
    if (this.workout) {
      // Vérifier si l'entraînement a été acheté
      this.isPurchased = this.purchaseService.hasPurchased(workoutId);
      
      // Charger le contenu détaillé
      if (this.isPurchased) {
        const purchasedWorkout = this.purchaseService.getPurchasedWorkout(workoutId);
        this.workoutContent = purchasedWorkout?.content || {};
      } else {
        // Contenu limité pour aperçu
        this.workoutContent = this.getPreviewContent(workoutId);
        this.workoutPrice = this.getWorkoutPrice(workoutId);
      }
    }
  }

  private getPreviewContent(workoutId: string): any {
    // Contenu limité pour les non-acheteurs
    const previewContents: { [key: string]: any } = {
      'squats-pattes': {
        echauffement: ['Marche sur place avec votre chien (3 min)'],
        exercices: ['3 séries de 15 squats avec high-five à votre chien'],
        etirements: ['Étirements quadriceps et ischios-jambiers'],
        duree: '25 minutes',
        difficulte: 'Débutant',
        materiel: ['Friandises pour chien', 'Tapis de sol (optionnel)']
      },
      'yoga-chien': {
        echauffement: ['Respiration profonde en position assise avec votre chien'],
        exercices: ['Position du chien tête en bas (avec votre chien qui imite)'],
        etirements: ['Savasana (relaxation finale) avec le chien'],
        duree: '45 minutes',
        difficulte: 'Tous niveaux',
        materiel: ['Tapis de yoga', 'Couverture pour le chien']
      }
    };

    return previewContents[workoutId] || {
      echauffement: ['Échauffement général (5 min)'],
      exercices: ['Exercices adaptés avec votre chien'],
      etirements: ['Retour au calme et étirements'],
      duree: '30 minutes',
      difficulte: 'Intermédiaire',
      materiel: ['Matériel de base']
    };
  }

  private getWorkoutPrice(workoutId: string): string {
    const prices: { [key: string]: string } = {
      'squats-pattes': '9,99 €',
      'parcours-zigzag': '12,99 €',
      'fentes-rotation': '8,99 €',
      'tir-corde': '7,99 €',
      'combo-fente': '11,99 €',
      'burpee-balle': '10,99 €',
      'saut-obstacles': '9,99 €',
      'russian-twist': '8,99 €',
      'planche-jouet': '9,99 €',
      'yoga-chien': '14,99 €'
    };
    return prices[workoutId] || '9,99 €';
  }

  addToCart() {
    if (this.workout) {
      this.cartService.addItem({
        id: this.workout.id,
        title: this.workout.title,
        description: this.workout.description,
        type: 'workout',
        addedAt: new Date()
      });
      
      // Afficher un message de confirmation ou rediriger vers le panier
      alert(`"${this.workout.title}" a été ajouté à votre panier !`);
    }
  }

  goBack() {
    this.location.back();
  }
}