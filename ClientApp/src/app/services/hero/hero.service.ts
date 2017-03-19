import { Injectable }    from '@angular/core';
import { Headers, Http, RequestOptions } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { Hero } from '../../data/entities/hero/hero';


/*
Services
Components are big consumers of services. Component classes should be lean. 
They don't fetch data from the server, validate user input, or log directly to the console. 
They delegate such tasks to services.

Observables
Each Http service method returns an Observable of HTTP Response objects.
Our HeroService converts that Observable into a Promise and returns the promise to the caller.
An observable is a stream of events that we can process with array-like operators.


*/

@Injectable()
export class HeroService {

 	//private heroesUrl = 'api/heroes';  // URL to web api
	private heroesUrl = 'http://localhost:5000/api/heroes';
	private headers = new Headers({'Content-Type': 'application/json'});
	private options: any;

	constructor(private http: Http) { 
		this.headers.append('Access-Control-Allow-Origin' , 'http://localhost:5000');
		this.options = new RequestOptions({ headers: this.headers });
	}


	getHeroes(): Promise<Hero[]> {
		return this.http.get(this.heroesUrl, this.options)
			.toPromise()
			.then(response => {
				console.log(response.json());
				var heroArray = this.createHeroesArray(response);
				return heroArray;
			})
			.catch(this.handleError);
	}

	getHero(id: number): Promise<Hero> {
	  const url = `${this.heroesUrl}/${id}`;
	  return this.http.get(url)
	    .toPromise()
	    .then(response => {
			let hero = this.createHeroObject(response.json());
		 	return hero;
		})
	    .catch(this.handleError);
	}

	update(hero: Hero): Promise<Hero> {
	  const url = `${this.heroesUrl}/${hero.id}`;
	  return this.http
	    .put(url, JSON.stringify(hero), this.options)
	    .toPromise()
	    .then(() => hero)
	    .catch(this.handleError);
	}

	create(name: string): Promise<Hero> {
	  return this.http
	    .post(this.heroesUrl, JSON.stringify({name: name}), this.options)
	    .toPromise()
	    .then(res => this.createHeroObject(res.json()))
	    .catch(this.handleError);
	}

	delete(id: number): Promise<void> {
	  const url = `${this.heroesUrl}/${id}`;
	  return this.http.delete(url, {headers: this.headers})
	    .toPromise()
	    .then(() => null)
	    .catch(this.handleError);
	}

	private handleError(error: any): Promise<any> {
		console.error('An error occurred', error); // for demo purposes only
		return Promise.reject(error.message || error);
	}

	private createHeroesArray(response: any): Array<Hero> {

		var heroArray: Array<Hero> = [] 
		for (let i = 0; i < response.json().length; i++) {
			let hero = this.createHeroObject(response.json()[i]);
			heroArray.push(hero);
		}

		return heroArray;
	}

	private createHeroObject(response: any): Hero {
		let hero = new Hero();
		hero.id = response['heroId']
		hero.name = response['heroName']
		return hero;
	}

}