import { Injectable } from "@angular/core";
import { Headers, Http } from "@angular/http";
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PopupService } from "../popup/popup.service";

@Injectable()
export class AuthService {
  public redirect: string;

  constructor(
    private http: Http,
    private popupService: PopupService
  ) { }

  get isLoggedIn(): boolean {
    let token = localStorage.getItem("token");
    if (token) {
      return true;
    }
    else {
      return false;
    }
  }

  public login(email: string, password: string): Observable<boolean> {
    let body = JSON.stringify({ email: email, password: password });
    let headers = new Headers({ "Content-Type": "application/json" });

    return this.popupService.addWaitDialog(
      this.http.post("/api/v1/auth", body, { headers: headers })
        .pipe(
        map(response => {
          let token = response.json() && response.json().token;
          if (token) {
            localStorage.setItem("token", token);
            return true;
          }
          else {
            return false;
          }
        })
        )
    , "Logging in ...");
  }

  public fbLogin(token: string): Observable<boolean> {
    let headers = new Headers({ "Content-Type": "application/json" });

    return this.popupService.addWaitDialog(
      this.http.post("/api/v1/fbauth", { token }, { headers: headers })
        .pipe(
        map(response => {
          let token = response.json() && response.json().token;
          if (token) {
            localStorage.setItem("token", token);
            return true;
          }
          else {
            return false;
          }
        }))
    , "Logging in ...");
  }

  public logout() {
    localStorage.removeItem("token");
  }
}
