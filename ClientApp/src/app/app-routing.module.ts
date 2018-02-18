import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';

import { FacebookLoginComponent } from './facebook-login/facebook-login.component';
import { HomeComponent } from './home/home.component';

const routes: Route[] = [
  { path: '', component: HomeComponent },
  { path: 'login', component: FacebookLoginComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
