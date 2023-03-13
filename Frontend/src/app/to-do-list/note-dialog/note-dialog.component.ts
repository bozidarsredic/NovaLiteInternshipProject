import { Component, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { GetNotesModel, Note, ToDoListClient } from 'src/app/api/api-reference';
import { MessageService } from 'src/app/message.service';


@Component({
  selector: 'app-note-dialog',
  templateUrl: './note-dialog.component.html',
  styleUrls: ['./note-dialog.component.css']
})
export class NoteDialogComponent implements OnDestroy {
  messages: any[] = [];
  subscription!: Subscription;
  note = new Note();
  

  constructor(@Inject(MAT_DIALOG_DATA) public data: string, private matDialogRef: MatDialogRef<NoteDialogComponent>,
    private listService: ToDoListClient, private messageService: MessageService) {

    this.subscription = this.messageService.subject$.subscribe(result => {
      if (result) {
        this.listService
          .createNote(result.id, this.note)
          .subscribe(
            data => {
              const notesModel = data as GetNotesModel;
              this.matDialogRef.close(notesModel);

            }
          );
      }
    });


  }

  content = '';

  onClosed() {
    this.matDialogRef.close()
  }

  onAdd() {
    this.note.content = this.content;
    if (!this.note.content) {
      return;
    }
    if (!this.data) {
      
      return this.messageService.createList();
    }
    this.listService
      .createNote(this.data, this.note)
      .subscribe(
        data => {
          const notesModel = data as GetNotesModel;
          this.matDialogRef.close(notesModel);
        }
      );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
