import { Component, OnInit } from '@angular/core';
import { MemberdetailsService } from '../m-services/memberdetails.service';
import { ToasterService } from '../../../app/services/toaster.service';
import { environment } from 'src/environments/environment';
import { pipe } from 'rxjs';
import {
  Form,
  FormControl,
  FormGroup,
  FormsModule,
  NgForm,
  Validators,
  ValidatorFn,
  AbstractControl,
} from '@angular/forms';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { requiredFileType } from './requireFileTypeValidator';
import { fileSizeValidator } from './fileSizeValidator';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-m-grivance',
  templateUrl: './m-grivance.component.html',
  styleUrls: ['./m-grivance.component.scss'],
})
export class MGrivanceComponent implements OnInit {
  complaintform = new FormGroup({
    complaintTitle: new FormControl(null, Validators.required),
    complaintType: new FormControl(null, Validators.required),
    file: new FormControl(null, [
      Validators.required,
      requiredFileType(['pdf']),
    ]),
    description: new FormControl(null, Validators.required),
  });
  IsAuthorised = true;
  path = '';
  complaintmodalview: any;
  getusercomplaint: any;
  file: File | null = null;
  filePath: string = `${environment.baseURL}Enquiry/api/EnquiryApi/GetPdfFile/`;
  constructor(
    private toast: ToasterService,
    private userdata: MemberdetailsService,
    private jwtHelper: JwtHelperService
  ) {}
  hasError(field: string, error: string) {
    const control = this.complaintform.get(field);
    return control?.dirty && control?.hasError(error);
  }



  onselectedfile(event: any) {
    if (event.target.files.length > 0) {
      this.file = event.target.files[0];
      this.complaintform
        .get('file')
        .setValidators([
          Validators.required,
          requiredFileType(['pdf']),
          fileSizeValidator(event.target.files),
        ]);
    }
  }

  fileUploadValidator(allowedExtensions: any): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      // Enter to validation only if has value or it's not undefined
      if (control.value !== undefined && isNaN(control.value)) {
        const file = control.value;
        // Get extension from file name
        const ext = file.substring(file.lastIndexOf('.') + 1);
        // Find extension file inside allowed extensions array
        if (allowedExtensions.includes(ext.toLowerCase())) {
          //allowedExtensions.includes(ext.toLowerCase())
        } else {
          return { extensionFile: null };
        }
      }
      return null;
    };
  }

  markAllAsDirty(form: FormGroup) {
    for (const control of Object.keys(form.controls)) {
      form.controls[control].markAsDirty();
    }
  }
  postcomplaint() {
    if (!this.complaintform.valid) {
      this.markAllAsDirty(this.complaintform);
      return;
    }
    var obj = {
      complaintTitle: this.complaintform?.get('complaintTitle')?.value,
      complaintType: this.complaintform?.get('complaintType')?.value,
      description: this.complaintform?.get('description')?.value,
      enquiryFilePath: 'string',
      docs: 'string',
      path: this.complaintform?.get('file')?.value,
      status: 'string',
    };

    const formData = new FormData();

    Object.entries(obj)?.forEach(([key, value]) => {
      formData.append(key, value);
    });
    if (this.file) {
      formData.append('file', this.file);
    }

    this.userdata.savemgrivance(formData).subscribe((result) => {
      if (result) {
        this.toast.success('Record saved successfully.');
        this.complaintform.reset();
        this.getComplaintData();
      } else {
        this.toast.error('Error occured while saving Record.');
      }
    });
  }

  ngOnInit() {
    this.isUserAuthenticated();
    this.getComplaintData();
  }
  getFile(url: string) {
    this.userdata.getUserFile(url).subscribe((res) => {
      return res;
    });
  }
  getComplaintData() {
    this.userdata.getmgrivance().subscribe((result) => {
      debugger
      this.getusercomplaint = result;
    });
  }

  isUserAuthenticated() {
    const token = localStorage.getItem('access_token');
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.IsAuthorised = true;
    } else {
      this.IsAuthorised = false;
    }
  }

  public openPDF(): void {
    let DATA: any = document.getElementById('contentPDF');
    html2canvas(DATA).then((canvas) => {
      let fileWidth = 208;
      let fileHeight = (canvas.height * fileWidth) / canvas.width;
      const FILEURI = canvas.toDataURL('image/png');
      let PDF = new jsPDF('p', 'mm', 'a4');
      let position = 0;
      PDF.addImage(FILEURI, 'PNG', 0, position, fileWidth, fileHeight);
      PDF.save('membergrivancedetails.pdf');
    })

  }
  complaintmodal(id: any) {
    debugger
    this.userdata.viewmgrivance(id).subscribe((result) => {
      this.complaintmodalview = result;
    });
  }
}
