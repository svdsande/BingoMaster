import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateBingoCardComponent } from './generate-bingo-card.component';

describe('BingoCardComponent', () => {
  let component: GenerateBingoCardComponent;
  let fixture: ComponentFixture<GenerateBingoCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerateBingoCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateBingoCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
