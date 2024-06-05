import {CUSTOM_ELEMENTS_SCHEMA, Component } from '@angular/core';
import '@aarsteinmedia/dotlottie-player'
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ContactComponent} from "../contact/contact.component";


@Component({
    selector: 'app-home',
    standalone: true,
    templateUrl: './home.component.html',
    styleUrl: './home.component.css',
  imports: [
    ReactiveFormsModule,
    ContactComponent
  ],
    schemas:[CUSTOM_ELEMENTS_SCHEMA]
})
export class HomeComponent {




}
