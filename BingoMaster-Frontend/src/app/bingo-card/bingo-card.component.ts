import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatSelectChange } from '@angular/material/select';
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
import { take } from 'rxjs/operators';
import { BingoCardCreationModel, BingoCardModel } from 'src/api/api';
import { BingoCardService } from '../services/bingo-card.service';

export interface Tile {
  cols: number;
  rows: number;
  text: string;
  color: string;
}

@Component({
  selector: 'bingo-card',
  templateUrl: './bingo-card.component.html',
  styleUrls: ['./bingo-card.component.scss']
})
export class BingoCardComponent implements OnInit {

  public bingoCards: BingoCardModel[];
  public gridTiles: Tile[];
  public gridColumns: number = 3;
  public backgroundColor: string = '#add8e6';
  public borderColor: string = '#ffffff';
  public bingoCardFormGroup: FormGroup = new FormGroup({
    name: new FormControl('', Validators.required),
    size: new FormControl(3, Validators.required),
    centerSquareFree: new FormControl(false),
    amount: new FormControl('', Validators.required),
    paperSize: new FormControl('A4')
  });

  constructor(private bingoCardService: BingoCardService) { }

  ngOnInit(): void {
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

  public downloadPDF(): void {
    window.scrollTo(0,0);
    let doc = new jsPDF('p', 'mm', this.bingoCardFormGroup.get('paperSize').value);
    const bingocards = document.querySelectorAll('.bingocard');

    for (let i = 0; i < bingocards.length; i++) {
      html2canvas(bingocards[i] as HTMLElement).then(function (canvas) {
        let image = canvas.toDataURL('image/png');
        doc.addImage(image, 'PNG', 3, 10, 200, 200);

        if (i + 1 === bingocards.length) {
          doc.save('bingo-cards.pdf');
        } else {
          doc.addPage();
        }
      });
    }
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
    return Math.floor(this.gridTiles.length / 2);
  }

  private generateTiles(columns: number): Tile[] {
    return Array.from(new Array(columns * columns), (_value, _index: number) => {
      return { cols: 1, rows: 1, text: '' } as Tile;
    });
  }
}
