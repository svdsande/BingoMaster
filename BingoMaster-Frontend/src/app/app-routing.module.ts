import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent,
    data: { title: 'Home' }
  },
  {
    path: 'bingo-cards',
    loadChildren: () => import('./bingo-card-detail/bingo-card-detail.module').then(m => m.BingoCardDetailModule)
  },
  {
    path: 'bingo-game',
    loadChildren: () => import('./bingo-game/bingo-game.module').then(m => m.BingoGameModule)
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
