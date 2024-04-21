import { Component } from '@angular/core';
import {
  FormGroup,
  FormControl,
  FormBuilder,
  NgForm,
  Validators,
} from '@angular/forms';
import { HttpClient } from '@angular/common/http';

import { ToasterService } from '../../../app/services/toaster.service';
import { ByeLawsModel } from '../../shared/models/byelaws.model';
import { ByeLawsRequestService } from 'src/app/services/bye-laws/bye-laws-request.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-bl-home',
  templateUrl: './bl-home.component.html',
  styleUrls: ['./bl-home.component.scss'],
})
export class BlHomeComponent {
  constructor(
    private toast: ToasterService,
    private service: ByeLawsRequestService,
    private router: Router
  ) {}

  model = new ByeLawsModel();

  isLoading = false;
  loadingTitle = 'Loading';
  info = true;
  details = false;
  upload = false;

  nextBtn() {
    if (this.info == true) {
      this.info = false;
      this.details = true;
      this.upload = false;
    } else if (this.details == true) {
      this.info = false;
      this.details = false;
      this.upload = true;
    }
  }

  backBtn() {
    if (this.details == true) {
      this.info = true;
      this.details = false;
      this.upload = false;
    } else if (this.upload == true) {
      this.info = false;
      this.details = true;
      this.upload = false;
    }
  }

  AddInformation() {
    if (this.model.NameOfCeo != '') {
      console.log(this.model);
      this.nextBtn();
    } else {
      this.toast.error('', 'Please enter details');
    }
  }

  DetailsSave() {
    if (this.model.AmemndmentInByLawRelatedTo != '') {
      this.nextBtn();
    } else {
      this.toast.error('', 'Please enter details');
    }
  }

  submit() {
    this.isLoading = true;
    this.loadingTitle = 'Saving Your Request';


    this.service.saveByeLawsRequest(this.model).subscribe({
      next: (res: any) => {
        console.log(res, 'Response');
        this.isLoading = false;
        if (res.isSuccess) {
          this.toast.success(res.massage);
          this.router.navigate(['/bye-laws-request-list']);
        } else this.toast.info(res.massage);
      },
      error: (err: any) => {
        this.isLoading = false;
        console.log(err, 'Error');
        this.toast.info(err.massage);
      },
    });
  }

  ResolutionOfGMDoc(event: any) {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onloadend = () => {
      const base64String = reader.result as string;
      console.log(base64String);
      this.model.ResolutionOfGMDoc = base64String;

      console.warn(this.model);
    };

    if (file) {
      reader.readAsDataURL(file);
    }
  }

  ResolutionOfAmendmentDoc(event: any) {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onloadend = () => {
      const base64String = reader.result as string;
      console.log(base64String);
      this.model.ResolutionOfAmendmentDoc = base64String;
      console.warn(this.model);
    };

    if (file) {
      reader.readAsDataURL(file);
    }
  }

  ReasonForAmendmentDoc(event: any) {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onloadend = () => {
      const base64String = reader.result as string;
      console.log(base64String);
      this.model.ReasonForAmendmentDoc = base64String;
      console.warn(this.model);
    };

    if (file) {
      reader.readAsDataURL(file);
    }
  }

  AmendedTextOfTheBylawsDoc(event: any) {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onloadend = () => {
      const base64String = reader.result as string;
      console.log(base64String);
      this.model.AmendedTextOfTheBylawsDoc = base64String;
      console.warn(this.model);
    };

    if (file) {
      reader.readAsDataURL(file);
    }
  }

  CertificationOfAmendmentBylaws(event: any) {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onloadend = () => {
      const base64String = reader.result as string;
      console.log(base64String);
      this.model.CertificationOfAmendmentBylaws = base64String;

      console.warn(this.model);
    };

    if (file) {
      reader.readAsDataURL(file);
    }
  }
}
