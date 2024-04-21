import { Injectable } from "@angular/core";

import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { IMemberDetails } from "../../shared/models/memberdetails";
import {
  SocietyViewModel,
  SocietyTransModel,
  SocietyDetailsModels,
  SocietiesMembersList,
  MembersListViewModel,
  SocietiesDropDown,
  DistrictDropDown,
  ARCSDropDown,
  MemberViewModel
} from "../../shared/models/society-trans-model.model";

@Injectable({
  providedIn: "root"
})
export class MemberdetailsService {
  private baseUrl = `${environment.baseURL}`;
  private baseMemberDetailsUrl = `${this.baseUrl}SocietyMemberApi/GetSocietyMemberDetails`;
  private baseSocitiesMembersListUrl = `${this.baseUrl}SocietyMember/api/SocietyTranslationApi/GetSocietiesMembersList`;
  private basemgrivance = `${environment.baseURL}SocietyMemberApiEnquiryApi/GetComplaintData`;
  private postbasemgrivance = `${environment.baseURL}SocietyMemberApiEnquiryApi/SaveUpdate`;

  private _GetAllSocietyTransData = `${environment.baseURL}SocietyMember/api/SocietyTranslationApi/GetAllSocietyTransData`;
  private _GetSocietyDetail = `${environment.baseURL}SocietyMember/api/SocietyTranslationApi/GetSocietyDetail`;
  private _GetSocietiesDropDown = `${environment.baseURL}SocietyMember/api/SocietyTranslationApi/GetSocietiesDropDown`;
  private _GetDistrictDropDown = `${environment.baseURL}UserManagement/api/UserControllerAPI/GetDistrict`;
  private _GetARCSDropDown = `${environment.baseURL}SocietyMember/api/SocietyTranslationApi/GetARCSDropDown`;

  headers = new HttpHeaders()
    .set("Access-Control-Allow-Origin", "*")
    .set("Content-Type", "application/json")
    .append("Authorization", "Bearer " + localStorage.getItem("access_token"));
  constructor(private http: HttpClient) { }

  getMembersListOfSocities(params: any): Observable<MembersListViewModel> {
    return this.http
      .get<MembersListViewModel>(`${this.baseSocitiesMembersListUrl}`, {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      })
      .pipe(catchError(this.error));
  }

  getSocietyMemberDetails(MemberSNo: number): Observable<IMemberDetails> {
    return this.http
      .get<IMemberDetails>(`${this.baseMemberDetailsUrl}/${MemberSNo}`)
      .pipe(catchError(this.handleError));
  }


  GetSocietiesDropDown(params: any): Observable<SocietiesDropDown> {
    return this.http
      .get<SocietiesDropDown>(`${this._GetSocietiesDropDown}`, {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      })
      .pipe(catchError(this.error));
  }
  
  GetDistrictDropDown(): Observable<DistrictDropDown> {
    return this.http
      .get<DistrictDropDown>(`${this._GetDistrictDropDown}`, {
        headers: this.headers,
        observe: "body",
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      })
      .pipe(catchError(this.error));
  }

  GetARCSDropDown(params: any): Observable<ARCSDropDown> {

    return this.http
      .get<ARCSDropDown>(`${this._GetARCSDropDown}`, {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      })
      .pipe(catchError(this.error));
  }

  GetMembersListOfSocities(params: any): Observable<MemberViewModel> {
    return this.http
      .get<MemberViewModel>(`${this.baseSocitiesMembersListUrl}`, {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      })
      .pipe(catchError(this.error));
  }



  private handleError(error: HttpErrorResponse) {
    console.error("server error:", error);
    if (error.error instanceof Error) {
      let errMessage = error.error.message;
      return throwError(() => new Error(errMessage));
    }
    return throwError(() => new Error(error.message || "server error"));
  }

  getmgrivance() {
    return this.http
      .get<any>(`${this.basemgrivance}`, {
        headers: this.headers,
        observe: "body",
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      })
      .pipe(catchError(this.error));
  }
  viewmgrivance(id: any) {
    return this.http
      .get<any>(`${this.basemgrivance}`, {
        headers: this.headers,
        observe: "body",
        params: id,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      })
      .pipe(catchError(this.error));
  }
  savemgrivance(formData: any) {
    return this.http
      .post<any>(this.postbasemgrivance, formData, { headers: this.headers })
      .pipe(catchError(this.error));
  }

  GetAllSocietyTransData(params: any): Observable<SocietyViewModel> {
    return this.http
      .get<SocietyViewModel>(`${this._GetAllSocietyTransData}`, {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      })
      .pipe(catchError(this.error));
  }

  GetSocietyDetail(params: any): Observable<SocietyDetailsModels> {
    return this.http
      .get<SocietyDetailsModels>(`${this._GetSocietyDetail}`, {
        headers: this.headers,
        observe: "body",
        params: params,
        reportProgress: true,
        responseType: "json",
        withCredentials: false
      })
      .pipe(catchError(this.error));
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
