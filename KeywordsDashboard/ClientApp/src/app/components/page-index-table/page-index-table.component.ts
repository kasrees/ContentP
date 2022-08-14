import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { PageService } from 'src/app/services/page.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortable, Sort } from '@angular/material/sort';
import { PagesDataSource } from 'src/app/models/pagesDataSource';
import { tap } from 'rxjs/operators';
import { merge } from "rxjs"
import { MatDialog, MatDialogRef, MatDialogConfig } from '@angular/material/dialog';
import { ConfirmPageDeleteComponent } from '../dialogs/confirm-page-delete/confirm-page-delete.component';
import { AddPageFormComponent } from '../dialogs/add-page-form/add-page-form.component';
import { PagesListRequest } from 'src/app/models/pagesListRequest';

@Component({
  selector: 'app-page-index-table',
  templateUrl: './page-index-table.component.html',
  styleUrls: ['./page-index-table.component.scss']
})
export class PageIndexTableComponent implements AfterViewInit, OnInit {
  dataSource: PagesDataSource;
  displayedColumns = ['title', 'description', 'languages', 'link', 'action'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  pagesCount: number = 0;
  private deletePagedialog: MatDialogRef<ConfirmPageDeleteComponent>;
  private createPagedialog: MatDialogRef<AddPageFormComponent>;
  private languageId: number = 1;
  private firstPageNumber: number = 1;
  private dialogConfig = new MatDialogConfig();
  
  constructor(public pageService: PageService, private dialog: MatDialog) {
    this.dialogConfig.disableClose = true;
    this.dialogConfig.backdropClass = "es-dialog-background";
    this.dialogConfig.panelClass = "dialog";
  }

  ngOnInit(): void {
    this.dataSource = new PagesDataSource(this.pageService);
    this.sort.sort(({ id: 'createdAt', start: 'asc' }) as MatSortable);
    this.dataSource.loadPages(new PagesListRequest(1, 1, 5, 'createdAt', 'desc'));
  }

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
          tap(() => this.reloadPages())
      )
      .subscribe();
  }

  sortData(sort: Sort) {
    if (sort.direction === "") {
      this.sort.direction = "asc";
      this.sort.active = "createdAt"
    }
  }

  reloadPages() {
    this.dataSource.loadPages(new PagesListRequest(
        this.languageId,
        this.paginator.pageIndex + this.firstPageNumber,
        this.paginator.pageSize,
        this.sort.active,
        this.sort.direction
      )
    );
  } 

  getTotalPagesCount() {
    return this.dataSource.getTotalPagesCount();
  }

  deletePage(id: number, link: string, title: string) {
    ;
    let pageName = title === '' ? link : title
    this.deletePagedialog = this.dialog.open(ConfirmPageDeleteComponent, this.dialogConfig);
    this.deletePagedialog.componentInstance.pageName = pageName;
    this.deletePagedialog.componentInstance.pageId = id;
    this.deletePagedialog.afterClosed().subscribe(result => {
      if (result === true) {
        this.reloadPages();
      }
    });
  }

  addPage() {
    this.createPagedialog = this.dialog.open(AddPageFormComponent, this.dialogConfig);
    this.createPagedialog.afterClosed().subscribe(result => {
      if (result === true) {
        this.reloadPages();
      }
    });
  }

  setHintMessage(message: string, pageLink: string) {
    return message + '\n' + pageLink;
  }
}
