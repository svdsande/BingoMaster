import { HttpClient, HttpHandler } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BingoCardClient } from 'src/api/api';

import { GenerateBingoCardComponent } from './generate-bingo-card.component';

describe('BingoCardComponent', () => {
  let component: GenerateBingoCardComponent;
  let fixture: ComponentFixture<GenerateBingoCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerateBingoCardComponent ],
      imports: [ BrowserAnimationsModule ],
      providers: [ BingoCardClient, HttpClient, HttpHandler ]
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
