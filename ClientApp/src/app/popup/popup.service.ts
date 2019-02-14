import { Injectable, ComponentFactoryResolver, ViewContainerRef, ComponentRef } from "@angular/core";
import { PopupComponent, PopupComponentConfig, PopupButton } from "./popup.component";

import { Observable } from "rxjs/Observable";
import { EmptyObservable } from 'rxjs/observable/EmptyObservable';
import "rxjs/add/operator/delay";
import "rxjs/add/operator/finally";
import "rxjs/add/observable/empty";


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

  public waitDialog(message = "Loading ...") {
    return this.createPopupComponent({
      heading: "Please Wait",
      message: message,
      spinner: true
    });
  }

  public addWaitDialog<T>(observable: Observable<T>, message = "Loading ..."): Observable<T> {
    let popupRef: ComponentRef<PopupComponent>;

    let p = new EmptyObservable()
      .delay(500)
      .subscribe(null, null, () => popupRef = this.waitDialog(message));

    return observable.finally(() => {
      if (!p.closed) {
        p.unsubscribe();
      }

      if (popupRef) {
        this.close(popupRef);
      }
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

    popupRef.changeDetectorRef.detectChanges();

    return popupRef;
  }

  public setRoot(ref: ViewContainerRef) {
    this.root = ref;
  }

  private close(popupRef: ComponentRef<PopupComponent>) {
    popupRef.destroy();
  }
}
