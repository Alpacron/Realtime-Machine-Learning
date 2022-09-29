import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const isSignedIn = this.authService.isSignedIn();

    if (route.url.length > 0 && route.url[0].path === 'account' && isSignedIn) {
      this.router.navigate(['']);
      return false;
    }

    else if ((route.url.length === 0 || route.url[0].path !== 'account') && !isSignedIn) {
      this.router.navigate(['account']);
      return false;
    }

    return true;
  }

}
