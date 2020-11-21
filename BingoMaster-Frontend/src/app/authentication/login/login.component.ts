import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public loginFormGroup: FormGroup;

  constructor(
    private router: Router,
    private authenticationSerivce: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.buildForm();
  }

  public authenticate(): void {
    const email = this.loginFormGroup.get('email').value;
    const password = this.loginFormGroup.get('password').value;
  }

  private buildForm(): void {
    this.loginFormGroup = new FormGroup({
      email: new FormControl('', Validators.required),
      password: new FormControl('' ,Validators.required),
    });
  }
}
