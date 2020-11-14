import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { BingoCardDetailComponent } from './bingo-card-detail.component';

describe('BingoCardDetailComponent', () => {
  let component: BingoCardDetailComponent;
  let fixture: ComponentFixture<BingoCardDetailComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ BingoCardDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BingoCardDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
