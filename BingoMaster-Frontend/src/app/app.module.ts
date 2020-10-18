import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MccColorPickerModule } from 'material-community-components';
import { API_BASE_URL, BingoCardClient, BingoGameClient } from 'src/api/api';
import { environment } from 'src/environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { GenerateBingoCardComponent } from './bingo-card-detail/generate-bingo-card/generate-bingo-card.component';
import { DownloadComponent } from './bingo-card-detail/download/download.component';
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { BingoCardDetailComponent } from './bingo-card-detail/bingo-card-detail.component';
import { PageHeaderComponent } from './page-header/page-header.component';
import { SharedModule } from './shared/shared.module';
import { MaterialModule } from './material.module';

@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    HeaderComponent,
    GenerateBingoCardComponent,
    DownloadComponent,
    HomeComponent,
    BingoCardDetailComponent,
    PageHeaderComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MccColorPickerModule,
    SharedModule,
    MaterialModule
  ],
  providers: [
    BingoCardClient,
    BingoGameClient,
    {
      provide: API_BASE_URL,
      useValue: environment.apiUrl
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
