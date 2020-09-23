import { Injectable } from '@angular/core';
import { HubConnectionBuilder } from '@aspnet/signalr';
import { HubConnection } from '@aspnet/signalr/dist/esm/HubConnection';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BingoGameService {

  public nextNumberReceived: Subject<number> = new Subject<number>();

  private hubConnection: HubConnection;

  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
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
