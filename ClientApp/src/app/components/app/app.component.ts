//Each Angular library name begins with the @angular prefix.
//You install them with the npm package manager and import parts of them with JavaScript import statements. 
import { Component } from '@angular/core';

/*
Directives
Angular templates are dynamic. When Angular renders them, it transforms the DOM according to the instructions given by directives.
Two other kinds of directives exist: structural and attribute directives.

Structural directives- alter layout by adding, removing, and replacing elements in DOM.
Ex: <li *ngFor="let hero of heroes"></li>

Attribute directives- alter the appearance or behavior of an existing element. 
In templates they look like regular HTML attributes, hence the name.
Ex: <input [(ngModel)]="hero.name">

*/



/*
Template synthax
You define a component's view with its companion template. 
A template is a form of HTML that tells Angular how to render the component.

4 types of data binding
{{interpolation}} - The material between the braces is often the name of a component property. One-way from data source to view target

[property]="template_expression" - the template expression is assigned to a property of a binding target. 
								The left hand side [property] is found on a different componenet (with @Input decorator)

(event)="statement" - One-way from view target to data source

[(ngModel)] = "expression" - two way binding

*/

@Component({
	selector: "my-app",
	moduleId: module.id,
	styleUrls: ['./app.component.css'],
	template: `
	    <h1>{{title}}</h1>
	       <nav>
		     <a routerLink="/dashboard" routerLinkActive="active">Dashboard</a>
		     <a routerLink="/heroes" routerLinkActive="active">Heroes</a>
		   </nav>
   		<router-outlet></router-outlet>
	`,
})

/*

*The export keyword makes the class public
*/
export class AppComponent {
	title = 'Tour of Heroes';
}