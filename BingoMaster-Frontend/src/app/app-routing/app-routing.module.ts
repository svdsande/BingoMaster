import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BingoCardComponent } from '../bingo-card/bingo-card.component';
import { HomeComponent } from '../home/home.component';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'bingo-cards',
    component: BingoCardComponent
  },
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
