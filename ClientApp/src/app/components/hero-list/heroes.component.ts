import { Component } from '@angular/core';
import { Hero } from '../../data/entities/hero/hero';
import { HeroService } from '../../services/hero/hero.service';
import { OnInit } from '@angular/core';
import { Router } from '@angular/router';



/*
To tell Angular that HeroListComponent is a component, attach metadata to the class.
it identifies the class immediately below it as a component class.
The metadata in the @Component tells Angular where to get the major building blocks you specify for the component.
*/
@Component({
	selector: 'my-heroes',
	moduleId: module.id,
	templateUrl: './heroes.component.html',
	styleUrls: ['./heroes.component.css'],
})


/*
A component controls a patch of screen called a view. You define a component's application logic—what it does to support the view—inside a class. 
The class interacts with the view through an API of properties and methods. 
Angular creates, updates, and destroys components as the user moves through the application.
Your app can take action at each moment in this lifecycle through optional lifecycle hooks, like ngOnInit() declared above.

A component's job is to enable the user experience and nothing more. It mediates between the view (rendered by the template) 
and the application logic (which often includes some notion of a model). 
A good component presents properties and methods for data binding. It delegates everything nontrivial to services.

*/
export class HeroesComponent implements OnInit {


	selectedHero: Hero;
	heroes: Hero[];

	/*
	Dependency Injection 
	Angular can tell which services a component needs by looking at the types of its constructor parameters.
	*/
	constructor(private heroService: HeroService, private router: Router) { }

  	ngOnInit(): void {
	    this.getHeroes();
  	}

	onSelect(hero: Hero): void {
		this.selectedHero = hero;
	}

	//Gets the heros list asynchronisly
  	getHeroes(): void {
    	var heroes = this.heroService.getHeroes().then(heroes => 
		{	
			console.log(heroes);
			this.heroes = heroes;
		});
		
  	}	

  	//navigates to hero-detail using the routes defined in app-routing.ts
  	gotoDetail(): void {
  		this.router.navigate(['/detail', this.selectedHero.id]);
  	}

  	add(name: string): void {
		name = name.trim();

		if (!name) { 
			return; 
		}

		this.heroService.create(name)
			.then(hero => {
				this.heroes.push(hero);
				this.selectedHero = null;
		});
	}

	delete(hero: Hero): void {
	  this.heroService
	      .delete(hero.id)
	      .then(() => {
	      	
	        this.heroes = this.heroes.filter(h => h !== hero);

	        if (this.selectedHero === hero) {
	        	this.selectedHero = null; 
	        }
	      });
	}

}





