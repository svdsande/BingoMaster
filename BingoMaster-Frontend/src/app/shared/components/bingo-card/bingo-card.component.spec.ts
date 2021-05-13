import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { BingoCardModel } from 'src/api/api';

import { BingoCardComponent } from './bingo-card.component';

describe('BingoCardComponent', () => {
  let component: BingoCardComponent;
  let fixture: ComponentFixture<BingoCardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ BingoCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BingoCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have the right background color', () => {
    const color = 'rgb(173, 216, 230)';
    const element = fixture.debugElement.nativeElement.querySelector('table');
    expect(element.style.backgroundColor).toBe(color);
  });

  it('should have three rows and no cells', () => {
    const model: BingoCardModel = new BingoCardModel({
      grid: [ [], [], [] ]
    });

    component.bingoCard = model;
    fixture.detectChanges();

    const rows = fixture.debugElement.nativeElement.getElementsByTagName('tr');
    const cells = fixture.debugElement.nativeElement.getElementsByTagName('td');

    expect(rows).toHaveSize(3);
    expect(cells).toHaveSize(0);
  });

  it('should have one row with three cells that all should have the right border color', () => {
    const color = 'rgb(255, 255, 255)';
    const model: BingoCardModel = new BingoCardModel({
      grid: [ [1, 2, 3] ]
    });

    component.bingoCard = model;
    fixture.detectChanges();

    const cells = fixture.debugElement.nativeElement.getElementsByTagName('td');

    for (let cell of cells) {
      expect(cell.style.borderColor).toBe(color);
    }
  });

  it('should have three rows and each row should have three cells', () => {
    const model: BingoCardModel = new BingoCardModel({
      grid: [ [1, 2, 3], [4, 5, 6], [7, 8, 9] ]
    });

    component.bingoCard = model;
    fixture.detectChanges();

    const rows = fixture.debugElement.nativeElement.getElementsByTagName('tr');
    const cells = fixture.debugElement.nativeElement.getElementsByTagName('td');

    expect(rows).toHaveSize(3);
    expect(cells).toHaveSize(9);

    for (let row of rows) {
      expect(row.children).toHaveSize(3);
    }
  });

  it('should have one row and second cell should be marked', () => {
    const model: BingoCardModel = new BingoCardModel({
      grid: [ [1, 2, 3] ]
    });

    component.bingoCard = model;
    component.drawnNumbers = [2];
    fixture.detectChanges();

    const rows = fixture.debugElement.nativeElement.getElementsByTagName('tr');
    const cells = fixture.debugElement.nativeElement.getElementsByTagName('td');

    expect(rows).toHaveSize(1);
    expect(cells).toHaveSize(3);
    expect(cells[1]).toHaveClass('marked');
  });
});
