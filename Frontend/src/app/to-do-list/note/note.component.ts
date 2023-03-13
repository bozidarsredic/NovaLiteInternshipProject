import { Component, EventEmitter, Input, Output, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Note, NoteModel, ToDoListClient } from 'src/app/api/api-reference';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NoteDialogComponent } from '../note-dialog/note-dialog.component';

@Component({
  selector: 'app-note',
  templateUrl: './note.component.html',
  styleUrls: ['./note.component.css']
})
export class NoteComponent {
  @Output() callDelete: EventEmitter<string> = new EventEmitter<string>();
  @Input() toDoListId: string | undefined;
  @Input() formGroup = new FormGroup({
    id: new FormControl<string | undefined>(undefined),
    content: new FormControl<string | undefined>(''),
    isCompleted: new FormControl<boolean>(false),
    position: new FormControl<number>(0)
  });
  constructor(private listService: ToDoListClient) { }


  updateNote() {
    this.listService
      .updateNote(this.toDoListId!, this.formGroup.controls['id'].value!, this.formGroup.value as Note)
      .subscribe();
  }
  deleteNote() {

    this.listService
      .deleteNote(this.toDoListId!, this.formGroup.controls['id'].value!)
      .subscribe();

    this.callDelete.emit(this.formGroup.controls['id'].value!)
  }


}



