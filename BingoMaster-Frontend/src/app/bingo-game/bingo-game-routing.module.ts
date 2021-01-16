import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BingoGameComponent } from './bingo-game.component';
import { GameSetupComponent } from './game-setup/game-setup.component';
import { GameComponent } from './game/game.component';

const routes: Routes = [
  {
    path: '',
    component: BingoGameComponent,
    data: { title: 'Bingo games' },
    children: [
      {
        path: 'setup',
        component: GameSetupComponent,
        data: { title: 'Setup' }
      },
      {
        path: 'game/:id',
        component: GameComponent,
        data: { title: 'Game'}
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BingoGameRoutingModule { }
