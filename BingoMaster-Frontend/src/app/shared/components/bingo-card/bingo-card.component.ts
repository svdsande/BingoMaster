import { Component, Input, OnInit } from '@angular/core';
import { BingoCardModel } from 'src/api/api';

@Component({
  selector: 'bingo-card',
  templateUrl: './bingo-card.component.html',
  styleUrls: ['./bingo-card.component.scss']
})
export class BingoCardComponent implements OnInit {

  @Input() bingoCard: BingoCardModel;
  @Input() drawnNumbers: number[] = [];
  @Input() backgroundColor: string = '#add8e6';
  @Input() borderColor: string = '#ffffff';

  constructor() { }

  ngOnInit(): void {
  }

}
