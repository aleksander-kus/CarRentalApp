import { Component, OnInit } from '@angular/core';
import { AuthService } from "../../auth.service";
import { Router } from "@angular/router";
import { map } from "rxjs/operators";

@Component({
  selector: 'app-unauthorized-page',
  templateUrl: './unauthorized-page.component.html',
  styleUrls: ['./unauthorized-page.component.css']
})
export class UnauthorizedPageComponent {
  isLogged$ = this.authService.user$.pipe(map(u => !!u));

  constructor(private authService: AuthService,
              private router: Router) {
  }

  public logout(): void {
    this.authService.logout();
  }

  public login(): void {
    this.authService.login();
  }

  public home(): void {
    this.router.navigate(['/']);
  }
}
