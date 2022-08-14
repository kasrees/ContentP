import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from "@angular/material/dialog";
import { HttpErrorResponse } from '@angular/common/http';
import { PageService } from 'src/app/services/page.service';
import { Router } from '@angular/router';
import { httpResponseStatusCodes } from 'src/app/models/httpResponseStatusCodes';

@Component({
  selector: 'app-add-page-form',
  templateUrl: './add-page-form.component.html',
  styleUrls: ['./add-page-form.component.scss']
})
export class AddPageFormComponent implements OnInit {
  form: FormGroup;
  wasAlreadyExistsLinkEntered: boolean = false;

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialogRef<AddPageFormComponent>,
    private pageService: PageService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      link: ['', [Validators.pattern('(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .#-]*/?')]]
    }, {updateOn:'submit'});
  }

  close() {
    this.dialog.close(false);
  }

  onSubmit() {
    this.wasAlreadyExistsLinkEntered = false;
    if (!this.form.valid) {
      return;
    }
    this.pageService.addPage(this.form.value).subscribe(
      (data) => {
        this.router.navigate(['/pages/' + data]);
        this.dialog.close(true);
      },
      (error) => {
        if (error instanceof HttpErrorResponse) {
          if (error.status === httpResponseStatusCodes.Conflict) {
            this.form.controls.link.setErrors({conflict: true})
            this.wasAlreadyExistsLinkEntered = true;
            return;
          } 
        }
        this.form.controls.link.setErrors({anyError: true})
      }
    )
  }

  onPasteEvent(event: any) {
    this.wasAlreadyExistsLinkEntered = false;
    this.form.controls.link.setErrors({linkAlredyExists: false})
  }

  onKeyupEvent(event: any) {
    this.form.controls.link.setErrors(null);
    if (this.wasAlreadyExistsLinkEntered) {
      this.form.controls.link.setErrors({linkAlredyExists: true})
    }
  }
}
