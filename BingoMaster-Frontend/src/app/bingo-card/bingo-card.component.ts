import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'bingo-card',
  templateUrl: './bingo-card.component.html',
  styleUrls: ['./bingo-card.component.scss']
})
export class BingoCardComponent implements OnInit {

  public bingoCardFormGroup: FormGroup = new FormGroup({
    name: new FormControl('',),
    size: new FormControl('')
  });

  constructor() { }

  ngOnInit(): void {
  }

}
