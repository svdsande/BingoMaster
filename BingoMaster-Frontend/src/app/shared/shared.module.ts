import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BingoCardComponent } from './components/bingo-card/bingo-card.component';
import { SpinnerButtonComponent } from './components/spinner-button/spinner-button.component';
import { CounterDirective } from './directives/counter.directive';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { MaterialModule } from '../material.module';
import { TranslateModule } from '@ngx-translate/core';
import { PlayerInGamePipe } from './pipes/player-in-game.pipe';

@NgModule({
  declarations: [
    BingoCardComponent,
    CounterDirective,
    PageHeaderComponent,
    SpinnerButtonComponent,
    PlayerInGamePipe
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
    TranslateModule,
    PlayerInGamePipe
  ]
})
export class SharedModule { }
