import { Component } from '@angular/core';
import { SocietyMemberBackTrack } from '../m-models/memberdetails';
import { MemberdetailsService } from '../m-services/memberdetails.service';
import { DomSanitizer } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { memberAreaPath } from '../m-services/constants';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { ActivatedRoute, ParamMap, Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
@Component({
  selector: 'app-m-memberbacktrack',
  templateUrl: './m-memberbacktrack.component.html',
  styleUrls: ['./m-memberbacktrack.component.scss']
})
export class MMemberbacktrackComponent {
  societyMemberBackTrackList!:SocietyMemberBackTrack[];
  isLoading = true;
  loadingTitle = "Loading";
  memberId:any=0;
  roleName:any;
  IsAuthorised = true;
  useridQP: number;
  societyId: number;
  societyName: string;
  constructor(private memberService:MemberdetailsService,
    private _sanitizer: DomSanitizer,
    private jwtHelper: JwtHelperService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    ){
      this.roleName=localStorage.getItem('roleName');
    }

  ngOnInit(){
    this.isUserAuthenticated();
    this.activatedRoute.queryParamMap.subscribe(params => {

      this.useridQP = Number(params.get("Id"));
      this.societyId = Number(params.get("sId"));
      this.societyName = String(params.get("sName"));
    });
    if(this.memberId===0){
      this.memberId=localStorage.getItem('memberSNo');
      if(this.memberId==0){
        this.memberId=sessionStorage.getItem('memberSNo');
       }
     }
    this.getSocitiesMembersList_BackTrack(this.memberId);//211725
  }
  isUserAuthenticated() {
    const token = localStorage.getItem("access_token");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.IsAuthorised = true;
    } else {
      this.IsAuthorised = false;
    }
  }
  getSocitiesMembersList_BackTrack(memberSNo:number){
    this.isLoading = true;
    this.loadingTitle = "Loading Details";
    this.memberService.getMembersListOfSocities_BackTrack(memberSNo)
       .subscribe({
     next:(response:any)=>{
      if(response.status==200)
      {
        this.societyMemberBackTrackList = response.data;
        
        // console.log(response.data);
        for (var key in this.societyMemberBackTrackList) {
          if (this.societyMemberBackTrackList.hasOwnProperty(key)) {
            // console.log(key);
            this.societyMemberBackTrackList[key].previousOwner.newImageSrc=this.societyMemberBackTrackList[key].previousOwner.newImageSrc==''?"/imgpath/dummy.jpg":this.societyMemberBackTrackList[key].previousOwner.newImageSrc;
            this.societyMemberBackTrackList[key].previousOwner.affidavidUrl=`${environment.baseURL+memberAreaPath}/GetPdfFile/11/`;
            this.societyMemberBackTrackList[key].previousOwner.identityUrl=`${environment.baseURL+memberAreaPath}/GetPdfFile/12/`;
            this.societyMemberBackTrackList[key].previousOwner.affidavidUrl+=`${this.societyMemberBackTrackList[key].previousOwner.societyTransID}/${this.societyMemberBackTrackList[key].previousOwner.memberSNo}`;
            this.societyMemberBackTrackList[key].previousOwner.identityUrl+=`${this.societyMemberBackTrackList[key].previousOwner.societyTransID}/${this.societyMemberBackTrackList[key].previousOwner.memberSNo}`;
            // var val = this.societyMemberBackTrackList[key].previousOwner.memberSNo;
            // console.log(val);
          }
          // console.log(this.societyMemberBackTrackList);
        }
        
      }
      else{
        // console.log(response.data );
      }
      this.isLoading = false;
       
     },
     error:(err:any)=>{console.log(err);this.isLoading=false;}
  
     }
     ) ;
  
   }

   downloadPdf(base64String:string,fileName:string): void {
    const byteCharacters = atob(base64String);
    const byteArrays = [];
   let filename=fileName+'.pdf';
    for (let offset = 0; offset < byteCharacters.length; offset += 512) {
      const slice = byteCharacters.slice(offset, offset + 512);
      const byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }
      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }

    const blob = new Blob(byteArrays, { type: 'application/pdf' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    link.click();
    window.URL.revokeObjectURL(url);
  }
  public openPDF(): void {
    this.isLoading = true;
    this.loadingTitle="Generating PDF";
    let DATA: any = document.getElementById('content');
    html2canvas(DATA).then((canvas) => {
      let fileWidth = 208;
      let fileHeight = (canvas.height * fileWidth) / canvas.width;
      const FILEURI = canvas.toDataURL('image/png');
      let PDF = new jsPDF('p', 'mm', 'a4');
      let position = 0;
      PDF.addImage(FILEURI, 'PNG', 0, position, fileWidth, fileHeight);
      PDF.save('memberdetails.pdf');
      this.isLoading=false;
    });
  }

  BackRequest() {
    this.router.navigate(['/admin-member-list', this.useridQP, this.societyId, this.societyName]);
  }
}
