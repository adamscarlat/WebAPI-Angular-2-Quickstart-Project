import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent }   from '../components/dashboard/dashboard.component';
import { HeroesComponent }      from '../components/hero-list/heroes.component';
import { HeroDetailComponent }  from '../components/hero-details/hero-detail.component';
import { LoggedInGuard } from '../login/logged-in.guard';
import { LoginComponent } from '../login/login.component';


const routes: Routes = [
  { path: '', redirectTo: '/heroes', pathMatch: 'full' },
  { path: 'dashboard',  component: DashboardComponent, canActivate: [LoggedInGuard] },
  { path: 'detail/:id', component: HeroDetailComponent, canActivate: [LoggedInGuard] },
  { path: 'heroes',     component: HeroesComponent, canActivate: [LoggedInGuard] },
  { path: 'login',      component: LoginComponent}
];


@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}