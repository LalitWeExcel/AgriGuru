import {
  Component,
  ElementRef,
  EventEmitter,
  OnInit,
  Output,
  ViewChild,
} from "@angular/core";
import { NgxSpinnerService } from "ngx-spinner";
import { JwtHelperService } from "@auth0/angular-jwt";
import { ToasterService } from "../../services/toaster.service";
import { FormBuilder, FormControl, FormGroup, NgForm } from "@angular/forms";
import { AuditservicesService } from "../../services/auditservices.service";
import { CommanDropDown, AuditSaveRequest, view_GetAuditRequest } from "../../shared/models/audit-model-request.model";
import { Role } from "../../shared/models/role-wise-module.model";
import { SocietiesDropDown } from "../../shared/models/society-trans-model.model";
import { DomSanitizer } from "@angular/platform-browser";
import { environment } from "src/environments/environment";
import { CellClass } from "../../shared/utility/utils";
import html2canvas from "html2canvas";
import jspdf from "jspdf";
import { HttpEventType } from "@angular/common/http";
import { debounce } from "rxjs";

@Component({
  selector: "app-audit-auditor-request",
  templateUrl: "./audit-auditor-request.component.html",
  styleUrls: ["./audit-auditor-request.component.scss"],
})
export class AuditAuditorRequestComponent implements OnInit {
  @ViewChild("myFile1") myFile1: ElementRef;
  @ViewChild("myFile2") myFile2: ElementRef;
  @ViewChild("myFile3") myFile3: ElementRef;

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
  public societyid: number = 0;
  public audit_PresidentRequest_Id: number = 0;
  public junior_Auditor_LoginId: number = 0;
  public auditStatus_Id: number = 0;
  public auditOrderDate: Date;
  public auditRequestedDate: Date;
  public auditDatesRange: string = "";
  public auditRemarks: string = "";

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
    this.SpinnerService.show();

    this.isUserAuthenticated();

    this.audit_Auditor_Id = Number(localStorage.getItem("userID"));

    this.GetAuditorSocietiesDropDown();

    this.GetAuditAuditorRequestData();
  }

  public GetAuditAuditorRequestData() {
    const params = this.RequestParams(
      this.pagenumber,
      this.pageSize,
      this.audit_Auditor_Id,
      this.societyid,
      this.auditDatesRange
    );
    this.loading = true;

    this._AuditService.GetAuditAuditorRequestData(params).subscribe({
      next: (response: any) => {
        this._auditArcsRequestlist = response.data;
        this.recordsCount = response.recordsCount;
        console.log(this._auditArcsRequestlist);

        this.loading = false;
        this.SpinnerService.hide();
      },
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

  public RequestParams(
    pagenumber: number,
    pageSize: number,
    audit_Auditor_Id: number,
    societyid: number,
    auditDatesRange: string
  ): any {
    let params: any = {};
    if (pagenumber) {
      params[`pagenumber`] = pagenumber;
    }

    if (pageSize) {
      params[`pageSize`] = pageSize;
    }
    if (audit_Auditor_Id) {
      params[`audit_Auditor_Id`] = audit_Auditor_Id;
    }
    if (societyid) {
      params[`societyid`] = societyid;
    }
    if (auditDatesRange) {
      params[`auditDatesRange`] = auditDatesRange;
    }

    return params;
  }

  public PageChanged(event: number): void {
    this.pagenumber = event;
    this.GetAuditAuditorRequestData();
  }

  public PageSizeChange(event: any): void {
    this.pageSize = event.target.value;
    this.pagenumber = 1;
    this.GetAuditAuditorRequestData();
  }

  public OpenJuniorAuditorRequest(request: view_GetAuditRequest): any {
    this.openStatusModel();
    this.audit_PresidentRequest_Id = request.audit_PresidentRequest_Id;
    this.audit_Officer_LoginId = request.audit_Officer_LoginId;
    this.audit_Auditor_Id = request.auditor_LoginId;
    this.junior_Auditor_LoginId = request.junior_Auditor_LoginId;
    this.auditStatus_Id = request.auditStatus_Id;
    this.audit_SocietyId = request.audit_SocietyId;
    this.GetJuniorAuditorDropDown();
  }

  public openStatusModel() {
    var statusModal = document.getElementById("statusModal");
    if (statusModal != null) {
      statusModal.style.display = "block";
    }

    this.societyid = 0;
    this.audit_PresidentRequest_Id = 0;
    this.junior_Auditor_LoginId = 0;
    this.auditDatesRange = "";
    this.auditRemarks = "";
    this.auditStatus_Id = 0;
    this.isErrMsg = false;
    this.isSuccessMsg = true;
    this.errMsg = "";
    this.fileType = "";
    this.imageURL = "";

    $("#btn_save").removeAttr("disabled");
    $("#btn_confirmyes").removeAttr("disabled");
  }

  public CallBackModal(request: view_GetAuditRequest): any {
    this.audit_PresidentRequest_Id = request.audit_PresidentRequest_Id;
    this.audit_Officer_LoginId = request.audit_Officer_LoginId;
    this.audit_Auditor_Id = request.auditor_LoginId;
    this.societyid = request.audit_SocietyId;
    this.auditStatus_Id = request.auditStatus_Id;
    document.getElementById("CallBackModal").style.display = "block";
    this.auditRemarks = "";

    $("#btn_save").removeAttr("disabled");
    $("#btn_confirmyes").removeAttr("disabled");
  }




  public GetJuniorAuditorDropDown() {
    let params: any = {};
    params[`audit_Officer_LoginId`] = this.audit_Officer_LoginId;
    params[`audit_Auditor_Id`] = this.audit_Auditor_Id;
    params[`junior_Auditor_LoginId`] = this.junior_Auditor_LoginId;
    params[`audit_SocietyId`] = this.audit_SocietyId;

    this._AuditService.GetJuniorAuditorDropDown(params).subscribe({
      next: (response: any) => { this._JuniorAuditor = response; },
    });
  }

  public SaveAuditOrder(): void {
    let IsVaild = true;
    if (this.toJrAuditor == true) {
      if (this.junior_Auditor_LoginId == 0 || this.junior_Auditor_LoginId == undefined) {
        this.toaster.error("Please Select Junior Auditor Circle Users", "Error");
        IsVaild = false;
      } else if (this.currentupload == undefined) {
        this.toaster.error("Please Select Audit Docoments", "Error");
        IsVaild = false;
      } else if (this.auditRemarks == "") {
        this.toaster.error("Please Enter Auditor  Remarks", "Error");
        IsVaild = false;
      }
    }
    if (this.toJrAuditor == false) {
      if (this.currentupload == undefined) {
        this.toaster.error("Please Select Audit Docoments", "Error");
        IsVaild = false;
      } else if (this.auditRemarks == "") {
        this.toaster.error("Please Enter Auditor  Remarks", "Error");
        IsVaild = false;
      }
    }
    if (IsVaild) {
      this.model.auditPresidentRequestId = Number(this.audit_PresidentRequest_Id);
      this.model.audit_Officer_Id = Number(this.audit_Officer_LoginId);
      this.model.audit_Auditor_Id = Number(this.audit_Auditor_Id);
      this.model.junior_Auditor_LoginId = Number(this.junior_Auditor_LoginId);
      this.model.audit_SocietyId = Number(this.audit_SocietyId);
      this.model.auditStatus_Id = Number(this.auditStatus_Id);
      this.model.auditRemarks = String(this.auditRemarks);
      this.model.toJrAuditor = Boolean(this.toJrAuditor);

      this.formData = new FormData();
      Object.entries(this.model)?.forEach(([key, value]) => {
        this.formData.append(key, value);
      });

      if (this.currentupload) {
        this.formData.append("file", this.currentupload);
      }
      this.SaveConfirmationMsg("Confirmation !!", "Are you sure, you want to assign this record?", function () { });
    }
  }

  public SaveConfirmationMsg(
    title: string,
    message: string,
    callback: () => void
  ): void {
    this.title = title;
    this.message = message;
    document.getElementById("ConfirmModal").style.display = "block";

    $("#btn_confirmyes").off("click").click(() => { if (callback !== null) { this.AssignJuniorAuditorOrder(); } });
  }

  public AssignJuniorAuditorOrder() {
    $("#btn_save").attr("disabled", "disabled");
    $("#btn_confirmyes").attr("disabled", "disabled");

    this._AuditService.AssignJuniorAuditorOrder(this.formData).subscribe((result) => {
      if (result.status == true) {
        this.GetAuditAuditorRequestData();
        this.toaster.success("Audit Order Successfully", "Success");
        this.isErrMsg = false;
        this.isSuccessMsg = true;
      } else {
        this.toaster.error("Error in Audit Order!!", "error");
        this.isErrMsg = true;
        this.isSuccessMsg = false;
      }
    });

    this.CloseConfirm();
    this.DeleteUploadFile();
    this.closeStatusModel();
    $("#btn_save").removeAttr("disabled");
    $("#btn_confirmyes").removeAttr("disabled");
  }

  public SendBackAuditToAuditorofficer(): void {
    this.formData = new FormData();

    if (this.auditRemarks == "") {
      this.toaster.error("Please Enter Auditor  Remarks", "Error");
    } else if (this.currentupload == undefined) {
      this.toaster.error("Please Select Audit Send Back Docoments", "Error");
    } else {

      this.model.auditPresidentRequestId = Number(this.audit_PresidentRequest_Id);
      this.model.audit_Auditor_Id = Number(this.audit_Auditor_Id);
      this.model.auditRemarks = String(this.auditRemarks);
      this.model.auditStatus_Id = Number(this.auditStatus_Id);
      this.model.audit_SocietyId = Number(this.societyid);
      this.model.audit_Officer_Id = Number(this.audit_Officer_LoginId);

      Object.entries(this.model)?.forEach(([key, value]) => {
        this.formData.append(key, value);
      });

      if (this.currentupload) {
        this.formData.append("file", this.currentupload);
      }
      this.ConfirmationMsg("Confirmation !!", "Are you sure, you want to send back this record?", function () { });
    }
  }

  public ConfirmSendBack() {
    $("#btn_save").attr("disabled", "disabled");
    $("#btn_confirmyes").attr("disabled", "disabled");

    this._AuditService.SendBackAuditToAuditorofficer(this.formData).subscribe((status) => {
      if (status == true) {
        this.GetAuditAuditorRequestData();
        this.toaster.success("Audit Request Send Back To Auditor officer Successfully", "Success");

      } else {
        this.toaster.error("Error in Audit Send Back!!", "error");
      }
    });

    this.CloseConfirm();
    this.DeleteUploadFile();
    this.closeStatusModel();

    $("#btn_save").removeAttr("disabled");
    $("#btn_confirmyes").removeAttr("disabled");
  }

  public ConfirmationMsg(
    title: string,
    message: string,
    callback: () => void
  ): void {
    this.title = title;
    this.message = message;
    document.getElementById("ConfirmModal").style.display = "block";

    $("#btn_confirmyes")
      .off("click")
      .click(() => {
        if (callback !== null) {
          this.ConfirmSendBack();
        }
      });
  }

  public closeStatusModel() {
    document.getElementById("statusModal").style.display = "none";
    document.getElementById("CallBackModal").style.display = "none";
    $("#btn_save").removeAttr("disabled");
    $("#btn_confirmyes").removeAttr("disabled");
  }

  public GetAuditorSocietiesDropDown() {
    let params: any = {};
    params[`auditor_LoginId`] = this.audit_Auditor_Id,
      this._AuditService.GetAuditorSocietiesDropDown(params).subscribe({
        next: (response: any) => {
          this.SocietiesDropDown = response;
        },
      });
  }

  public onSocietyDropDownChange() {
    this.GetAuditAuditorRequestData();
  }

  public clearDropdowns() {
    this.societyid = 0;
    this.auditDatesRange = "";
    this.GetAuditAuditorRequestData();
  }

  public setDatepicker(_this) {
    this.GetAuditAuditorRequestData();
  }

  private formatDate1(dateString) {
    let dateArray = dateString.split("-");
    let newDate = dateArray[2] + "/" + dateArray[1] + "/" + dateArray[0];
    return newDate;
  }

  public OpenDocomentsModel(): any {
    var statusModal = document.getElementById("documentModel");
    if (statusModal != null) {
      statusModal.style.display = "block";
    }
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

  public DeleteUploadFile() {
    this.imageURL = "";
    this.fileType = '';
    this.currentupload = '';
    this.currentuploadName = '';
  }

  public GetAuditAuditHistoryStatus() {
    let params: any = {};
    params[`audit_SocietyId`] = this._auditArcsModel.audit_SocietyId;
    params[`audit_PresidentRequest_Id`] =
      this._auditArcsModel.audit_PresidentRequest_Id;

    this.SpinnerService.show();
    this._AuditService.GetAuditAuditHistoryStatus(params).subscribe({
      next: (response: any) => {
        this.auditCurrentStatusModel = response.data;
        this.SpinnerService.hide();
      },
    });
  }

  public OpenReportRequest(_auditArcsRequestlist: view_GetAuditRequest): any {
    document.getElementById("statusInspectorModal").style.display = "block";
    this._auditArcsModel = _auditArcsRequestlist;
    if (this._auditArcsModel.isProfitable == true)
      this.amountLebel = 'Profitable Amount'
    else
      this.amountLebel = 'Loss Amount'

    this.audit_SocietyId = _auditArcsRequestlist.audit_SocietyId;
    this.GetAuditAuditHistoryStatus();
  }

  public closeReportsMode() {
    document.getElementById("statusInspectorModal").style.display = "none";
    this.currentupload = '';
  }

  public CloseConfirm(): void {
    this.title = "";
    this.message = "";
    document.getElementById("ConfirmModal").style.display = "none";

    $("#btn_save").removeAttr("disabled");
    $("#btn_confirmyes").removeAttr("disabled");
    this.currentupload = '';
  }

  public GetDownloadFile(fileName: string) {
    let params: any = {};
    params[`fileName`] = fileName;
    this._AuditService.GetDownloadFile(params).subscribe((event) => {
      if (event.type === HttpEventType.Response) {
        const downloadedFile = new Blob([event.body], {
          type: event.body.type,
        });
        const a = document.createElement("a");
        a.setAttribute("style", "display:none;");
        document.body.appendChild(a);
        a.download = this.generateGUID();
        a.href = URL.createObjectURL(downloadedFile);
        //   a.target = '_blank';
        a.click();
        document.body.removeChild(a);
      }
    });
  }
  public captureScreen() {
    var data = document.getElementById("statusInspectorModal");
    html2canvas(data).then((canvas) => {
      var imgWidth = 230;
      var pageHeight = 300;
      var imgHeight = (canvas.height * imgWidth) / canvas.width;
      var heightLeft = imgHeight;

      const contentDataURL = canvas.toDataURL("image/png");
      let pdf = new jspdf("p", "mm", "a4");
      var position = 0;
      pdf.addImage(contentDataURL, "PNG", 0, position, imgWidth, imgHeight);
      pdf.save(this.generateGUID() + ".pdf");
    });
  }
  public generateGUID() {
    return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(
      /[xy]/g,
      function (c) {
        var r = (Math.random() * 16) | 0,
          v = c === "x" ? r : (r & 0x3) | 0x8;
        return v.toString(16);
      }
    );
  }

  public showSelected(toJrAuditor: boolean) {
    this.toJrAuditor = toJrAuditor;
    if (this.toJrAuditor === true) {
      document.getElementById("txtAuditorCircle").style.display = "block";
      this.buttionLebel = "Forward";
      this.AssignPopopLebel = 'Assign Audit File to Jr. Auditor ';
    } else {
      document.getElementById("txtAuditorCircle").style.display = "none";
      this.buttionLebel = "Final Audit Submit ";
      this.AssignPopopLebel = ' Self Assign Audit ';
      this.junior_Auditor_LoginId = 0;
    }
    this.myFile1.nativeElement.value = "";
    this.currentupload = '';
    this.auditRemarks = '';
  }

  public FinalAuditSubmit(audit_PresidentRequest_Id: number,
    audit_SocietyId: number,
    audit_Officer_LoginId: number,
    auditor_LoginId: number,
    junior_Auditor_LoginId: number,
  ): void {
    let params: any = {};
    params[`audit_PresidentRequest_Id`] = audit_PresidentRequest_Id;
    params[`audit_SocietyId`] = audit_SocietyId;
    params[`audit_Officer_LoginId`] = audit_Officer_LoginId;
    params[`auditor_LoginId`] = auditor_LoginId;
    params[`junior_Auditor_LoginId`] = junior_Auditor_LoginId;

    this.finalAuditSubmitMsg("Confirmation !!", "Are you sure, you want to Final Audit Submit this record?", params, function () { });
  }
  public finalAuditSubmitMsg(title: string, message: string, params: any, callback: () => void): void {
    this.title = title;
    this.message = message;
    document.getElementById("ConfirmModal").style.display = "block";

    $("#btn_confirmyes").off("click").click(() => {
      if (callback !== null) {

        $("#btn_FinalAuditSubmit").attr("disabled", "disabled");
        $("#btn_confirmyes").attr("disabled");

        this._AuditService.FinalAuditSubmit(params).subscribe((status) => {
          if (status == true) {
            this.toaster.success("Final Audit Submit Successfully", "Success");
            this.GetAuditAuditorRequestData();
          } else {
            this.toaster.error(
              "Error in Final Audit Submit !!",
              "error"
            );
          }
        });


        this.CloseConfirm();
        this.closeStatusModel();
        this.closeReportsMode();
        this.DeleteUploadFile();

        $("#btn_FinalAuditSubmit").removeAttr("disabled");
        $("#btn_SendBackAuditOrder").removeAttr("disabled");
        $("#btn_SendBackToJuniorAuditor").removeAttr("disabled");
        $("#btn_confirmyes").removeAttr("disabled");
      }
    });
  }


  public OpenSendBackToJuniorAuditor(audit_PresidentRequest_Id: number,
    audit_SocietyId: number,
    audit_Officer_LoginId: number,
    auditor_LoginId: number,
    junior_Auditor_LoginId: number,
  ): void {

    this.CloseSendBackToJuniorAuditor();
    this.model.auditPresidentRequestId = audit_PresidentRequest_Id;
    this.model.audit_SocietyId = audit_SocietyId;
    this.model.audit_Officer_Id = audit_Officer_LoginId;
    this.model.audit_Auditor_Id = auditor_LoginId
    this.model.junior_Auditor_LoginId = junior_Auditor_LoginId;
    document.getElementById("SendBackToJuniorAuditor").style.display = "block";
  }


  public SendBackToJuniorAuditor() {

    if (this.auditRemarks == "") {
      this.toaster.error("Please Enter Auditor  Remarks", "Error");
    } else if (this.currentupload == undefined) {
      this.toaster.error("Please Select Audit Send Back Docoments", "Error");
    } else {

      this.model.auditRemarks = String(this.auditRemarks);

      this.formData = new FormData();
      Object.entries(this.model)?.forEach(([key, value]) => { this.formData.append(key, value);});

      if (this.currentupload) { this.formData.append("file", this.currentupload); }
      this.ConfirmationSendBackToJuniorAuditorMsg("Confirmation !!", "Are you sure, you want to Send Back to Junior Auditor this record?", function () { });
    }
  }

  public ConfirmationSendBackToJuniorAuditorMsg(title: string, message: string, callback: () => void): void {

    this.title = title;
    this.message = message;
    document.getElementById("ConfirmModal").style.display = "block";

    $("#btn_confirmyes").off("click").click(() => {
      if (callback !== null) {

        $("#btn_SendBackAuditOrder").attr("disabled", "disabled");
        $("#btn_confirmyes").attr("disabled");

        this._AuditService.SendBackToJuniorAuditor(this.formData).subscribe((status) => {
          if (status == true) {
            this.toaster.success("Send Back to Junior Auditor Successfully", "Success");
            this.GetAuditAuditorRequestData();

          } else {
            this.toaster.error("Error in Send Back to Junior Auditor !!", "error");
          }
        }); 
        this.CloseSendBackToJuniorAuditor();
        this.CloseConfirm();
        this.closeStatusModel();
        this.closeReportsMode();
        this.DeleteUploadFile();
      }
    });
  }

  public CloseSendBackToJuniorAuditor() {
    document.getElementById("SendBackToJuniorAuditor").style.display = "none";
    document.getElementById("statusInspectorModal").style.display = "none";

    this.fileType = "";
    this.imageURL = "";
    this.audit_PresidentRequest_Id = 0;
    this.audit_SocietyId = 0;
    this.auditRemarks = '';

    $("#btn_FinalAuditSubmit").removeAttr("disabled");
    $("#btn_SendBackAuditOrder").removeAttr("disabled");
    $("#btn_SendBackToJuniorAuditor").removeAttr("disabled");
    $("#btn_confirmyes").removeAttr("disabled");
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
}
