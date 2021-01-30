import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BingoGameDetailModel, PlayerClient } from 'src/api/api';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  constructor(private playerClient: PlayerClient) { }

  public getGamesForPlayer(id: string): Observable<BingoGameDetailModel[]> {
    return this.playerClient.gamesForPlayer(id);
  }
}
