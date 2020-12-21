import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { AuthenticateUserModel } from 'src/api/api';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss', '../form-styling.scss'],
})
export class LoginComponent implements OnInit {

  public loginFormGroup: FormGroup;
  public loading: boolean = false;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.buildForm();
  }

  public authenticate(): void {
    this.loading = true;

    const authenticateUserModel: AuthenticateUserModel = new AuthenticateUserModel();
    authenticateUserModel.emailAddress = this.loginFormGroup.get('email').value;
    authenticateUserModel.password = this.loginFormGroup.get('password').value;

    this.authenticationService.login(authenticateUserModel)
      .pipe(take(1))
      .subscribe(user => {
        console.log(user);
        this.loading = false;
      },
      error => {
        console.error(error);
      });
  }

  private buildForm(): void {
    this.loginFormGroup = new FormGroup({
      email: new FormControl('', Validators.required),
      password: new FormControl('' ,Validators.required),
    });
  }
}
