import { Component, ElementRef, NgModule, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgxSpinnerService } from 'ngx-spinner';
import { MemberdetailsService } from "../../services/member-services/memberdetails.service";
import { SocietyDetailsModels } from 'src/app/shared/models/society-trans-model.model';
import { ToasterService } from 'src/app/services/toaster.service';
import { ElectionRequest } from 'src/app/shared/models/election/election_model';
import { ElectionServiceService } from 'src/app/services/election/election-service.service';

import {
  FormGroup,
  FormControl,
  FormBuilder,
  NgForm,
  Validators,
  AbstractControl,
} from '@angular/forms';
import { Utility } from 'src/app/shared/utility/utils';
import { DomSanitizer } from '@angular/platform-browser';


@Component({
  selector: 'app-election-request',
  templateUrl: './election-request.component.html',
  styleUrls: ['./election-request.component.scss']
})

export class ElectionRequestComponent {

  SocietyDetailsModels!: SocietyDetailsModels;
  imageURL: string;
  withintime:boolean =false;
  afterdue:boolean = false; 
  id: number = 0;
  model = new ElectionRequest();

  submitted = false;
  isLoading = false;
  loadingTitle = 'Loading';
  fileType = "";
  newFile: File;
  file_ProceedingDoc:File;


  constructor(
    private toast: ToasterService,
    private _memberService: MemberdetailsService,
    private eService: ElectionServiceService,
    private SpinnerService: NgxSpinnerService,
    private router: Router,
    public sanitizer: DomSanitizer
  ) {
    this.id = Number(localStorage.getItem("userID")!);
    this.GetSocietyDetail(this.id, 0);
  }
  ngOnInit() {
    this.model.isWithinTimeLimit=null;
    this.model.pO_BOA_Id=null;
  }

  @ViewChild('myFile') myFile: ElementRef
  @ViewChild('myFile_procedingDoc') myFile_procedingDoc: ElementRef

  GetSocietyDetail(userId: number, societyId: number): void {
    let params: any = {};
    params[`userId`] = userId;
    params[`societyId`] = societyId;

    this._memberService.GetSocietyDetail(params).subscribe({

      next: (response: any) => {
        this.SocietyDetailsModels = response.data;      
        console.log(this.SocietyDetailsModels);
        this.SpinnerService.hide();
      }

    });
  }
  errMsg = "";
  isErrMsg = false;
  successMsg = "";
  isSuccessMsg = false;
  formValidate = false;
  isElectionHeldOnErr = false;
  isDateOfIssueErr = false;
  isDateOfMeetingErr = false;
  isImage = false;

  SaveElectionRequest(form: NgForm) {
    debugger
    // if (this.model.lastElectionHeldOn == "" || this.model.lastElectionHeldOn == undefined) {
    //   this.errMsg = "Please select Last Election Held On";
    //   this.isErrMsg = true;
    //   this.isSuccessMsg = false;
    // }
    // else if (this.model.dateOfIssue == "" || this.model.dateOfIssue == undefined) {
    //   this.errMsg = "Please select Date of Agenda Issue";
    //   this.isErrMsg = true;
    //   this.isSuccessMsg = false;
    // } else if (this.model.dateOfHoldingGBMM == "" || this.model.dateOfHoldingGBMM == undefined) {
    //   this.errMsg = "Please select Date of Holding GB Meeting";
    //   this.isErrMsg = true;
    //   this.isSuccessMsg = false;
    // }
    // else {


      var date = new Utility();

      this.model.societyId = this.SocietyDetailsModels.societyId;
      this.model.societyTransactionId = this.SocietyDetailsModels.societyTransID;
      this.model.userId = this.SocietyDetailsModels.userId;
      this.model.agendaHoldingGBMDoc = "No ducument Available";
      this.model.createdDate = date.formatDate(new Date());
      this.model.createdBy = this.id;
      this.model.status = 0;
      this.model.updatedBy = 0;
      this.model.updatedDate = date.formatDate(new Date());
      if(this.electionType=='true'){
        this.model.pO_BOA_Id=0;
      }
      // if (this.model.dateOfIssue == "" || this.model.dateOfIssue == undefined) {
      //   this.model.dateOfIssue=date.formatDate(new Date());
      //  }
      //  if (this.model.dateOfHoldingGBMM == "" || this.model.dateOfHoldingGBMM == undefined) {
      //   this.model.dateOfHoldingGBMM =date.formatDate(new Date());
      //   }
      // this.model.uploadFile = this.newFile;
      //  this.model.fileType = this.fileType;

      // this.model.remarks = CKEDITOR.instances.content.getData()

      console.log(this.model);

      var formData = new FormData();
      Object.entries(this.model)?.forEach(([key, value]) => {
        formData.append(key, value);
      });

      if (this.newFile) {
        formData.append("file", this.newFile)
      }
      if (this.file_ProceedingDoc){
        formData.append("file_ProceedingDoc",this.file_ProceedingDoc);
      }



      this.eService.saveElectionRequest(formData).subscribe({
        next: (res: any) => {
          console.log(res, 'Response');
          this.isLoading = false;
          if (res.isSuccess) {
            this.toast.success(res.massage);

            this.isErrMsg = false;
            this.isSuccessMsg = true;
            this.successMsg = "New Request Submit Success";

            this.clearForm();
            // this.router.navigate(['/bye-laws-request-list']); 
          } else {
            this.toast.info(res.massage);
            this.isErrMsg = true;
            this.isSuccessMsg = false;
            this.errMsg = res.massage;
          }
        },
        error: (err: any) => {
          this.isLoading = false;
          console.log(err, 'Error');
          this.toast.info(err.massage);
          this.isErrMsg = true;
          this.isSuccessMsg = false;
          this.errMsg = err.massage;
        },
      });



    // }
  }

  clearForm() {
    this.model.societyId = 0
    this.model.societyTransactionId = ""
    this.model.userId = 0
    this.model.agendaHoldingGBMDoc = ""
    this.model.createdDate = ""
    this.model.createdBy = 0
    this.model.status = 0;
    this.model.updatedBy = 0;
    this.model.updatedDate = ""
    this.model.lastElectionHeldOn = ""
    this.model.dateOfIssue = ""
    this.model.dateOfHoldingGBMM = ""
    this.model.remarks = ""

    this.router.navigate(['/election-requestlist']);
  }
    imageURL_ProceedingDoc:string;
    fileName:string;
  onFileSelected(event){
    const file = (event.target as HTMLInputElement).files[0];
    // File Preview
    var size = file.size / 1024;
    this.fileType = file.type;

    if (size < 500) {
      if (this.fileType === 'application/pdf') {
        this.isImage = false;
      } else {
        this.isImage = true;

      }
      this.file_ProceedingDoc = file;
      this.fileName=file.name;
      const reader = new FileReader();
      reader.onload = () => {
        this.imageURL_ProceedingDoc = reader.result as string;
        console.log(this.imageURL);
      }
      reader.readAsDataURL(file)
    } else {
      this.isErrMsg = true;
      this.errMsg = 'Please select less then 500kb';
      this.toast.error('', 'Please select less then 500kb')
    }
  }
    fileNameAgenda:string;
  fileUpload(event) {
    const file = (event.target as HTMLInputElement).files[0];
    // File Preview
    var size = file.size / 1024;
    this.fileType = file.type;
    this.fileNameAgenda=file.name;
    if (size < 500) {
      if (this.fileType === 'application/pdf') {
        this.isImage = false;
      } else {
        this.isImage = true;

      }
      this.newFile = file;
      const reader = new FileReader();
      reader.onload = () => {
        this.imageURL = reader.result as string;
        console.log(this.imageURL);
      }
      reader.readAsDataURL(file)
    } else {
      this.isErrMsg = true;
      this.errMsg = 'Please select less then 500kb';
      this.toast.error('', 'Please select less then 500kb')
    }
  }

  clearUploadFile() {
    this.myFile.nativeElement.value = '';
    this.imageURL = null;
  }


  openViewModel() {
    var statusModal = document.getElementById('statusModal');
    if (statusModal != null) {
      statusModal.style.display = 'block';
    }
  }

  closeViewModel() {
    var statusModal = document.getElementById('statusModal');
    if (statusModal != null) {
      statusModal.style.display = 'none';
    }
  }

  electionType = "";

  onElectionLimitation(value:string): void {
    debugger
		this.electionType = value;
    console.log(value);
	}

  onRequestForChange(value:string): void {
		//this.electionType = value;
    console.log(value);
	}

  filterchanges(data:any){
    if(data.target.value == "widthin"){
           this.withintime = true;
           this.afterdue = false;
    }else{
      this.afterdue = true;
      this.withintime = false;
    }
    
     
  }

  btnClick(){
    this.router.navigateByUrl('/election-requestlist');
  }

}







