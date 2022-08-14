import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Meta } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'app';
  isLoginPage: boolean;

  constructor(private location: Location, private readonly meta: Meta) { }

  ngOnInit(): void {
    this.meta.removeTag('name="viewport"');
    this.meta.addTag({ name: 'viewport', content: 'width=device-width, shrink-to-fit=no' });
    if (this.location.path().indexOf('/login') > -1) {
      this.isLoginPage = true;
    }
  }
}
