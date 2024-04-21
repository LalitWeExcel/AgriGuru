import {
  Component,
  ElementRef,
  EventEmitter,
  OnInit,
  Output,
  ViewChild,
} from "@angular/core";
import html2canvas from "html2canvas";
import jspdf from "jspdf";
import { NgxSpinnerService } from "ngx-spinner";
import { JwtHelperService } from "@auth0/angular-jwt";
import { FormBuilder, FormControl, FormGroup, NgForm } from "@angular/forms";
import { DomSanitizer } from "@angular/platform-browser";
import { environment } from "src/environments/environment";
import { HttpEventType } from "@angular/common/http";
import { AuditservicesService } from "../../../services/auditservices.service";
import { AuditSaveRequest, CommanDropDown, view_GetAuditRequest } from "../../../shared/models/audit-model-request.model";
import { ToasterService } from "../../../services/toaster.service";


@Component({
  selector: 'app-audit-officer-dash-board',
  templateUrl: './audit-officer-dash-board.component.html',
  styleUrls: ['./audit-officer-dash-board.component.scss']
})

export class AuditOfficerDashBoardComponent implements OnInit {
  @ViewChild("myFile") myFile: ElementRef;

  public IsAuthorised: boolean = false
  public formData = new FormData();
  public basurl = environment.baseURL;
  public loading = false;
  public pagenumber = 1;
  public pageSize = 10;
  public recordsCount = 0;
  public pageSizes = [10, 20, 50, 100];
  public amountLebel: string = "Profitable Amount";

  public audit_Officer_Id: number = 0;
  public _audit_finalSubmit_date: string = '';
  public audit_SocietyId: number = 0;
  public audit_PresidentRequest_Id: number = 0;
  public auditStatus_Id: number = 0;
  public audit_Auditor_Id: number = 0;


  public auditRemarks = '';
  public fileType = "";
  public isImage = false;
  public imageURL: string = "";
  public errMsg = "";
  public isErrMsg = false;
  public successMsg = "";
  public isSuccessMsg = false;
  public auditsocId: number = 0;
  public hero: any;
  public toJrAuditor = true;
  public title: string = "";
  public message: string = "";
  public buttionLebel: string = "Forward";

  public _auditOfficerlist!: view_GetAuditRequest[];
  public _auditArcsModel!: view_GetAuditRequest
  public auditCurrentStatusModel: view_GetAuditRequest[] = [];
  public model!: AuditSaveRequest;
  public _AuditorAssign: CommanDropDown[] = [];

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
    this.audit_Officer_Id = Number(localStorage.getItem("userID"));
    this.GetAuditAuditorRequestData();
  }
  public GetAuditAuditorRequestData() {
    const params = this.RequestParams(
      this.pagenumber,
      this.pageSize,
      this.audit_Officer_Id,
      this.auditStatus_Id,
      this.audit_SocietyId,
      this._audit_finalSubmit_date,
      this.audit_Auditor_Id
    );
    this.loading = true;
    this._AuditService.GetAuditOfficerRequestData(params).subscribe({
      next: (response: any) => {
        this._auditOfficerlist = response.data;
        this.recordsCount = response.recordsCount;
        console.log(this._auditOfficerlist);
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
    audit_Officer_Id: number,
    auditStatus_Id: number,
    audit_SocietyId: number,
    _audit_finalSubmit_date: string,
    audit_Auditor_Id: number
  ): any {
    let params: any = {};
    if (pagenumber) {
      params[`pagenumber`] = pagenumber;
    }
    if (pageSize) {
      params[`pageSize`] = pageSize;
    }
    if (audit_Officer_Id) {
      params[`audit_Officer_Id`] = audit_Officer_Id;
    }
    if (auditStatus_Id) {
      params[`audit_Status_Id`] = auditStatus_Id;
    }
    if (audit_SocietyId) {
      params[`audit_society_Id`] = audit_SocietyId;
    }
    if (_audit_finalSubmit_date) {
      params[`audit_finalSubmit_date`] = _audit_finalSubmit_date;
    }
    if (audit_Auditor_Id) {
      params[`audit_auditor_Id`] = audit_Auditor_Id;
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

  public OpenReportRequest(_auditArcsRequestlist: view_GetAuditRequest): any {
    var statusModal = document.getElementById("statusInspectorModal");
    if (statusModal != null) {
      statusModal.style.display = "block";
      this._auditArcsModel = _auditArcsRequestlist;

      if (this._auditArcsModel.isProfitable == true)
        this.amountLebel = 'Profitable Amount'
      else
        this.amountLebel = 'Loss  Amount'

      this.audit_SocietyId = _auditArcsRequestlist.audit_SocietyId;
      this.GetAuditAuditHistoryStatus();
    }
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

  public closeReportsMode() {
    document.getElementById("statusInspectorModal").style.display = "none";
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

  public openStatusModel() {
    document.getElementById("statusModal").style.display = "block";
    this.audit_SocietyId = 0;
    this.audit_PresidentRequest_Id = 0;
    this.auditRemarks = "";
    this.auditStatus_Id = 0;
    this.audit_Auditor_Id = 0;
    this.isErrMsg = false;
    this.isSuccessMsg = true;
    this.errMsg = "";
    this.fileType = "";
    this.imageURL = "";
    this.currentupload = '';
    this.currentuploadName = '';
    $("#btn_save").removeAttr("disabled");
    $("#btn_confirmyes").removeAttr("disabled");
  }

  public closeStatusModel() {
    document.getElementById("statusModal").style.display = "none";
    document.getElementById("CallBackModal").style.display = "none";
    this.audit_Auditor_Id = 0;
    $("#btn_save").removeAttr("disabled");
    $("#btn_confirmyes").removeAttr("disabled");
  }
  public DeleteUploadFile() {
    this.imageURL = "";
    this.fileType = '';
    this.currentupload = '';
    this.currentuploadName = '';
  }

  public CloseConfirm(): void {
    this.title = "";
    this.message = "";
    document.getElementById("ConfirmModal").style.display = "none";
    $("#btn_save").removeAttr("disabled");
    $("#btn_confirmyes").removeAttr("disabled");
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


  public CallBackModal(request: view_GetAuditRequest): any {

    this.audit_PresidentRequest_Id = request.audit_PresidentRequest_Id;
    this.audit_SocietyId = request.audit_SocietyId;
    this.audit_Officer_Id = request.audit_Officer_LoginId;
    this.auditStatus_Id = request.auditStatus_Id;
    this.auditRemarks = "";

    $("#btn_save").removeAttr("disabled");
    $("#btn_confirmyes").removeAttr("disabled");
    document.getElementById("CallBackModal").style.display = "block";
  }

  public SendBackAuditOrder(): void {
    this.formData = new FormData();

    if (this.auditRemarks == "") {
      this.toaster.error("Please Enter Auditor  Remarks", "Error");
    } else if (this.currentupload == undefined) {
      this.toaster.error("Please Select Audit Send Back Docoments", "Error");
    } else {

      this.model.auditPresidentRequestId = Number(this.audit_PresidentRequest_Id);
      this.model.audit_Officer_Id = Number(this.audit_Officer_Id);
      this.model.audit_SocietyId = Number(this.audit_SocietyId);
      this.model.auditStatus_Id = Number(this.auditStatus_Id);
      this.model.auditRemarks = String(this.auditRemarks);

      Object.entries(this.model)?.forEach(([key, value]) => {
        this.formData.append(key, value);
      });

      if (this.currentupload) {
        this.formData.append("file", this.currentupload);
      }

      this.ConfirmationMsg("Confirmation !!", "Are you sure, you want to send back this record?", function () { });
    }
  }

  public ConfirmationMsg(
    title: string,
    message: string,
    callback: () => void
  ): void {
    this.title = title;
    this.message = message;
    document.getElementById("ConfirmModal").style.display = "block";
    $("#btn_confirmyes").off("click").click(() => {
      if (callback !== null) {

        $("#btn_save").attr("disabled", "disabled");
        $("#btn_confirmyes").attr("disabled", "disabled");

        this._AuditService.SendBackAuditFileToPersidentFromAuditOfficer(this.formData).subscribe((status) => {
          if (status == true) {
            this.GetAuditAuditorRequestData();
            this.toaster.success("Audit Request Send Back To President Successfully", "Success");
          } else {
            this.toaster.error("Error in Audit Send Back!!", "error");
          }

          this.closeStatusModel();
          this.CloseConfirm();
          this.DeleteUploadFile();

          $("#btn_save").removeAttr("disabled");
          $("#btn_confirmyes").removeAttr("disabled");
        });

      }
    });
  }



  public OpenAuditorRequest(request: view_GetAuditRequest): any {
    this.openStatusModel();
    this.audit_PresidentRequest_Id = request.audit_PresidentRequest_Id;
    this.audit_SocietyId = request.audit_SocietyId;
    this.GetAuditAuditorDropDownByOfficer();
  }

  public GetAuditAuditorDropDownByOfficer() {
    let params: any = {};
    params[`audit_Officer_Id`] = this.audit_Officer_Id,
      this._AuditService.GetAuditAuditorDropDownByOfficer(params).subscribe({
        next: (response: any) => {
          this._AuditorAssign = response;
        },
      });
  }


  public ForwardtoAuditor(): void {
    let IsVaild = true;
    if (this.audit_Auditor_Id == 0 || this.audit_Auditor_Id == undefined) {
      this.toaster.error("Please Select Auditor", "Error");
      IsVaild = false;
    } else if (this.currentupload == undefined) {
      this.toaster.error("Please Select Audit Docoments", "Error");
      IsVaild = false;
    } else if (this.auditRemarks == "") {
      this.toaster.error("Please Enter Auditor  Remarks", "Error");
      IsVaild = false;
    }
    if (IsVaild) {
      this.model.auditPresidentRequestId = Number(this.audit_PresidentRequest_Id);
      this.model.audit_Auditor_Id = Number(this.audit_Auditor_Id);
      this.model.auditRemarks = String(this.auditRemarks);
      this.model.audit_SocietyId = Number(this.audit_SocietyId);

      this.formData = new FormData();
      Object.entries(this.model)?.forEach(([key, value]) => {
        this.formData.append(key, value);
      });

      if (this.currentupload) { this.formData.append("file", this.currentupload); }
      this.SaveConfirmationMsg("Confirmation !!", "Are you sure, you want to assign this record?", function () { });
    }
  }

  public SaveConfirmationMsg(title: string, message: string, callback: () => void): void {
    this.title = title;
    this.message = message;
    document.getElementById("ConfirmModal").style.display = "block";
    $("#btn_confirmyes").off("click").click(() => {
      if (callback !== null) {
        $("#btn_save").attr("disabled", "disabled");
        $("#btn_confirmyes").attr("disabled", "disabled");

        this._AuditService.AuditForwardtoAuditor(this.formData).subscribe((result) => {
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

        this.closeStatusModel();
        this.CloseConfirm();
        this.DeleteUploadFile();

        $("#btn_save").removeAttr("disabled");
        $("#btn_confirmyes").removeAttr("disabled");
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

}
