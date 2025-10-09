import { Injectable } from '@angular/core';
import { BehaviorSubject, fromEvent, merge, Observable, of, timer } from 'rxjs';
import { debounceTime, switchMap, tap } from 'rxjs/operators';
import { UserService } from './user-service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  private sessionTimer: any;
  private UserId: Observable<number> = of(-1);
  private readonly SESSION_TIMEOUT = 60 * 60 * 1000; // 1 heure en millisecondes

  isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(private userService: UserService) {
    this.initActivityMonitoring();
  }

  login(credentials: any): boolean {
    this.UserId = this.validateCredentials(credentials);
    
    if (this.UserId != null) {
      this.isLoggedInSubject.next(true);
      this.startSessionTimer();
      this.saveSession();
      return true;
    }
    return false;
  }

  logout() {
    this.isLoggedInSubject.next(false);
    this.clearSessionTimer();
    this.clearSession();
  }

  private validateCredentials(credentials: any): Observable<number> {
    return this.userService.verifLogin(credentials.username, credentials.password);
  }

  private startSessionTimer() {
    this.clearSessionTimer();
    this.sessionTimer = setTimeout(() => {
      this.logout();
      alert('Session expirée. Vous avez été déconnecté après 1 heure d\'inactivité.');
    }, this.SESSION_TIMEOUT);
  }

  private clearSessionTimer() {
    if (this.sessionTimer) {
      clearTimeout(this.sessionTimer);
      this.sessionTimer = null;
    }
  }

  private initActivityMonitoring() {
    const activity$ = merge(
      fromEvent(document, 'mousedown'),
      fromEvent(document, 'mousemove'),
      fromEvent(document, 'keypress'),
      fromEvent(document, 'scroll'),
      fromEvent(document, 'touchstart')
    );

    activity$.pipe(
      debounceTime(1000)
    ).subscribe(() => {
      if (this.isLoggedInSubject.value) {
        this.resetSessionTimer();
      }
    });
  }

  private resetSessionTimer() {
    if (this.isLoggedInSubject.value) {
      this.startSessionTimer();
    }
  }

  private saveSession() {
    sessionStorage.setItem('isLoggedIn', 'true');
    sessionStorage.setItem('loginTime', Date.now().toString());
  }

  private clearSession() {
    sessionStorage.removeItem('isLoggedIn');
    sessionStorage.removeItem('loginTime');
  }

  checkExistingSession() {
    const isLoggedIn = sessionStorage.getItem('isLoggedIn');
    const loginTime = sessionStorage.getItem('loginTime');
    
    if (isLoggedIn === 'true' && loginTime) {
      const timeElapsed = Date.now() - parseInt(loginTime);
      
      if (timeElapsed < this.SESSION_TIMEOUT) {
        this.isLoggedInSubject.next(true);
        const remainingTime = this.SESSION_TIMEOUT - timeElapsed;
        this.sessionTimer = setTimeout(() => {
          this.logout();
          alert('Session expirée. Vous avez été déconnecté après 1 heure d\'inactivité.');
        }, remainingTime);
      } else {
        this.clearSession();
      }
    }
  }

  whoIsLoggedIn(): Observable<number>{
    if(this.UserId != undefined)
    {
      return this.UserId;
    }
    else
    {
      return of(-1);
    }
    
  }

  isLoggedIn(): boolean {
    return this.isLoggedInSubject.value;
  }
}