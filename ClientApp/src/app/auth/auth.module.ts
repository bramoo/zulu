import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { BrowserModule } from "@angular/platform-browser";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule } from "@angular/forms";
import { Route, RouterModule } from "@angular/router";

import { LoginComponent } from "./login.component";

import { AuthGuard } from "./auth.guard";
import { AuthService } from "./auth.service";

const authRoutes: Route[] = [
  { path: "login", component: LoginComponent }
];

//export function authHttpServiceFactory(httpClient: HttpClient, options: HttpRequestOptions) {
//  return new AuthHttp(new AuthConfig(), httpClient, options);
//}

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    HttpClientModule,
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
    //{
    //  provide: AuthHttp,
    //  useFactory: authHttpServiceFactory,
    //  deps: [HttpClient, RequestOptions]
    //},
    AuthService
  ]
})
export class AuthModule { }
