import { Component, OnInit } from '@angular/core';
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import { EventMessage, EventType, InteractionStatus } from '@azure/msal-browser';
import { filter, Subject, takeUntil } from 'rxjs';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    title = 'to-do-web-app';
    isIframe = false;
    loginDisplay = false;
    private readonly _destroying$ = new Subject<void>();
    email!: string

    constructor(private broadcastService: MsalBroadcastService, private authService: MsalService, private msalBroadcastService: MsalBroadcastService) { }

    ngOnInit() {

        this.msalBroadcastService.msalSubject$
            .pipe(
                filter((msg: EventMessage) => msg.eventType === EventType.LOGIN_SUCCESS),
            )
            .subscribe((result: EventMessage) => {
                console.log(result);
            });

        this.isIframe = window !== window.parent && !window.opener;

        this.broadcastService.inProgress$
            .pipe(
                filter((status: InteractionStatus) => status === InteractionStatus.None),
                takeUntil(this._destroying$)
            )
            .subscribe(() => {
                this.setLoginDisplay();
            })

        }

    login() {
        this.authService.loginPopup()
            .subscribe({
                next: (result) => {
                    console.log(result);
                    this.setLoginDisplay();
                    localStorage.setItem('token', result.accessToken);
                },
                error: (error) => console.log(error)
            });
    }

    logout() {
        this.authService.logoutPopup({
            mainWindowRedirectUri: "/"
        });
        localStorage.removeItem('token');
    }

    setLoginDisplay() {
        this.loginDisplay = this.authService.instance.getAllAccounts().length > 0;
    }

    ngOnDestroy(): void {
        this._destroying$.next(undefined);
        this._destroying$.complete();
    }
}




