import {  HttpClient,  HttpHeaders,  HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { environment } from "../../../environments/environment";
import { ByeLawsRequest } from 'src/app/shared/models/bye-laws-request-list.model';

@Injectable({
  providedIn: "root"
})
export class ByeLawsRequestService {
  baseURL: string = environment.baseURL;
  headers = new HttpHeaders()
    .set("Access-Control-Allow-Origin", "*")
    .set("Content-Type", "application/json")
    .append("Authorization", "Bearer " + localStorage.getItem('access_token'));
  constructor(private http: HttpClient) { }

  saveByeLawsRequest(inputData : object)
  {
    return this.http.post(this.baseURL + "ByeLawsApi/api/ByeLawsApi/SaveByeLaws",inputData,{headers: this.headers}).pipe(catchError(this.error));
  }

  getBayeLawsRequestList(params: any): Observable<ByeLawsRequest> {
    return this.http.get<ByeLawsRequest>(this.baseURL + "ByeLawsApi/api/ByeLawsApi/GetAll",
      { headers: this.headers, observe: 'body', params: params, reportProgress: true, responseType: 'json', withCredentials: false })
      .pipe(catchError(this.error));
  }

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
