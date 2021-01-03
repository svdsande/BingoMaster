import { Component, OnInit } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { of } from 'rxjs';
import { delay, map, switchMap, take } from 'rxjs/operators';
import { RegisterUserModel } from 'src/api/api';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss', '../form-styling.scss']
})
export class RegisterComponent implements OnInit {

  public registerFormGroup: FormGroup;
  public loading: boolean = false;

  constructor(
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.buildForm();
  }

  public register(): void {
    this.loading = true;

    const registerUserModel: RegisterUserModel = new RegisterUserModel();
    registerUserModel.userName = this.registerFormGroup.get('userName').value;
    registerUserModel.emailAddress = this.registerFormGroup.get('email').value;
    registerUserModel.password = this.registerFormGroup.get('password').value;

    this.authenticationService.register(registerUserModel)
      .pipe(take(1))
      .subscribe(user => {
        this.loading = false;
        this.snackBar.open('Account successfully created for ' + user.userName, '', {
          duration: 2000
        });
      },
      error => {
        this.loading = false;
        this.snackBar.open('Account registration failed', '', {
          duration: 2000
        });
      });
  }

  private buildForm(): void {
    this.registerFormGroup = new FormGroup({
      userName: new FormControl('', [Validators.required, Validators.minLength(3)], this.uniqueUsernameValidator()),
      email: new FormControl('', [Validators.required, Validators.pattern('\\w+([\\.-]?\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+')], this.uniqueEmailAddressValidator()),
      password: new FormControl('', [Validators.required, Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&].{8,}')])
    });
  }

  private uniqueUsernameValidator(): AsyncValidatorFn {
    return (control: AbstractControl) => {
      return of(control.value).pipe(
        delay(500),
        switchMap((userName: string) => this.userService.userNameUnique(userName).pipe(
          map(isUnique => isUnique ? null : { duplicateUserName: true }),
        )));
    };
  }

  private uniqueEmailAddressValidator(): AsyncValidatorFn {
    return (control: AbstractControl) => {
      return of(control.value).pipe(
        delay(500),
        switchMap((emailAddress: string) => this.userService.userEmailAddressUnique(emailAddress).pipe(
          map(isUnique => isUnique ? null : { duplicateEmailAddress: true }),
        )));
    };
  }
}
