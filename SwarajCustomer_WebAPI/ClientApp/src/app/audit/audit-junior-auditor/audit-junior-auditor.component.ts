import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from "@angular/core";
import { NgxSpinnerService } from "ngx-spinner";
import { JwtHelperService } from "@auth0/angular-jwt";
import { ToasterService } from "../../services/toaster.service";
import { FormBuilder, FormControl, FormGroup, NgForm } from "@angular/forms";
import { AuditservicesService } from "../../services/auditservices.service";
import { AuditSaveRequest, CommanDropDown, view_GetAuditRequest, } from "../../shared/models/audit-model-request.model";
import { Role } from "../../shared/models/role-wise-module.model";
import { AuditorDropDown, SocietiesDropDown } from "../../shared/models/society-trans-model.model";
import { DomSanitizer } from "@angular/platform-browser";
import { environment } from "../../../environments/environment";
import html2canvas from "html2canvas";
import jspdf from "jspdf";
import { HttpEventType } from "@angular/common/http";

@Component({
  selector: "audit-junior-auditor",
  templateUrl: "./audit-junior-auditor.component.html",
  styleUrls: ["./audit-junior-auditor.component.scss"]
})

export class AuditJuniorAuditorComponent implements OnInit {

  @ViewChild('myFile') myFile: ElementRef
  public basurl = environment.baseURL;
  public loading = false;
  public pagenumber = 1;
  public pageSize = 10;
  public recordsCount = 0;
  public pageSizes = [10, 20, 50, 100];

  public _auditArcsRequestlist!: view_GetAuditRequest[];
  public _auditArcsModel!: view_GetAuditRequest;
  public model!: AuditSaveRequest;

  public Role: Role[] = [];
  public _JuniorAuditor: CommanDropDown[] = [];
  public auditCurrentStatusModel: view_GetAuditRequest[] = [];
  public SocietiesDropDown!: SocietiesDropDown[];
  public IsAuthorised = true;
  public audit_Auditor_Id: number = 0;
  public audit_Officer_LoginId: number = 0;
  public audit_PresidentRequest_Id: number = 0;
  public junior_Auditor_LoginId: number = 0;
  public auditStatus_Id: number = 0;
  public auditOrderDate: Date;
  public auditRequestedDate: Date;
  public auditDatesRange: string = "";
  public auditRemarks: string = "";
  public uploadfilename: any

  public AuditorDropDown!: AuditorDropDown[];
  public fileType = "";
  public isImage = false;
  public imageURL: string = "";
  public errMsg = "";
  public isErrMsg = false;
  public successMsg = "";
  public isSuccessMsg = false;
  public audit_SocietyId = 0;

  public hero: any;
  public toJrAuditor = true;
  public title: string = "";
  public message: string = "";
  public amountLebel: string = "Profitable Amount";
  public buttionLebel: string = "Forward";
  public AssignPopopLebel: string = 'Assign Audit File to Jr. Auditor ';
  public formData = new FormData();


  public currentupload: any;
  public currentuploadName: any;

  constructor(
    private _AuditService: AuditservicesService,
    private jwtHelper: JwtHelperService,
    private SpinnerService: NgxSpinnerService,
    private toaster: ToasterService,
    public sanitizer: DomSanitizer
  ) {
    this._auditArcsModel = {} as view_GetAuditRequest;
    this.model = {} as AuditSaveRequest;
  }

  ngOnInit() {

    this.junior_Auditor_LoginId = Number(localStorage.getItem("userID"));

    this.SpinnerService.show();

    this.isUserAuthenticated();

    this.GetJuniorAuditorSocietiesDropDown();

    this.GetAuditAuditorDropDown();

    this.GetAuditJuniorAuditorRequestData();

  }

  public GetAuditJuniorAuditorRequestData() {

    this.loading = true;
    const params = {
      pagenumber: this.pagenumber,
      pageSize: this.pageSize,
      junior_Auditor_LoginId: this.junior_Auditor_LoginId,
      societyid: this.audit_SocietyId,
      audit_Auditor_Id: this.audit_Auditor_Id,
      auditDatesRange: this.auditDatesRange
    };
    this._AuditService.GetAuditJuniorAuditorRequestData(params).subscribe({
      next: (response: any) => {
        this._auditArcsRequestlist = response.data;
        this.recordsCount = response.recordsCount;
        console.log(this._auditArcsRequestlist);
        console.log("recordsCount :  " + this.recordsCount);
        this.loading = false;
        this.SpinnerService.hide();
      }
    });
  }

  public isUserAuthenticated() {
    const token = localStorage.getItem("access_token");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.IsAuthorised = true;
    } else {
      this.IsAuthorised = false;
    }
  }


  public PageChanged(event: number): void {
    this.pagenumber = event;
    this.GetAuditJuniorAuditorRequestData();
  }

  public PageSizeChange(event: any): void {
    this.pageSize = event.target.value;
    this.pagenumber = 1;
    this.GetAuditJuniorAuditorRequestData();
  }

  public formatDate(dateString: string): string {

    const dateObject = new Date(dateString);

    const year = dateObject.getFullYear();
    const month = ('0' + (dateObject.getMonth() + 1)).slice(-2); // Months are zero-indexed
    const day = ('0' + dateObject.getDate()).slice(-2);

    return `${year}-${month}-${day}`;
  }

  public OpenReportRequest(_auditArcsRequestlist: view_GetAuditRequest): any {
      document.getElementById("statusInspectorModal").style.display = "block";   
      this._auditArcsModel = _auditArcsRequestlist
      if (this._auditArcsModel.isProfitable == true)
        this.amountLebel = 'Profitable  Amount'
      else
        this.amountLebel = 'Loss  Amount'

      this.audit_SocietyId = _auditArcsRequestlist.audit_SocietyId;
      this.GetAuditAuditHistoryStatus();
  }
  public closeStatusModel() {

    document.getElementById("statusModal").style.display = "none";
    document.getElementById("CallBackModal").style.display = "none";

    this.auditDatesRange = "";
    this.auditRemarks = "";
    this.currentupload = "";
    this.uploadfilename = false;

    $("#btn_save").removeAttr('disabled');
    $("#btn_confirmyes").removeAttr('disabled');

  }

  public setDatepicker(_this) {
    this.GetAuditJuniorAuditorRequestData();
  }

  private formatDate1(dateString) {
    let dateArray = dateString.split("-");
    let newDate = dateArray[2] + "/" + dateArray[1] + "/" + dateArray[0];
    return newDate;
  }
  public GetAuditAuditHistoryStatus() {
    let params: any = {};
    params[`audit_SocietyId`] = this._auditArcsModel.audit_SocietyId;
    params[`audit_PresidentRequest_Id`] = this._auditArcsModel.audit_PresidentRequest_Id;

    this.SpinnerService.show();
    this._AuditService.GetAuditAuditHistoryStatus(params).subscribe({
      next: (response: any) => {
        this.auditCurrentStatusModel = response.data;
        this.SpinnerService.hide();
      }
    });
  }

  public closeReportsMode() {
    document.getElementById("statusInspectorModal").style.display = "none";
    this.audit_SocietyId = 0;
    this.audit_PresidentRequest_Id = 0;
    this.auditDatesRange = "";
    this.auditRemarks = "";

    this.currentupload = "";
    this.uploadfilename = '';

    $("#btn_save").removeAttr('disabled');
    $("#btn_confirmyes").removeAttr('disabled');

  }

  public fileUpload(event) {
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
        this.currentuploadName = target.files[0].name;
      }
      this.currentupload = file;

      const reader = new FileReader();
      reader.onload = () => {
        this.imageURL = reader.result as string;
      };
      reader.readAsDataURL(file);
    } else {
      this.isErrMsg = true;
      this.errMsg = "Please select less then 5 MB";
      this.toaster.error("", "Please select less then 5 MB");
    }
  }


  public CloseConfirm(): void {
    this.title = '';
    this.message = '';
    document.getElementById("ConfirmModal").style.display = "none";
    this.currentupload = "";
    this.uploadfilename = false;
    $("#btn_save").removeAttr('disabled');
    $("#btn_confirmyes").removeAttr('disabled');
  }

  public openStatusModel() {
    this.audit_SocietyId = 0;
    this.audit_PresidentRequest_Id = 0;
    this.auditStatus_Id = 0;
    this.auditDatesRange = "";
    this.auditRemarks = "";
    this.currentupload = '';
    this.currentuploadName = '';
    $("#btn_save").removeAttr('disabled');
    $("#btn_confirmyes").removeAttr('disabled');
  }

  public OpenForWardRequest(request: view_GetAuditRequest): any {
    this.openStatusModel();
    document.getElementById("statusModal").style.display = "block";
    this.model.auditPresidentRequestId = request.audit_PresidentRequest_Id;
    this.model.audit_Officer_Id = request.audit_Officer_LoginId;
    this.model.audit_Auditor_Id = request.auditor_LoginId;
    this.model.junior_Auditor_LoginId = request.junior_Auditor_LoginId;
    this.model.audit_SocietyId = request.audit_SocietyId;
    this.model.auditStatus_Id = request.auditStatus_Id;
  }
  public submitForm() {

    if (this.currentupload == undefined) {
      this.toaster.error("Please Select Audit Final  Docoments", "Error");
    }

    else if (this.auditRemarks == '') {
      this.toaster.error("Please Enter Jr. Auditor Remarks", "Error");
    }
    else {
      this.formData = new FormData();
      this.model.auditRemarks = this.auditRemarks;
      Object.entries(this.model)?.forEach(([key, value]) => { this.formData.append(key, value); });

      if (this.currentupload) { this.formData.append("file", this.currentupload) }
      this.ConfirmAuditorOrderMessage('Confirmation !!', 'Are you sure, you want to save this record?', function () { });
    }

  }
  public ConfirmAuditorOrderMessage(title: string, message: string, callback: (() => void)): void {
    this.title = title;
    this.message = message;
    document.getElementById("ConfirmModal").style.display = "block";


    $("#btn_confirmyes").off('click').click(() => {
      if (callback !== null) {

        $("#btn_save").attr('disabled', 'disabled');
        $("#btn_confirmyes").attr('disabled', 'disabled');
        for (const entry in this.formData) {
          console.log(entry);
        }

        this._AuditService.SubmitAuditFromJuniorAuditor(this.formData).subscribe(
          result => {
            if (result.status == true) {
              this.toaster.success("Audit Order Successfully", "Success");
              this.GetAuditJuniorAuditorRequestData();
            }
            else {
              this.toaster.error("Error in Audit Order!!", "error");
            }
          });

        $("#btn_save").removeAttr('disabled');
        $("#btn_confirmyes").removeAttr('disabled');

        this.CloseConfirm();
        this.DeleteUploadFile();
        this.closeStatusModel();
      }
    });
  }


  public CallBackModal(request: view_GetAuditRequest): any {

    document.getElementById("CallBackModal").style.display = "block";
    this.audit_PresidentRequest_Id = request.audit_PresidentRequest_Id;
    this.audit_Officer_LoginId = request.audit_Officer_LoginId;
    this.audit_Auditor_Id = request.auditor_LoginId;
    this.junior_Auditor_LoginId = request.junior_Auditor_LoginId;
    this.audit_SocietyId = request.audit_SocietyId;
    this.auditStatus_Id = request.auditStatus_Id;

    this.auditDatesRange = "";
    this.auditRemarks = "";
    this.currentupload = "";
    this.uploadfilename = false;

    $("#btn_save").removeAttr('disabled');
    $("#btn_confirmyes").removeAttr('disabled');

  }
  public SendBackAuditOrder(): void {

    this.formData = new FormData();
    if (this.currentupload == undefined) {
      this.toaster.error("Please Select Audit Send Back Docoments", "Error");
    }
    else if (this.auditRemarks == '') {
      this.toaster.error("Please Enter Jr. Auditor Remarks", "Error");
    }
    else {

      this.model.auditPresidentRequestId = this.audit_PresidentRequest_Id;
      this.model.junior_Auditor_LoginId = this.junior_Auditor_LoginId;
      this.model.audit_Officer_Id = this.audit_Officer_LoginId;
      this.model.audit_Auditor_Id = this.audit_Auditor_Id;
      this.model.audit_SocietyId = this.audit_SocietyId;
      this.model.auditStatus_Id = this.auditStatus_Id;
      this.model.auditRemarks = String(this.auditRemarks);

      Object.entries(this.model)?.forEach(([key, value]) => { this.formData.append(key, value); });

      if (this.currentupload) { this.formData.append("file", this.currentupload) }
      this.ConfirmSendBackAuditOrderessage('Confirmation !!', 'Are you sure, you want to send back this record?', function () { });

    }

  }
  public ConfirmSendBackAuditOrderessage(title: string, message: string, callback: (() => void)): void {
    this.title = title;
    this.message = message;
    document.getElementById("ConfirmModal").style.display = "block";


    $("#btn_confirmyes").off('click').click(() => {
      if (callback !== null) {

        $("#btn_save").attr('disabled', 'disabled');
        $("#btn_confirmyes").attr('disabled', 'disabled');
        this._AuditService.SendBackJuniorAuditorToAuditor(this.formData).subscribe(status => {
          if (status == true) {
            this.toaster.success("Audit Request Send Back To ARCS Successfully", "Success");
            this.GetAuditJuniorAuditorRequestData();
          } else {
            this.toaster.error("Error in Audit Send Back!!", "error");
          }
        });


        this.CloseConfirm();
        this.DeleteUploadFile();
        this.closeStatusModel();

        $("#btn_save").removeAttr('disabled');
        $("#btn_confirmyes").removeAttr('disabled');
      }
    });
  }


  public GetDownloadFile(fileName: string) {

    let params: any = {};
    params[`fileName`] = fileName;
    this._AuditService.GetDownloadFile(params).subscribe((event) => {
      if (event.type === HttpEventType.Response) {
        const downloadedFile = new Blob([event.body], { type: event.body.type });
        const a = document.createElement('a');
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        a.download = this.generateGUID();
        a.href = URL.createObjectURL(downloadedFile);
        a.click();
        document.body.removeChild(a);
      }
    });
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


  public GetAuditAuditorDropDown() {
    let params: any = {};
    params[`divCode`] = 0;

    this._AuditService.GetAuditAuditorDropDown(params).subscribe({
      next: (response: any) => {
        this.AuditorDropDown = response.data;
      }
    });
  }

  public onARCDropDownChange() {
    this.GetJuniorAuditorSocietiesDropDown();
    this.GetAuditJuniorAuditorRequestData();
  }

  public GetJuniorAuditorSocietiesDropDown() {
    let params: any = {};
    params[`junior_Auditor_LoginId`] = this.junior_Auditor_LoginId;

    this._AuditService.GetJuniorAuditorSocietiesDropDown(params).subscribe({
      next: (response: any) => {
        this.SocietiesDropDown = response;
      }
    });
  }

  public onSocietyDropDownChange() {
    this.GetAuditJuniorAuditorRequestData();
  }

  public clearDropdowns() {
    this.currentupload = '';
    this.audit_SocietyId = 0;
    this.auditDatesRange = "";
    this.audit_Auditor_Id = 0;
    this.GetAuditJuniorAuditorRequestData();
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

  public DeleteUploadFile() {
    this.imageURL = "";
    this.fileType = '';
    this.currentupload = '';
    this.currentuploadName = '';
  }
}
