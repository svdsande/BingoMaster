import { Component, OnInit } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { of } from 'rxjs';
import { delay, map, switchMap, take } from 'rxjs/operators';
import { RegisterUserModel } from 'src/api/api';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { PlayerService } from 'src/app/services/player/player.service';
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
    private playerService: PlayerService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.buildForm();
  }

  public register(): void {
    this.loading = true;

    const registerUserModel: RegisterUserModel = new RegisterUserModel();
    registerUserModel.playerName = this.registerFormGroup.get('playerName').value;
    registerUserModel.emailAddress = this.registerFormGroup.get('email').value;
    registerUserModel.password = this.registerFormGroup.get('password').value;

    this.authenticationService.register(registerUserModel)
      .pipe(take(1))
      .subscribe(user => {
        this.loading = false;
        this.snackBar.open('Account successfully created for ' + user.playerName, '', {
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
      playerName: new FormControl('', [Validators.required, Validators.minLength(3)], this.uniquePlayerNameValidator()),
      email: new FormControl('', [Validators.required, Validators.pattern('\\w+([\\.-]?\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+')], this.uniqueEmailAddressValidator()),
      password: new FormControl('', [Validators.required, Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&].{8,}')])
    });
  }

  private uniquePlayerNameValidator(): AsyncValidatorFn {
    return (control: AbstractControl) => {
      return of(control.value).pipe(
        delay(500),
        switchMap((playerName: string) => this.playerService.playerNameUnique(playerName).pipe(
          map(isUnique => isUnique ? null : { duplicatePlayerName: true }),
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
