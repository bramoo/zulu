import { Component, Inject, OnInit } from '@angular/core';

@Component({
  selector: 'app-facebook-login',
  templateUrl: './facebook-login.component.html',
  styleUrls: ['./facebook-login.component.css']
})
export class FacebookLoginComponent implements OnInit {
  private authWindow: Window;
  failed: boolean;
  error: string;
  errorDescription: string;
  isRequesting: boolean

  constructor( @Inject("BASE_URL") private baseurl: string) {
    if (window.addEventListener) {
      window.addEventListener("message", (event) => this.handleMessage(event), false);
    }
    else {
      console.error("window.addEventListener does not exist");
    }
  }

  ngOnInit() {
  }

  launchFbLogin() {
    // TODO: client id in config
    let redirect = this.baseurl + "facebook-auth.html";
    console.log(redirect);
    let clientId = "474257665919778";
    this.authWindow = window.open(`https://www.facebook.com/v2.11/dialog/oauth?&response_type=token&display=popup&client_id=${clientId}&redirect_uri=${redirect}&scope=email`, null, 'width=600,height=400')
  }

  handleMessage(event: Event) {
    const message = event as MessageEvent;
    
    let result: any;

    if (typeof message.data === 'string') {
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

      // TODO: get jwt token
      // eg. this.userService.facebookLogin(result.accessToken)

    }
  }
}
