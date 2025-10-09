import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environement/environment';
import { Observable } from 'rxjs';
import { ProgramModel } from '../interfaces/program-models';


@Injectable({
  providedIn: 'root'
})
export class ProgramModelService {
  constructor(private http: HttpClient) { }

  getListe(): Observable<ProgramModel[]> {
    return this.http.get<ProgramModel[]>(environment.apiUrl + "programmodel");
  }

  get(id: number) {
    return this.http.get(environment.apiUrl + "programmodel/" + id);
  }

  post(data: ProgramModel) {
    const body = JSON.stringify(data);

    this.http.post(environment.apiUrl + "programmodel", body, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      console.log("crud service post programmodel OK");
    });
  }

  delete(id: number) {
    this.http.delete(environment.apiUrl + "programmodel/" + id).subscribe(response => {
      console.log("crud service delete programmodel OK");
    });
  }

  put(data: ProgramModel) {
    const body = JSON.stringify(data);

    this.http.put(environment.apiUrl + "programmodel", body, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      console.log("crud service put programmodel OK");
    });
  }
}
;

