import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from '../home/home.component';
import { BingoCardDetailComponent } from '../bingo-card-detail/bingo-card-detail.component';
import { BingoGameComponent } from '../bingo-game/bingo-game.component';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent,
    data: { title: 'Home' }
  },
  {
    path: 'bingo-cards',
    component: BingoCardDetailComponent,
    data: { title: 'Bingo cards' }
  },
  {
    path: 'bingo-game',
    component: BingoGameComponent,
    data: { title: 'Bingo game' }
  },
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'top' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
