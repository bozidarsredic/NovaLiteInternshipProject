import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MatCardModule } from '@angular/material/card';
import { ToDoPreviewComponent } from './dashboard/to-do-preview/to-do-preview.component';
import { ToDoListComponent } from './to-do-list/to-do-list.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NoteComponent } from './to-do-list/note/note.component';
import { MatSnackBarModule, MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';
import { MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { NoteDialogComponent } from './to-do-list/note-dialog/note-dialog.component';
import { MessageService } from './message.service';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MsalModule, MsalRedirectComponent } from '@azure/msal-angular';
import { PublicClientApplication } from '@azure/msal-browser';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { HTTP_INTERCEPTORS, } from "@angular/common/http"; // Import 
import { MsalGuard, MsalInterceptor } from '@azure/msal-angular'; // Import MsalInterceptor
import { InteractionType, } from '@azure/msal-browser';
import { ToDoListShareComponent } from './to-do-list-share/to-do-list-share.component';








const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;


@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    ToDoPreviewComponent,
    ToDoListComponent,
    NoteComponent,
    NoteDialogComponent,
    ToDoListShareComponent,

  ],
  imports: [
    BrowserModule,
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatToolbarModule,
    MatListModule,
    MatCardModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatSnackBarModule,
    MatDialogModule,
    DragDropModule,

    MsalModule.forRoot(new PublicClientApplication({
      auth: {
        clientId: '4e1ff54b-bf34-4f45-83ce-e50fc32967cd',
        authority: 'https://login.microsoftonline.com/common',
        redirectUri: 'http://localhost:4200'
      },
      cache: {
        cacheLocation: 'localStorage',
        storeAuthStateInCookie: isIE
      }
    }), null!, {
      interactionType: InteractionType.Redirect,
      protectedResourceMap: new Map([
        [
          'https://localhost:7044/api/to-do-lists/share', null
        ],
        ['https://localhost:7044/api/',
          [
            'api://dbf7f51e-d046-435b-88ee-c4f9ee872967/to-do-lists.read',
            'api://dbf7f51e-d046-435b-88ee-c4f9ee872967/to-do-lists.write'
          ]]
      ])
    }),



  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    MsalGuard,
    MessageService,
    {
      provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: { duration: 2500 }
    }
  ],
  bootstrap: [AppComponent, MsalRedirectComponent]
})
export class AppModule { }
