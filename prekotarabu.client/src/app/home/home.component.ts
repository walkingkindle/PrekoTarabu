import {CUSTOM_ELEMENTS_SCHEMA, Component } from '@angular/core';
import '@aarsteinmedia/dotlottie-player'
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";


@Component({
    selector: 'app-home',
    standalone: true,
    templateUrl: './home.component.html',
    styleUrl: './home.component.css',
  imports: [
    ReactiveFormsModule
  ],
    schemas:[CUSTOM_ELEMENTS_SCHEMA]
})
export class HomeComponent {
    waitListForm!:FormGroup
    constructor(private formBuilder:FormBuilder,) {
    }
    ngOnInit(){
      this.buildForm()

    }

  private buildForm() {
    this.waitListForm = this.formBuilder.group({
      name:[[''],Validators.required],
      email:[[''],Validators.required],
      message:[['']]
    })
  }

  onSubmit() {

  }
}
