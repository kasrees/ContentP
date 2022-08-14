import { Component, OnInit } from '@angular/core';
import { LoginFormComponent } from '../dialogs/login-form/login-form.component';
import { MatDialogRef, MatDialogConfig, MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit {
  private dialogConfig = new MatDialogConfig();
  
  constructor(private dialog: MatDialog) { 
    this.dialogConfig.disableClose = true;
    this.dialogConfig.backdropClass = 'login-page-dialog-background';
    this.dialogConfig.panelClass = "login-page-dialog";
  }

  ngOnInit(): void {
    this.openDialog();
  }

  openDialog() {
    this.dialog.open(LoginFormComponent, this.dialogConfig);
  }
}
