import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BingoGameComponent } from './bingo-game.component';
import { GameOverviewComponent } from './game-overview/game-overview.component';
import { GameSetupComponent } from './game-setup/game-setup.component';
import { GameComponent } from './game/game.component';

const routes: Routes = [
  {
    path: '',
    component: BingoGameComponent,
    children: [
      {
        path: '',
        component: GameOverviewComponent,
        data: { title: 'Bingo games' },
      },
      {
        path: 'setup',
        component: GameSetupComponent,
        data: { title: 'Setup' }
      },
      {
        path: 'game',
        component: GameComponent,
        data: { title: 'Game'}
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BingoGameRoutingModule { }
