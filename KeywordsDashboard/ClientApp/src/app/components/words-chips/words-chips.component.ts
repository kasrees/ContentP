import { ENTER } from '@angular/cdk/keycodes';
import { Component, Input } from '@angular/core';
import { MatChipInputEvent } from '@angular/material/chips';
import { Word } from 'src/app/models/Word';
import { FormGroup } from '@angular/forms';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { SortWordsComponent } from '../dialogs/sort-words/sort-words.component';

@Component({
  selector: 'app-words-chips',
  templateUrl: './words-chips.component.html',
  styleUrls: ['./words-chips.component.scss']
})

export class WordsChipsComponent {
  public readonly separatorKeysCodes = [ENTER] as const;
  public suggestedWords: Word[] = [];
  @Input() form: FormGroup;
  @Input() languageName: string;
  @Input() maxWordsListLength: number;

  constructor(private dialog: MatDialog) { }

  get wordsControl(): Word[] {
    return this.form.controls.words.value;
  }

  add(event: MatChipInputEvent): void {
    const newWord = (event.value || '').trim();
    if (newWord) {
      let existingWord = this.getWord(newWord);
      if (existingWord) {
        this.replaceExistingWordToTheEnd(existingWord);
        this.form.get('words')?.setErrors(null);
      } else if (this.wordsControl.length < this.maxWordsListLength) {
        this.wordsControl.push({phrase: newWord, isSuggested: false});
        this.form.get('words')?.setErrors(null);
      }
    }
    event.chipInput?.clear();
  }

  remove(word: Word): void {
    const index = this.wordsControl.indexOf(word)
    if (index >= 0) {
      this.wordsControl.splice(index, 1);
    }
    if (word.isSuggested) {
      this.suggestedWords.push(word);
    }
  }

  moveSuggestedWordToKeywordsList(word: Word): void {
    if (this.wordsControl.length < this.maxWordsListLength) {
      this.wordsControl.push(word);
      this.form.get('words')?.setErrors(null);
      const index = this.suggestedWords.indexOf(word);
      if (index >= 0) {
        this.suggestedWords.splice(index, 1);
      }
    }
  }

  drop(event: CdkDragDrop<Word[]>): void {
    moveItemInArray(this.wordsControl, event.previousIndex, event.currentIndex);
  }

  onInput(): void {
    this.form.get('words')?.setErrors(null);
  }

  sortWords(): void {
    if (this.wordsControl.length > 1) {
      const dialog: MatDialogRef<SortWordsComponent> = this.dialog.open(SortWordsComponent);
      dialog.componentInstance.words = this.wordsControl;
    }
  }

  private getWord(soughtWord: string): Word | undefined {
    let foundWord: Word | undefined = this.wordsControl.find((word: Word) => { 
      return (word.phrase === soughtWord) 
    });
    return foundWord;
  }

  private replaceExistingWordToTheEnd(word: Word): void {
    const index = this.wordsControl.indexOf(word);
    let deletedWords: Word[] = this.wordsControl.splice(index, 1);
    this.wordsControl.push(...deletedWords);
  }
}
