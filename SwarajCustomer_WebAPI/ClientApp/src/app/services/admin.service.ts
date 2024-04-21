import { Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { environment } from "../../environments/environment";
import { DashBoard, NewsFeeds } from "../../app/shared/models/dash-board.model";

import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse
} from "@angular/common/http";

@Injectable({
  providedIn: "root"
})
export class AdminService {
  baseURL: string = environment.baseURL;
  headers = new HttpHeaders()
    .set("Access-Control-Allow-Origin", "*")
    .set("Content-Type", "application/json")
    .append("Authorization", "Bearer " + localStorage.getItem("access_token"));

  constructor(private http: HttpClient) { }
  // Create
  createTask(data: any): Observable<any> {
    let API_URL = `${this.baseURL}/create-task`;
    return this.http.post(API_URL, data).pipe(catchError(this.error));
  }

  // Get DashBoard Data
  GetDashBoardData(): Observable<DashBoard> {
    return this.http
      .get<DashBoard>(this.baseURL + "Admin/api/AdminAPI/GetDashBoardData", {
        headers: this.headers
      })
      .pipe(catchError(this.error));
  }

  // Update
  updateTask(id: any, data: any): Observable<any> {
    let API_URL = `${this.baseURL}/update-task/${id}`;
    return this.http
      .put(API_URL, data, { headers: this.headers })
      .pipe(catchError(this.error));
  }
  // Delete
  deleteTask(id: any): Observable<any> {
    var API_URL = `${this.baseURL}/delete-task/${id}`;
    return this.http.delete(API_URL).pipe(catchError(this.error));
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

  // Get News Feeds Data
  GetNewsFeedsData(params: any): Observable<NewsFeeds> {
    return this.http
      .get<NewsFeeds>(this.baseURL + "Admin/api/NewsFeedsAPI/GetAll", {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      })
      .pipe(catchError(this.error));
  }
}
