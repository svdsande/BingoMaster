import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BingoCardComponent } from './components/bingo-card/bingo-card.component';
import { SpinnerButtonComponent } from './components/spinner-button/spinner-button.component';



@NgModule({
  declarations: [
    BingoCardComponent,
    SpinnerButtonComponent
  ],
  imports: [
    CommonModule,
    MatProgressSpinnerModule
  ],
  exports: [
    BingoCardComponent,
    SpinnerButtonComponent
  ]
})
export class SharedModule { }
