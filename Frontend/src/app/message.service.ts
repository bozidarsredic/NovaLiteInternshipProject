import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { CreateToDoListCommand, ToDoListClient } from './api/api-reference';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private subject = new Subject<any>();
  public subject$ = this.subject.asObservable();

  public constructor(private readonly client: ToDoListClient) { }

  public createList = () => {
    this.client.createToDoList(new CreateToDoListCommand())
      .subscribe(result => this.subject.next(result));
  }


}
