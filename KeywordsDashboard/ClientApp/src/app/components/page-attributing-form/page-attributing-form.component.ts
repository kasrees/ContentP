import { Component, OnInit, Input } from '@angular/core';
import { PageDetail } from 'src/app/models/pageDetail';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PageService } from 'src/app/services/page.service';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog, MatDialogRef, MatDialogConfig } from '@angular/material/dialog';
import { YesNoModalComponent } from '../dialogs/yes-no-modal/yes-no-modal.component';
import { httpResponseStatusCodes } from 'src/app/models/httpResponseStatusCodes';
import { Word } from '../../models/Word';

@Component({
  selector: 'app-page-attributing-form',
  templateUrl: './page-attributing-form.component.html',
  styleUrls: ['./page-attributing-form.component.scss']
})

export class PageAttributingFormComponent implements OnInit {
  @Input() pageTranslation: PageDetail;
  public form: FormGroup;
  public languageName: string;
  public heading: string;
  public languageId: number;
  public maxWordsListLength: number = 30;
  private pageId: number;
  private title: string;
  private description: string;
  private words: Word[]; 
  private initialState: {
    'title': string,
    'description': string,
    'words': Word[]
  } = {
    'title': "",
    'description': "",
    'words': []
  };
  private dialogConfig = new MatDialogConfig();
  private dialogData = {
    'heading': 'Такое название страницы уже существует',
    'message': 'Такое название указано в атрибутах для другой страницы личного кабинета TravelLine.' + '\n' + '\n' +
      'Если вы сохраните это название сейчас, то пользователь ЛК увидит несколько результатов поиска с одинаковым названием, но с переходом на разные страницы.' + '\n\n' +
      'Рекомендуем в название страницы добавить название раздела, в котором эта страница находится.',
    'buttonYesCaption': 'Редактировать название',
    'buttonNoCaption': 'Сохранить все равно'
  };
 
  constructor(private fb: FormBuilder, private pageService : PageService, private dialog: MatDialog) {
    this.dialogConfig.disableClose = true;
    this.dialogConfig.backdropClass = 'es-dialog-background';
    this.dialogConfig.panelClass = "es-dialog";
  }

  ngOnInit(): void {
    this.pageId = this.pageTranslation.pageId;
    this.heading = this.pageTranslation.heading;
    this.languageName = this.pageTranslation.languageName;
    this.title = this.pageTranslation.title;
    this.description = this.pageTranslation.description;
    this.languageId = this.pageTranslation.languageId;
    this.mapKeywordsToWords();
    this.setForm();
    this.setInitialState();
  }

  isPageJustAddedAndNotChanged(): boolean {
    return (
      !this.hasFormChanged() &&
      this.form.controls.title.value === "" &&
      this.form.controls.description.value === "" &&
      this.form.controls.words.value.length === 0
    );
  }

  setValidators(): void {
    this.form.controls["title"].addValidators([Validators.required]);
    this.form.controls["description"].addValidators([Validators.maxLength(200)]);
    this.form.controls["words"].addValidators([Validators.required, Validators.maxLength(this.maxWordsListLength)]);
  }

  updateValueAndValidity(): void {
    for (let controlName in this.form.controls) {
      this.form.controls[controlName].updateValueAndValidity();
    }
  }

  hasFormChanged(): boolean {
    let hasTitleChanged = this.initialState['title'] !== this.form.get("title")?.value;
    let hasDescriptionChanged = this.initialState['description'] !== this.form.get("description")?.value;
    let haveWordsChanged = this.doWordsObjectsDiffer(this.initialState['words'], this.form.get("words")?.value);
    return hasTitleChanged || hasDescriptionChanged || haveWordsChanged;
  }

  onInput(fieldName : string): void {
    this.form.get(fieldName)?.clearValidators();
  }

  onSubmit(): void {
    this.form.setErrors(null);
    this.form.markAllAsTouched();
    this.setValidators();
    this.updateValueAndValidity();

    if (!this.hasFormChanged() || this.form.invalid) {
      return;
    }
    const keywords: Object[] = this.generateKeywordsListForRequest();
    this.pageService.getPageTitleDuplicatesToObservable(this.pageId, this.form.controls.title.value).subscribe(
      () => {
        const dialog: MatDialogRef<YesNoModalComponent> = this.dialog.open(YesNoModalComponent, this.dialogConfig);
        dialog.componentInstance.data = this.dialogData;
        dialog.afterClosed().subscribe(result => {
          if (result === true) {
            this.savePage(keywords);
          }
        });
      },
      (error: any) => {
        if (error instanceof HttpErrorResponse) {
          if (error.status === httpResponseStatusCodes.NotFound) {
            this.savePage(keywords);
            return;
          } 
        }
        this.form.setErrors({error: true})
      }
    )
  }

  private savePage(keys: Object[]): void {
    this.pageService.updatePageToObservable(this.pageId, {PageAttributes: [{
      Title: this.form.controls.title.value,
      Description: this.form.controls.description.value,
      LanguageId: this.languageId,
      Keywords: keys
    }]}).subscribe(
      () => {
        this.setInitialState();
      },
      () => {
        this.form.setErrors({error: true})
      }
    )
  }

  private generateKeywordsListForRequest(): Object[]  {
    return this.form.controls.words.value.reduce((accumulator: any, word: Word) => {
      {accumulator[word.phrase] = this.form.controls.words.value.indexOf(word)};
      return accumulator;
    }, {});
  }

  private setForm(): void {
    this.form = this.fb.group({
      title: [this.title],
      description: [this.description],
      words: [this.words],
    });
  }

  private setInitialState(): void {
    this.initialState['title'] = this.form.controls.title.value;
    this.initialState['description'] = this.form.controls.description.value;
    this.initialState['words'] = this.form.controls.words.value.map((word: Word) => word);
  }

  private mapKeywordsToWords(): void {
    this.words = this.pageTranslation.keywords.map((keyword) => {
      return {
        phrase: keyword.phrase,
        isSuggested: false
      }
    })
  }

  private doWordsObjectsDiffer(x: Word[], y: Word[]): boolean {
    if (x.length !== y.length) {
      return true;
    }
    for (let i = 0; i < x.length; ++i) {
      if (x[i].phrase !== y[i].phrase) {
        return true;
      }
    }
    return false;
  }
}
