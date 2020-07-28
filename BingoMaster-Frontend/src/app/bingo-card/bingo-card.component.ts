import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatSelectChange } from '@angular/material/select';
import { take } from 'rxjs/operators';
import { BingoCardCreationModel, BingoCardModel } from 'src/api/api';
import { BingoCardService } from '../services/bingo-card.service';
import { trigger, transition, style, animate } from '@angular/animations';

export interface Tile {
  cols: number;
  rows: number;
  text: string;
  color: string;
}

@Component({
  selector: 'bingo-card',
  templateUrl: './bingo-card.component.html',
  styleUrls: ['./bingo-card.component.scss'],
  animations: [
    trigger('createBingoCards', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-100px)' }),
        animate('300ms ease-out', style({ opacity: 1, transform: 'none' }))
      ]),
      transition(':leave', [
        animate('300ms', style({ opacity: 0 }))
      ])
    ]),
    trigger('generateBingoCards', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-100px)', }),
        animate('300ms 1000ms ease-out', style({ opacity: 1, transform: 'none' }))
      ]),
      transition(':leave', [
        animate('0.3s', style({ opacity: 0 }))
      ])
    ]),
  ]
})
export class BingoCardComponent implements OnInit {

  public bingoCards: BingoCardModel[] = [];
  public gridTiles: Tile[];
  public gridColumns: number = 3;
  public backgroundColor: string = '#add8e6';
  public borderColor: string = '#ffffff';
  public bingoCardFormGroup: FormGroup;

  constructor(private bingoCardService: BingoCardService) { }

  ngOnInit(): void {
    this.buildForm();
    this.gridTiles = this.generateTiles(3);
  }

  public selectChange(event: MatSelectChange): void {
    this.gridTiles = this.generateTiles(event.value);
    this.setCenterTileFreeSpace();
  }

  public getNumberOfColumns(): number {
    return Math.sqrt(this.gridTiles.length);
  }

  public generateBingoCards(): void {
    const bingoCardModel: BingoCardCreationModel = new BingoCardCreationModel();
    bingoCardModel.name = this.bingoCardFormGroup.get('name').value;
    bingoCardModel.size = this.bingoCardFormGroup.get('size').value;
    bingoCardModel.isCenterSquareFree = this.bingoCardFormGroup.get('centerSquareFree').value;
    bingoCardModel.amount = this.bingoCardFormGroup.get('amount').value;
    bingoCardModel.backgroundColor = this.backgroundColor;
    bingoCardModel.borderColor = this.borderColor;

    this.bingoCardService.generateBingoCards(bingoCardModel)
      .pipe(take(1))
      .subscribe((result) => {
        this.bingoCards = result;
      });
  }

  public centerSquareChange(_changeEvent: MatCheckboxChange): void {
    this.setCenterTileFreeSpace();
  }

  public backgroundColorSelected(color: string): void {
    this.backgroundColor = color;
  }

  public borderColorSelected(color: string): void {
    this.borderColor = color;
  }

  public resetBingoCardForm(): void {
    this.bingoCardFormGroup.reset();
    this.gridTiles = this.generateTiles(3);
  }

  private setCenterTileFreeSpace() {
    const index = this.getCenterTileIndex();
    const freeSpace: boolean = this.bingoCardFormGroup.get('centerSquareFree').value;

    if (freeSpace) {
      this.gridTiles[index].text = 'x';
    } else {
      this.gridTiles[index].text = '';
    }
  }

  private getCenterTileIndex(): number {
    if (this.gridTiles.length % 2 === 0) {
      return (this.gridTiles.length / 2) - 1;
    } else {
      return Math.floor(this.gridTiles.length / 2);
    }
  }

  private generateTiles(columns: number): Tile[] {
    return Array.from(new Array(columns * columns), (_value, _index: number) => {
      return { cols: 1, rows: 1, text: '' } as Tile;
    });
  }

  private buildForm(): void {
    this.bingoCardFormGroup = new FormGroup({
      name: new FormControl('', Validators.required),
      size: new FormControl(3, Validators.required),
      centerSquareFree: new FormControl(false),
      amount: new FormControl('', Validators.required)
    });
  }
}
