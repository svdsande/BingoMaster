import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public loginFormGroup: FormGroup;

  constructor() { }

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
