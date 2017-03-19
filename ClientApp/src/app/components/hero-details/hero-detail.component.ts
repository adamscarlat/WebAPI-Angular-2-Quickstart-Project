import { Component, OnInit } from '@angular/core';

import { Hero } from '../../data/entities/hero/hero';
import { ActivatedRoute, Params }   from '@angular/router';
import { Location }                 from '@angular/common';
import { HeroService } from '../../services/hero/hero.service';

import 'rxjs/add/operator/switchMap';


@Component({
  //sets the source of the base address (module.id) for module-relative URLs such as the templateUrl.
  moduleId: module.id,

  //CSS selector that tells Angular to create and insert an instance of this component where it finds a <selector-name> tag in parent HTML. 
  selector: 'my-hero-detail',

  // module-relative address of this component's HTML template
  templateUrl: './hero-detail.component.html',

  //array of css files
  styleUrls: ['./hero-detail.component.css'],

  //array of dependency injection providers for services that the component requires
  providers: []

})

export class HeroDetailComponent implements OnInit {

	constructor(
	  private heroService: HeroService,
	  private route: ActivatedRoute,
	  private location: Location
	) {}

	//@Input decorator tells Angular that this property is public and available for binding by a parent component.
	hero: Hero;

	//runs when the component is instantiated. checks router for parameter (hero id) and gets the hero by his id.
	ngOnInit(): void {
	  this.route.params
	    .switchMap((params: Params) => this.heroService.getHero(+params['id']))
	    .subscribe(hero => this.hero = hero);
	}

	save(): void {
	  this.heroService.update(this.hero)
	    .then(() => this.goBack());
	}

	goBack(): void {
	  this.location.back();
	}	

}