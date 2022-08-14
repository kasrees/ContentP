import {CollectionViewer, DataSource} from "@angular/cdk/collections";
import { PageService } from '../services/page.service';
import { PageIndex } from './pageIndex';
import { BehaviorSubject, Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { PagesListRequest } from './pagesListRequest';

export class PagesDataSource implements DataSource<PageIndex> {

    private pagesSubject = new BehaviorSubject<PageIndex[]>([]);

    constructor(private pageIndexService: PageService) { }

    connect(collectionViewer: CollectionViewer): Observable<PageIndex[]> {
        return this.pagesSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
        this.pagesSubject.complete();
    }

    loadPages(pageIndexRequest: PagesListRequest) {
        this.pageIndexService.getPageIndex(pageIndexRequest).pipe(
            catchError(() => of([])),
        )
        .subscribe(pages => this.pagesSubject.next(pages));
    }  
    
    getTotalPagesCount() {
        return this.pageIndexService.getTotalPagesCount();
    }
}
