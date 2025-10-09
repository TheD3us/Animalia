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
      description: 'Accès à 10 séances d\'entraînement + suivi personnalisé par un coach certifié.',
      price: '29,99 €',
      difficulty: 'easy',
      image: 'assets/images/Tucker_le_chat_le_plus_triste_du_monde.jpg',
      features: [
        '10 séances d\'entraînement',
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
      description: '20 séances + accès illimité à la bibliothèque vidéo + conseils nutritionnels.',
      price: '49,99 €',
      image: 'assets/images/chien.jpg',
      features: [
        '20 séances d\'entraînement',
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
      description: 'Accès illimité à toutes les séances + coaching vidéo en direct + suivi santé complet.',
      price: '79,99 €',
      image: 'assets/images/rendre-un-chien-heureux.jpg',
      features: [
        'Accès illimité aux séances',
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