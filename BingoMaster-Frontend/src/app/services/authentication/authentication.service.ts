import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthenticatedUserModel, AuthenticateUserModel, UserClient } from 'src/api/api';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  public currentUser: Observable<AuthenticatedUserModel>;
  private currentUserSubject: BehaviorSubject<AuthenticatedUserModel>;

  constructor(private userClient: UserClient) {
    this.currentUserSubject = new BehaviorSubject<AuthenticatedUserModel>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public login(model: AuthenticateUserModel): Observable<AuthenticatedUserModel> {
    return this.userClient.authenticate(model)
      .pipe(map(user => {
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      }));
  }

  public logout(): void {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  public userNameUnique(userName: string): Observable<boolean> {
    return this.userClient.userNameUnique(userName);
  }

  public userEmailAddressUnique(emailAddress: string): Observable<boolean> {
    return this.userClient.emailAddressUnique(emailAddress);
  }
}
