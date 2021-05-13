import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { By } from '@angular/platform-browser';

import { SpinnerButtonComponent } from './spinner-button.component';

describe('SpinnerButtonComponent', () => {
  let component: SpinnerButtonComponent;
  let fixture: ComponentFixture<SpinnerButtonComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SpinnerButtonComponent ],
      imports: [ MatProgressSpinnerModule ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpinnerButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should hide spinner', () => {
    expect(fixture.debugElement.query(By.css('.spinner-container'))).toBeNull();
  });

  it('should show spinner', () => {
    component.loading = true;
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('.spinner-container'))).toBeTruthy();
  });

  it('should first show and then hide spinner', () => {
    component.loading = true;
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('.spinner-container'))).toBeTruthy();

    component.loading = false;
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('.spinner-container'))).toBeNull();
  });
});
