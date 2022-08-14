import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppComponent } from './app.component';
import { PageIndexComponent } from './components/page-index/page-index.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './modules/material/material.module';
import { PageIndexTableComponent } from './components/page-index-table/page-index-table.component';
import { HeaderComponent } from './components/layout/header/header.component';
import { FooterComponent } from './components/layout/footer/footer.component';
import { PreloaderInterceptor } from './interceptors/preloader.interceptor';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { AddPageFormComponent } from './components/dialogs/add-page-form/add-page-form.component';
import { PreloaderComponent } from './components/layout/preloader/preloader.component';
import { ConfirmPageDeleteComponent } from './components/dialogs/confirm-page-delete/confirm-page-delete.component';
import { LoginFormComponent } from './components/dialogs/login-form/login-form.component';
import { LoginPageComponent } from './components/login-page/login-page.component';
import { UnauthorizedInterceptor } from './interceptors/unauthorized.interceptor';
import { PageDetailComponent } from './components/page-detail/page-detail.component';
import { PageAttributingFormComponent } from './components/page-attributing-form/page-attributing-form.component';
import { WordsChipsComponent } from './components/words-chips/words-chips.component';
import { ExitGuard } from './guards/exit.guard';
import { YesNoModalComponent } from './components/dialogs/yes-no-modal/yes-no-modal.component';
import { ExitPageComponent } from './components/dialogs/exit-page/exit-page.component';
import { SortWordsComponent } from './components/dialogs/sort-words/sort-words.component';
import { PaginatorConfiguration } from './configuration/paginatorConfiguration';

@NgModule({
  declarations: [
    AppComponent,
    PageIndexComponent,
    PageIndexTableComponent,
    HeaderComponent,
    FooterComponent,
    AddPageFormComponent,
    PreloaderComponent,
    ConfirmPageDeleteComponent,
    LoginFormComponent,
    LoginPageComponent,
    PageDetailComponent,
    PageAttributingFormComponent,
    WordsChipsComponent,
    YesNoModalComponent,
    ExitPageComponent,
    SortWordsComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    MaterialModule,
    HttpClientModule,
    BrowserAnimationsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: PreloaderInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: UnauthorizedInterceptor,
      multi: true
    },
    {
      provide: MatPaginatorIntl,
      useValue: PaginatorConfiguration.ConfigurePaginatorTranslation()
    },
    {
      provide: ExitGuard,
      useClass: ExitGuard
    },
    ExitGuard
  ],
  bootstrap: [AppComponent],
  entryComponents: [
    AddPageFormComponent,
    ConfirmPageDeleteComponent,
    LoginFormComponent
  ]
})
export class AppModule { }
