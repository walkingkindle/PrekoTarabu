import { Injectable } from '@angular/core';
import * as sweetalert2 from "sweetalert2";
import {Subject} from "rxjs";
import Swal from "sweetalert2";

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  private alertSubject = new Subject<any>();
  constructor() { }
  getAlerts(){
    return this.alertSubject.asObservable();
  }
  error(title: string, text: string) {
    const Toast = Swal.mixin({
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer);
        toast.addEventListener('mouseleave', Swal.resumeTimer);
      }
    });

    Toast.fire({
      icon: 'error',
      title: title,
      text: text
    });
  }

  success(title: string, text: string) {
    const Toast = Swal.mixin({
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer);
        toast.addEventListener('mouseleave', Swal.resumeTimer);
      },

    })

    Toast.fire({
      icon: 'success',
      title: title,
      text: text
    });

    setTimeout(() => {
      location.reload()
    },3000)
  }

}
