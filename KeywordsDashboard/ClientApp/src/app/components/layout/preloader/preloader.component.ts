import { Component } from '@angular/core';
import { Subject } from 'rxjs';
import { PreloaderService } from 'src/app/services/preloader.service';

@Component({
  selector: 'app-preloader',
  templateUrl: './preloader.component.html',
  styleUrls: ['./preloader.component.scss']
})
export class PreloaderComponent {
  isLoading: Subject<boolean> = this.preloaderService.isLoading;

  constructor(private preloaderService: PreloaderService) {
  }
}
