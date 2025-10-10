import { Component, signal } from '@angular/core';
import { CarouselComponent, CarouselItem } from '../../components/carousel/carousel.component';
import { ProgramCardComponent, ProgramCard } from '../../components/program-card/program-card.component';
import { ProgramModelService } from '../../services/program-model';
import { Testimonial } from '../../interfaces/testimonial';
import { TestimonialService } from '../../services/testimonial-service';
import { ProgramModel } from '../../interfaces/program-models';
import { Router } from '@angular/router';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CarouselComponent, ProgramCardComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  ListProgramCard: ProgramCard[] = [];

    // Avis clients pour le carousel
  protected readonly testimonials: CarouselItem[] = [];

  constructor(private programService: ProgramModelService, 
    private testimonialService: TestimonialService,
    private router: Router
            ){}

  ngOnInit(){
    //On récupère la liste de programModel
    this.programService.getListe().subscribe({
  next: (res : ProgramModel[]) => {
    
    this.FillProgramCard(res);
    console.log(this.testimonials);
  },
  error: (err) => console.error(err)
  });
  

    //on récupère la liste de testimonial
    this.testimonialService.getListe().subscribe({
      next: (res: Testimonial[]) => {
        this.FillTestimonial(res);
        console.log("Resultat : testimonial ", res);
      },
      error: (err) => console.error(err)
    })
    
  }

  FillProgramCard(res: ProgramModel[]){
    res.forEach( (item) => {
      const card: ProgramCard = {id: item.Id.toString(),
      title: item.Title, 
      description: item.Summary,
      image: item.ImageUrl,
      difficulty:item.Difficulty,
      buttonText: 'Découvrir',
      buttonClass: 'btn-primary',
      buttonIcon: 'fa-solid fa-paw'};
        
        console.log(card);
      this.ListProgramCard.push(
        card
      )
    })
    console.log(this.ListProgramCard);
  }

  FillTestimonial(res: Testimonial[]){
    res.forEach(item => {
      const testimonial: CarouselItem = {
        id : item.Id.toString(),
        content : item.Text,
        author : item.AuthorName
      }
      
      this.testimonials.push(testimonial);
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
    this.router.navigate(['/workout', programId]);
    // Logique de navigation vers la page détaillée du programme
  }
}
