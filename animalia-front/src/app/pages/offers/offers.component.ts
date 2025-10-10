import { Component, signal } from '@angular/core';
import { ProgramCardComponent, ProgramCard } from '../../components/program-card/program-card.component';

@Component({
  selector: 'app-offers',
  standalone: true,
  imports: [ProgramCardComponent],
  templateUrl: './offers.component.html',
  styleUrl: './offers.component.scss'
})
export class OffersComponent {
  protected readonly offers: ProgramCard[] = [
    {
      id: 'pack-debutant',
      title: 'Pack Débutant',
      description: 'Accès à 10 programmes d\'entraînement + suivi personnalisé par un coach certifié.',
      price: '29,99 €',
      difficulty: 'easy',
      image: 'PackDebutant.png',
      features: [
        '10 programmes d\'entraînement',
        'Suivi personnalisé',
        'Coach certifié',
        'Support client'
      ],
      buttonText: 'Choisir',
      buttonClass: 'btn-primary',
      buttonIcon: 'fa-solid fa-paw'
    },
    {
      id: 'pack-intermediaire',
      title: 'Pack Intermédiaire',
      difficulty: 'easy',
      description: '20 programmes + accès illimité à la bibliothèque vidéo + conseils nutritionnels.',
      price: '49,99 €',
      image: 'PackIntermediaire.png',
      features: [
        '20 programmes d\'entraînement',
        'Bibliothèque vidéo illimitée',
        'Conseils nutritionnels',
        'Support premium'
      ],
      buttonText: 'Choisir',
      buttonClass: 'btn-success',
      buttonIcon: 'fa-solid fa-paw'
    },
    {
      id: 'pack-premium',
      title: 'Pack Premium',
      difficulty: 'easy',
      description: 'Accès illimité à tous les programmes + coaching vidéo en direct + suivi santé complet.',
      price: '79,99 €',
      image: 'PackPremium.png',
      features: [
        'Accès illimité aux programmes',
        'Coaching vidéo en direct',
        'Suivi santé complet',
        'Support VIP 24/7'
      ],
      buttonText: 'Choisir',
      buttonClass: 'btn-danger',
      buttonIcon: 'fa-solid fa-paw'
    }
  ];

  // Gestionnaire pour les clics sur les cartes d'offres
  onOfferClick(offerId: string) {
    console.log('Offre sélectionnée:', offerId);
    // Logique de traitement de commande ou navigation
  }
}
