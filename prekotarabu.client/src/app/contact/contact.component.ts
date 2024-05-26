import { Component } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {WaitLister} from "../models/wait-lister";
import {MailService} from "../services/mail.service";
import {response} from "express";
import {AlertService} from "../services/alert.service";

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.css'
})
export class ContactComponent {
  waitListForm!:FormGroup
  constructor(private formBuilder:FormBuilder,private mailService:MailService,private alertService:AlertService) {
  }
  ngOnInit(){
    this.buildForm()

  }

  private buildForm() {
    this.waitListForm = new FormGroup({
      name: new FormControl('',Validators.required),
      email:new FormControl('',Validators.required),
      message:new FormControl('')
    })
  }

  onSubmit() {
   const waitLister:WaitLister = {id:0,name: this.waitListForm.get('name')?.value,hisHerMessage:this.waitListForm.get('message')?.value,
     hisHerMail:this.waitListForm.get('email')?.value}

    console.log(waitLister.name,waitLister.id,waitLister.hisHerMail,waitLister.hisHerMessage);

    this.mailService.sendMail(waitLister).subscribe(response => {

      if([200,201].includes(response.status)){
        this.alertService.success('All Right','Check your e-mail and your spam folder')
      }

      else{
       this.alertService.error('Uhh','There was a problem with the server')
      }
    })

  }
}
