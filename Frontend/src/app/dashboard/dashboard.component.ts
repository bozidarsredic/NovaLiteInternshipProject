import { Component, OnInit, } from '@angular/core';
import { Response, ToDoList, ToDoListClient } from '../api/api-reference';
import { CdkDragDrop, moveItemInArray, } from '@angular/cdk/drag-drop';
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})

export class DashboardComponent implements OnInit {
  constructor(private listService: ToDoListClient, private authService: MsalService, private msalBroadcastService: MsalBroadcastService, private router: Router, private http: HttpClient) { }
  toDoLists: Response[] = [];
  selectedProduct: any;
  data: any;
  loginDisplay = false;
  email!: string

  item!: ToDoList
  ngOnInit(): void {
    this.listService
      .getToDoLists()
      .subscribe(response => this.toDoLists = response);


    this.email = this.authService.instance.getAllAccounts()[0].username

  }
  deleteToDoList(listId: string) {
    this.toDoLists = this.toDoLists.filter(item => item.id !== listId);
  }
  drop(event: CdkDragDrop<string[]>) {
    this.listService
      .updateToDoListPosition(this.toDoLists[event.previousIndex].id!, Math.abs(event.currentIndex - this.toDoLists.length + 1))
      .subscribe();

    moveItemInArray(this.toDoLists, event.previousIndex, event.currentIndex);
  }

}




