import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
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
  public returnUrl: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.buildForm();

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  public authenticate(): void {
    this.loading = true;

    const authenticateUserModel: AuthenticateUserModel = new AuthenticateUserModel();
    authenticateUserModel.emailAddress = this.loginFormGroup.get('email').value;
    authenticateUserModel.password = this.loginFormGroup.get('password').value;

    this.authenticationService.login(authenticateUserModel)
      .pipe(take(1))
      .subscribe(user => {
        this.loading = false;
        this.router.navigateByUrl(this.returnUrl);
      },
      error => {
        this.loading = false;
        this.snackBar.open('Login failed, check email and password and try again', '', {
          duration: 2000
        });
      });
  }

  private buildForm(): void {
    this.loginFormGroup = new FormGroup({
      email: new FormControl('', Validators.required),
      password: new FormControl('' ,Validators.required),
    });
  }
}
