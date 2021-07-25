import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { API_BASE_URL, RegisterUserModel, UserClient, UserModel } from 'src/api/api';
import { environment } from 'src/environments/environment';
import { AuthenticationService } from './authentication.service';

describe('AuthenticationService', () => {
  let service: AuthenticationService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [
        UserClient,
        {
          provide: API_BASE_URL,
          useValue: environment.apiUrl
      } ]
    });

    service = TestBed.inject(AuthenticationService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should register new user', () => {
    let userModel: UserModel = new UserModel();
    userModel.id = 'someUserId';
    userModel.emailAddress = 'EddieVedder@pearl-jam.com';
    userModel.playerName = 'EddieVedder';

    let registerModel: RegisterUserModel = new RegisterUserModel();
    registerModel.emailAddress = 'EddieVedder@pearl-jam.com';
    registerModel.playerName = 'EddieVedder';
    registerModel.password = 'somePassword';

    service.register(registerModel).subscribe((model: UserModel) => {
      expect(model).toBeDefined();
      expect(model.playerName).toBe('EddieVedder');
    });

    const mockRequest = httpMock.expectOne(environment.apiUrl + '/api/User/register');

    mockRequest.flush(new Blob([JSON.stringify(userModel)]));
  });
});
