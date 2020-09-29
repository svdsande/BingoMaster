import { Injectable } from '@angular/core';
import { HubConnectionBuilder } from '@aspnet/signalr';
import { HubConnection } from '@aspnet/signalr/dist/esm/HubConnection';
import { Observable, Subject } from 'rxjs';
import { BingoCardModel, BingoGameClient } from 'src/api/api';

@Injectable({
  providedIn: 'root'
})
export class BingoGameService {

  public nextNumberReceived: Subject<number> = new Subject<number>();

  private hubConnection: HubConnection;

  constructor(private bingoGameClient: BingoGameClient) {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  public createBingoGame(name: string, amountOfPlayers: number, size: number): Observable<BingoCardModel[]> {
    return this.bingoGameClient.createBingoGame(name, amountOfPlayers, size);
  }

  public requestNextNumber(): void {
    this.hubConnection.invoke('GetNextNumber');
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
    this.hubConnection.on('NextNumber', (data: number) => {
      this.nextNumberReceived.next(data);
    });
  }
}
