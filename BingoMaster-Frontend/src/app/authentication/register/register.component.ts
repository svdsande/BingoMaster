import { Component, OnInit } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { stat } from 'fs';
import { Observable, of, timer } from 'rxjs';
import { delay, map, switchMap, tap } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss', '../form-styling.scss']
})
export class RegisterComponent implements OnInit {

  public registerFormGroup: FormGroup;
  public loading: boolean = false;

  constructor(private authenticateService: AuthenticationService) { }

  ngOnInit(): void {
    this.buildForm();
  }

  public register(): void {

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
        switchMap((userName: string) => this.authenticateService.userNameUnique(userName).pipe(
          map(isUnique => isUnique ? null : { duplicateUserName: true }),
        )));
    };
  }

  private uniqueEmailAddressValidator(): AsyncValidatorFn {
    return (control: AbstractControl) => {
      return of(control.value).pipe(
        delay(500),
        switchMap((emailAddress: string) => this.authenticateService.userEmailAddressUnique(emailAddress).pipe(
          map(isUnique => isUnique ? null : { duplicateEmail: true }),
        )));
    };
  }
}
