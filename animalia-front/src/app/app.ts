import { Component, signal, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './components/navbar/navbar.component';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  protected readonly title = signal('animalia-front');

  constructor(private authService: AuthService) {}

  ngOnInit() {
    // Vérifie s'il y a une session existante au démarrage de l'app
    this.authService.checkExistingSession();
  }
}
