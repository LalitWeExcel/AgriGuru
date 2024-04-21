import { Injectable } from '@angular/core';
import { Observable, throwError, delay, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { environment } from "../../../environments/environment";
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse, HttpParams
} from "@angular/common/http";
import { ErrorHandlingService } from '../HandleError/error-handling.service';

@Injectable({
  providedIn: 'root'
})
export class UsermanagementserviceService {

  baseURL: string = environment.baseURL;
  headers = new HttpHeaders()
    .set("Access-Control-Allow-Origin", "*")
    .set("Content-Type", "application/json")
    .append("Authorization", "Bearer " + localStorage.getItem('access_token'));
  constructor(private _httpService: HttpClient, private errorHandlingService: ErrorHandlingService) { }
  Error = this.errorHandlingService.error
  getData(): Observable<any> {
    return this._httpService.get<any>(this.baseURL + "UserManagement/api/UserControllerAPI/GetAll", { 'headers': this.headers })
      .pipe(catchError(this.Error))
    
  } 
  getmodule(): Observable<any> {
    return this._httpService.get<any>(this.baseURL + "UserManagement/api/UserControllerAPI/GetModule", { 'headers': this.headers })
  }

  postModule(ModuleId: any, RoleId: any): Observable<any> {
    const body = {
      RoleId: RoleId,
      ModuleId: ModuleId
    };

    return this._httpService.post<void>(this.baseURL + "UserManagement/api/UserControllerAPI/SaveUpdate", body, { 'headers': this.headers }).pipe(catchError(this.errorHandlingService.error));
  }

  DeleteRoleModule(Id: number): Observable<any> {
    const body = {
      Id: Id,
    };
    return this._httpService.post<any>(this.baseURL + "UserManagement/api/UserControllerAPI/DeleteRoleModule", body, { 'headers': this.headers }).pipe(catchError(this.Error));
  }

  GetRoleWiseModuleId(RoleId: any, show: any): Observable<any> {

    var JsonData = {
      RoleId: RoleId,
      show: show
    };
    return this._httpService.post<any>(this.baseURL + "UserManagement/api/UserControllerAPI/GetRoleWiseModuleBYID", JsonData, { 'headers': this.headers }).pipe(catchError(this.Error));
}
 
}
