import { Component } from '@angular/core';
import { PageIndex } from 'src/app/models/pageIndex';
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { AddPageFormComponent } from '../dialogs/add-page-form/add-page-form.component';

@Component({
  selector: 'app-page-index',
  templateUrl: './page-index.component.html',
  styleUrls: ['./page-index.component.scss']
})
export class PageIndexComponent {
  pageIndex: PageIndex[];
  private dialogConfig = new MatDialogConfig();
  
  constructor(private dialog: MatDialog) {
    this.dialogConfig.disableClose = true;
    this.dialogConfig.backdropClass = "es-dialog-background";
    this.dialogConfig.panelClass = "add-page-dialog";
  }

  openDialog() {
    this.dialog.open(AddPageFormComponent, this.dialogConfig);
  }
}
