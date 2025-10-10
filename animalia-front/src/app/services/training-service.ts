import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environement/environment';
import { Training } from '../interfaces/training';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TrainingService {
  constructor(private http: HttpClient){}
  
  getListe() : Observable<Training[]>{
    return this.http.get<Training[]>(environment.apiUrl + "training");
  }

  get(id: number): Observable<Training>{
    return this.http.get<Training>(environment.apiUrl + "training/" + id);
  }

  post(data: Training) {
    const body = JSON.stringify(data);
    this.http.post(environment.apiUrl + "training", body, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    }).subscribe(() => {
      console.log("crud service post training OK - Entraînement créé");
    });
  }

  delete(id: number) {
    this.http.delete(environment.apiUrl + "training/" + id).subscribe(() => {
      console.log("crud service delete training OK");
    });
  }

  put(data: Training) {
    const body = JSON.stringify(data);
    this.http.put(environment.apiUrl + "training", body, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    }).subscribe(() => {
      console.log("crud service put training OK");
    });
  }

  getByUser(id: number){
    return this.http.get(environment.apiUrl + "training/getuser/" + id)
  }
}
