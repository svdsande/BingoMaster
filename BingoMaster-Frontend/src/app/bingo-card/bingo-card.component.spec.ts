import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BingoCardComponent } from './bingo-card.component';

describe('BingoCardComponent', () => {
  let component: BingoCardComponent;
  let fixture: ComponentFixture<BingoCardComponent>;

  beforeEach(async(() => {
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
});
