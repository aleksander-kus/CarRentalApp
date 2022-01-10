import { Injectable } from '@angular/core';
import {HttpClient, HttpEvent, HttpProgressEvent, HttpRequest} from "@angular/common/http";
import { BehaviorSubject, Observable } from "rxjs";
import { protectedResources } from "../../auth.config";
import { finalize } from "rxjs/operators";
import { ApiResponse } from "../common/api-response.interface";
import {CarReturnResponse} from "./model/car-return-response.interface";
import {CarReturnRequest} from "./model/car-return-request.interface";
import {UploadFileResponse} from "./model/upload-file-response.inteface";

@Injectable({
  providedIn: 'root'
})

export class UploadFileService {
  constructor(private http: HttpClient) { }

  public uploadFile(formData: FormData): Observable<HttpEvent<any>> {

    return this.http.post<UploadFileResponse>(protectedResources.uploadFileApi().endpoint, formData, {
      reportProgress: true,
      observe: 'events'
    });
  }
}
