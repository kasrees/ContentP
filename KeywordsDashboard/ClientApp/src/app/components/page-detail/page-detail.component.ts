import { Component, OnInit, HostListener, ViewChildren, QueryList } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageService } from 'src/app/services/page.service';
import { PageDetail } from 'src/app/models/pageDetail';
import { ComponentCanDeactivate } from 'src/app/guards/exit.guard';
import { Observable } from 'rxjs';
import { PageAttributingFormComponent } from '../page-attributing-form/page-attributing-form.component';
import { MatDialog, MatDialogRef, MatDialogConfig } from '@angular/material/dialog';
import { YesNoModalComponent } from '../dialogs/yes-no-modal/yes-no-modal.component';
import { ExitPageComponent } from '../dialogs/exit-page/exit-page.component';
import { Word } from 'src/app/models/Word';
import { PageAttributes } from 'src/app/models/Queries/UpdatePageQuery';
import { LanguagesMapper } from 'src/app/models/LanguagesMapper';

@Component({
  selector: 'app-page-detail',
  templateUrl: './page-detail.component.html',
  styleUrls: ['./page-detail.component.scss']
})
export class PageDetailComponent implements OnInit, ComponentCanDeactivate {
  @ViewChildren(PageAttributingFormComponent) forms: QueryList<PageAttributingFormComponent>
  public link: string;  
  public pageTranslations: PageDetail[];
  public isError: boolean = false;
  private pageId: number;
  private dialogConfig = new MatDialogConfig();
  private pageIsNotAttributedDialogData = {
    'heading': 'На заданы атрибуты для страницы',
    'message': 'Вы действительно хотите уйти и оставить страницу без атрибутов?',
    'buttonYesCaption': 'Остаться и атрибутировать',
    'buttonNoCaption': 'Уйти'
  };
 
  constructor(
    private pageService: PageService, 
    private route: ActivatedRoute, 
    private dialog: MatDialog
  ) {
    this.dialogConfig.disableClose = true;
    this.dialogConfig.backdropClass = 'es-dialog-background';
    this.dialogConfig.panelClass = "es-dialog";
  }

  ngOnInit(): void {
    this.pageId = Number(this.route.snapshot.paramMap.get('id'));
    this.pageService.getPageDetail(this.pageId).subscribe(
      (data) => {
        this.pageTranslations = data;
        this.link = data[0].link;
      }
    )
  }

  //@HostListener('window:beforeunload')
  public canDeactivate() : boolean | Observable<boolean> | Promise<boolean> {
    this.isError = false;
    this.forms.forEach(formComponent => {
      if (formComponent.hasFormChanged()) {
        formComponent.form.setErrors(null);
        formComponent.form.markAllAsTouched();
        formComponent.setValidators();
        formComponent.updateValueAndValidity();
      }
    })
    if (this.isPageNotAttributed()) {
      return this.canExitWhenPageIsNotAttributed();
    }
    if (this.areAnyPageAttributesInvalid()) {
      return this.canExitWhenPageAttributesAreInvalid();
    }
    return this.canExitWhenPageAttributesWereEdited();
  }

  public setHintMessage(message: string, pageLink: string): string {
    return message + '\n' + pageLink;
  }

  private isPageNotAttributed(): boolean {
    const isAtLeastOneFormAttributed: boolean = this.forms.some((form) => {
      return form.isPageJustAddedAndNotChanged() === false;
    });
    return !isAtLeastOneFormAttributed;
  }

  private areAnyPageAttributesInvalid(): boolean {
    return this.forms.some((formComponent) => {
      return (formComponent.form.invalid && formComponent.hasFormChanged());
    });
  }

  private canExitWhenPageIsNotAttributed(): Observable<boolean>  {
    const dialog: MatDialogRef<YesNoModalComponent> = this.dialog.open(YesNoModalComponent, this.dialogConfig);
    dialog.componentInstance.data = this.pageIsNotAttributedDialogData;
    return dialog.afterClosed();
  }

  private canExitWhenPageAttributesAreInvalid(): Observable<boolean>  {
    const languages = this.getLanguagesOfChangedForms();
    const dialog: MatDialogRef<YesNoModalComponent> = this.dialog.open(YesNoModalComponent, this.dialogConfig);
    dialog.componentInstance.data = this.generateDialogDataForPageWithNotSavedAttributes(languages);
    return dialog.afterClosed();
  }

  private canExitWhenPageAttributesWereEdited(): Observable<boolean> | Promise<boolean> | boolean {
    const languages = this.getLanguagesOfChangedForms();
    const formsData: Array<PageAttributes> = this.generateFormsData();
    if (formsData.length > 0) {
      const titles: string[] = formsData.reduce((accumulator: string[], pageAttributes: PageAttributes) => {
        accumulator.push(pageAttributes.Title);
        return accumulator;
      }, [])
      const dialog: MatDialogRef<ExitPageComponent> = this.dialog.open(ExitPageComponent, this.dialogConfig);
      dialog.componentInstance.languages = languages;
      return dialog.afterClosed().toPromise()
        .then(async result => {
          if (result.event == 'Save') {
            return await this.pageService.getPageTitleDuplicatesToPromise(this.pageId, titles)
              .then(async res => {
                const dialog: MatDialogRef<YesNoModalComponent> = this.dialog.open(YesNoModalComponent, this.dialogConfig);
                dialog.componentInstance.data = this.generateDialogDataWhenPageTitleDuplicates(res);
                return dialog.afterClosed().toPromise()
                  .then(async res => {
                    if (res === true) {
                      return await this.pageService.updatePageToPromise(this.pageId, {PageAttributes: formsData})
                        .then(() => true)
                        .catch(() => {
                          this.isError = true;
                          return false;
                        })
                    }
                    return false;
                  })
              })
              .catch(async () => {
                return await this.pageService.updatePageToPromise(this.pageId, {PageAttributes: formsData})
                .then(() => true)
                .catch(() => {
                  this.isError = true;
                  return false;
                })
              })
          }
          if (result.event == 'Stay') {
            return false;
          }
          return true;
        })
    }
    return true;
  }

  private generateDialogDataForPageWithNotSavedAttributes(languages: string): Object {
    return {
      'heading': 'На заданы атрибуты для страницы',
      'message': 'Если вы уйдете, не сохранятся изменения во вкладке(ах) ' + languages + '.',
      'buttonYesCaption': 'Остаться и заполнить поля',
      'buttonNoCaption': 'Уйти без сохранения'
    };
  }

  private generateFormsData(): Array<PageAttributes> {
    return this.forms.reduce((accumulator: Array<PageAttributes>, formComponent: PageAttributingFormComponent) => {
      if (formComponent.form.valid && formComponent.hasFormChanged()) {
        const keys = formComponent.form.controls.words.value.reduce((accumulator: any, word: Word) => {
          {accumulator[word.phrase] = formComponent.form.controls.words.value.indexOf(word)};
          return accumulator;
        }, {});
        
        accumulator.push({
          Title: formComponent.form.controls.title.value,
          Description: formComponent.form.controls.description.value,
          LanguageId: formComponent.languageId,
          Keywords: keys
        });
      }
      return accumulator;
    }, []);
  }
  
  private getLanguagesOfChangedForms(): string {
    return this.forms.reduce((accumulator: any, formComponent: PageAttributingFormComponent) => {
      if (formComponent.hasFormChanged()) {
        accumulator.push('"' + formComponent.heading + '"');
      }
      return accumulator;
    }, []).join(', ');
  }

  private generateDialogDataWhenPageTitleDuplicates(languageIds: number[]): Object {
    const languages: string = languageIds.reduce((accumulator: any, languageId: number) => {
        accumulator.push('"' + LanguagesMapper[languageId].name + '"');
      return accumulator;
    }, []).join(', ');
    return {
      'heading': 'Такое название страницы уже существует',
      'message': 'Такое название на языке(-ах) ' + languages + ' указано в атрибутах для другой страницы личного кабинета TravelLine.' +
        'Если вы сохраните это название сейчас, то пользователь ЛК увидит несколько результатов поиска с одинаковым названием, но с переходом на разные страницы.' +
        'Рекомендуем в название страницы добавить название раздела, в котором эта страница находится.',
      'buttonYesCaption': 'Редактировать название',
      'buttonNoCaption': 'Сохранить все равно'
    }
  }
}
