import {CUSTOM_ELEMENTS_SCHEMA, Component } from '@angular/core';
import '@aarsteinmedia/dotlottie-player'


@Component({
    selector: 'app-home',
    standalone: true,
    templateUrl: './home.component.html',
    styleUrl: './home.component.css',
    imports: [],
    schemas:[CUSTOM_ELEMENTS_SCHEMA]
})
export class HomeComponent {

    ngOnInit(){
    }

}
