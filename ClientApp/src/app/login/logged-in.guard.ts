import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { UserService } from '../services/user/user.service';

/*
A guard that will be used by the routes (see routes.ts).
If CanActivate returns false than the route will be inacessible

TODO: pass an informative error msg to redirect page if guard fails
*/
@Injectable()
export class LoggedInGuard implements CanActivate {
    constructor(private user: UserService, private router: Router) { }

    canActivate() {
        console.log('LoggedInGuard.canActivate');
        if (this.user.isLoggedIn()) {
            return this.user.isLoggedIn();
        }
        this.router.navigate(['./login']);
        return false;
    }
}