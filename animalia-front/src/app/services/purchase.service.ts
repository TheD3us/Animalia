import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface PurchasedWorkout {
  id: string;
  title: string;
  description: string;
  purchaseDate: Date;
  price?: string;
  content: {
    echauffement: string[];
    exercices: string[];
    etirements: string[];
    duree: string;
    difficulte: string;
    materiel: string[];
  };
}

@Injectable({ providedIn: 'root' })
export class PurchaseService {
  private purchasedWorkouts: PurchasedWorkout[] = [];
  private purchasedWorkoutsSubject = new BehaviorSubject<PurchasedWorkout[]>([]);
  purchasedWorkouts$ = this.purchasedWorkoutsSubject.asObservable();
  private readonly PURCHASES_STORAGE_KEY = 'animalia_purchases';

  constructor() {
    this.loadPurchasesFromStorage();
  }

  purchaseWorkouts(workouts: any[]) {
    const purchases: PurchasedWorkout[] = workouts.map(workout => ({
      id: workout.id,
      title: workout.title,
      description: workout.description,
      purchaseDate: new Date(),
      price: this.getWorkoutPrice(workout.id),
      content: this.getWorkoutContent(workout.id)
    }));

    this.purchasedWorkouts.push(...purchases);
    this.savePurchasesToStorage();
    this.purchasedWorkoutsSubject.next(this.purchasedWorkouts);
  }

  getPurchasedWorkouts(): PurchasedWorkout[] {
    return this.purchasedWorkouts;
  }

  hasPurchased(workoutId: string): boolean {
    return this.purchasedWorkouts.some(workout => workout.id === workoutId);
  }

  getPurchasedWorkout(workoutId: string): PurchasedWorkout | undefined {
    return this.purchasedWorkouts.find(workout => workout.id === workoutId);
  }

  private getWorkoutPrice(workoutId: string): string {
    // Prix simulés pour les entraînements individuels
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

  private getWorkoutContent(workoutId: string): any {
    // Contenu détaillé simulé pour chaque entraînement
    const contents: { [key: string]: any } = {
      'squats-pattes': {
        echauffement: [
          'Marche sur place avec votre chien (3 min)',
          'Mouvements d\'articulation des hanches et genoux',
          'Quelques caresses pour motiver votre compagnon'
        ],
        exercices: [
          '3 séries de 15 squats avec high-five à votre chien',
          'Squats isométriques de 30 secondes (chien assis devant vous)',
          'Squats sautés avec encouragements vocaux pour le chien',
          'Finir par 10 squats lents avec friandise à chaque montée'
        ],
        etirements: [
          'Étirements quadriceps et ischios-jambiers',
          'Étirements des mollets',
          'Moment de détente avec caresses pour votre chien'
        ],
        duree: '25 minutes',
        difficulte: 'Débutant',
        materiel: ['Friandises pour chien', 'Tapis de sol (optionnel)']
      },
      'parcours-zigzag': {
        echauffement: [
          'Jogging léger autour du parcours (5 min)',
          'Échauffement articulaire complet',
          'Présentation du parcours à votre chien'
        ],
        exercices: [
          'Circuit complet x5 : slalom entre les cônes',
          'Course relais : vous courez, le chien vous suit',
          'Sauts par-dessus obstacles bas avec le chien',
          'Sprint final sur 50m ensemble'
        ],
        etirements: [
          'Étirements dynamiques jambes',
          'Retour au calme progressif',
          'Récompenses et félicitations pour le chien'
        ],
        duree: '35 minutes',
        difficulte: 'Intermédiaire',
        materiel: ['Cônes ou objets pour délimiter', 'Obstacles bas', 'Laisse longue']
      },
      'yoga-chien': {
        echauffement: [
          'Respiration profonde en position assise avec votre chien',
          'Mouvements doux des bras et du tronc',
          'Connexion énergétique avec votre animal'
        ],
        exercices: [
          'Position du chien tête en bas (avec votre chien qui imite)',
          'Guerrier I et II avec participation canine',
          'Position de l\'enfant avec chien couché à côté',
          'Posture de la montagne avec chien assis en face',
          'Torsions assises avec caresses alternées'
        ],
        etirements: [
          'Savasana (relaxation finale) avec le chien',
          'Méditation courte à deux',
          'Massage mutuel de détente'
        ],
        duree: '45 minutes',
        difficulte: 'Tous niveaux',
        materiel: ['Tapis de yoga', 'Couverture pour le chien', 'Musique douce']
      }
    };

    return contents[workoutId] || {
      echauffement: ['Échauffement général (5 min)'],
      exercices: ['Exercices adaptés avec votre chien'],
      etirements: ['Retour au calme et étirements'],
      duree: '30 minutes',
      difficulte: 'Intermédiaire',
      materiel: ['Matériel de base']
    };
  }

  private savePurchasesToStorage() {
    try {
      localStorage.setItem(this.PURCHASES_STORAGE_KEY, JSON.stringify(this.purchasedWorkouts));
    } catch (error) {
      console.warn('Impossible de sauvegarder les achats dans le localStorage:', error);
    }
  }

  private loadPurchasesFromStorage() {
    try {
      const savedPurchases = localStorage.getItem(this.PURCHASES_STORAGE_KEY);
      if (savedPurchases) {
        this.purchasedWorkouts = JSON.parse(savedPurchases).map((p: any) => ({
          ...p,
          purchaseDate: new Date(p.purchaseDate)
        }));
        this.purchasedWorkoutsSubject.next(this.purchasedWorkouts);
      }
    } catch (error) {
      console.warn('Impossible de charger les achats depuis le localStorage:', error);
      this.purchasedWorkouts = [];
    }
  }
}