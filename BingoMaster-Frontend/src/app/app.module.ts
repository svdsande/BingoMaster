import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatRippleModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatToolbarModule } from '@angular/material/toolbar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MccColorPickerModule } from 'material-community-components';
import { API_BASE_URL, BingoCardClient, BingoGameClient } from 'src/api/api';
import { environment } from 'src/environments/environment';
import { AppRoutingModule } from './app-routing/app-routing.module';
import { AppComponent } from './app.component';
import { GenerateBingoCardComponent } from './bingo-card-detail/generate-bingo-card/generate-bingo-card.component';
import { DownloadComponent } from './bingo-card-detail/download/download.component';
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { BingoCardDetailComponent } from './bingo-card-detail/bingo-card-detail.component';
import { PageHeaderComponent } from './page-header/page-header.component';
import { BingoGameComponent } from './bingo-game/bingo-game.component';
import { GameSetupComponent } from './bingo-game/game-setup/game-setup.component';
import { SharedModule } from './shared/shared.module';
import { CounterDirective } from './directives/counter.directive';
import { CountDownComponent } from './bingo-game/count-down/count-down.component';

@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    HeaderComponent,
    GenerateBingoCardComponent,
    DownloadComponent,
    HomeComponent,
    BingoCardDetailComponent,
    PageHeaderComponent,
    BingoGameComponent,
    GameSetupComponent,
    CounterDirective,
    CountDownComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatRippleModule,
    MatSelectModule,
    MatToolbarModule,
    MccColorPickerModule,
    SharedModule
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
