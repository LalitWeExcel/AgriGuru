import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError,  } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { IMemberDetails, SocietyMembersListModal} from '../m-models/memberdetails';
import { environment } from 'src/environments/environment';
import { memberAreaPath } from './constants';

@Injectable({
  providedIn: 'root'
})

export class MemberdetailsService {
 private baseUrl = `${environment.baseURL}${memberAreaPath}`;
 private baseMemberDetailsUrl=`${this.baseUrl}/GetSocietyMemberDetails`;
 private baseSocitiesMembersListUrl= `${this.baseUrl}/GetSocietiesMembersList`;
 private basemgrivance = `${environment.baseURL}Enquiry/api/EnquiryApi/GetComplaintData`;
 private postbasemgrivance =`${environment.baseURL}Enquiry/api/EnquiryApi/SaveUpdate`;
  private url_SocietyMemberBackTrack = `${this.baseUrl}/GetSocietyMemberList_BackTrack`;
  headers = new HttpHeaders()
  .set("Access-Control-Allow-Origin", "*")
  .set("Content-Type", "application/json")
  .append("Authorization", "Bearer " + localStorage.getItem("access_token"));
  
  headerMultiPart=new HttpHeaders()
  .set("Access-Control-Allow-Origin", "*")
  
  .append("Authorization", "Bearer " + localStorage.getItem("access_token"));

 constructor(private http:HttpClient) { }

 getMembersListOfSocities_BackTrack(MemberSNo:number):Observable<any>
 {
   return this.http.get<any>(`${this.url_SocietyMemberBackTrack}/${MemberSNo}`,{headers:this.headers})
    .pipe(
      catchError(this.handleError)
    );
 }

 getMembersListOfSocities(userId:number):Observable<any>
 {
    return this.http.get<any>(`${this.baseSocitiesMembersListUrl}/${userId}`)
    .pipe(
      catchError(this.handleError)
    );
 }

 getUserFile(url:string)
 {
  return this.http.get<any>(url,{headers:this.headers})
  .pipe(
    catchError(this.handleError)
  );
 }
  getSocietyMemberDetails(MemberSNo:number):Observable<IMemberDetails>{
    return this.http.get<IMemberDetails>(`${this.baseMemberDetailsUrl}/${MemberSNo}`,{headers:this.headers})
    .pipe(
      catchError(this.handleError)
    );
  }
  private handleError(error: HttpErrorResponse) {
    console.error('server error:', error); 
    if (error.error instanceof Error) {
      let errMessage = error.error.message;
      return throwError(() => new Error(errMessage));
      // Use the following instead if using lite-server
      //return Observable.throw(err.text() || 'backend server error');
    }
    return throwError(() => new Error(error.message || 'server error'));
}
   
getmgrivance(){
  return this.http.get(this.basemgrivance,{headers:this.headers});
}
viewmgrivance(id:any){
  return this.http.get(`${this.basemgrivance}/${id}`,{headers:this.headers});
}
savemgrivance(data:any){
  // const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'multipart/form-data', }) };
  return this.http.post(this.postbasemgrivance,data,{headers:this.headerMultiPart});
}


}
