import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BingoCardComponent } from './components/bingo-card/bingo-card.component';
import { SpinnerButtonComponent } from './components/spinner-button/spinner-button.component';
import { CounterDirective } from './directives/counter.directive';

@NgModule({
  declarations: [
    BingoCardComponent,
    SpinnerButtonComponent,
    CounterDirective
  ],
  imports: [
    CommonModule,
    MatProgressSpinnerModule
  ],
  exports: [
    BingoCardComponent,
    SpinnerButtonComponent,
    CounterDirective
  ]
})
export class SharedModule { }
