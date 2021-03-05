import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BingoGameDetailModel } from 'src/api/api';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { PlayerService } from 'src/app/services/player/player.service';

@Component({
  selector: 'app-game-overview',
  templateUrl: './game-overview.component.html',
  styleUrls: ['./game-overview.component.scss']
})
export class GameOverviewComponent implements OnInit {

  public gamesForCurrentPlayer: Observable<BingoGameDetailModel[]>;
  public playerId: string;

  constructor(
    private playerService: PlayerService,
    private authenticationService: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.playerId = this.authenticationService.currentUserValue.playerId;
    this.gamesForCurrentPlayer = this.playerService.getGamesForPlayer(this.playerId);
  }

  public leave(gameId: string): void {

  }

  public join(gameId: string): void {

  }
}
