import { Component } from '@angular/core';
import {CdkDragDrop, moveItemInArray} from '@angular/cdk/drag-drop';
import { Word } from 'src/app/models/Word';

@Component({
  selector: 'app-sort-words',
  templateUrl: './sort-words.component.html',
  styleUrls: ['./sort-words.component.scss']
})
export class SortWordsComponent {
  public words: Word[];
  constructor() { }

  drop(event: CdkDragDrop<any[]>) {
    moveItemInArray(this.words, event.previousIndex, event.currentIndex);
  }
}
