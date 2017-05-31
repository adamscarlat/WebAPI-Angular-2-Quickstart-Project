import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/user/user.service';
import { NgForm } from '@angular/forms'
import { FormBuilder, Validators } from '@angular/forms';

@Component({
    //sets the source of the base address (module.id) for module-relative URLs such as the templateUrl.
    moduleId: module.id,

    selector: 'login',
    
    templateUrl: './login.component.html'
})
export class LoginComponent {

    constructor(private userService: UserService, public fb: FormBuilder, private router: Router) { }
    
      public loginForm = this.fb.group({
        username: ["", Validators.required],
        password: ["", Validators.required]
    });

    // onSubmit(email: string, password: string) {
    //     console.log(email, password)
    //     this.userService.login(email, password).subscribe((result) => {
    //         if (result) {
    //             this.router.navigate(['']);
    //         }
    //     });
    // }

    onSubmit(event: any)
    {
        console.log(event);

        var username = this.loginForm.value.username;
        var password = this.loginForm.value.password;
        this.userService.login(username, password).subscribe((result) => {
            if (result) {
                this.router.navigate(['']);
            }
        });
    }
}