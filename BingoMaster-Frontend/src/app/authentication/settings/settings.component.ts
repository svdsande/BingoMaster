import { Component, OnInit } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Observable, of } from 'rxjs';
import { delay, map, mergeMap, switchMap, take, tap } from 'rxjs/operators';
import { UserModel } from 'src/api/api';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {

  public user: Observable<UserModel>;
  public settingsFormGroup: FormGroup;
  public loading: boolean = false;
  private currentEmailAddress: string;
  private userId: string;
  private userName: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private userService: UserService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.buildForm();

    this.user = this.activatedRoute.paramMap.pipe(
      mergeMap((params: ParamMap) => this.userService.getUser(params.get('id'))),
      tap((user: UserModel) => {
        this.settingsFormGroup.patchValue(user);
        this.currentEmailAddress = user.emailAddress;
        this.userId = user.id;
        this.userName = user.userName;
      })
    );
  }

  public save(): void {
    this.loading = true;

    const userModel = this.getUserModel();

    this.userService.updateUser(userModel)
      .pipe(take(1))
      .subscribe(user => {
        this.loading = false;
        this.snackBar.open('Account information successfully updated', '', {
          duration: 2000
        });
      },
      error => {
        this.loading = false;
        this.snackBar.open('Account update failed', '', {
          duration: 2000
        });
      });
  }

  private getUserModel(): UserModel {
    const userModel: UserModel = new UserModel();
    userModel.id = this.userId;
    userModel.userName = this.userName;
    userModel.emailAddress = this.settingsFormGroup.get('emailAddress').value;
    userModel.firstName = this.settingsFormGroup.get('firstName').value;
    userModel.lastName = this.settingsFormGroup.get('lastName').value;

    return userModel;
  }

  private buildForm(): void {
    this.settingsFormGroup = new FormGroup({
      emailAddress: new FormControl('', [Validators.required, Validators.pattern('\\w+([\\.-]?\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+')], this.uniqueEmailAddressValidator()),
      firstName: new FormControl(''),
      middleName: new FormControl(''),
      lastName: new FormControl(''),
      playerFormGroup: new FormGroup({
        name: new FormControl(''),
        description: new FormControl('')
      })
    });
  }

  private uniqueEmailAddressValidator(): AsyncValidatorFn {
    return (control: AbstractControl) => {
      if (control.value === this.currentEmailAddress) {
        return of(null);
      }

      return of(control.value).pipe(
        delay(500),
        switchMap((emailAddress: string) => this.userService.userEmailAddressUnique(emailAddress).pipe(
          map(isUnique => isUnique ? null : { duplicateEmailAddress: true }),
        )));
    };
  }
}
