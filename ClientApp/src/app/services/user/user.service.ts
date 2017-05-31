import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';

@Injectable()
export class UserService {

    private loggedIn = true;
    private storage = localStorage;
    private loginUrl = 'http://localhost:5000/api/auth/login'
    private headers = new Headers({'Content-Type': 'application/json'});
	private options: any;

    /*
    Check if auth token is in localstorage, if so, set loggedin status to true
    */
    constructor(private http: Http) {
        //TODO: if no auth token this will return false
        this.loggedIn = !!this.storage.getItem('auth_token');
        this.headers.append('Access-Control-Allow-Origin' , 'http://localhost:5000');
		this.options = new RequestOptions({ headers: this.headers });
    }

    /*
    After a successful login the server will return an auth token to the client app.
    The client app will store the token in the local storage (browser local storage)
    and set the logged in status to true.
    */
    login(username: string, password: string) {

        return this.http
            .post(
                this.loginUrl,
                JSON.stringify(
                {   
                    username : username, 
                    password : password 
                }),
                this.options
            )
            .map(res => res.json())
            .map((res) => {
                console.log(res)
                console.log(res.responseObject.accessToken)
                if (res.isSuccess == 'true') {
                    console.log('in conditional...')
                    this.storage.setItem('auth_token', res.responseObject.accessToken);
                    this.loggedIn = true;
                }

                return res.isSuccess;
            });
    }

    /*
    remove auth token from localstorage and set logged in status to false
    */
    logout() {
        //TODO: define remove item in the abstract StorageService class and use it here
        //localStorage.removeItem('auth_token');
        this.loggedIn = false;
    }

    isLoggedIn() {
        console.log('UserService.isLoggedIn' + this.loggedIn);
        return this.loggedIn;
    }
}