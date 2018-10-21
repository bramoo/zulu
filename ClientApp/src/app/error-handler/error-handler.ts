import { ErrorHandler, NgZone, Injectable } from "@angular/core";
import { PopupService } from "../popup/popup.service";


@Injectable()
export class PopupErrorHandler implements ErrorHandler {

  private defaultHandler = new ErrorHandler();

  constructor(
    private popupService: PopupService,
    private zone: NgZone
  ) { }

  handleError(error: any): void {
    console.log("[popup-error-handler] yeah boiiii");

    this.zone.run(() => setTimeout(() => this.popupService.alert(error)));

    this.defaultHandler.handleError(error);
  }
}
