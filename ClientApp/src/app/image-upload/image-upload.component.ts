import { Component, Inject, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { PopupService } from '../popup/popup.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'image-upload',
  templateUrl: './image-upload.component.html',
  styleUrls: ['./image-upload.component.css']
})

export class Image {
  id: number;
  displayName: string;
  contentType: string;
  description: string;
}

export class ImageUploadComponent implements OnInit {
  @Input() public uploadUrl: string;
  @Output() public uploaded = new EventEmitter<string>();
  public file: File;

  constructor(
    @Inject("BASE_URL") private baseUrl: string,
    private httpClient: HttpClient,
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
        contentType: this.file.type
      };

      this.popupService.addWaitDialog(
        this.httpClient.post<Image>(this.uploadUrl, body)
      ).subscribe((data: Image) => this.getData(this.file, data.id));
    }
  }

  getData(file: File, id: number) {
    if (file) {
      let reader = new FileReader();
      reader.readAsArrayBuffer(file);
      reader.onload = () => this.putData(id, reader.result, file.type);
    }
  }

  putData(id: number, data: any, type: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': type
      })
    };

    this.popupService.addWaitDialog(
      this.httpClient.put(this.baseUrl + "api/v1/images/" + id, data, httpOptions)
    ).subscribe(() => this.uploaded.emit('' + id));
  }
}
