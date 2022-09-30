import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { AccountRoutingModule } from './account-routing.module';
import { SigninComponent } from './components/signin/signin.component';
import { SignupComponent } from './components/signup/signup.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { AccountComponent } from './account.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    AccountComponent,
    SigninComponent,
    SignupComponent,
    ResetPasswordComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    AccountRoutingModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [AccountComponent]
})
export class AccountModule { }
