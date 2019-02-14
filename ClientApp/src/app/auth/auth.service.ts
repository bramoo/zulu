import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PopupService } from "../popup/popup.service";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class AuthService {
  public redirect: string;

  constructor(
    private httpClient: HttpClient,
    private popupService: PopupService
  ) { }

  get isLoggedIn(): boolean {
    let token = localStorage.getItem("token");
    console.log(token);
    if (token) {
      return true;
    } else {
      return false;
    }
  }

  public login(email: string, password: string): Observable<boolean> {
    let body = { email: email, password: password };

    return this.popupService.addWaitDialog(
      this.httpClient.post<{ token: string }>("/api/v1/auth", body)
        .pipe(map(
          res => {
            localStorage.setItem("token", res.token);
            return true;
          },
          () => false)),
      "Logging in ..."
    );
  }

  public fbLogin(token: string): Observable<boolean> {
    let body = { token };

    return this.popupService.addWaitDialog(
      this.httpClient.post<{ token: string }>("/api/v1/fbauth", body)
        .pipe(map(res => {
          console.log(res);
          localStorage.setItem("token", res.token);
          return true;
          //}));
          //res => {
          //  localStorage.setItem("token", res.token);
          //  return true;
        },
          () => false)),
      "Logging in ..."
    );
  }

  public logout() {
    localStorage.removeItem("token");
  }
}
