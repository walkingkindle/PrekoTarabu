import { Injectable } from '@angular/core';
import {HttpErrorResponse} from "@angular/common/http";
import {throwError} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ConfigService {

  constructor() { }


  public handleError(error:HttpErrorResponse){
    if(error.status === 0){
      console.error('An error occurred:',error.message)
    }else{
      console.error(`Backend returned code ${error.status}, body was`,error)
    }
    return throwError(() => new Error('Something bad happened, please try again'))
  }
}
