import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { BingoGameRoutingModule } from './bingo-game-routing.module';
import { BingoGameComponent } from './bingo-game.component';
import { GameSetupComponent } from './game-setup/game-setup.component';
import { SharedModule } from '../shared/shared.module';
import { CountDownComponent } from './count-down/count-down.component';
import { MaterialModule } from '../material.module';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';

@NgModule({
  declarations: [
    BingoGameComponent,
    CountDownComponent,
    GameSetupComponent,
    ConfirmDialogComponent
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
