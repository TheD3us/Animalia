import { Component, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ProgramCardComponent, ProgramCard } from '../../components/program-card/program-card.component';
import { Event } from '../../interfaces/events';
import { EventService } from '../../services/event.service';
import { ProgramModelService } from '../../services/program-model';
import { DatePipe, NgIf, NgFor, NgForOf } from '@angular/common';


@Component({
  selector: 'app-sports-program',
  standalone: true,
  imports: [ReactiveFormsModule, ProgramCardComponent, DatePipe, NgIf, NgFor, NgForOf],
  templateUrl: './sports-program.component.html',
  styleUrl: './sports-program.component.scss'
})
export class SportsProgramComponent {
  private readonly fb = new FormBuilder();

  constructor(
    private eventService: EventService,
    private programService: ProgramModelService,
  ) { }

  // Liste des événements récupérés en base
  events: Event[] = [];
  programModels: any[] = [];

  ngOnInit(): void {
    this.loadEvents();
    this.loadProgramModels();

  }
  programCards: ProgramCard[] = [];

  loadProgramModels() {
    this.programService.getListe().subscribe({
      next: (data: any) => {
        this.programModels = data;

        //Mapping ProgramModel -> ProgramCard
        this.programCards = data.map((pm: { Id: { toString: () => any; }; Title: any; Summary: any; Difficulty: any; Price: string; ImageUrl: any; }) => ({
          id: pm.Id.toString(),
          title: pm.Title,
          description: pm.Summary,
          difficulty: pm.Difficulty,
          price: pm.Price ? pm.Price + " €" : "Gratuit",
          buttonText: "Essayer",
          buttonClass: "btn-primary",
          buttonIcon: "bi bi-check-circle",
          type: "workout",
          image: pm.ImageUrl
        }));
      },
      error: (err) => console.error("Erreur lors du chargement des ProgramModels", err)
    });
  }

  loadEvents() {
    this.eventService.getListe().subscribe({
      next: (data: any) => {
        console.log(data);
        this.events = data;
      },
      error: (err) => {
        console.error("Erreur lors du chargement des événements", err);
      }
    });
  }

  protected readonly packs: ProgramCard[] = [
    {
      id: 'pack-debutant',
      title: 'Pack Débutant',
      description: 'Accès à 10 séances d\'entraînement + suivi personnalisé par un coach certifié.',
      price: '29,99 €',
      difficulty: 'easy',
      buttonText: 'Choisir',
      buttonClass: 'btn-primary',
      buttonIcon: 'bi bi-star-fill text-warning',
      type: 'pack'
    },
    {
      id: 'pack-intermediaire', 
      title: 'Pack Intermédiaire',
      description: '20 séances + accès illimité à la bibliothèque vidéo + conseils nutritionnels.',
      price: '49,99 €',
      difficulty: 'easy',
      buttonText: 'Choisir',
      buttonClass: 'btn-success',
      buttonIcon: 'bi bi-lightning-charge-fill text-success',
      type: 'pack'
    },
    {
      id: 'pack-premium',
      title: 'Pack Premium', 
      description: 'Accès illimité à toutes les séances + coaching vidéo en direct + suivi santé complet.',
      price: '79,99 €',
      difficulty: 'easy',
      buttonText: 'Choisir',
      buttonClass: 'btn-danger',
      buttonIcon: 'bi bi-trophy-fill text-danger',
      type: 'pack'
    }
  ];


  protected readonly proposalForm = this.fb.group({
    titre: ['', [Validators.required, Validators.minLength(3)]],
    description: ['', [Validators.required, Validators.minLength(10)]]
  });

  protected readonly eventForm = this.fb.group({
    nomEvent: ['', [Validators.required, Validators.minLength(3)]],
    dateEvent: ['', Validators.required],
    lieuEvent: ['', [Validators.required, Validators.minLength(3)]]
  });

  protected readonly isSubmittingProposal = signal(false);
  protected readonly isSubmittingEvent = signal(false);

  onSubmitProposal() {
    if (this.proposalForm.valid) {
      this.isSubmittingProposal.set(true);
      const data = this.proposalForm.value;

      const newProgramModel = {
        Id: 0,
        Title: data.titre!,
        Summary: data.description!,
        Difficulty: 'Facile',
        Price: 0,
        ImageUrl: ''
      };

      this.programService.post(newProgramModel); // le subscribe est dans le service

      // On recharge la liste après un petit délai (pour laisser l’API répondre)
      setTimeout(() => {
        this.loadProgramModels();
        this.isSubmittingProposal.set(false);
        this.proposalForm.reset();
      }, 500);
    }
  }

  onSubmitEvent() {
    if (this.eventForm.valid) {
      this.isSubmittingEvent.set(true);
      const data = this.eventForm.value;

      this.eventService.post({
        Id: 0,
        UserId: 1, // à remplacer par l’ID de l’utilisateur connecté
        Title: data.nomEvent!,
        DateTime: data.dateEvent!,
        Location: data.lieuEvent!,
        Notes: '',
        MaxParticipants: 20
      });

      // Le .subscribe() est dans le service.
      setTimeout(() => {
        this.isSubmittingEvent.set(false);
        this.eventForm.reset();
      }, 1000);
    }
  }

  onProgramCardClick(programId: string) {
    console.log('Programme sélectionné:', programId);

    if (this.programCards.find(p => p.id === programId)) {
      console.log('Pack sélectionné:', programId);
      window.location.href = `/workout/${programId}`;
    }
  }


}
