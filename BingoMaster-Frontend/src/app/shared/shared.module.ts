import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BingoCardComponent } from './components/bingo-card/bingo-card.component';
import { SpinnerButtonComponent } from './components/spinner-button/spinner-button.component';
import { CounterDirective } from './directives/counter.directive';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { MaterialModule } from '../material.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    BingoCardComponent,
    CounterDirective,
    PageHeaderComponent,
    SpinnerButtonComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    TranslateModule
  ],
  exports: [
    BingoCardComponent,
    CounterDirective,
    PageHeaderComponent,
    SpinnerButtonComponent,
    TranslateModule
  ]
})
export class SharedModule { }
