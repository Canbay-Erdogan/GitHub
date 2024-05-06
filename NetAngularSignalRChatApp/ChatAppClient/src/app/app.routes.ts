import { Routes } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';

export const routes: Routes = [
    {
        path:"",
        component:HomeComponent,
        canActivate:[()=>inject(AuthService).isAuthenticate()]
    },
    {
        path:"login",
        component:LoginComponent
    },

    {
        path:"register",
        component:RegisterComponent
    }
];
