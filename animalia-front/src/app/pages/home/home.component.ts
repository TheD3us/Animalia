import { Component, signal } from '@angular/core';
import { CarouselComponent, CarouselItem } from '../../components/carousel/carousel.component';
import { ProgramCardComponent, ProgramCard } from '../../components/program-card/program-card.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CarouselComponent, ProgramCardComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  protected readonly title = signal('Animalia - Sport avec votre compagnon');

  // Données pour le carousel d'images principal (you'll need to create actual images)
  protected readonly heroCarousel: CarouselItem[] = [
    {
      id: '1',
      image: 'assets/images/canicross.png',
      title: 'Canicross Aventure',
      content: 'Découvrez le plaisir de courir avec votre chien'
    },
    {
      id: '2', 
      image: 'assets/images/yoga.png',
      title: 'Yoga Canin',
      content: 'Moments de détente partagés'
    },
    {
      id: '3',
      image: 'assets/images/obérythmée.png', 
      title: 'Fitness Duo',
      content: 'Entraînements ludiques en binôme'
    }
  ];

  // Programmes principaux
  protected readonly programs: ProgramCard[] = [
    {
      id: 'canicross',
      title: 'Canicross',
      description: 'Courez aux côtés de votre chien et améliorez votre endurance tout en renforçant vos liens.',
      image: 'assets/images/canicross.png',
      buttonText: 'Découvrir',
      buttonClass: 'btn-primary',
      buttonIcon: 'fa-solid fa-paw'
    },
    {
      id: 'canivtt',
      title: 'CanivTT', 
      description: 'Partagez des sensations fortes avec votre chien lors de balades sportives en VTT.',
      image: 'assets/images/canivtt.png',
      buttonText: 'Découvrir',
      buttonClass: 'btn-primary',
      buttonIcon: 'fa-solid fa-paw'
    },
    {
      id: 'yoga',
      title: 'Yoga Canin',
      description: 'Détendez-vous et trouvez l\'harmonie avec votre chien grâce à des séances de yoga adaptées.',
      image: 'assets/images/yoga.png',
      buttonText: 'Découvrir', 
      buttonClass: 'btn-primary',
      buttonIcon: 'fa-solid fa-paw'
    }
  ];

  // Avis clients pour le carousel
  protected readonly testimonials: CarouselItem[] = [
    {
      id: 'avis1',
      content: 'Une expérience incroyable ! Mon chien et moi avons adoré le canicross, une vraie complicité retrouvée.',
      author: 'Marie & Rex'
    },
    {
      id: 'avis2', 
      content: 'Le yoga avec mon chien a été une révélation, détente assurée pour nous deux !',
      author: 'Thomas & Bella'
    },
    {
      id: 'avis3',
      content: 'Super organisation, activités variées et adaptées à tous les chiens. On reviendra sans hésiter !',
      author: 'Sophie & Lucky'
    }
  ];

  // Gestionnaire pour les clics sur les cartes de programmes
  onProgramClick(programId: string) {
    console.log('Programme sélectionné depuis la page d\'accueil:', programId);
    // Logique de navigation vers la page détaillée du programme
  }
}