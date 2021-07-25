import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { of } from 'rxjs';
import { AuthenticationService } from '../services/authentication/authentication.service';
import { MatIconTestingModule } from '@angular/material/icon/testing';

import { HeaderComponent } from './header.component';

let authenticationServiceSpy: jasmine.SpyObj<AuthenticationService>;

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  beforeEach(waitForAsync(() => {
    const spy = jasmine.createSpyObj('AuthenticationService', [], ['currentUser']);

    TestBed.configureTestingModule({
      declarations: [ HeaderComponent ],
      providers: [ { provide: AuthenticationService, useValue: spy } ],
      imports: [TranslateModule.forRoot(), MatIconModule, MatIconTestingModule, RouterModule.forRoot([]) ]
    })
    .compileComponents();

    authenticationServiceSpy = TestBed.inject(AuthenticationService) as jasmine.SpyObj<AuthenticationService>;
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show login section', () => {
    spyOnProperty(authenticationServiceSpy, 'currentUser').and.returnValue(of(null));

    const loginButtonElement = fixture.debugElement.nativeElement.getElementById('login-button');

    expect(loginButtonElement).toBeTruthy();
  });
});
