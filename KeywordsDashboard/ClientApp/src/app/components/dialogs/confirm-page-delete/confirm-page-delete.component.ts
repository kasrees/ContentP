import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { PageService } from 'src/app/services/page.service';

@Component({
  selector: 'app-confirm-page-delete',
  templateUrl: './confirm-page-delete.component.html',
  styleUrls: ['./confirm-page-delete.component.scss']
})

export class ConfirmPageDeleteComponent {
  public pageName: string;
  public pageId: number;
  public isError = false;
  
  constructor(
    public pageService : PageService,
    public dialog: MatDialogRef<ConfirmPageDeleteComponent>
  ) { }

  deletePage() {
    this.isError = false;
    this.pageService.deletePage(this.pageId).subscribe(
      (data) => {
        this.dialog.close(true);
      },
      (error) => {
        this.isError = true;
      }
    )
  }
}