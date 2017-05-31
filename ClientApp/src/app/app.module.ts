import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule }   from '@angular/forms';
import { HttpModule }    from '@angular/http';
import { ReactiveFormsModule } from '@angular/forms';

// Imports for loading & configuring the in-memory web api
import { InMemoryWebApiModule } from 'angular-in-memory-web-api';
//import { InMemoryDataService }  from './services/in-memory-data/in-memory-data.service';

import { AppComponent }  from './components/app/app.component';
import { HeroDetailComponent } from './components/hero-details/hero-detail.component';
import { HeroesComponent } from './components/hero-list/heroes.component';
import { HeroService } from './services/hero/hero.service';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AppRoutingModule } from './router/app-routing.module';
import { HeroSearchComponent } from './components/hero-search/hero-search.component';
import { UserService } from './services/user/user.service';
import { LoggedInGuard } from './login/logged-in.guard';
import { LoginComponent } from './login/login.component';



/*
Angular Modules help organize an application into cohesive blocks of functionality.
An Angular Module is a class adorned with the @NgModule decorator function. 
@NgModule takes a metadata object that tells Angular how to compile and run module code.
It identifies the module's own components, directives and pipes, 
making some of them public so external components can use them.  
*/


//An Angular module, whether a root or feature, is a class with an @NgModule decorator
@NgModule({

  //other modules whose exported classes are needed by component templates declared in this module
  imports: [BrowserModule,ReactiveFormsModule, FormsModule, AppRoutingModule, HttpModule, 
          //InMemoryWebApiModule.forRoot(InMemoryDataService),
   ],

  //the subset of declarations that should be visible and usable in the component templates of other modules.
  //A root module has no reason to export anything because other components don't need to import the root module.
  exports:		[],

  //the view classes that belong to this module. user defined components, directives and pipes
  declarations: [ AppComponent, HeroDetailComponent, HeroesComponent, DashboardComponent, HeroSearchComponent, LoginComponent ],

  //the main application view, called the root component, that hosts all other app views. Only the root module should set this bootstrap property
  bootstrap:    [ AppComponent ],
  
  //used for dependency injection- creators of services that this module contributes to the global collection of services; they become accessible in all parts of the app.
  providers: 	[ HeroService, UserService, LoggedInGuard ],

})
export class AppModule { }
