import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { PageService } from 'src/app/services/page.service';

@Component({
  selector: 'app-exit-page',
  templateUrl: './exit-page.component.html',
  styleUrls: ['./exit-page.component.scss']
})

export class ExitPageComponent {
  public languages: string;

  constructor(
    public dialog: MatDialogRef<ExitPageComponent>,
    public pageService : PageService,
  ) { }
}
