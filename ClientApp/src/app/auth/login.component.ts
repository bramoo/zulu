import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { AuthService } from "./auth.service";

@Component({
  templateUrl: "./login.component.html"
})
export class LoginComponent {
  public email: string;
  public password: string;
  public message: string;

  constructor(
    private auth: AuthService,
    private router: Router
  ) { }

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

  public logout() {
    this.auth.logout();
  }
}
