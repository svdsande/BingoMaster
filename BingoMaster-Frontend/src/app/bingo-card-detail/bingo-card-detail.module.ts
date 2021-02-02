import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { MaterialModule } from '../material.module';
import { BingoCardDetailRoutingModule } from './bingo-card-detail-routing.module';
import { BingoCardDetailComponent } from './bingo-card-detail.component';
import { GenerateBingoCardComponent } from './generate-bingo-card/generate-bingo-card.component';
import { DownloadComponent } from './download/download.component';
import { MccColorPickerModule } from 'material-community-components/color-picker';

@NgModule({
  declarations: [
    DownloadComponent,
    GenerateBingoCardComponent,
    BingoCardDetailComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    BingoCardDetailRoutingModule,
    MaterialModule,
    SharedModule,
    MccColorPickerModule
  ]
})
export class BingoCardDetailModule { }
