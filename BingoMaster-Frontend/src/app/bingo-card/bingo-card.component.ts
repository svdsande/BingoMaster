import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { BingoCardCreationModel, BingoCardModel } from 'src/api/api';
import { BingoCardService } from '../services/bingo-card.service';
import jsPDF from 'jspdf';

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

  @ViewChild('bingocardsData') bingocardsData: ElementRef<HTMLElement>;
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

    this.bingoCardService.generateBingoCards(bingoCardModel).subscribe((result) => {
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
    const data = this.bingocardsData.nativeElement;
    let doc = new jsPDF('p','mm', this.bingoCardFormGroup.get('paperSize').value);

    let handleElement = {
      '#editor':function(element,renderer){
        return true;
      }
    };
    doc.fromHTML(data.innerHTML,15,15,{
      'width': 200,
      'elementHandlers': handleElement
    });

    doc.save('bingocards');
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
      return { cols: 1, rows: 1, text: '' } as Tile
    });
  }
}
