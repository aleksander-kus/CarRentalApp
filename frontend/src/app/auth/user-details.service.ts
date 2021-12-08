import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from "rxjs";
import { UserDetails } from "./model/user-details.interface";
import { HttpClient } from "@angular/common/http";
import { protectedResources } from "../../auth.config";
import { finalize } from "rxjs/operators";

@Injectable()
export class UserDetailsService {
  private userSubject: Subject<UserDetails | null> = new Subject<UserDetails | null>();
  private isLoadingSubject = new BehaviorSubject<boolean>(false);
  public isLoading$ = this.isLoadingSubject.asObservable();

  public user$: Observable<UserDetails | null> = this.userSubject.asObservable();
  constructor(private http: HttpClient) { }

  public loadUserDetails(): void {
    this.isLoadingSubject.next(true);
    this.http.get<UserDetails>(protectedResources.userDetailsApi.endpoint).pipe(finalize(() => this.isLoadingSubject.next(false))).subscribe(
      u => this.userSubject.next(u)
    )
  }
}
