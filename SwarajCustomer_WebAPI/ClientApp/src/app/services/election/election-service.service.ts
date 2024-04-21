import { HttpClient, HttpHeaders, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { environment } from "../../../environments/environment";

// import { ElectionRequest } from "src/app/shared/models/election/election_model";
// import { RemarkstrackingModel } from "src/app/shared/models/remarks/remarks-tracking-model";

import { ARCSResponse_Election, ElectionRequest, electionRole, ExecutiveMembers, Remarks, RemarkstrackingModel, User } from "src/app/shared/models/election/election_model";
import { users } from "src/app/shared/models/user-mangement-model.model";
import { observableToBeFn } from "rxjs/internal/testing/TestScheduler";
 
@Injectable({
  providedIn: 'root'
})
export class ElectionServiceService {

  constructor(private http: HttpClient) { }

  baseURL: string = environment.baseURL;
  headers = new HttpHeaders()
    .set("Access-Control-Allow-Origin", "*")
    .set("Content-Type", "application/json")
    .append("Authorization", "Bearer " + localStorage.getItem('access_token'));

  headerMultiPart = new HttpHeaders()
    .set("Access-Control-Allow-Origin", "*")
    .append("Authorization", "Bearer " + localStorage.getItem("access_token"));

    saveARCSMemberStatus(electionId: number) {
      return this.http.post(this.baseURL + "api/ElectionApi/SaveARCSMemberStatus", electionId, { headers: this.headers }).pipe(catchError(this.error));
    }
  saveElectionRequest(formData: Object) {
    return this.http.post(this.baseURL + "api/ElectionApi/SaveElectionRequest", formData, { headers: this.headerMultiPart }).pipe(catchError(this.error));
  }
 
  saveElectionRequestReport(formData: Object) {
    return this.http.post(this.baseURL + "api/ElectionApi/saveElectionReportIns", formData, { headers: this.headers }).pipe(catchError(this.error));
  }



  saveRemarksElectionRequest(formData: Object) {
    return this.http.post(this.baseURL + "api/ElectionApi/SaveRemarksForRecordKeepar", formData, { headers: this.headers }).pipe(catchError(this.error));
  }


  getElectionRequestList(params: any): Observable<ElectionRequest> {
    return this.http.get<ElectionRequest>(this.baseURL + "api/ElectionApi/GetElectionRequestListForPs",
      {
         headers: this.headers, 
         observe: 'body',
          params: params,
           reportProgress: true,
            responseType: 'json',
             withCredentials: false 
            })
      .pipe(catchError(this.error));
  }


  getElectionRequestListForInspReport(params: any): Observable<ElectionRequest> {
    return this.http.get<ElectionRequest>(this.baseURL + "api/ElectionApi/GetElectionListForInsReport",
      { headers: this.headers, observe: 'body', params: params, reportProgress: true, responseType: 'json', withCredentials: false })
      .pipe(catchError(this.error));
  }

  getElectionRequestListForInspReportList(params: any): Observable<ElectionRequest> {
    return this.http.get<ElectionRequest>(this.baseURL + "api/ElectionApi/GetElectionListForInsReportList",
      { headers: this.headers, observe: 'body', params: params, reportProgress: true, responseType: 'json', withCredentials: false })
      .pipe(catchError(this.error));
  }
  

  getElectionRequestListForExecutiveMem(params: any): Observable<ElectionRequest> {
    return this.http.get<ElectionRequest>(this.baseURL + "api/ElectionApi/getElectionRequestListForExecutiveMem",
      { headers: this.headers, observe: 'body', params: params, reportProgress: true, responseType: 'json', withCredentials: false })
      .pipe(catchError(this.error));
  }

  getElectionRequestListForRecordKeepar(params: any): Observable<ElectionRequest> {
    return this.http.get<ElectionRequest>(this.baseURL + "api/ElectionApi/GetElectionRequestListForRecordKeeparList",
      { headers: this.headers, observe: 'body', params: params, reportProgress: true, responseType: 'json', withCredentials: false })
      .pipe(catchError(this.error));
  }

  getRemarksByRequest(rId: any,module: any): Observable<RemarkstrackingModel> {
    return this.http.get<RemarkstrackingModel>(this.baseURL + "api/RemarksTrackingApi/GetRemarksByRequestId",
      { headers: this.headers, observe: 'body', params: {rId:rId,module:module}, reportProgress: true, responseType: 'json', withCredentials: false })
      .pipe(catchError(this.error));
  }
 
  approveElectionRequest(arcsResponse_Election:ARCSResponse_Election):Observable<any>{
    return this.http.post<ARCSResponse_Election>(this.baseURL+"api/ElectionApi/SaveARCSResponse",arcsResponse_Election, {
      headers: this.headers,
      // observe: "body",
      // // params: id,
      // reportProgress: true,
      // responseType: "json",
      // withCredentials: false
    })
    .pipe(catchError(this.error));
  }
  GetElectionRequestsList(params:any):Observable<ElectionRequest>{
    return this.http.get<ElectionRequest>(this.baseURL+"api/ElectionApi/GetElectionRequestsList",{
      headers: this.headers,
      observe: "body",
      params: params,
      reportProgress: true,
      responseType: "json",
      withCredentials: false
    })
    .pipe(catchError(this.error));
  }

  GetRoles(params):Observable<electionRole>{
    return this.http.get<electionRole>(this.baseURL+"api/ElectionApi/GetRoles",{
      headers: this.headers,
      observe: "body",
      params: params,
      reportProgress: true,
      responseType: "json",
      withCredentials: false
    })
    .pipe(catchError(this.error));
  }

  GetSocietyMembers(params):Observable<any>{
    return this.http.get<any>(this.baseURL+"api/ElectionApi/GetSocietyMembers",{
      headers: this.headers,
      observe: "body",
      params: params,
      reportProgress: true,
      responseType: "json",
      withCredentials: false
    })
    .pipe(catchError(this.error));
  }

  GetRemarks(params):Observable<Remarks>{
    return this.http.get<Remarks>(this.baseURL+"api/ElectionApi/GetRemarks",{
      headers: this.headers,
      observe: "body",
      params: params,
      reportProgress: true,
      responseType: "json",
      withCredentials: false
    })
    .pipe(catchError(this.error));
  }

  GetUsersBasedOnRole(params):Observable<User>{
    debugger
    return this.http.get<User>(this.baseURL+"api/ElectionApi/GetUsersBasedOnRole",{
      headers: this.headers,
      observe: "body",
      params: params,
      reportProgress: true,
      responseType: "json",
      withCredentials: false
    })
    .pipe(catchError(this.error));
  }

  GetSelectedExecutiveMembers(params):Observable<ExecutiveMembers>{
    return this.http.get<ExecutiveMembers>(this.baseURL+"api/ElectionApi/GetSelectedExecutiveMembers",{
      headers: this.headers,
      observe: "body",
      params: params,
      reportProgress: true,
      responseType: "json",
      withCredentials: false
    })
    .pipe(catchError(this.error));
  }

  GetARCSCode(params):Observable<any>{
    return this.http.get<any>(this.baseURL+"api/ElectionApi/getARCSCode",{
      headers: this.headers,
      observe: "body",
      params: params,
      reportProgress: true,
      responseType: "json",
      withCredentials: false
    })
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
