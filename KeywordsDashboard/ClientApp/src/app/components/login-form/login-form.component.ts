import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { MatDialogRef } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { httpResponseStatusCodes } from 'src/app/models/httpResponseStatusCodes';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})
export class LoginFormComponent implements OnInit, AfterViewInit, OnDestroy {

  public form: FormGroup;
  public submitted: boolean = false;
  public returnUrl: string = '';
  public error: string = '';
  public isPasswordHidden: boolean = true;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    public dialog: MatDialogRef<LoginFormComponent>,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.setLoginForm();
  }

  toggleEyeIconVisibility() {
    this.isPasswordHidden = !this.isPasswordHidden;
  }

  ngAfterViewInit() {
    document.querySelector('body')?.classList.add('-type-login');
  }

  ngOnDestroy() {
    document.querySelector('body')?.classList.remove('-type-login');
    this.dialog.close();
  }

  setLoginForm() {
    this.form = this.fb.group({
      login: ['', Validators.required],
      password: ['', Validators.required]
    }, {updateOn:'submit'});
  }

  onSubmit():void {
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }

    this.authService.login(this.form.controls['login'].value, this.form.controls['password'].value).subscribe(
      (data) => {
        this.router.navigate(this.redirectTo())
        this.dialog.close(true);
      },
      (error) => {
        if (error instanceof HttpErrorResponse) {
          if (error.status === httpResponseStatusCodes.Unauthorized) {
            this.form.setErrors({unauthtorized: true})
            return;
          } 
        }
        this.form.setErrors({anyError: true})
      }
    )
  }

  redirectTo(): any[] {
    if (this.route.snapshot.paramMap.get('returnUrl')) {
      return [this.route.snapshot.paramMap.get('returnUrl')];
    }
    return [this.returnUrl];
  }

  onKeyupEvent(fieldName : string) {
    this.form.get(fieldName)?.setErrors(null);
  }
}
