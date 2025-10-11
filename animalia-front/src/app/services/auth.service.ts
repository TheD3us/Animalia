import { Injectable } from '@angular/core';
import { BehaviorSubject, fromEvent, merge, Observable, of, timer } from 'rxjs';
import { debounceTime, switchMap, tap } from 'rxjs/operators';
import { user } from '../interfaces/user';
import { UserService } from './user-service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  private sessionTimer: any;
  private UserId: Observable<number> = of(-1);
  private readonly SESSION_TIMEOUT = 60 * 60 * 1000; // 1 heure en millisecondes
  private currentUser: user | null = null;

  isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(private userService: UserService) {
    this.initActivityMonitoring();
  }

  login(credentials: any): Observable<boolean> {
    return this.validateCredentials(credentials).pipe(
      tap(user => {
        console.log('User:', user);
        if (user) {
          this.currentUser = user;
          this.isLoggedInSubject.next(true);
          this.startSessionTimer();
          //sessionStorage.setItem('userId', user.id.toString());
          //sessionStorage.setItem('isAdmin', user.isAdmin ? 'true' : 'false');
          this.saveSession(user);
        } else {
          this.isLoggedInSubject.next(false);
        }
      }),
      switchMap(user => of(!!user))
    );
  }

  logout() {
    this.isLoggedInSubject.next(false);
    this.clearSessionTimer();
    this.clearSession();
  }


  private validateCredentials(credentials: any): Observable<user> {
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

  private saveSession(user: user) {
    sessionStorage.setItem('isLoggedIn', 'true');
    sessionStorage.setItem('loginTime', Date.now().toString());
    sessionStorage.setItem('userId', user.Id.toString());
    sessionStorage.setItem('isAdmin', user.IsAdmin ? 'true' : 'false');
  }

  private clearSession() {
  sessionStorage.removeItem('isLoggedIn');
  sessionStorage.removeItem('loginTime');
  sessionStorage.removeItem('userId');
  sessionStorage.removeItem('isAdmin');
  }

  checkExistingSession() {
    const isLoggedIn = sessionStorage.getItem('isLoggedIn');
    const loginTime = sessionStorage.getItem('loginTime');
    const userId = sessionStorage.getItem('userId');
    const IsAdmin = sessionStorage.getItem('isAdmin') === 'true';

    if (isLoggedIn === 'true' && loginTime && userId) {
      const timeElapsed = Date.now() - parseInt(loginTime);
      if (timeElapsed < this.SESSION_TIMEOUT) {
        this.isLoggedInSubject.next(true);
        this.UserId = of(parseInt(userId));
        this.currentUser = {
          Id: parseInt(userId),
          Email: '',
          Prenom: '',
          Nom: '',
          Password: '',
          IsAdmin
        };
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

  whoIsLoggedIn(): user | null{
    return this.currentUser;
  }

  isLoggedIn(): boolean {
    return this.isLoggedInSubject.value;
  }

  isAdmin(): boolean {
    return sessionStorage.getItem('isAdmin') === 'true';
  }

}
