import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { protectedResources } from "../../auth.config";
import { Observable } from "rxjs";
import { FileUriResponse } from "./model/file-uri-response";

@Injectable({
  providedIn: 'root'
})
export class GetFileUri {

  constructor(private http: HttpClient) {
  }

  public getFileSas(fileName: string): Observable<FileUriResponse> {
    return this.http.get<FileUriResponse>(protectedResources.fileUriApi(fileName).endpoint);
  }
}
