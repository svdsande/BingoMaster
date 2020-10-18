import { NgModule } from '@angular/core';
import { BingoCardDetailComponent } from './bingo-card-detail.component';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    component: BingoCardDetailComponent,
    data: { title: 'Bingo cards' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BingoCardDetailRoutingModule { }
