import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { PurchaseService, PurchasedWorkout } from '../../services/purchase.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-my-purchases',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './my-purchases.component.html',
  styleUrl: './my-purchases.component.scss'
})
export class MyPurchasesComponent implements OnInit {
  purchasedWorkouts: PurchasedWorkout[] = [];
  isLoggedIn = false;

  constructor(
    private purchaseService: PurchaseService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    // Vérifier l'état de connexion
    this.authService.isLoggedIn$.subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
      if (loggedIn) {
        this.loadPurchasedWorkouts();
      }
    });
  }

  private loadPurchasedWorkouts() {
    this.purchasedWorkouts = this.purchaseService.getPurchasedWorkouts();
  }

  viewWorkoutDetail(workoutId: string) {
    // Naviguer vers la page de détail de l'entraînement
    this.router.navigate(['/workout', workoutId]);
  }

  getLastPurchaseDate(): Date | null {
    if (this.purchasedWorkouts.length === 0) return null;
    
    return this.purchasedWorkouts.reduce((latest, workout) => {
      return workout.purchaseDate > latest ? workout.purchaseDate : latest;
    }, this.purchasedWorkouts[0].purchaseDate);
  }
}