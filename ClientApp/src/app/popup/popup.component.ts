import { Component, Input, EventEmitter, Output } from "@angular/core";


@Component({
  templateUrl: './popup.component.html',
  styles: ['.modal { display: block;}']
})
export class PopupComponent {
  @Input() public config: PopupComponentConfig;
  @Output() public close = new EventEmitter<PopupButton>();

  public result: any;

  public get heading() {
    return this.config.heading || "Heading";
  }

  public get message() {
    return this.config.message || "Message";
  }

  public get haveClose() {
    return !this.config || !!(this.config.buttons & PopupButton.Close);
  }

  public doClose() {
    this.close.emit(PopupButton.Close);
  }
}

export class PopupComponentConfig {
  heading?: string;
  message?: string;
  buttons?: PopupButton
}

export enum PopupButton {
  Close = 1
}
