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
    return this.config && this.config.heading || "Heading";
  }

  public get message() {
    return this.config && this.config.message || "Message";
  }

  public get spinner() {
    return this.config && this.config.spinner;
  }

  public get haveClose() {
    return !this.config || !!(this.config.buttons & PopupButton.Close);
  }

  public get haveFooter() {
    return this.haveClose;
  }

  public doClose() {
    this.close.emit(PopupButton.Close);
  }
}

export class PopupComponentConfig {
  heading?: string;
  message?: string;
  spinner?: boolean;
  buttons?: PopupButton
}

export enum PopupButton {
  Close = 1
}
