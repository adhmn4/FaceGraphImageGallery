import { Routes, RouterModule, CanActivate } from '@angular/router';

import { HomeComponent } from './home';
import { LoginComponent } from './login';
import { AuthGuard } from './_guards';
import { ImagesComponent } from './images';

const appRoutes: Routes = [
    {
        path: '',
        component: HomeComponent
        // canActivate: [AuthGuard]
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'images',
        component: ImagesComponent
    },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);