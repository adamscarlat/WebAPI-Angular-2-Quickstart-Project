import { Injectable } 					from '@angular/core';
import { Http }       					from '@angular/http';
import { Observable }    		 		from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { Hero }           				from '../../data/entities/hero/hero';

/*
the heroes property is now an Observable of hero arrays, rather than just a hero array. 
The *ngFor can't do anything with an Observable until we flow it through the async pipe (AsyncPipe). 
The async pipe subscribes to the Observable and produces the array of heroes to *ngFor.
*/

@Injectable()
export class HeroSearchService {

  constructor(private http: Http) {}

  search(term: string): Observable<Hero[]> {
    return this.http
               .get(`app/heroes/?name=${term}`)
               .map(response => response.json().data as Hero[]);
  }
}