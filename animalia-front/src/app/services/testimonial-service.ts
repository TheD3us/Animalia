import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { testimonial } from '../interfaces/testimonial';
import { environment } from '../../environement/environment';

@Injectable({
  providedIn: 'root'
})
export class TestimonialService {
  constructor(private http: HttpClient){}
  
  getListe(){
    return this.http.get(environment.apiUrl + "testimonial");
  }

  get(id: number){
    return this.http.get(environment.apiUrl + "testimonial/" + id);
  }

  post(data: testimonial){
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

  put(data: testimonial){
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
