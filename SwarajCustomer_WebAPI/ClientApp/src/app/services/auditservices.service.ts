import { Injectable } from "@angular/core";
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders
} from "@angular/common/http";
import { BehaviorSubject, Observable, throwError } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { ARCSDropDown, SocietiesDropDown } from "../shared/models/society-trans-model.model";
import { CommanDropDown, AuditViewModel, view_GetAuditRequest } from "../shared/models/audit-model-request.model";

@Injectable({
  providedIn: "root"
})
export class AuditservicesService {
  private baseUrl = `${environment.baseURL}`;
  private _GetAuditOfficerRequestSendData = `${environment.baseURL}Audit/api/AuditAPI/GetAuditOfficerRequestSendData`;
  private _GetAuditAuditorRequestData = `${environment.baseURL}Audit/api/AuditAPI/GetAuditAuditorRequestData`;
  private _GetAuditJuniorAuditorRequestData = `${environment.baseURL}Audit/api/AuditAPI/GetAuditJuniorAuditorRequestData`;
  private _GetJuniorAuditorDropDown = `${environment.baseURL}Audit/api/AuditAPI/GetJuniorAuditorDropDown`;
  private _AssignJuniorAuditorOrder = `${environment.baseURL}Audit/api/AuditAPI/AssignJuniorAuditorOrder`;
  private _GetAuditorSocietiesDropDown = `${environment.baseURL}Audit/api/AuditAPI/GetAuditorSocietiesDropDown`;
  private _SendBackAuditToAuditorofficer = `${environment.baseURL}Audit/api/AuditAPI/SendBackAuditToAuditorofficer`;
  private savePersidentAuditRequest = `${environment.baseURL}Audit/api/AuditAPI/SavePersidentAuditRequest`;
  private _AuditForwardtoAuditor = `${environment.baseURL}Audit/api/AuditAPI/AuditForwardtoAuditor`;
  private _GetJuniorAuditorSocietiesDropDown = `${environment.baseURL}Audit/api/AuditAPI/GetJuniorAuditorSocietiesDropDown`
  private _SendsAuditRequestforSocietyPresidentFromAuditOfficer = `${environment.baseURL}Audit/api/AuditAPI/SendsAuditRequestforSocietyPresidentFromAuditOfficer`;
  private url_GetAuditFinancialYearSocietyWise = `${environment.baseURL}Audit/api/AuditAPI/GetAuditFinancialYearSocietyWise`;
  private url_GetAuditPresidentData = `${environment.baseURL}Audit/api/AuditAPI/GetPersidentAuditRequestData`;
  private _SubmitAuditorOfficerOrder = `${environment.baseURL}Audit/api/AuditAPI/SubmitAuditorOfficerOrder`;
  private _SubmitAuditFromJuniorAuditor = `${environment.baseURL}Audit/api/AuditAPI/SubmitAuditFromJuniorAuditor`;
  private _GetAuditAuditHistoryStatus = `${environment.baseURL}Audit/api/AuditAPI/GetAuditAuditHistoryStatus`;
  private _SendBackJuniorAuditorToAuditor = `${environment.baseURL}Audit/api/AuditAPI/SendBackJuniorAuditorToAuditor`;
  private _GetDownloadFile = `${environment.baseURL}Audit/api/AuditAPI/GetDownloadFile`;
  private _save_FinalAuditSubmit = `${environment.baseURL}Audit/api/AuditAPI/SaveFinalAuditSubmit`;
  private _SendBackToJuniorAuditor = `${environment.baseURL}Audit/api/AuditAPI/SendBackToJuniorAuditor`;
  private _GetAuditOfficerRequestData = `${environment.baseURL}Audit/api/AuditAPI/GetAuditOfficerRequestData`;
  private _GetChiefAuditOfficerRequestData = `${environment.baseURL}Audit/api/AuditAPI/GetChiefAuditOfficerRequestData`;
  private _SendBackAuditFileToPersidentFromAuditOfficer = `${environment.baseURL}Audit/api/AuditAPI/SendBackAuditFileToPersidentFromAuditOfficer`;
  private _GetAuditAuditorDropDownByOfficer = `${environment.baseURL}Audit/api/AuditAPI/GetAuditAuditorDropDownByOfficer`;
  private _GetAuditAuditorDropDown = `${environment.baseURL}Audit/api/AuditAPI/GetAuditAuditorDropDown`;
  private _GetAuditPlanerStatusDropDown = `${environment.baseURL}Audit/api/AuditAPI/GetAuditPlanerStatusDropDown`;


  public headers = new HttpHeaders()
    .set("Access-Control-Allow-Origin", "*")
    .set("Content-Type", "application/json")
    .append("Authorization", "Bearer " + localStorage.getItem("access_token"));


  public headerMultiPart = new HttpHeaders()
    .set("Access-Control-Allow-Origin", "*")
    .append("Authorization", "Bearer " + localStorage.getItem("access_token"));

  constructor(private http: HttpClient) { }

  public handleError(error: HttpErrorResponse) {
    console.error("server error:", error);
    if (error.error instanceof Error) {
      let errMessage = error.error.message;
      return throwError(() => new Error(errMessage));
    }
    return throwError(() => new Error(error.message || "server error"));
  }

  public error(error: HttpErrorResponse) {
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

  public GetJuniorAuditorDropDown(params: any): Observable<CommanDropDown> {
    return this.http
      .get<CommanDropDown>(`${this._GetJuniorAuditorDropDown}`, {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      }).pipe(catchError(this.error));
  }

  public GetAuditPlanerStatusDropDown(): Observable<CommanDropDown> {
    return this.http
      .get<CommanDropDown>(`${this._GetAuditPlanerStatusDropDown}`, {
        headers: this.headers,
        observe: "body",
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      }).pipe(catchError(this.error));
  }


  public GetAuditAuditorDropDownByOfficer(params: any): Observable<CommanDropDown> {
    return this.http
      .get<CommanDropDown>(`${this._GetAuditAuditorDropDownByOfficer}`, {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      }).pipe(catchError(this.error));
  }
  public GetJuniorAuditorSocietiesDropDown(params: any): Observable<CommanDropDown> {
    return this.http
      .get<CommanDropDown>(`${this._GetJuniorAuditorSocietiesDropDown}`, {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      }).pipe(catchError(this.error));
  }



  public AssignJuniorAuditorOrder(formData: Object): Observable<any> {
    return this.http.post<any>(`${this._AssignJuniorAuditorOrder}`, formData, { headers: this.headerMultiPart }).pipe(catchError(this.error));
  }

  public SubmitAuditFromJuniorAuditor(formData: Object): Observable<any> {
    return this.http.post<any>(`${this._SubmitAuditFromJuniorAuditor}`, formData, { headers: this.headerMultiPart }).pipe(catchError(this.error));
  }

  public GetAuditorSocietiesDropDown(params: any): Observable<SocietiesDropDown> {
    return this.http
      .get<SocietiesDropDown>(`${this._GetAuditorSocietiesDropDown}`, {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      }).pipe(catchError(this.error));
  }

  public savePresidentAuditRequest(formData: Object): Observable<any> {
    return this.http.post<any>(`${this.savePersidentAuditRequest}`, formData, { headers: this.headerMultiPart }).pipe(catchError(this.error));
  }

  public GetAuditFinancialYearSocietyWise(params: any): Observable<CommanDropDown> {
    return this.http
      .get<CommanDropDown>(`${this.url_GetAuditFinancialYearSocietyWise}`, {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      }).pipe(catchError(this.error));
  }



  public SubmitAuditorOfficerOrder(formData: Object): Observable<any> {
    return this.http.post<any>(`${this._SubmitAuditorOfficerOrder}`, formData, { headers: this.headerMultiPart }).pipe(catchError(this.error));
  }

  public AuditForwardtoAuditor(formData: Object): Observable<any> {
    return this.http.post<any>(`${this._AuditForwardtoAuditor}`, formData, { headers: this.headerMultiPart }).pipe(catchError(this.error));
  }



  public SendBackAuditToAuditorofficer(formData: Object): Observable<any> {
    return this.http.post<any>(`${this._SendBackAuditToAuditorofficer}`, formData, { headers: this.headerMultiPart }).pipe(catchError(this.error));
  }

  public SendBackJuniorAuditorToAuditor(formData: Object): Observable<any> {
    return this.http.post<any>(`${this._SendBackJuniorAuditorToAuditor}`, formData, { headers: this.headerMultiPart }).pipe(catchError(this.error));
  }

  public SendBackAuditFileToPersidentFromAuditOfficer(formData: Object): Observable<any> {
    return this.http.post<any>(`${this._SendBackAuditFileToPersidentFromAuditOfficer}`, formData, { headers: this.headerMultiPart }).pipe(catchError(this.error));
  }

  public GetDownloadFile(params: any) {
    return this.http.get(`${this._GetDownloadFile}`, {
      headers: this.headers,
      observe: "events",
      params: params,
      reportProgress: true,
      responseType: "blob",
      withCredentials: false
    });
  }

  public GetAuditAuditorDropDown(params: any): Observable<ARCSDropDown> {

    return this.http.get<ARCSDropDown>(`${this._GetAuditAuditorDropDown}`, {
      headers: this.headers,
      observe: "body",
      params: params,
      reportProgress: true,
      responseType: "json",
      withCredentials: false
    }).pipe(catchError(this.error));
  }

  public FinalAuditSubmit(params: any): Observable<any> {
    return this.http.get<any>(`${this._save_FinalAuditSubmit}`, {
      headers: this.headers,
      observe: "body",
      params: params,
      reportProgress: true,
      responseType: "json",
      withCredentials: false
    }).pipe(catchError(this.error));
  }

  public SendBackToJuniorAuditor(formData: Object): Observable<any> {
    return this.http.post<any>(`${this._SendBackToJuniorAuditor}`, formData, { headers: this.headerMultiPart }).pipe(catchError(this.error));
  }

  public GetAuditPresidentData(params: any): Observable<AuditViewModel> {
    return this.http.get<AuditViewModel>(this.url_GetAuditPresidentData, {
      headers: this.headers,
      observe: "body",
      params: params,
      reportProgress: true,
      responseType: "json",
      withCredentials: false
    })
      .pipe(catchError(this.error));
  }

  public GetAuditAuditorRequestData(params: any): Observable<AuditViewModel> {
    return this.http
      .get<AuditViewModel>(`${this._GetAuditAuditorRequestData}`, {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      })
      .pipe(catchError(this.error));
  }

  public GetAuditJuniorAuditorRequestData(params: any): Observable<AuditViewModel> {
    return this.http.get<AuditViewModel>(`${this._GetAuditJuniorAuditorRequestData}`, {
      headers: this.headers,
      observe: "body",
      params: params,
      reportProgress: true,
      responseType: "json",
      withCredentials: false
    }).pipe(catchError(this.error));
  }

  public GetAuditOfficerRequestData(params: any): Observable<AuditViewModel> {
    return this.http
      .get<AuditViewModel>(`${this._GetAuditOfficerRequestData}`, {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      }).pipe(catchError(this.error));
  }
  public GetChiefAuditOfficerRequestData(params: any): Observable<AuditViewModel> {
    return this.http
      .get<AuditViewModel>(`${this._GetChiefAuditOfficerRequestData}`, {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      }).pipe(catchError(this.error));
  }
  public GetAuditAuditHistoryStatus(params: any): Observable<view_GetAuditRequest> {
    return this.http.get<view_GetAuditRequest>(`${this._GetAuditAuditHistoryStatus}`, {
      headers: this.headers,
      observe: "body",
      params: params,
      reportProgress: true,
      responseType: "json",
      withCredentials: false
    }).pipe(catchError(this.error));
  }

  public GetAuditOfficerRequestSendData(params: any): Observable<view_GetAuditRequest> {

    return this.http.get<view_GetAuditRequest>(`${this._GetAuditOfficerRequestSendData}`, {
      headers: this.headers,
      observe: "body",
      params: params,
      reportProgress: true,
      responseType: "json",
      withCredentials: false
    }).pipe(catchError(this.error));
  }


  public SendsAuditRequestforSocietyPresidentFromAuditOfficer(requestList: any, params: any): Observable<any> {
    return this.http.post<any>(`${this._SendsAuditRequestforSocietyPresidentFromAuditOfficer}`,
      requestList, {
      headers: this.headers,
      params: params,
      reportProgress: true,
      withCredentials: false
    }).pipe(catchError(this.error));
  }

}
