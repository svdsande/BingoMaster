import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { Title } from '@angular/platform-browser';

import { PageHeaderComponent } from './page-header.component';

let titleServiceSpy: jasmine.SpyObj<Title>;

describe('PageHeaderComponent', () => {
  let component: PageHeaderComponent;
  let fixture: ComponentFixture<PageHeaderComponent>;

  beforeEach(waitForAsync(() => {
    const spy = jasmine.createSpyObj('Title', ['getTitle']);

    TestBed.configureTestingModule({
      declarations: [ PageHeaderComponent ],
      providers: [ { provide: Title, useValue: spy } ]
    })
    .compileComponents();

    titleServiceSpy = TestBed.inject(Title) as jasmine.SpyObj<Title>;
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageHeaderComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show title', () => {
    (titleServiceSpy.getTitle as jasmine.Spy).and.returnValue('Home | BingoMaster');
    fixture.detectChanges();
    expect(fixture.debugElement.nativeElement.querySelector('#page-title').textContent).toBe('Home | BingoMaster');
  });

  it('should show updated title', () => {
    (titleServiceSpy.getTitle as jasmine.Spy).and.returnValue('Home | BingoMaster');
    fixture.detectChanges();
    expect(fixture.debugElement.nativeElement.querySelector('#page-title').textContent).toBe('Home | BingoMaster');

    (titleServiceSpy.getTitle as jasmine.Spy).and.returnValue('Settings | BingoMaster');
    fixture.detectChanges();
    expect(fixture.debugElement.nativeElement.querySelector('#page-title').textContent).toBe('Settings | BingoMaster');
  });
});
