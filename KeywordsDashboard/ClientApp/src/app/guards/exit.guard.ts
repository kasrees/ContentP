import { CanDeactivate } from "@angular/router";
import { Observable } from "rxjs";
import { Injectable } from '@angular/core';
 
export interface ComponentCanDeactivate{
    canDeactivate: () => boolean | Observable<boolean> | Promise<boolean>;
}

@Injectable()
export class ExitGuard implements CanDeactivate<ComponentCanDeactivate>{
    canDeactivate(component: ComponentCanDeactivate) : Observable<boolean> | Promise<boolean> | boolean {
        return component.canDeactivate ? component.canDeactivate() : true;
    }
}
