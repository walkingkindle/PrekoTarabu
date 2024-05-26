import { Injectable } from '@angular/core';
import {HttpClient, HttpResponse} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {WaitLister} from "../models/wait-lister";
import {catchError, map, Observable} from "rxjs";
import {ConfigService} from "./config.service";
import {Result} from "../models/result";
import {response} from "express";

@Injectable({
  providedIn: 'root'
})
export class MailService {

  private mailerUrl = ''
  constructor(private httpClient:HttpClient,private configService:ConfigService) {
    this.mailerUrl = environment.mailerUrl
  }


  sendMail(waitLister: WaitLister):Observable<HttpResponse<Result>> {
    return this.httpClient.post<Result>(this.mailerUrl,waitLister, {observe:'response'})
      .pipe(
      map(response => {
        return response
      }),
      catchError(
        this.configService.handleError
      )
    )

  }
}
