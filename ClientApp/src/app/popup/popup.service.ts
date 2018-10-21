import { Injectable, ComponentFactoryResolver, ViewContainerRef, ComponentRef } from "@angular/core";
import { PopupComponent, PopupComponentConfig, PopupButton } from "./popup.component";


@Injectable()
export class PopupService {
  private root: ViewContainerRef;

  constructor(private componentFactoryResolver: ComponentFactoryResolver) { }

  public alert(message: string) {
    return this.createPopupComponent({
      heading: "Alert",
      message: message,
      buttons: PopupButton.Close
    });
  }

  public waitDialog(message: string) {
    return this.createPopupComponent({
      heading: "Please Wait",
      message: message
    });
  }

  private createPopupComponent(config: PopupComponentConfig) {
    if (!this.root) {
      throw new Error("[PopupService] root view container not set");
    }

    let popupFactory = this.componentFactoryResolver.resolveComponentFactory(PopupComponent);
    let popupRef = this.root.createComponent(popupFactory);

    popupRef.instance.config = config;
    popupRef.instance.close.subscribe(() => this.close(popupRef));

    return popupRef;
  }

  public setRoot(ref: ViewContainerRef) {
    this.root = ref;
  }

  private close(popupRef: ComponentRef<PopupComponent>) {
    popupRef.destroy();
  }
}
