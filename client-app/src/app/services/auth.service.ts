import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private router: Router) { }

  isSignedIn(): boolean | undefined {
    return localStorage.getItem('token') != undefined;
  }

  signIn(email: string, password: string): void {
    localStorage.setItem('token', 'hehe');
    this.router.navigate(['']);
  }

  signUp(email: string, password: string): void {
    localStorage.setItem('token', 'hehe');
    this.router.navigate(['']);
  }

  resetPassword(email: string): void { }

  signOut(): void {
    localStorage.removeItem('token');
    this.router.navigate(['account']);
  }
}
