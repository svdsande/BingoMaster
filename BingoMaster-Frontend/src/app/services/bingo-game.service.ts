import { Injectable } from '@angular/core';
import { HubConnectionBuilder } from '@aspnet/signalr';
import { HubConnection } from '@aspnet/signalr/dist/esm/HubConnection';
import { Observable, Subject } from 'rxjs';
import { BingoGameClient, BingoGameDetailModel, BingoGameModel, PlayerGameModel } from 'src/api/api';

@Injectable({
  providedIn: 'root'
})
export class BingoGameService {

  public nextRoundReceived: Subject<BingoGameModel> = new Subject<BingoGameModel>();

  private hubConnection: HubConnection;

  constructor(private bingoGameClient: BingoGameClient) {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  public createBingoGame(bingoGameModel: BingoGameDetailModel): Observable<BingoGameModel> {
    return this.bingoGameClient.createBingoGame(bingoGameModel);
  }

  public getAllPublicGames(): Observable<BingoGameDetailModel[]> {
    return this.bingoGameClient.getAllBingoGames();
  }

  public joinGame(gameId: string, playerId: string): Observable<void> {
    return this.bingoGameClient.joinGame(gameId, playerId);
  }

  public leaveGame(gameId: string, playerId: string): Observable<void> {
    return this.bingoGameClient.leaveGame(gameId, playerId);
  }

  public playNextRound(players: PlayerGameModel[], drawnNumber: number[]): void {
    this.hubConnection.invoke('PlayNextRound', players, drawnNumber);
  }

  private createConnection(): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5001/BingoGame')
      .build();
  }

  private startConnection(): void {
    this.hubConnection.start()
      .then(() => {
        console.log('Hub connection started');
      })
      .catch(err => {
        console.error('Error while establishing connection, retrying...', err);
        setTimeout(function () { this.startConnection(); }, 5000);
      });
  }

  private registerOnServerEvents(): void {
    this.hubConnection.on('PlayNextRound', (data: BingoGameModel) => {
      this.nextRoundReceived.next(data);
    });
  }
}
