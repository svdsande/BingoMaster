import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthenticatedUserModel, AuthenticateUserModel, RegisterUserModel, UserClient, UserModel } from 'src/api/api';

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

  get currentUserValue(): AuthenticatedUserModel {
    return this.currentUserSubject.value;
  }

  public register(model: RegisterUserModel): Observable<UserModel> {
    return this.userClient.register(model);
  }
}
