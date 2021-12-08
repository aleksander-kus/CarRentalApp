import { Component, OnDestroy, OnInit } from "@angular/core";
import { AuthService } from "../../auth.service";
import { UserDetailsService } from "../../user-details.service";

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit, OnDestroy {
  user$ = this.userDetailsService.user$;
  loading$ = this.userDetailsService.isLoading$;

  constructor(
    private authService: AuthService,
    private userDetailsService: UserDetailsService) { }

  public login(): void {
    this.authService.login();
  }

  public logout(): void {
    this.authService.logout();
  }

  public ngOnDestroy(): void {
    this.authService.destroy();
  }

  public ngOnInit(): void {
    this.authService.init();
  }
}
