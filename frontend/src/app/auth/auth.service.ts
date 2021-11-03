import { Inject, Injectable } from "@angular/core";
import { MSAL_GUARD_CONFIG, MsalBroadcastService, MsalGuardConfiguration, MsalService } from "@azure/msal-angular";
import { Observable, Subject } from "rxjs";
import { InteractionStatus, RedirectRequest } from "@azure/msal-browser";
import { filter, takeUntil } from "rxjs/operators";
import { User } from "./model/user.interface";
import { TokenClaims } from "./model/token-claims.interface";
import { Role } from "./model/role.enum";
import { UserDetailsService } from "./user-details.service";

@Injectable()
export class AuthService {
  private _destroying$ = new Subject<void>();
  private userSubject: Subject<User | null> = new Subject<User | null>();

  public user$: Observable<User | null> = this.userSubject.asObservable();

  constructor(@Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
              private userDetailsService: UserDetailsService,
              private msalService: MsalService,
              private msalBroadcastService: MsalBroadcastService) { }

  public login(): void {
    if (this.msalGuardConfig.authRequest){
      this.msalService.loginRedirect(this.msalGuardConfig.authRequest as RedirectRequest);
    } else {
      this.msalService.loginRedirect();
    }
  }

  public logout(): void {
    this.msalService.logout();
  }

  public init() {
    this.msalBroadcastService.inProgress$
      .pipe(
        filter((status: InteractionStatus) => status === InteractionStatus.None),
        takeUntil(this._destroying$)
      )
      .subscribe(() => {
        this.refresh();
      });
  }

  public destroy(): void {
    this._destroying$.next(undefined);
    this._destroying$.complete();
  }

  private refresh(): void {
    if (this.msalService.instance.getAllAccounts().length > 0) {
      const account = this.msalService.instance.getAllAccounts()[0];
      const tokenClaims = account.idTokenClaims as TokenClaims;

      const user: User = {
        familyName: tokenClaims.family_name,
        givenName: tokenClaims.given_name,
        role: Role[tokenClaims.extension_Role as keyof typeof Role]
      };

      this.userDetailsService.loadUserDetails();
      this.userSubject.next(user);
    } else {
      this.userSubject.next(null);
    }
  }
}
