import { Component, OnInit } from '@angular/core';
import { BingoGameModel } from 'src/api/api';
import { BingoGameService } from '../services/bingo-game.service';

@Component({
  selector: 'app-bingo-game',
  templateUrl: './bingo-game.component.html',
  styleUrls: ['./bingo-game.component.scss']
})
export class BingoGameComponent implements OnInit {

  public drawnNumbers: number[] = [];
  public bingoGame: BingoGameModel;
  public countDownDone: boolean = false;

  constructor(private bingoGameService: BingoGameService) { }

  ngOnInit(): void {
    this.bingoGameService.nextRoundReceived.subscribe((model: BingoGameModel) => {
      this.drawnNumbers.push(model.drawnNumber);
    });
  }

  public playNextRound(): void {
    this.bingoGameService.playNextRound(this.bingoGame.players, this.drawnNumbers);
  }

  public onCountDownDone(): void {
    this.countDownDone = true;
    this.drawnNumbers.push(this.bingoGame.drawnNumber);
  }
}
