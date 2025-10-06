import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Testimonial } from '../interfaces/testimonial';
import { environment } from '../../environement/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TestimonialService {
  constructor(private http: HttpClient){}
  
  getListe() : Observable<Testimonial[]>{
    return this.http.get<Testimonial[]>(environment.apiUrl + "testimonial");
  }

  get(id: number){
    return this.http.get(environment.apiUrl + "testimonial/" + id);
  }

  post(data: Testimonial){
    const body = JSON.stringify(data);

    this.http.post(environment.apiUrl + "testimonial", body, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).
      subscribe(response => {

        console.log("crud service post testimonial OK");

      });
  }

  delete(id: number){
    this.http.delete(environment.apiUrl + "testimonial/" + id).subscribe(response => {

        console.log("crud service delete testimonial OK");

      });
  }

  put(data: Testimonial){
    const body = JSON.stringify(data);
    this.http.put(environment.apiUrl + "testimonial", body, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {

        console.log("crud service put testimonial OK");

      });
  }

}
