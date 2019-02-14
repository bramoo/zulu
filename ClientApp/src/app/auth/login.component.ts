import { Component, Inject } from "@angular/core";
import { Router } from "@angular/router";

import { AuthService } from "./auth.service";

@Component({
  templateUrl: "./login.component.html"
})
export class LoginComponent {
  public email: string;
  public password: string;
  public message: string;

  private authWindow: Window;
  failed: boolean;
  error: string;
  errorDescription: string;
  isRequesting: boolean;

  constructor(
    private auth: AuthService,
    @Inject("BASE_URL") private baseurl: string,
    private router: Router
  ) {
    if (window.addEventListener) {
      window.addEventListener("message", (event) => this.handleMessage(event), false);
    }
    else {
      console.error("window.addEventListener does not exist");
    }
  }

  get isLoggedIn(): boolean {
    return this.auth.isLoggedIn;
  }

  public login() {
    this.message = "Logging in ..."

    this.auth.login(this.email, this.password)
      .subscribe(ok => {
        if (ok) {
          let redirect = this.auth.redirect || "/home";
          this.router.navigateByUrl(redirect);
        }
        else {
          this.message = "Login failed";
        }
      });
  }

  fbLogin() {
    // TODO: client id in config
    let redirect = this.baseurl + "facebook-auth.html";
    console.log(redirect);
    let clientId = "474257665919778";
    this.authWindow = window.open(`https://www.facebook.com/v3.2/dialog/oauth?&response_type=token&display=popup&client_id=${clientId}&redirect_uri=${redirect}&scope=email`, null, 'width=600,height=400')
  }

  handleMessage(event: Event) {
    const message = event as MessageEvent;

    let result: any;

    if (typeof message.data === "string") {
      result = JSON.parse(message.data);
    }
    else {
      result = message.data;
    }

    if (result.type != "facebookLogin") return;

    this.authWindow.close();

    if (!result.status) {
      this.failed = true;
      this.error = result.error;
      this.errorDescription = result.errorDescription;
    }
    else {
      this.failed = false;
      this.isRequesting = true;

      this.auth.fbLogin(result.accessToken)
        .subscribe(ok => {
          if (ok) {
            let redirect = this.auth.redirect || "/home";
            this.router.navigateByUrl(redirect);
          }
          else {
            this.error = "Token rejected by backend";
          }
        });
    }
  }

  public logout() {
    this.auth.logout();
  }
}
