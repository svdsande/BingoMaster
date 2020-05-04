import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'bingo-card',
  templateUrl: './bingo-card.component.html',
  styleUrls: ['./bingo-card.component.scss']
})
export class BingoCardComponent implements OnInit {

  public bingoCardFormGroup: FormGroup = new FormGroup({
    name: new FormControl('', Validators.required),
    size: new FormControl('', Validators.required),
    centerSquareFree: new FormControl(false)
  });

  constructor() { }

  ngOnInit(): void {
  }

  public generateBingoCards(): void {
    console.log('form submitted');
  }
}
