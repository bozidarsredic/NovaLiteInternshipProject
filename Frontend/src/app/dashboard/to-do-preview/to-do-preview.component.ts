import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Response, ToDoList, ToDoListClient } from 'src/app/api/api-reference';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';



@Component({
  selector: 'app-to-do-preview',
  templateUrl: './to-do-preview.component.html',
  styleUrls: ['./to-do-preview.component.css']
})
export class ToDoPreviewComponent {

  @Input() toDoList = new Response();
  @Output() callDelete: EventEmitter<string> = new EventEmitter<string>();


  constructor(private listService: ToDoListClient, private router: Router, private http: HttpClient, private snackBar: MatSnackBar) { }
  toDoLists: ToDoList[] = [];
  delete(toDoList: ToDoList) {
    this.listService
      .deleteToDoList(toDoList.id!)
      .subscribe();




    this.callDelete.emit(this.toDoList.id);
  }

  update(toDoList: ToDoList) {
    this.router.navigateByUrl('/to-do-list/' + toDoList.id);
  }

  share(toDoList: ToDoList) {
    toDoList.shareTime = new Date()
    this.listService
      .updateToDoList(this.toDoList)
      .subscribe(_ => this.snackBar.open('saved!'));
  }
}
