import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-yes-no-modal',
  templateUrl: './yes-no-modal.component.html',
  styleUrls: ['./yes-no-modal.component.scss']
})

export class YesNoModalComponent implements OnInit{
  public data: any;
  
  constructor(
    public dialog: MatDialogRef<YesNoModalComponent>
  ) { }

  ngOnInit(): void { }
}
