import { Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { environment } from "../../../environments/environment";

import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse
} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class AccountSectionService {

  baseURL: string = environment.baseURL;
  headers = new HttpHeaders()
    .set("Access-Control-Allow-Origin", "*")
    .set("Content-Type", "application/json")
    .append("Authorization", "Bearer " + localStorage.getItem('access_token'));
  constructor(private _httpService: HttpClient) { }

  GetRoleWiseModuleId(RoleId: any): Observable<any> {
    var JsonData = { RoleId: RoleId, show: true };
    return this._httpService.post<any>(this.baseURL + "UserManagement/api/UserControllerAPI/GetRoleWiseModuleBYID", JsonData, { 'headers': this.headers }).pipe(catchError(this.error));
  }
  // Handle Errors
  error(error: HttpErrorResponse) {
    let errorMessage = "";
    if (error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.log(errorMessage);
    return throwError(() => {
      return errorMessage;
    });
  }




}
