import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BingoGameComponent } from './bingo-game.component';

const routes: Routes = [
  {
    path: '',
    component: BingoGameComponent,
    data: { title: 'Bingo game' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BingoGameRoutingModule { }
