import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BingoCardClient, BingoCardCreationModel, BingoCardModel } from 'src/api/api';

@Injectable({
  providedIn: 'root'
})
export class BingoCardService {

  constructor(private bingoCardClient: BingoCardClient) { }

  public generateBingoCards(model: BingoCardCreationModel): Observable<BingoCardModel[]> {
    return this.bingoCardClient.generateBingoCards(model);
  }
}
