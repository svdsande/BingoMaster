import { Injectable } from '@angular/core';
import { BingoCardClient, BingoCardCreationModel } from 'src/api/api';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BingoCardService {

  constructor(private bingoCardClient: BingoCardClient) { }

  generateBingoCards(model: BingoCardCreationModel) {
    return this.bingoCardClient.generateBingoCards(model);
    console.log("sdfsfsdf");
  }
}
