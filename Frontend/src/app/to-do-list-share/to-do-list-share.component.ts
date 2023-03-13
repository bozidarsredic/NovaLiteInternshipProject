import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToDoList, ToDoListClient } from '../api/api-reference';

@Component({
  selector: 'app-to-do-list-share',
  templateUrl: './to-do-list-share.component.html',
  styleUrls: ['./to-do-list-share.component.css']
})
export class ToDoListShareComponent implements OnInit {

  constructor(private listService: ToDoListClient,private activatedRoute: ActivatedRoute) { }
  

  toDoList= new ToDoList()

  ngOnInit() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');

    this.listService
      .getShareToDoListById(id!)
      .subscribe(response => {
        this.toDoList = response;
      });
  }

}
