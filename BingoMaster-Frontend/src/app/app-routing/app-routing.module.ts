import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BingoCardComponent } from '../bingo-card/bingo-card.component';

const routes: Routes = [
  {
    path: '',
    component: BingoCardComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [ RouterModule ]
})
export class AppRoutingModule { }
