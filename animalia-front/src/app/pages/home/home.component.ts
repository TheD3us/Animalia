import { Component, signal } from '@angular/core';
import { CarouselComponent, CarouselItem } from '../../components/carousel/carousel.component';
import { ProgramCardComponent, ProgramCard } from '../../components/program-card/program-card.component';
import { ProgramModelService } from '../../services/program-model';
import { Testimonial } from '../../interfaces/testimonial';
import { TestimonialService } from '../../services/testimonial-service';
import { ProgramModel } from '../../interfaces/program-models';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CarouselComponent, ProgramCardComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  ListProgramCard: ProgramCard[] = [];
  ListProgram: ProgramModel[] = [];
  ListTestimonial: Testimonial[] = [];
    // Avis clients pour le carousel
  protected readonly testimonials: CarouselItem[] = [];
  testimonial: CarouselItem = {
    id: '',
    content: '',
    author: ''
  };
  card: ProgramCard = {id: '',
      title: '', 
      description: '',
      image: '',
      difficulty:'',
      buttonText: 'Découvrir',
      buttonClass: 'btn-primary',
      buttonIcon: 'fa-solid fa-paw'};

  constructor(private programService: ProgramModelService, 
              private testimonialService : TestimonialService
            ){}

  ngOnInit(){
    //On récupère la liste de programModel
    this.programService.getListe().subscribe({
  next: (res : ProgramModel[]) => {
    this.ListProgram = res;
    console.log(this.ListProgram);
  },
  error: (err) => console.error(err)
  });
  this.FillProgramCard();

    //on récupère la liste de testimonial
    this.testimonialService.getListe().subscribe({
      next: (res: Testimonial[]) => {
        this.ListTestimonial = res;
        console.log(this.ListTestimonial);
      },
      error: (err) => console.error(err)
    })
    this.FillTestimonial();
  }

  FillProgramCard(){
    this.ListProgram.forEach(item => {
        this.card.id = item.Id.toString();
        this.card.title = item.Title;
        this.card.description = item.Summary;
        this.card.image = item.ImageUrl;
        this.card.difficulty = item.Difficulty;
        
      this.ListProgramCard.push(
        this.card
      )
    })
  }

  FillTestimonial(){
    this.ListTestimonial.forEach(item => {
      this.testimonial.id = item.Id.toString();
      this.testimonial.content = item.Text;
      this.testimonial.author = item.AuthorName;
    })
  }

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

  



  // Gestionnaire pour les clics sur les cartes de programmes
  onProgramClick(programId: string) {
    console.log('Programme sélectionné depuis la page d\'accueil:', programId);
    // Logique de navigation vers la page détaillée du programme
  }
}