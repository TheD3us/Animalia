import { Component, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ProgramCardComponent, ProgramCard } from '../../components/program-card/program-card.component';
import { Event } from '../../interfaces/events';
import { EventService } from '../../services/event.service';
import { ProgramModelService } from '../../services/program-model';
import { DatePipe, NgIf, NgFor, NgForOf } from '@angular/common';
import { AuthService } from '../../services/auth.service';


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
    private authService: AuthService
  ) { }

  // Liste des événements récupérés en base
  events: Event[] = [];
  programModels: any[] = [];
  userId: number = -1;

  ngOnInit(): void {
    this.loadEvents();
    this.loadProgramModels();
    this.authService.whoIsLoggedIn().subscribe(res => {
      if(res != undefined)
      {
        this.userId = res;
      }
      
      console.log("Id de l'user qui poste l'envent : ", res);
    })
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

  protected readonly workouts: ProgramCard[] = [
    {
      id: 'squats-pattes',
      title: 'Squats & Pattes',
      description: 'Renforcez vos jambes tout en amusant votre chien : chaque squat est l\'occasion d\'une caresse ou d\'une friandise.',
      image: 'assets/images/squat-avec-son-chien.jpg',
      difficulty: 'easy',
      buttonText: 'Essayer',
      buttonClass: 'btn-outline-primary',
      type: 'workout'
    },
    {
      id: 'parcours-zigzag',
      title: 'Parcours & Zigzag',
      description: 'Créez un petit parcours d\'obstacles et alternez course et slalom avec votre compagnon pour travailler cardio et agilité.',
      image: 'assets/images/Parcours_et_Zigzag.jpg',
      difficulty: 'easy',
      buttonText: 'Essayer',
      buttonClass: 'btn-outline-primary',
      type: 'workout'
    },
    {
      id: 'fentes-rotation',
      title: 'Fentes & Rotation',
      description: 'Effectuez des fentes avant tout en faisant tourner un jouet autour de vous pour stimuler votre équilibre et l\'attention du chien.',
      buttonText: 'Essayer',
      difficulty: 'easy',
      buttonClass: 'btn-outline-primary',
      type: 'workout'
    },
    {
      id: 'tir-corde',
      title: 'Le tir à la corde',
      description: 'Un classique ludique : musclez vos bras et amusez votre chien avec une corde solide, en alternant traction et relâchement.',
      image: 'assets/images/Le_tir_a_la_corde.jpg',
      buttonText: 'Essayer',
      difficulty: 'easy',
      buttonClass: 'btn-outline-primary',
      type: 'workout'
    },
    {
      id: 'combo-fente',
      title: 'Combo Fente & Équilibre',
      description: 'Associez fentes et maintien en équilibre pendant que votre chien vous tourne autour ou saute par-dessus votre jambe.',
      image: 'assets/images/sport-a-la-maison.jpg',
      buttonText: 'Essayer',
      difficulty: 'easy',
      buttonClass: 'btn-outline-primary',
      type: 'workout'
    },
    {
      id: 'burpee-balle',
      title: 'Burpee & Rattrapage de balle',
      description: 'Faites un burpee, lancez la balle, puis repartez pour un nouveau tour pendant que votre chien la rapporte.',
      buttonText: 'Essayer',
      difficulty: 'easy',
      buttonClass: 'btn-outline-primary',
      type: 'workout'
    },
    {
      id: 'saut-obstacles',
      title: 'Saut d\'obstacles fait-maison',
      description: 'Disposez des chaises, balais ou coussins et sautez avec votre chien pour travailler coordination et explosivité.',
      buttonText: 'Essayer',
      difficulty: 'easy',
      buttonClass: 'btn-outline-primary',
      type: 'workout'
    },
    {
      id: 'russian-twist',
      title: 'Russian Twist & Jouet',
      description: 'En position assise, effectuez des rotations du buste en tenant un jouet que votre chien essaiera d\'attraper.',
      buttonText: 'Essayer',
      difficulty: 'easy',
      buttonClass: 'btn-outline-primary',
      type: 'workout'
    },
    {
      id: 'planche-jouet',
      title: 'Planche haute & Jouet',
      description: 'Tenez la position de planche pendant que votre chien tente de récupérer un jouet placé devant vous.',
      buttonText: 'Essayer',
      difficulty: 'easy',
      buttonClass: 'btn-outline-primary',
      type: 'workout'
    },
    {
      id: 'yoga-chien',
      title: 'Yoga avec son chien',
      description: 'Pratiquez des postures douces de yoga en intégrant votre chien pour un moment de détente et de complicité.',
      image: 'assets/images/yoga-avec-son-chien-1.jpg',
      buttonText: 'Essayer',
      difficulty: 'easy',
      buttonClass: 'btn-outline-primary',
      type: 'workout'
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
        UserId: this.userId, 
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


  //onProgramCardClick(programId: string) {
  //  console.log('Programme sélectionné:', programId);
  //  if (this.workouts.find(w => w.id === programId)) {
  //    window.location.href = `/workout/${programId}`;
  //  } else {
  //    console.log('Pack sélectionné:', programId);
  //  }
  //}

  onProgramCardClick(programId: string) {
    console.log('Programme sélectionné:', programId);

    // Si c’est un workout statique
    if (this.workouts.find(w => w.id === programId)) {
      window.location.href = `/workout/${programId}`;
    }
    // Sinon, c’est un ProgramModel (pack)
    else if (this.programCards.find(p => p.id === programId)) {
      console.log('Pack sélectionné:', programId);
      window.location.href = `/workout/${programId}`;
    }
  }


}
