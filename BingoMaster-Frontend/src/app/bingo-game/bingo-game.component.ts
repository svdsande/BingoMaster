import { Component, OnInit } from '@angular/core';
import { BingoGameService } from '../services/bingo-game.service';

@Component({
  selector: 'app-bingo-game',
  templateUrl: './bingo-game.component.html',
  styleUrls: ['./bingo-game.component.scss']
})
export class BingoGameComponent implements OnInit {

  public numbers: number[] = [];

  constructor(private bingoGameService: BingoGameService) { }

  ngOnInit(): void {
    this.bingoGameService.nextNumberReceived.subscribe((value) => {
      this.numbers.push(value);
    });
  }

  public requestNextNumber(): void {
    this.bingoGameService.requestNextNumber();
  }
}