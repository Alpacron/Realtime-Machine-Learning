import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['../../styles/form-styling.scss']
})
export class SigninComponent implements OnInit {

  appname = environment.appname;

  signinForm = this.formBuilder.group({
    email: '',
    password: ''
  });

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit(): void {
  }

  onSubmit(): void {
    const form = this.signinForm.value;
    if (typeof form.email !== "string") return;
    if (typeof form.password !== "string") return;
    this.authService.signIn(form.email, form.password);
  }
}
