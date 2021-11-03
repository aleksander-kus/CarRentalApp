import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AuthService } from "../auth.service";
import { map, tap } from "rxjs/operators";
import { Role } from "../model/role.enum";

@Injectable()
export class EmployeeGuard implements CanActivate {

  constructor(private authService: AuthService,
              private router: Router) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.authService.user$.pipe(
      map(user => !!user && user.role == Role.Employee),
      tap(pass => {
        if (!pass) {
          this.router.navigate(['/unauthorized']);
        }
      }));
  }
}
