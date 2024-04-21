import { Injectable } from '@angular/core';

import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { environment } from "../../environments/environment";
import { HttpClient, HttpHeaders, HttpErrorResponse } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {

    baseURL: string = environment.baseURL;
  headers = new HttpHeaders()
    .set("access-control-allow-origin", "*")
    .set("content-type", "application/json")
    .append("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");

  constructor(private http: HttpClient) { }

  // POST DATA
  public Registration(formData: any): Observable<RegistrationService> {
    debugger;
    return this.http.post<any>(this.baseURL + "api/AccountAPI/Registration", formData, { headers: this.headers }).pipe(catchError(this.error));
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
  

  getDistrictItems(): Observable<any> {
    return this.http.get<any>(this.baseURL+"UserManagement/api/UserControllerAPI/GetDistrict", { headers: this.headers })
      .pipe(catchError(this.error));
  }
     
}
