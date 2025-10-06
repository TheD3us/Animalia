import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environement/environment';
import { Event } from '../interfaces/events';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  constructor(private http: HttpClient) { }

  getListe() {
    return this.http.get(environment.apiUrl + "event");
  }

  get(id: number) {
    return this.http.get(environment.apiUrl + "event/" + id);
  }

  post(data: Event) {
    const body = JSON.stringify(data);
    this.http.post(environment.apiUrl + "event", body, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    }).subscribe(() => console.log("crud service post event OK - Événement créé"));
  }


  delete(id: number) {
    this.http.delete(environment.apiUrl + "event/" + id).subscribe(response => {
      console.log("crud service delete event OK");
    });
  }

  put(data: Event) {
    const body = JSON.stringify(data);

    this.http.put(environment.apiUrl + "event", body, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      console.log("crud service put event OK");
    });
  }


}
