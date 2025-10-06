import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environement/environment';
import { Training } from '../interfaces/training';

@Injectable({
  providedIn: 'root'
})
export class TrainingService {
  constructor(private http: HttpClient){}
  
  getListe(){
    return this.http.get(environment.apiUrl + "training");
  }

  get(id: number){
    return this.http.get(environment.apiUrl + "training/" + id);
  }

  post(data: Training){
    const body = JSON.stringify(data);

    this.http.post(environment.apiUrl + "training", body, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).
      subscribe(response => {

        console.log("crud service post training OK");

      });
  }

  delete(id: number){
    this.http.delete(environment.apiUrl + "training/" + id).subscribe(response => {

        console.log("crud service delete training OK");

      });
  }

  put(data: Training){
    const body = JSON.stringify(data);
    this.http.put(environment.apiUrl + "training", body, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {

        console.log("crud service put training OK");

      });
  }
}
