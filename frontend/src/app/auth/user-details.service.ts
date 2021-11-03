import { Injectable } from '@angular/core';
import { Observable, Subject } from "rxjs";
import { UserDetails } from "./model/user-details.interface";
import { HttpClient } from "@angular/common/http";
import { protectedResources } from "../../auth.config";

@Injectable()
export class UserDetailsService {
  private userSubject: Subject<UserDetails | null> = new Subject<UserDetails | null>();

  public user$: Observable<UserDetails | null> = this.userSubject.asObservable();
  constructor(private http: HttpClient) { }

  public loadUserDetails(): void {
    this.http.get<UserDetails>(protectedResources.userDetailsApi.endpoint).subscribe(
      u => this.userSubject.next(u)
    )
  }
}
