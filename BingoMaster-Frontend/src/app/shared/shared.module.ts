import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { SpinnerButtonComponent } from './components/spinner-button/spinner-button.component';



@NgModule({
  declarations: [
    SpinnerButtonComponent
  ],
  imports: [
    CommonModule,
    MatProgressSpinnerModule
  ],
  exports: [
    SpinnerButtonComponent
  ]
})
export class SharedModule { }
