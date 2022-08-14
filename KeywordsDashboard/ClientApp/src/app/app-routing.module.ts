import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageIndexComponent } from './components/page-index/page-index.component';
import { PageDetailComponent } from './components/page-detail/page-detail.component';
import { ExitGuard } from './guards/exit.guard';
import { LoginPageComponent } from './components/login-page/login-page.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full'},
  { path: 'home', component: PageIndexComponent, pathMatch: 'full'},
  { path: 'login', component: LoginPageComponent, pathMatch: 'full'},
  { path: 'pages/:id', component: PageDetailComponent, canDeactivate: [ExitGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
