import { Component, OnDestroy, OnInit } from "@angular/core";
import { AuthService } from "../auth.service";
import { MsalBroadcastService } from "@azure/msal-angular";

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit, OnDestroy {
  user$ = this.authService.user$;

  constructor(
    private authService: AuthService) { }

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
