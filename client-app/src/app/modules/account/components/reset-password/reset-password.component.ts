import { Component, OnInit, Type } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['../../styles/form-styling.scss']
})
export class ResetPasswordComponent implements OnInit {

  resetPasswordForm = this.formBuilder.group({
    email: ''
  });

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit(): void {
  }

  onSubmit(): void {
    const form = this.resetPasswordForm.value;
    if (typeof form.email !== "string") return;
    this.authService.resetPassword(form.email);
  }
}
