import { Component } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {WaitLister} from "../models/wait-lister";
import {MailService} from "../services/mail.service";
import {response} from "express";
import {AlertService} from "../services/alert.service";
import {NgClass, NgIf} from "@angular/common";
import {animate, state, style, transition, trigger} from "@angular/animations";
import {Result} from "../models/result";
import {HttpResponse} from "@angular/common/http";

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgIf,
    NgClass
  ],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.css',
  animations: [
    trigger('fadeInOut', [
      state('void', style({
        opacity: 0
      })),
      transition(':enter, :leave', [
        animate('500ms ease-in-out')
      ])
    ])
  ]
})
export class ContactComponent {
  waitListForm!: FormGroup
  isFormSubmitted!: boolean;

  constructor(private formBuilder: FormBuilder, private mailService: MailService, private alertService: AlertService) {
  }

  ngOnInit() {
    this.buildForm()
    this.isFormSubmitted = false;

  }

  private buildForm() {
    this.waitListForm = new FormGroup({
      name: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      message: new FormControl('')
    })
  }

  onSubmit() {
    const waitLister: WaitLister = {
      id: 0,
      name: this.waitListForm.get('name')?.value,
      hisHerMessage: this.waitListForm.get('message')?.value,
      hisHerMail: this.waitListForm.get('email')?.value
    };

    this.isFormSubmitted = true;
    setTimeout(() => {
      this.isFormSubmitted = false;
    }, 3000); // Hide after 3 seconds

    this.mailService.sendMail(waitLister).subscribe({
      next: (response: HttpResponse<Result>) => {
        const result:Result | null = response.body;
        console.log(result)
        if (result?.isSuccess) {
          this.alertService.success('All Right', 'Check your e-mail and your spam folder');
        } else if(!result?.isSuccess) {
          this.alertService.error(result?.error.code, result?.error.description);
        }
      },
      error: (error: any) => {
        this.alertService.error('Oops','Looks like there was a problem with our servers. Try again later?')
      }
    });
  }
}
