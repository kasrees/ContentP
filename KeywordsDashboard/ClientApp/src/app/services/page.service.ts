import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { PageIndex } from '../models/pageIndex';
import { PagesListResponseHttp } from '../models/pagesListResponseHttp';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { PagesListRequest } from '../models/pagesListRequest';
import { PagesResponseHttp } from '../models/pageResponseHttp';
import { PageDetail } from '../models/pageDetail';
import { LanguagesMapper } from '../models/LanguagesMapper';
import { UpdatePageQuery } from '../models/Queries/UpdatePageQuery';

@Injectable({
  providedIn: 'root'
})
export class PageService {

  private pagesCount: number;
  constructor(private http: HttpClient) { }

  getPageIndex(pagesListRequest : PagesListRequest) : Observable<PageIndex[]> {
    const queryParams = {
      "LanguageId": pagesListRequest.LanguageId,
      "PageNumber": pagesListRequest.PageNumber,
      "Limit": pagesListRequest.Limit,
      "SortField": pagesListRequest.SortField,
      "SortDirection": pagesListRequest.SortDirection
    };
    return this.http.get<PagesListResponseHttp>(environment.apiUrl + 'api/pages', {params:queryParams}).pipe(
      map((data) => {
        let pagesList = data.pages;
        this.pagesCount = data.pagesCount;
        return pagesList.map(function(page: any): PageIndex {
          return ({
            "id": page.id,
            "title": page.title, 
            "link" : 'https://www.travelline.ru/secure/Extranet/#' + page.link, 
            "description": page.description, 
            "languages": page.languages.map
              (function(languageInfo: any): any {
                return ([languageInfo.code]);
              }).join(', '),
             "isEmpty" : page.title === "" && page.description === "" && page.languages.length === 0
          });
        });
      }),
      catchError((error: any) => {
        return throwError(error)
      })
    )
  }

  getPageDetail(pageId : number) : Observable<PageDetail[]> {
    return this.http.get<PagesResponseHttp>(environment.apiUrl + 'api/pages/' + pageId).pipe(
      map((data) => {
        let pageId = data.id;
        let link = 'https://www.travelline.ru/secure/Extranet/#' + data.link;
        let pageTranslations = data.pageTranslations;

        return Object.entries(LanguagesMapper).map(function([languageId, value]): PageDetail {
          let pageTranslation = pageTranslations.find(obj => {
            return obj.languageId === Number(languageId)
          });
          return {
            "pageId": pageId,
            "link": link, 
            "heading": value.title,
            "languageName": value.name,
            "pageTranslationId": pageTranslation ? pageTranslation.id : undefined,
            "title": pageTranslation ? pageTranslation.title : "",
            "description": pageTranslation ? pageTranslation.description : "",
            "languageId": Number(languageId),
            "keywords": pageTranslation ? pageTranslation.keywords : [],
          }
        });
      }),
      catchError((error: any) => {
        return throwError(error)
      })
    )
  }

  getTotalPagesCount() {
    return this.pagesCount;
  }

  deletePage(id : number) : Observable<[]> {
    return this.http.delete<any>(environment.apiUrl + 'api/pages/' + id);
  }

  addPage(link : string) : Observable<[]> {
    return this.http.post<any>(environment.apiUrl + 'api/pages', link);
  }

  updatePageToObservable(pageId : number, data: UpdatePageQuery) : Observable<any> {
    return this.http.put<any>(environment.apiUrl + 'api/pages/' + pageId, data);
  }

  async updatePageToPromise(pageId : number, data: UpdatePageQuery) : Promise<any> {
    return this.http.put<any>(environment.apiUrl + 'api/pages/' + pageId, data).toPromise();
  }

  getPageTitleDuplicatesToObservable(pageId: number, title: string): Observable<any> {
    const body = {
      'Titles': [title]
    };
    return this.http.post<any>(environment.apiUrl + 'api/pages/duplicates/' + pageId, body);
  }

  getPageTitleDuplicatesToPromise(pageId: number, titles: string[]): Promise<any> {
    const body = {
      'Titles': titles
    };
    return this.http.post<any>(environment.apiUrl + 'api/pages/duplicates/' + pageId, body).toPromise();
  }
}
