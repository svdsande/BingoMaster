import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BingoGameComponent } from './bingo-game.component';

describe('BingoGameComponent', () => {
  let component: BingoGameComponent;
  let fixture: ComponentFixture<BingoGameComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BingoGameComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BingoGameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
