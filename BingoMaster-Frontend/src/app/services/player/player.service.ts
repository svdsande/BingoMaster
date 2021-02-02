import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BingoGameDetailModel, PlayerClient, PlayerModel } from 'src/api/api';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  constructor(private playerClient: PlayerClient) { }

  public getPlayer(id: string): Observable<PlayerModel> {
    return this.playerClient.getPlayer(id);
  }

  public updatePlayer(playerModel: PlayerModel) {
    return this.playerClient.updatePlayer(playerModel);
  }

  public getGamesForPlayer(id: string): Observable<BingoGameDetailModel[]> {
    return this.playerClient.gamesForPlayer(id);
  }
}
