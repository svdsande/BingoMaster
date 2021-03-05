import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { BingoGameRoutingModule } from './bingo-game-routing.module';
import { BingoGameComponent } from './bingo-game.component';
import { GameSetupComponent } from './game-setup/game-setup.component';
import { SharedModule } from '../shared/shared.module';
import { CountDownComponent } from './game/count-down/count-down.component';
import { MaterialModule } from '../material.module';
import { ConfirmDialogComponent } from './game/confirm-dialog/confirm-dialog.component';
import { GameControlsComponent } from './game/game-controls/game-controls.component';
import { WinnerDialogComponent } from './game/winner-dialog/winner-dialog.component';
import { GameComponent } from './game/game.component';
import { GameOverviewComponent } from './game-overview/game-overview.component';

@NgModule({
  declarations: [
    BingoGameComponent,
    CountDownComponent,
    GameSetupComponent,
    ConfirmDialogComponent,
    GameControlsComponent,
    WinnerDialogComponent,
    GameComponent,
    GameOverviewComponent
  ],
  imports: [
    CommonModule,
    BingoGameRoutingModule,
    ReactiveFormsModule,
    MaterialModule,
    SharedModule
  ]
})
export class BingoGameModule { }
