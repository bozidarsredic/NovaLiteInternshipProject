import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from "@angular/router";
import { MsalService } from "@azure/msal-angular";
import { Observable } from "rxjs";

@Injectable({
    'providedIn': 'root'
})
export class RouteGuard implements CanActivate {
    constructor(private readonly msalService: MsalService) { }

    canActivate(_route: ActivatedRouteSnapshot, _state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
       
        if (localStorage.getItem('token'))
          return true;
        else {
           return false;
        }
    }
}