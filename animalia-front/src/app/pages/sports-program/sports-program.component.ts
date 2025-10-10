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
    private authService: AuthService,
    private trainingService: TrainingService
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

      this.trainingService.post(newTraining); // le subscribe est dans le service

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

  if (this.programCards.find(p => p.id === programId)) {
      console.log('Pack sélectionné:', programId);
      window.location.href = `/workout/${programId}`;
    }
  }


}
