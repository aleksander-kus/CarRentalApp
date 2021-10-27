import { Directive, Input, OnDestroy, OnInit, TemplateRef, ViewContainerRef } from "@angular/core";
import { Role } from "./user/role.enum";
import { AuthService } from "./auth.service";
import { Subscription } from "rxjs";

@Directive({
  selector: '[appVisibleFor]'
})
export class VisibleForDirective implements OnInit, OnDestroy {
  private roles: Role[] = [];
  private hasView = false;
  private subscription: Subscription | null = null;

  constructor(
    private templateRef: TemplateRef<any>,
    private authService: AuthService,
    private viewContainer: ViewContainerRef) { }

  @Input()
  public set appVisibleFor(roles: Role[]) {
    this.roles = roles;
    this.destroySubscription();
    this.initSubscription();
  }

  public ngOnDestroy(): void {
    this.destroySubscription();
  }

  public ngOnInit(): void {
    this.destroySubscription();
    this.initSubscription();
  }

  private initSubscription(): void {
    this.subscription = this.authService.user$.subscribe(
      user => {
        if (!!user && (this.roles.length === 0)) {
          if (!this.hasView) {
            this.viewContainer.createEmbeddedView(this.templateRef);
            this.hasView = true;
          }
        } else {
          if (this.hasView) {
            this.viewContainer.clear();
            this.hasView = false;
          }
        }
      }
    );
  }

  private destroySubscription(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
