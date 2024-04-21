import { Component, OnInit } from '@angular/core';

import { Router } from "@angular/router";
import { ToasterService } from "../services/toaster.service";
import { hex_hmac_md5 } from "../../assets/ts/hex_hmac_md5";
import { FormBuilder, FormControl, FormGroup, Validators, NgForm } from '@angular/forms';
import { RegistrationService } from '../services/registration.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {



  Registration: any;
  first = true;
  second = false;
  third = false;

  name: string = '';
  age: number = 0;

  LoginID :number = 0; 

  Password: string = "";
  DeptID: number = 0;
  DisCode: string = ""; 
 

  ARCSCode: string = "";

  Username: string = "";
  UserTypeCode: number = 0;
  DeptDesignationCode: number = 0;
  FirstName: string = "";
  EmailID: string = "";
  gender: number = 0;
  CreatedDate: string = "";
  CreatedBy: string = "";
  ModifiedDate: string = "";
  TransferRecordModifiedBy: string = "";
  SecurityQuestionCode: string = "";
  SecurityAnswer: string = "";
  Role: number = 0;
  Hash: string = "";
  Address1: string = "";
  Address2: string = "";
  PostalCode: string = "";
  Salt: string = "";

  confirmPassword: string="";

  DistrictOfOperation: number = 0;
  PostOffice: string = "";
  Age: number = 0;
  Gender: string = "";
  Mobile: string = "";

  SocietyStatus: string = "";
  SocietyRegistrationNo: string = "";
  //md5
  $_md5_key: string = "sblw-3hn8-sqoy19";


  //for drop down district
  public District: District[] = [];


  SelectedDisCode: Number = 0;

  Discode = this.SelectedDisCode;




  // for radio
  options = ['MALE', 'FEMALE', 'OTHER'];
  selectedOption: string="";





  constructor(
    private router: Router,
    private toaster: ToasterService,
    private _Registrationservice: RegistrationService,

  ) {
    
  }
  public Register(form: NgForm): void {
    debugger;
    if (form.invalid) {
      for (const control of Object.keys(form.controls)) {
        debugger;
        form.controls[control].markAsTouched();
      }
      return;
    }

    if (this.Password === this.confirmPassword) {
      console.log('Password matched!');
      // Further processing...
    } else {
      console.log('Password does not match!');
      // Handle password mismatch...
    }

    debugger;
    if (this.Username == "" && this.Password == "") {
      this.toaster.error("", "Please enter all the field");
    }
    else if (this.Username == "") {
      this.toaster.error("Please enter User Name", "Error");
    }
    else if (this.FirstName == "") {
      this.toaster.error("Please enter  Name", "Error");
    } else if (this.Password == "") {
      this.toaster.error("Please enter Password", "Error");
    } else {
      var JsonData = {
        UserName: hex_hmac_md5(this.$_md5_key, this.Username),
        Password: hex_hmac_md5(this.$_md5_key, this.Password),
        DecodeUserName: this.Username,
        DecodePassword: this.Password,
        age: this.Age,
        Emailid: this.EmailID,
        Discode: this.DisCode,
        FirstName: this.FirstName,
        Mobile: this.Mobile,
        Address1: this.Address1,
        PostOffice: this.PostOffice,
        PostalCode: this.PostalCode,
        gender: this.gender,




      };
      this._Registrationservice.Registration(JsonData).subscribe(result => {
       
      
        this.toaster.success("Sucessfully Registered", "Success");
        this.router.navigate(["/Login"]);
          
      });
    }

  }

  ngOnInit(): void {
   

    this._Registrationservice.getDistrictItems().subscribe(
      (Data) => {
        this.District = Data
          ;
      });
  }




















  next()
  {

     if(this.first==true)
     {
       

      this.second = true;
      this.first = false;
     }else if(this.second==true)
     {
        this.third = true;
        this.second = false;
     }
  }

  back()
  {
    if(this.second==true)
    {
     this.second = false;
     this.first = true;
    }else if(this.third==true)
    {
       this.third = false;
       this.second = true;
    }

  }

}


interface District {
  districtName: string;
  districtCode: string;
  coopDisCode: string;
}

