import { Component, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ProgramCardComponent, ProgramCard } from '../../components/program-card/program-card.component';
import { Event } from '../../interfaces/events';
import { EventService } from '../../services/event.service';
import { ProgramModelService } from '../../services/program-model';
import { DatePipe, NgIf, NgFor, NgForOf } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { TrainingService } from '../../services/training-service';
import { Training } from '../../interfaces/training';


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

    title: ['', [Validators.required, Validators.minLength(3)]],
    durationMinutes: [0, [Validators.required, Validators.min(5)]],
    equipment: ['', [Validators.required, Validators.minLength(2)]],
    level: ['', [Validators.required]]
  });

  protected readonly eventForm = this.fb.group({
    nomEvent: ['', [Validators.required, Validators.minLength(3)]],
    dateEvent: ['', Validators.required],
    lieuEvent: ['', [Validators.required, Validators.minLength(3)]],
    note: [''],
    maxParticipants: ['', [Validators.required, Validators.min(2), Validators.max(100)]]
  });

  protected readonly isSubmittingProposal = signal(false);
  protected readonly isSubmittingEvent = signal(false);

  onSubmitProposal() {
    if (this.proposalForm.valid) {
      this.isSubmittingProposal.set(true);
      const data = this.proposalForm.value;


      const newTraining: Training = {
        Id: 0, // l’API génère l’ID
        Title: data.title!,
        Description: '',
        DurationMinutes: data.durationMinutes!,
        Equipment: data.equipment!,
        Level: data.level!,
        UserId: this.userId

      };

      this.programService.post(newProgramModel); // le subscribe est dans le service

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
        Notes: data.note || '',
        MaxParticipants: data.maxParticipants ? Number(data.maxParticipants) : undefined
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
