import {
  Component,
  ElementRef,
  EventEmitter,
  OnInit,
  Output,
  ViewChild,
} from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import {
  AbstractControl,
  FormGroup,
  FormBuilder,
  Validators,
} from "@angular/forms";
import { AuditSaveRequest, CommanDropDown, view_GetAuditRequest } from "src/app/shared/models/audit-model-request.model";
import { AuditservicesService } from "../../services/auditservices.service";
import { MemberdetailsService } from "../../services/member-services/memberdetails.service";
import { SocietyDetailsModels } from "src/app/shared/models/society-trans-model.model";
import { NgxSpinnerService } from "ngx-spinner";
import { JwtHelperService } from "@auth0/angular-jwt";
import { ToasterService } from "src/app/services/toaster.service";
import { DomSanitizer } from "@angular/platform-browser";
import { environment } from "src/environments/environment";

import jspdf from 'jspdf';
import html2canvas from 'html2canvas';
import { HttpEventType, HttpResponse } from "@angular/common/http";

@Component({
  selector: "app-audit-president-request",
  templateUrl: "./audit-president-request.component.html",
  styleUrls: ["./audit-president-request.component.scss"]
})
export class AuditPresidentRequestComponent implements OnInit {

  @ViewChild("myFile1") myFile1: ElementRef;
  @ViewChild("myFile2") myFile2: ElementRef;
  @ViewChild("myFile3") myFile3: ElementRef;

  public basurl = environment.baseURL;
  public loading = true;

  public pagenumber = 1;
  public pageSize = 10;
  public recordsCount = 0;
  public pageSizes = [10, 20, 50, 100];
  public auditRemarks: string = "";
  public previousTurnOver: string = "";


  public fileType = "";
  public isImage = false;

  public errMsg = "";
  public isErrMsg = false;
  public successMsg = "";
  public isSuccessMsg = false;
  public title: string = '';
  public message: string = '';


  public AuditPresidentRequestForm: FormGroup = {} as FormGroup;
  public AuditPresidentRequest: view_GetAuditRequest[] = [];
  public _auditArcsModel!: view_GetAuditRequest;
  public _presidentRequest!: AuditSaveRequest;
  public auditCurrentStatusModel: view_GetAuditRequest[] = [];
  public SocietyDetailsModels!: SocietyDetailsModels;


  public errorMessage = "";
  public audit_PresidentRequest_Id: number;
  
  public IsAuthorised = true;
  public defaultValue: any = 0;

  public finYears: CommanDropDown[] = [];
  public userID: number;
  public arcsCode: number;
  public audit_financial_year_Id: number;
  public audit_SocietyId: number;
  
  public formData: FormData;
  public auditStatus_Id: number;
  public isProfitable: boolean = true;
  public amountLebel: string = 'Profitabl Amount';

  public balanseSheetUploadPath: string = "";
  public treadingAccountUploadPath: string = "";
  public profitLossAccountUploadPath: string = "";
  public AllFiletUploadPath: string = "";


  public BalanseSheet: any;
  public BalanseSheetName: any;
  public TreadingAccount: any;
  public TreadingAccountName: any;
  public ProfitLossAccount: any;
  public ProfitLossAccountName: any;
  get f(): { [key: string]: AbstractControl } {
    return this.AuditPresidentRequestForm.controls;
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private jwtHelper: JwtHelperService,
    private auditservices: AuditservicesService,
    private _memberService: MemberdetailsService,
    private formBuilder: FormBuilder,
    private SpinnerService: NgxSpinnerService,
    private toaster: ToasterService,
    public sanitizer: DomSanitizer
  ) {

    this._presidentRequest = {} as AuditSaveRequest
    this._auditArcsModel = {} as view_GetAuditRequest;
  }

  ngOnInit() {
    this.SpinnerService.show();
    this.isUserAuthenticated();
    this.buildForm();

    this.userID = Number(localStorage.getItem("userID"));
    this.arcsCode = Number(localStorage.getItem("arcsCode"));
    this.audit_SocietyId = Number(localStorage.getItem("societyId"));
    this.GetSocietyDetail(this.userID, this.audit_SocietyId);

  }

  GetSocietyDetail(userId: number, societyId: number): void {
    let params: any = {};
    params[`userId`] = userId;
    params[`societyId`] = societyId;

    this._memberService.GetSocietyDetail(params).subscribe({
      next: (response: any) => {
        this.SocietyDetailsModels = response.data;
        this.audit_SocietyId = this.SocietyDetailsModels.societyId;
        this.GetFinancialYear();
        this.GetPresidentAuditRequestData();

        this.SpinnerService.hide();
      },
    });
  }
  buildForm() {
    this.AuditPresidentRequestForm = this.formBuilder.group({
        audit_financial_year_Id: [
        this._presidentRequest.audit_financial_year_Id,
        Validators.required,
      ],
      previousTurnOver: [
        this._presidentRequest.previousTurnOver,
        [Validators.required, Validators.max(999999999)],
      ],
      auditRemarks: [
        this._presidentRequest.auditRemarks,
        Validators.required,
      ],

    });
  }
  hasError(field: string, error: string) {
    const control = this.AuditPresidentRequestForm.get(field);
    return control?.dirty && control?.hasError(error);
  }
  markAllAsDirty(form: FormGroup) {
    for (const control of Object.keys(form.controls)) {
      form.controls[control].markAsDirty();
    }
  }

  GetFinancialYear() {
    this.auditservices.GetAuditFinancialYearSocietyWise({ societyId: this.audit_SocietyId }).subscribe({
        next: (response: any) => {
          this.finYears = response;
        },
      });
  }

  GetPresidentAuditRequestData() {
    const params = this.RequestParams(
      this.pagenumber,
      this.pageSize,
      this.userID,
      this.arcsCode,
      this.audit_SocietyId);
    this.loading = true;
    this.auditservices.GetAuditPresidentData(params).subscribe({
      next: (response: any) => {

        this.AuditPresidentRequest = response.data;

        this.recordsCount = response.recordsCount;

        console.log(this.AuditPresidentRequest);

        this.GetFinancialYear();

        this.SpinnerService.hide();

        this.loading = false;

      },
    });
  }
  isUserAuthenticated() {
    const token = localStorage.getItem("access_token");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.IsAuthorised = true;
    } else {
      this.IsAuthorised = false;
    }
  }

  public PageChanged(event: number): void {
    this.pagenumber = event;
    this.GetPresidentAuditRequestData();
  }

  public PageSizeChange(event: any): void {
    this.pageSize = event.target.value;
    this.pagenumber = 1;
    this.GetPresidentAuditRequestData();
  }

  public RequestParams(
    pagenumber: number,
    pageSize: number,
    userID: number,
    arcsCode: number,
    audit_SocietyId: number,
  ): any {
    let params: any = {};
    if (pagenumber) {
      params[`pagenumber`] = pagenumber;
    }
    if (pageSize) {
      params[`pageSize`] = pageSize;
    }
    if (userID) {
      params[`userID`] = userID;
    }
    if (arcsCode) {
      params[`arcsCode`] = arcsCode;
    }
    if (audit_SocietyId) {
      params[`audit_SocietyId`] = audit_SocietyId;
    }
    return params;
  }

  public OpenViewsModal(): any {
    document.getElementById("OpenViewsModal").style.display = "block";
    this.previousTurnOver = "";
    this.auditRemarks = "";
    this.audit_financial_year_Id = 0;
    this.audit_PresidentRequest_Id = 0;
    this.auditStatus_Id = 0;
    this.balanseSheetUploadPath = "";
    this.treadingAccountUploadPath = "";
    this.profitLossAccountUploadPath = "";
    this.BalanseSheet = null;
    this.TreadingAccount = null;
    this.ProfitLossAccount = null;

    $("#btn_save").removeAttr('disabled');
    $("#btn_confirmyes").removeAttr('disabled');

  }

  public GetAuditAuditHistoryStatus() {
    let params: any = {};
    params[`audit_SocietyId`] = this._auditArcsModel.audit_SocietyId
    params[`audit_PresidentRequest_Id`] = this._auditArcsModel.audit_PresidentRequest_Id


    this.SpinnerService.show();
    this.auditservices.GetAuditAuditHistoryStatus(params).subscribe({
      next: (response: any) => {
        this.auditCurrentStatusModel = response.data;
        this.SpinnerService.hide();
      },
    });
  }

  public OpenReportRequest(_auditArcsRequestlist: view_GetAuditRequest): any {
    var statusModal = document.getElementById("statusInspectorModal");
    if (statusModal != null) {
      statusModal.style.display = "block";
      this._auditArcsModel = _auditArcsRequestlist;

      if (this._auditArcsModel.isProfitable == true)
        this.amountLebel = 'Profitable  Amount'
      else
        this.amountLebel = 'Loss  Amount'

      this.audit_SocietyId = _auditArcsRequestlist.audit_SocietyId;
      this.GetAuditAuditHistoryStatus();
    }
  }

  public UpdateViewsModal(request: view_GetAuditRequest): any {

    document.getElementById("UpdateViewsModal").style.display = "block";
    this.previousTurnOver = "";
    this.auditRemarks = "";
    this.audit_financial_year_Id = 0;

    this.balanseSheetUploadPath = "";
    this.treadingAccountUploadPath = "";
    this.profitLossAccountUploadPath = "";

    this.BalanseSheet = null;
    this.TreadingAccount = null;
    this.ProfitLossAccount = null;

    this.BalanseSheetName = '';
    this.TreadingAccountName = '';
    this.ProfitLossAccountName = '';
    this.audit_PresidentRequest_Id = request.audit_PresidentRequest_Id;
    this.auditStatus_Id = request.auditStatus_Id;

  }


  public closeReportsMode() {
    var statusModal = document.getElementById("statusInspectorModal");
    if (statusModal != null) {
      statusModal.style.display = "none";
    }

    this.audit_SocietyId = this.audit_SocietyId;
  }

  public OnlyNumbers(event) {
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
      event.preventDefault();
      return false;
    }
    return true;
  }

  public Edit({ value, valid }: { value: AuditSaveRequest; valid: boolean }) {

    if (this.previousTurnOver == '') {
      this.toaster.error("Please Enter  " + this.amountLebel, "Error");
    }
    else if (this.BalanseSheet == undefined) {
      this.toaster.error("Please Select Balanse Sheet", "Error");
    }
    else if (this.TreadingAccount == undefined) {
      this.toaster.error("Please Select Trading Account", "Error");
    }
    else if (this.ProfitLossAccount == undefined) {
      this.toaster.error("Please Select Profit Loss Account", "Error");
    }
    else if (this.auditRemarks == '') {
      this.toaster.error("Please Enter President Remarks", "Error");
    }
    else {

      this._presidentRequest.arcsCode = this.arcsCode;
      this._presidentRequest.audit_SocietyId = Number(this.audit_SocietyId);
      this._presidentRequest.audit_financial_year_Id = this.audit_financial_year_Id;
      this._presidentRequest.auditRemarks = this.auditRemarks;
      this._presidentRequest.auditPresidentRequestId = Number(this.audit_PresidentRequest_Id);;
      this._presidentRequest.previousTurnOver = Number(this.previousTurnOver);
      this._presidentRequest.auditStatus_Id = Number(this.auditStatus_Id);
      this._presidentRequest.flags = false;
      this._presidentRequest.isProfitable = this.isProfitable;
      this.formData = new FormData();
      Object.entries(this._presidentRequest)?.forEach(([key, value]) => {
        this.formData.append(key, value);
      });
      this.formData.append("BalanseSheet1", this.BalanseSheet);
      this.formData.append("TreadingAccount1", this.TreadingAccount);
      this.formData.append("ProfitLossAccount1", this.ProfitLossAccount);
      this.ConfirmationMsg('Confirmation !!', 'Are you sure, you want to save this record?', function () { });
    }
  }



  public submit({ value, valid }: { value: AuditSaveRequest; valid: boolean; }) {

    if (this.audit_financial_year_Id == 0 || this.audit_financial_year_Id == undefined) {
      this.toaster.error("Please Select financial Year", "Error");
    }
    else if (this.previousTurnOver == '') {
      this.toaster.error("Please Enter  " + this.amountLebel, "Error");
    }
    else if (this.BalanseSheet == undefined) {
      this.toaster.error("Please Select Balanse Sheet", "Error");
    }
    else if (this.TreadingAccount == undefined) {
      this.toaster.error("Please Select Trading  Account", "Error");
    }
    else if (this.ProfitLossAccount == undefined) {
      this.toaster.error("Please Select Profit Loss Account", "Error");
    }
    else if (this.auditRemarks == '') {
      this.toaster.error("Please Enter President Remarks", "Error");
    }
    else {
      this._presidentRequest.arcsCode = this.arcsCode;
      this._presidentRequest.audit_SocietyId = Number(this.audit_SocietyId);
      this._presidentRequest.audit_financial_year_Id = this.audit_financial_year_Id;
      this._presidentRequest.auditRemarks = this.auditRemarks;
      this._presidentRequest.auditPresidentRequestId = 0;
      this._presidentRequest.previousTurnOver = Number(this.previousTurnOver);
      this._presidentRequest.auditStatus_Id = Number(this.auditStatus_Id);
      this._presidentRequest.flags = true;
      this._presidentRequest.isProfitable = this.isProfitable;
      this.formData = new FormData();


      Object.entries(this._presidentRequest)?.forEach(([key, value]) => {
        this.formData.append(key, value);
      });


      this.formData.append("BalanseSheet1", this.BalanseSheet);
      this.formData.append("TreadingAccount1", this.TreadingAccount);
      this.formData.append("ProfitLossAccount1", this.ProfitLossAccount);

      this.ConfirmationMsg('Confirmation !!', 'Are you sure, you want to save this record?', function () { });

    }
  }

  public ConfirmationMsg(title: string, message: string, callback: (() => void)): void {
    this.title = title;
    this.message = message;
    document.getElementById("ConfirmModal").style.display = "block";

    $("#btn_confirmyes").off('click').click(() => {
      if (callback !== null) {
        this.ConfirmSubmit();
      }
    });
  }

  public ConfirmSubmit() {
    $("#btn_save").attr('disabled', 'disabled');
    $("#btn_confirmyes").attr('disabled', 'disabled');

    this.auditservices.savePresidentAuditRequest(this.formData).subscribe((result) => {
      if (result.status == true) {
        this.GetPresidentAuditRequestData();

        this.toaster.success("Audit request updated successfully", "Success");
        this.isErrMsg = false;
        this.isSuccessMsg = true;
      }
      else {
        this.toaster.error("Error in Audit Request!!", "error");
        this.isErrMsg = true;
        this.isSuccessMsg = false;
      }
    });

    this.GetFinancialYear();
    this.clearUploadFile()
    this.CloseConfirm();

  }

  public CloseConfirm(): void {
    this.title = '';
    this.message = '';
    $("#btn_save").attr('disabled', '');
    $("#btn_confirmyes").attr('disabled', '');
    document.getElementById("ConfirmModal").style.display = "none";

    $("#btn_save").removeAttr('disabled');
    $("#btn_confirmyes").removeAttr('disabled');

  }

  public GetDownloadFile(fileName: string) {

    let params: any = {};
    params[`fileName`] = fileName;
    this.auditservices.GetDownloadFile(params).subscribe((event) => {
      if (event.type === HttpEventType.Response) {
        const downloadedFile = new Blob([event.body], { type: event.body.type });
        const a = document.createElement('a');
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        a.download = this.generateGUID();
        a.href = URL.createObjectURL(downloadedFile);
        //a.target = '_blank';
        a.click();
        document.body.removeChild(a);
      }
    });
  }

  

  public ArrayBufferlob(dataURI) {
    var byteString = atob(dataURI.split(',')[1]);
    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0]
    var ab = new ArrayBuffer(byteString.length);
    var ia = new Uint8Array(ab);
    for (var i = 0; i < byteString.length; i++) {
      ia[i] = byteString.charCodeAt(i);
    }
    var _blob = new Blob([ab], { type: mimeString });
    var fileURL = URL.createObjectURL(_blob);
    var win = window.open();
    win.document.write('<iframe src="' + fileURL + '" frameborder="0" style="border:0; top:0px; left:0px; bottom:0px; right:0px; width:100%; height:100%;" allowfullscreen></iframe>')
}

  public captureScreen() {
    var data = document.getElementById('statusInspectorModal');
    html2canvas(data).then((canvas) => {
      var imgWidth = 230;
      var pageHeight = 300;
      var imgHeight = (canvas.height * imgWidth) / canvas.width;
      var heightLeft = imgHeight;

      const contentDataURL = canvas.toDataURL('image/png');
      let pdf = new jspdf('p', 'mm', 'a4');
      var position = 0;
      pdf.addImage(contentDataURL, 'PNG', 0, position, imgWidth, imgHeight);
      pdf.save(this.generateGUID() + '.pdf');
    });
  }
  public generateGUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0,
        v = c === 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }


  public showSelected(_IsProfitable: boolean) {
    this.isProfitable = _IsProfitable;
    this.previousTurnOver = '';

    if (this.isProfitable == true) {
      this.amountLebel = 'Profitable Amount'
    }
    else {
      this.amountLebel = 'Loss Amount'
    }
  }


  public balanseSheetUpload(event) {

    const file = (event.target as HTMLInputElement).files[0];
    var size = file.size / 1024;
    this.fileType = file.type;
    if (size < 5242880) {
      if (this.fileType === "application/pdf") {
        this.isImage = false;
      } else {
        this.isImage = true;
      }

      const target = event.target as HTMLInputElement;
      if (target.files && target.files.length > 0) {
        this.BalanseSheetName = target.files[0].name;

      }

      this.BalanseSheet = file;

      const reader = new FileReader();
      reader.onload = () => {

        this.balanseSheetUploadPath = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
    else {
      this.isErrMsg = true;
      this.errMsg = "Please select less then 5 MB";
      this.toaster.error("", "Please select less then 5 MB");
    }
  }

  public treadingAccountUpload(event) {
    const file = (event.target as HTMLInputElement).files[0];
    var size = file.size / 1024;
    this.fileType = file.type;

    if (size < 5242880) {
      if (this.fileType === "application/pdf") {
        this.isImage = false;
      } else {
        this.isImage = true;
      }

      const target = event.target as HTMLInputElement;
      if (target.files && target.files.length > 0) {
        this.TreadingAccountName = target.files[0].name;
      }

      this.TreadingAccount = file;
      const reader = new FileReader();
      reader.onload = () => {
        this.treadingAccountUploadPath = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
    else {
      this.isErrMsg = true;
      this.errMsg = "Please select less then 5 MB";
      this.toaster.error("", "Please select less then 5 MB");
    }
  }

  public profitLossAccountUpload(event) {
    const file = (event.target as HTMLInputElement).files[0];
    var size = file.size / 1024;
    this.fileType = file.type;

    if (size < 5242880) {
      if (this.fileType === "application/pdf") {
        this.isImage = false;
      } else {
        this.isImage = true;
      }

      const target = event.target as HTMLInputElement;
      if (target.files && target.files.length > 0) {
        this.ProfitLossAccountName = target.files[0].name;
      }

      this.ProfitLossAccount = file;
      const reader = new FileReader();
      reader.onload = () => {
        this.profitLossAccountUploadPath = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
    else {
      this.isErrMsg = true;
      this.errMsg = "Please select less then 5 MB";
      this.toaster.error("", "Please select less then 5 MB");
    }
  }


  public OpenDocomentsModel(_AllFiletUploadPath: string): any {
    var statusModal = document.getElementById("documentModel");
    if (statusModal != null) {
      statusModal.style.display = "block";
    }
    this.AllFiletUploadPath = _AllFiletUploadPath;
  }



  public clearUploadFile() {

    document.getElementById("OpenViewsModal").style.display = "none";
    document.getElementById("UpdateViewsModal").style.display = "none";
    this.previousTurnOver = "";
    this.auditRemarks = "";
    this.audit_financial_year_Id = 0;
    this.clearbalanseSheet();
    this.cleartreadingAccount();
    this.clearprofitLossAccount();
    this.AuditPresidentRequestForm.reset();

  }

  public clearbalanseSheet() {
    this.balanseSheetUploadPath = "";
    this.BalanseSheet = '';
    this.BalanseSheetName = '';

  }

  public cleartreadingAccount() {
    this.treadingAccountUploadPath = "";
    this.TreadingAccount = '';
    this.TreadingAccountName = '';
  }

  public clearprofitLossAccount() {
    this.profitLossAccountUploadPath = "";
    this.ProfitLossAccount = '';
    this.ProfitLossAccountName = '';
  }
}



