import { Inject, Injectable } from "@angular/core";
import { MSAL_GUARD_CONFIG, MsalBroadcastService, MsalGuardConfiguration, MsalService } from "@azure/msal-angular";
import { Observable, Subject } from "rxjs";
import { InteractionStatus, RedirectRequest } from "@azure/msal-browser";
import { filter, takeUntil } from "rxjs/operators";
import { User } from "./user/user.interface";

@Injectable()
export class AuthService {
  private _destroying$ = new Subject<void>();
  private userSubject: Subject<User | null> = new Subject<User | null>();

  public user$: Observable<User | null> = this.userSubject.asObservable();

  constructor(@Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
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

      console.log(account);

      const user: User = {
        username: 'aaaa'
      };

      this.userSubject.next(user);
    } else {
      this.userSubject.next(null);
    }
  }
}
