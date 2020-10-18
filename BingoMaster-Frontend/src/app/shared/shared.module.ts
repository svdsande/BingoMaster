import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BingoCardComponent } from './components/bingo-card/bingo-card.component';
import { SpinnerButtonComponent } from './components/spinner-button/spinner-button.component';
import { CounterDirective } from './directives/counter.directive';
import { PageHeaderComponent } from './components/page-header/page-header.component';

@NgModule({
  declarations: [
    BingoCardComponent,
    CounterDirective,
    PageHeaderComponent,
    SpinnerButtonComponent
  ],
  imports: [
    CommonModule,
    MatProgressSpinnerModule
  ],
  exports: [
    BingoCardComponent,
    CounterDirective,
    PageHeaderComponent,
    SpinnerButtonComponent
  ]
})
export class SharedModule { }
