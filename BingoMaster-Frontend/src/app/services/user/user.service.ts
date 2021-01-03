import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserClient, UserModel } from 'src/api/api';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private userClient: UserClient) { }

  public getUser(id: string): Observable<UserModel> {
    return this.userClient.getUser(id);
  }

  public updateUser(userModel: UserModel) {
    return this.userClient.updateUser(userModel);
  }

  public userNameUnique(userName: string): Observable<boolean> {
    return this.userClient.userNameUnique(userName);
  }

  public userEmailAddressUnique(emailAddress: string): Observable<boolean> {
    return this.userClient.emailAddressUnique(emailAddress);
  }
}
