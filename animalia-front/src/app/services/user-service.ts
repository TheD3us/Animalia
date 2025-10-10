import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environement/environment';
import { user } from '../interfaces/user';
import { Observable } from 'rxjs';
import { Event } from '../interfaces/events';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient){}
  
  getListe(){
    return this.http.get(environment.apiUrl + "user");
  }

  get(id: number){
    return this.http.get(environment.apiUrl + "user/" + id);
  }

  post(data: user){
    const body = JSON.stringify(data);

    this.http.post(environment.apiUrl + "user", body, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).
      subscribe(response => {

        console.log("crud service post user OK");

      });
  }

  delete(id: number){
    this.http.delete(environment.apiUrl + "user/" + id).subscribe(response => {

        console.log("crud service delete user OK");

      });
  }

  put(data: user){
    const body = JSON.stringify(data);
    this.http.put(environment.apiUrl + "user", body, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {

        console.log("crud service put user OK");

      });
  }

  //verifLogin(login: string, mdp: string): Observable<number>{
  //  return this.http.get<number>(`${environment.apiUrl}user/verif`, {params:{login, mdp}});
  //}
  verifLogin(login: string, mdp: string): Observable<user> {
    return this.http.get<user>(`${environment.apiUrl}user/verif`, {
      params: { login, mdp }
    });
  }


  selectEventParticipation(id: number): Observable<Event[]>{
    return this.http.get<Event[]>(environment.apiUrl + "user/events/" + id)
  }

  eventSubscription(userId: number, eventId: number){
    this.http.post(`${environment.apiUrl}user/subscribe/`+ userId + "/events/" + eventId, {}, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      console.log("crud service post event subscription OK");
    });
  }


}

