import {HttpClient,HttpHeaders,HttpErrorResponse} from "@angular/common/http";
import { Inject, Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ErrorHandlingService } from '../HandleError/error-handling.service';


@Injectable({
  providedIn: 'root'
})
export class UserDispService {
  apiUrl: string = environment.baseURL;
  headers = new HttpHeaders()
    .set("Access-Control-Allow-Origin", "*")
    .set("Content-Type", "application/json")
    .append("Authorization", "Bearer " + localStorage.getItem('access_token'));

  constructor(private _httpService: HttpClient, @Inject('BASE_URL') private baseUrl: string, private errorHandlingService: ErrorHandlingService) { }
  Error = this.errorHandlingService.error



  getDistrictItems(): Observable<any> {
    return this._httpService.get<any>(`${this.apiUrl}UserManagement/api/UserControllerAPI/GetDistrict`, { headers: this.headers })
      .pipe(catchError(this.Error));
  }

  getDataByRoleDist(district: number, role: number, societyName: string, mobile: string, email: string, pagenumber: number, pageSize: number):
    Observable<any> {
    if (societyName === '') {
      societyName = 'null';
    }
    if (mobile === '') {
      mobile = 'null';
    }
    if (email === '') {
      email = 'null';
    }
    return this._httpService.get(`${this.apiUrl}GetUsers/${district}/${role}/${societyName}/${mobile}/${email}/${pagenumber}/${pageSize}`, { headers: this.headers }).pipe(catchError(this.Error));
  }
  getDataBySocietyType(district: number, societyId: number, societyName: string, pageNumber: number, pageSize: number): Observable<any> {
    if (societyName === '') {
      societyName = 'null';
    }
    return this._httpService.get(`${this.apiUrl}GetSocietyType/${district}/${societyName}/${societyId}/${pageNumber}/${pageSize}`, { headers: this.headers }).pipe(catchError(this.Error));
  }

  //ChangeUserStatus(status: number, userId: number): Observable<any> {
  //  const body = { userId, status };
  //  const url = `${this.apiUrl}UserManagement/api/UserControllerAPI/UserStatus/${userId}/${status}`;
  //  return this._httpService.post(url, body, { headers: this.headers })
  //    .pipe(catchError(this.Error));
  //}
  ChangeUserStatus(status: number, userId: number): Observable<any> {
    const body = { userId, status };
    // return this._httpService.post(`${this.apiUrl}UserStatus/${userId}/${status}`, body);
    const url = `${this.apiUrl}UserStatus/${userId}/${status}`;
    return this._httpService.post(url, body, { headers: this.headers })
      .pipe(catchError(this.Error));
  }
  UpdatePassword(emailid: string,password: string): Observable<any> {
    const body = { emailid, password };

    return this._httpService.post(`${this.apiUrl}UserManagement/api/UserControllerAPI/UpdatePassword/${emailid}/${password}`, body, { headers: this.headers }).pipe(catchError(this.Error));
  }

  getData(): Observable<any> {

    return this._httpService
      .get<any>(this.apiUrl + "UserManagement/api/UserControllerAPI/GetAll", {
        headers: this.headers,
        observe: "body",
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      })
      .pipe(catchError(this.error));
  }


  getDistrictTypeItems(): Observable<any> {
    return this._httpService.get<any>(`${this.apiUrl}UserManagement/api/UserControllerAPI/GetDistrictType`, { headers: this.headers })
      .pipe(catchError(this.Error));
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
