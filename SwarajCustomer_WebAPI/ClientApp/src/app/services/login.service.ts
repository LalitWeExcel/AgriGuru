import { Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { environment } from "../../environments/environment";
import { HttpClient, HttpHeaders, HttpErrorResponse } from "@angular/common/http";
import { LoginDataModal } from "../shared/models/login-user-details.model";


@Injectable({
  providedIn: "root"
})


export class LoginService {

  baseURL: string = environment.baseURL;
  headers = new HttpHeaders()
    .set("access-control-allow-origin", "*")
    .set("content-type", "application/json")
    .append("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");

  constructor(private http: HttpClient) {}

  // Get login
  public LoginUser(formData: any): Observable<LoginDataModal> {
    return this.http.post<LoginDataModal>(this.baseURL + "api/AccountAPI/Login", formData, { headers: this.headers }).pipe(catchError(this.error));
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
