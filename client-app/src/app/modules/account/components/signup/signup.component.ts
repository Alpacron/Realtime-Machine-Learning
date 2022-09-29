import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['../../styles/form-styling.scss']
})
export class SignupComponent implements OnInit {

  appname = environment.appname;

  signupForm = this.formBuilder.group({
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
    const form = this.signupForm.value;
    if (typeof form.email !== "string") return;
    if (typeof form.password !== "string") return;
    this.authService.signUp(form.email, form.password);
  }
}
