import { Component, OnInit } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { forkJoin, Observable, of } from 'rxjs';
import { delay, map, mergeMap, switchMap, tap } from 'rxjs/operators';
import { PlayerModel, UserModel } from 'src/api/api';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { PlayerService } from 'src/app/services/player/player.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {

  public settings: Observable<[UserModel, PlayerModel]>;
  public settingsFormGroup: FormGroup;
  public loading: boolean = false;
  private currentEmailAddress: string;
  private userId: string;
  private userName: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private userService: UserService,
    private playerService: PlayerService,
    private snackBar: MatSnackBar,
    private authenticationService: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.buildForm();

    this.settings = this.activatedRoute.paramMap.pipe(
      mergeMap((params: ParamMap) => {
        const user = this.userService.getUser(params.get('id'));
        const player = this.playerService.getPlayer(this.authenticationService.currentUserValue.playerId);

        return forkJoin([user, player]);
      }),
      tap((result: [UserModel, PlayerModel]) => {
        this.settingsFormGroup.patchValue(result[0]);
        this.settingsFormGroup.get('player').patchValue(result[1]);
        this.currentEmailAddress = result[0].emailAddress;
        this.userId = result[0].id;
        this.userName = result[0].userName;
      })
    );
  }

  public save(): void {
    this.loading = true;
    const userModel = this.getUserModel();
    const playerModel = this.getPlayerModel();

    forkJoin({
      user: this.userService.updateUser(userModel),
      player: this.playerService.updatePlayer(playerModel)
    }).subscribe(() => {
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

  private getPlayerModel(): PlayerModel {
    const playerModel: PlayerModel = new PlayerModel();
    playerModel.id = this.authenticationService.currentUserValue.playerId;
    playerModel.name = this.settingsFormGroup.get('player.name').value;
    playerModel.description = this.settingsFormGroup.get('player.description').value;

    return playerModel;
  }

  private buildForm(): void {
    this.settingsFormGroup = new FormGroup({
      emailAddress: new FormControl('', [Validators.required, Validators.pattern('\\w+([\\.-]?\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+')], this.uniqueEmailAddressValidator()),
      firstName: new FormControl(''),
      middleName: new FormControl(''),
      lastName: new FormControl(''),
      player: new FormGroup({
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
