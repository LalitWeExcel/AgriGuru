import {
  Component,
  ElementRef,
  EventEmitter,
  OnInit,
  Output,
  ViewChild
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
import {
  AuditSaveRequest,
  CommanDropDown,
  view_GetAuditRequest
} from "../../../shared/models/audit-model-request.model";
import { ToasterService } from "../../../services/toaster.service";

@Component({
  selector: "app-audit-officer-request-send",
  templateUrl: "./audit-officer-request-send.component.html",
  styleUrls: ["./audit-officer-request-send.component.scss"]
})
export class AuditOfficerRequestSendComponent implements OnInit {
  @ViewChild("myFile") myFile: ElementRef;

  public IsAuthorised: boolean = false;
  public formData = new FormData();
  public basurl = environment.baseURL;
  public loading = false;
  public pagenumber = 1;
  public pageSize = 10;
  public recordsCount = 0;
  public IsShowButtion: boolean = false;
  public pageSizes = [10, 20, 50, 100];
  public title: string = "";
  public message: string = "";

  public audit_Officer_Id: number = 0;
  public audit_financial_year_Id: number = 0;
  public audit_SocietyId: number = 0;
  public audit_PresidentRequest_Id: number = 0;
  public audit_Status_Id: number = 0;
  public audit_Month_Id: number = 0;

  public audit_Auditor_Id: number = 0;
  public audit_arcsCode: number = 0;
  public audit_Status: string = '';
  public _auditOfficerlist!: view_GetAuditRequest[];
  public finYears: CommanDropDown[] = [];
  public _AuditorAssign: CommanDropDown[] = [];
  public _planerStatus: CommanDropDown[] = [];
  public _popopplaner: CommanDropDown[] = [];

  public masterSelected: boolean;
  public checkedList: any;

  public _monthList: { id: number; name: string }[] = [
    { id: 4, name: "April" },
    { id: 5, name: "May" },
    { id: 6, name: "June" },
    { id: 7, name: "July" },
    { id: 8, name: "Auhest" },
    { id: 9, name: "September" },
    { id: 10, name: "October" },
    { id: 11, name: "November" },
    { id: 12, name: "December" },
    { id: 1, name: "January" },
    { id: 2, name: "February" },
    { id: 3, name: "March" }
  ];

  constructor(
    private _auditService: AuditservicesService,
    private jwtHelper: JwtHelperService,
    private SpinnerService: NgxSpinnerService,
    private toaster: ToasterService,
    public sanitizer: DomSanitizer
  ) { }

  ngOnInit() {
    this.masterSelected = false;
    this.SpinnerService.show();
    this.isUserAuthenticated();
    this.audit_Officer_Id = Number(localStorage.getItem("userID"));
    this.GetFinancialYear();
    this.GetAuditPlanerStatusDropDown();
    this.GetAuditAuditorDropDownByOfficer();
    this.GetAuditOfficerRequestSendData();
  }

  public checkUncheckAll() {
    for (var i = 0; i < this._auditOfficerlist.length; i++) {
      this._auditOfficerlist[i].isSelected = this.masterSelected;
    }
    this.getCheckedItemList();
  }
  public isAllSelected() {
    this.masterSelected = this._auditOfficerlist.every(function (item: any) {
      return item.isSelected == true;
    });
    this.getCheckedItemList();
  }
  public getCheckedItemList() {
    this.checkedList = [];
    for (var i = 0; i < this._auditOfficerlist.length; i++) {
      if (this._auditOfficerlist[i].isSelected)
        this.checkedList.push(this._auditOfficerlist[i]);
    }
    if (this.checkedList.length > 0) {
      this.IsShowButtion = true;
    } else {
      this.IsShowButtion = false;
    }
  }

  public PageChanged(event: number): void {
    this.pagenumber = event;
    this.masterSelected = false;
    this.GetAuditOfficerRequestSendData();
  }

  public PageSizeChange(event: any): void {
    this.pageSize = event.target.value;
    this.pagenumber = 1;
    this.GetAuditOfficerRequestSendData();
  }

  public GetAuditOfficerRequestSendData() {
    const params = this.RequestParams(
      this.pagenumber,
      this.pageSize,
      this.audit_Officer_Id,
      this.audit_Auditor_Id,
      1,
      2,
      this.audit_Status_Id
    );
    this.loading = true;
    this._auditService.GetAuditOfficerRequestSendData(params).subscribe({
      next: (response: any) => {
        this._auditOfficerlist = response.data;
        this.recordsCount = response.recordsCount;
        console.log(this._auditOfficerlist);
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

  public GetFinancialYear() {
    this._auditService
      .GetAuditFinancialYearSocietyWise({ societyId: this.audit_SocietyId })
      .subscribe({
        next: (response: any) => {
          this.finYears = response;
        }
      });
  }

  public GetAuditAuditorDropDownByOfficer() {
    let params: any = {};
    (params[`audit_Officer_Id`] = this.audit_Officer_Id),
      this._auditService.GetAuditAuditorDropDownByOfficer(params).subscribe({
        next: (response: any) => {
          this._AuditorAssign = response;
        }
      });
  }

  public GetAuditPlanerStatusDropDown() {
    this._auditService.GetAuditPlanerStatusDropDown().subscribe({
      next: (response: any) => {
        this._planerStatus = response;
      }
    });
  }

  public onYearChange() {
    this.GetAuditOfficerRequestSendData();
  }
  public onAuditorChange() {
    this.GetAuditOfficerRequestSendData();
  }
  public onStatusChange() {
    this.GetAuditOfficerRequestSendData();
  }

  public RequestParams(
    pagenumber: number,
    pageSize: number,
    audit_Officer_Id: number,
    audit_Auditor_Id: number,
    audit_financial_year_Id: number,
    audit_Month_Id: number,
    audit_Status_Id: number
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
    if (audit_Auditor_Id) {
      params[`audit_Auditor_Id`] = audit_Auditor_Id;
    }
    if (audit_financial_year_Id) {
      params[`audit_financial_year_Id`] = audit_financial_year_Id;
    }
    if (audit_Month_Id) {
      params[`audit_Month_Id`] = audit_Month_Id;
    }
    if (audit_Status_Id) {
      params[`audit_Status_Id`] = audit_Status_Id;
    }
    return params;
  }

  public SendRequest(): any {
    this.CloseConfirm();
    this._popopplaner = this._planerStatus;
    this._popopplaner = this._popopplaner.filter((x) => x.name !== 'To Be Initiated');
    document.getElementById("SendRequest").style.display = "block";
  }

  public SendsAuditRequest(): any {

    if (this.audit_Status_Id === 0) {
      this.toaster.error("Please Enter Status", "Error");
    }
    else {

      let params: any = {};
      params[`audit_Status_Id`] = this.audit_Status_Id;
      params[`audit_FinancialYear_Id`] = this.audit_financial_year_Id;
      params[`audit_Month_Id`] = this.audit_Month_Id;

      this.audit_Status = this._planerStatus.filter(x => x.id === this.audit_Status_Id) .map(result => result.name).toString();
      this.SendsRequestforAudit("Confirmation !!", "Are you sure, you want to  Sends “ " + this.audit_Status + " ” For  selected register society.?", this.checkedList, params , function () { });
    }
  }

  public SendsRequestforAudit(
    title: string,
    message: string,
    requestList: any,
    params: any,
    callback: () => void
  ): void {
    this.title = title;
    this.message = message;
    document.getElementById("ConfirmModal").style.display = "block";

    $("#btn_confirmyes")
      .off("click")
      .click(() => {
        if (callback !== null) {
          this._auditService.SendsAuditRequestforSocietyPresidentFromAuditOfficer(requestList, params)
            .subscribe(status => {
              if (status == true) {
                debugger;
                this.clearDropdowns();
                this.toaster.success("Auditor officer Sends “ " + this.audit_Status + " ” For selected register society Successfully", "Success" );
              } else {
                this.toaster.error("Error in Auditor officer Sends “ " + this.audit_Status + " ” For selected register society !!",  "error");
              }
            });
        }
      });
  }

  public SendsReminderforARCS(request: view_GetAuditRequest): any {
    let params: any = {};
    params[`audit_Officer_LoginId`] = request.audit_Officer_LoginId;
    params[`audit_Auditor_Id`] = request.auditor_LoginId;
    params[`audit_SocietyId`] = request.audit_SocietyId;
    params[`society_President_LoginId`] = request.society_President_LoginId;
    params[`audit_financial_year_Id`] = request.audit_FinancialYear_Id;
    params[`audit_Status_Id`] = request.auditStatus_Id;

    $("#btn_save").removeAttr("disabled");
    $("#btn_confirmyes").removeAttr("disabled");
  }

  public CloseConfirm(): void {
    this.title = "";
    this.message = "";
    this.audit_Status_Id = 0;


    document.getElementById("ConfirmModal").style.display = "none";
    document.getElementById("SendRequest").style.display = "none";

    $("#btn_save").removeAttr("disabled");
    $("#btn_confirmyes").removeAttr("disabled");
  }

  public clearDropdowns() {
    this.audit_SocietyId = 0;
    this.audit_financial_year_Id = 0;
    this.audit_Month_Id = 0;
    this.audit_Auditor_Id = 0;
    this.audit_Status_Id = 0;
    this.audit_arcsCode = 0;
    this.CloseConfirm();
    this.GetAuditOfficerRequestSendData();
  }
}

