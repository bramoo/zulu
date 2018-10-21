import { Component, Inject, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Headers, Response } from "@angular/http";
import { AuthHttp } from 'angular2-jwt';
import { PopupService } from '../popup/popup.service';

@Component({
  selector: 'image-upload',
  templateUrl: './image-upload.component.html',
  styleUrls: ['./image-upload.component.css']
})
export class ImageUploadComponent implements OnInit {
  @Input() public eventid: string;
  @Output() public uploaded = new EventEmitter<string>();
  public file: File;

  constructor(
    @Inject("BASE_URL") private baseUrl: string,
    private http: AuthHttp,
    private popupService: PopupService
  ) { }

  ngOnInit() {
    console.log("[image-upload] ngOnInit()");
  }

  imageChanged(event: Event) {
    let input = event.srcElement as HTMLInputElement;
    if (input.files.length === 1) {
      this.file = input.files[0]
    }
    else {
      this.file = null;
    }

    console.log(this.file);
  }

  uploadImage() {
    if (this.file) {
      let body = {
        displayName: this.file.name,
        contentType: this.file.type,
        description: "an image"
      };

      this.popupService.addWaitDialog(
        this.http.post(this.baseUrl + "api/v1/events/" + this.eventid + "/images", body)
      ).subscribe(response => this.getData(this.file, response));
    }
  }

  getData(file: File, response: Response) {
    if (file && response.ok) {
      let id = response.json().id;

      let reader = new FileReader();
      reader.readAsArrayBuffer(file);
      reader.onload = (event) => this.putData(id, reader.result, file.type);
    }
  }

  putData(id: string, data: any, type: string) {
    let headers = new Headers({ "Content-Type": type });

    this.popupService.addWaitDialog(
      this.http.put(this.baseUrl + "api/v1/images/" + id, data, { headers: headers })
    ).subscribe(response => {
      if (response.ok) {
        this.uploaded.emit(id);
      }
    });
  }
}
