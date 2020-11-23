﻿/* tslint:disable */
/* eslint-disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.5.0.0 (NJsonSchema v10.1.15.0 (Newtonsoft.Json v12.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming

import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class BingoCardClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    generateBingoCards(bingoCardModel: BingoCardCreationModel): Observable<BingoCardModel[]> {
        let url_ = this.baseUrl + "/api/BingoCard";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(bingoCardModel);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGenerateBingoCards(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGenerateBingoCards(<any>response_);
                } catch (e) {
                    return <Observable<BingoCardModel[]>><any>_observableThrow(e);
                }
            } else
                return <Observable<BingoCardModel[]>><any>_observableThrow(response_);
        }));
    }

    protected processGenerateBingoCards(response: HttpResponseBase): Observable<BingoCardModel[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(BingoCardModel.fromJS(item));
            }
            return _observableOf(result200);
            }));
        } else if (status === 400) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = resultData400 !== undefined ? resultData400 : <any>null;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<BingoCardModel[]>(<any>null);
    }
}

@Injectable()
export class BingoGameClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    createBingoGame(gameCreationModel: BingoGameCreationModel): Observable<BingoGameModel> {
        let url_ = this.baseUrl + "/api/BingoGame";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(gameCreationModel);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processCreateBingoGame(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCreateBingoGame(<any>response_);
                } catch (e) {
                    return <Observable<BingoGameModel>><any>_observableThrow(e);
                }
            } else
                return <Observable<BingoGameModel>><any>_observableThrow(response_);
        }));
    }

    protected processCreateBingoGame(response: HttpResponseBase): Observable<BingoGameModel> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = BingoGameModel.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status === 400) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = resultData400 !== undefined ? resultData400 : <any>null;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<BingoGameModel>(<any>null);
    }
}

@Injectable()
export class UserClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    authenticate(authenticateUserModel: AuthenticateUserModel): Observable<AuthenticatedUserModel> {
        let url_ = this.baseUrl + "/api/User/authenticate";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(authenticateUserModel);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processAuthenticate(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processAuthenticate(<any>response_);
                } catch (e) {
                    return <Observable<AuthenticatedUserModel>><any>_observableThrow(e);
                }
            } else
                return <Observable<AuthenticatedUserModel>><any>_observableThrow(response_);
        }));
    }

    protected processAuthenticate(response: HttpResponseBase): Observable<AuthenticatedUserModel> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = AuthenticatedUserModel.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status === 400) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = resultData400 !== undefined ? resultData400 : <any>null;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<AuthenticatedUserModel>(<any>null);
    }
}

export class BingoCardModel implements IBingoCardModel {
    name?: string | undefined;
    grid?: number[][] | undefined;

    constructor(data?: IBingoCardModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.name = _data["name"];
            if (Array.isArray(_data["grid"])) {
                this.grid = [] as any;
                for (let item of _data["grid"])
                    this.grid!.push(item);
            }
        }
    }

    static fromJS(data: any): BingoCardModel {
        data = typeof data === 'object' ? data : {};
        let result = new BingoCardModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        if (Array.isArray(this.grid)) {
            data["grid"] = [];
            for (let item of this.grid)
                data["grid"].push(item);
        }
        return data; 
    }
}

export interface IBingoCardModel {
    name?: string | undefined;
    grid?: number[][] | undefined;
}

export class BingoCardCreationModel implements IBingoCardCreationModel {
    name?: string | undefined;
    size!: number;
    isCenterSquareFree!: boolean;
    amount!: number;

    constructor(data?: IBingoCardCreationModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.name = _data["name"];
            this.size = _data["size"];
            this.isCenterSquareFree = _data["isCenterSquareFree"];
            this.amount = _data["amount"];
        }
    }

    static fromJS(data: any): BingoCardCreationModel {
        data = typeof data === 'object' ? data : {};
        let result = new BingoCardCreationModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["size"] = this.size;
        data["isCenterSquareFree"] = this.isCenterSquareFree;
        data["amount"] = this.amount;
        return data; 
    }
}

export interface IBingoCardCreationModel {
    name?: string | undefined;
    size: number;
    isCenterSquareFree: boolean;
    amount: number;
}

export class BingoGameModel implements IBingoGameModel {
    drawnNumber!: number;
    players?: PlayerModel[] | undefined;

    constructor(data?: IBingoGameModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.drawnNumber = _data["drawnNumber"];
            if (Array.isArray(_data["players"])) {
                this.players = [] as any;
                for (let item of _data["players"])
                    this.players!.push(PlayerModel.fromJS(item));
            }
        }
    }

    static fromJS(data: any): BingoGameModel {
        data = typeof data === 'object' ? data : {};
        let result = new BingoGameModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["drawnNumber"] = this.drawnNumber;
        if (Array.isArray(this.players)) {
            data["players"] = [];
            for (let item of this.players)
                data["players"].push(item.toJSON());
        }
        return data; 
    }
}

export interface IBingoGameModel {
    drawnNumber: number;
    players?: PlayerModel[] | undefined;
}

export class PlayerModel implements IPlayerModel {
    name?: string | undefined;
    bingoCard?: BingoCardModel | undefined;
    isHorizontalLineDone!: boolean;
    isVerticalLineDone!: boolean;
    isFullCardDone!: boolean;

    constructor(data?: IPlayerModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.name = _data["name"];
            this.bingoCard = _data["bingoCard"] ? BingoCardModel.fromJS(_data["bingoCard"]) : <any>undefined;
            this.isHorizontalLineDone = _data["isHorizontalLineDone"];
            this.isVerticalLineDone = _data["isVerticalLineDone"];
            this.isFullCardDone = _data["isFullCardDone"];
        }
    }

    static fromJS(data: any): PlayerModel {
        data = typeof data === 'object' ? data : {};
        let result = new PlayerModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["bingoCard"] = this.bingoCard ? this.bingoCard.toJSON() : <any>undefined;
        data["isHorizontalLineDone"] = this.isHorizontalLineDone;
        data["isVerticalLineDone"] = this.isVerticalLineDone;
        data["isFullCardDone"] = this.isFullCardDone;
        return data; 
    }
}

export interface IPlayerModel {
    name?: string | undefined;
    bingoCard?: BingoCardModel | undefined;
    isHorizontalLineDone: boolean;
    isVerticalLineDone: boolean;
    isFullCardDone: boolean;
}

export class BingoGameCreationModel implements IBingoGameCreationModel {
    name?: string | undefined;
    players?: PlayerModel[] | undefined;
    size!: number;

    constructor(data?: IBingoGameCreationModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.name = _data["name"];
            if (Array.isArray(_data["players"])) {
                this.players = [] as any;
                for (let item of _data["players"])
                    this.players!.push(PlayerModel.fromJS(item));
            }
            this.size = _data["size"];
        }
    }

    static fromJS(data: any): BingoGameCreationModel {
        data = typeof data === 'object' ? data : {};
        let result = new BingoGameCreationModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        if (Array.isArray(this.players)) {
            data["players"] = [];
            for (let item of this.players)
                data["players"].push(item.toJSON());
        }
        data["size"] = this.size;
        return data; 
    }
}

export interface IBingoGameCreationModel {
    name?: string | undefined;
    players?: PlayerModel[] | undefined;
    size: number;
}

export class AuthenticatedUserModel implements IAuthenticatedUserModel {
    id!: string;
    firstName?: string | undefined;
    middleName?: string | undefined;
    lastName?: string | undefined;
    emailAddress?: string | undefined;
    token?: string | undefined;

    constructor(data?: IAuthenticatedUserModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
            this.firstName = _data["firstName"];
            this.middleName = _data["middleName"];
            this.lastName = _data["lastName"];
            this.emailAddress = _data["emailAddress"];
            this.token = _data["token"];
        }
    }

    static fromJS(data: any): AuthenticatedUserModel {
        data = typeof data === 'object' ? data : {};
        let result = new AuthenticatedUserModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["firstName"] = this.firstName;
        data["middleName"] = this.middleName;
        data["lastName"] = this.lastName;
        data["emailAddress"] = this.emailAddress;
        data["token"] = this.token;
        return data; 
    }
}

export interface IAuthenticatedUserModel {
    id: string;
    firstName?: string | undefined;
    middleName?: string | undefined;
    lastName?: string | undefined;
    emailAddress?: string | undefined;
    token?: string | undefined;
}

export class AuthenticateUserModel implements IAuthenticateUserModel {
    emailAddress?: string | undefined;
    password?: string | undefined;

    constructor(data?: IAuthenticateUserModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.emailAddress = _data["emailAddress"];
            this.password = _data["password"];
        }
    }

    static fromJS(data: any): AuthenticateUserModel {
        data = typeof data === 'object' ? data : {};
        let result = new AuthenticateUserModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["emailAddress"] = this.emailAddress;
        data["password"] = this.password;
        return data; 
    }
}

export interface IAuthenticateUserModel {
    emailAddress?: string | undefined;
    password?: string | undefined;
}

export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return _observableThrow(result);
    else
        return _observableThrow(new ApiException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader();
            reader.onload = event => {
                observer.next((<any>event.target).result);
                observer.complete();
            };
            reader.readAsText(blob);
        }
    });
}