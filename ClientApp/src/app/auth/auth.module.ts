import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { Http, RequestOptions } from "@angular/http";
import { Route, RouterModule } from "@angular/router";
import { AuthHttp, AuthConfig } from "angular2-jwt";

import { LoginComponent } from "./login.component";

import { AuthGuard } from "./auth.guard";
import { AuthService } from "./auth.service";

const authRoutes: Route[] = [
  { path: "login", component: LoginComponent }
];

export function authHttpServiceFactory(http: Http, options: RequestOptions) {
  return new AuthHttp(new AuthConfig(), http, options);
}

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(authRoutes)
  ],
  exports: [
    RouterModule
  ],
  declarations: [
    LoginComponent
  ],
  providers: [
  AuthGuard,
    {
      provide: AuthHttp,
      useFactory: authHttpServiceFactory,
      deps: [Http, RequestOptions]
    },
    AuthService
  ]
})
export class AuthModule { }
