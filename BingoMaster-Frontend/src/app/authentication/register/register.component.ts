import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss', '../form-styling.scss']
})
export class RegisterComponent implements OnInit {

  public registerFormGroup: FormGroup;
  public loading: boolean = false;

  constructor() { }

  ngOnInit(): void {
    this.buildForm();
  }

  public register(): void {

  }

  private buildForm(): void {
    this.registerFormGroup = new FormGroup({
      email: new FormControl('', Validators.required),
      userName: new FormControl('', Validators.required),
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      password: new FormControl('' ,Validators.required),
      confirmPassword: new FormControl('' ,Validators.required)
    });
  }
}
