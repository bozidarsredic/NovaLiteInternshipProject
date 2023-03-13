import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { GetNotesModel, Response, ToDoListClient } from 'src/app/api/api-reference';
import { NoteDialogComponent } from './note-dialog/note-dialog.component';


import { MessageService } from '../message.service';
import { Subscription } from 'rxjs';
import { MsalService } from '@azure/msal-angular';

@Component({
  selector: 'app-to-do-list',
  templateUrl: './to-do-list.component.html',
  styleUrls: ['./to-do-list.component.css']
})
export class ToDoListComponent implements OnDestroy, OnInit {

  subscription!: Subscription;
  messages: any[] = [];

  constructor( private authService: MsalService,private activatedRoute: ActivatedRoute, private listService: ToDoListClient,
    private snackBar: MatSnackBar, public dialog: MatDialog, private messageService: MessageService) {

    this.subscription = this.messageService.subject$.subscribe(result => {
      if (result) {
        this.toDoList.id = result.id;
      }
    });
  }

  toDoList: Response = new Response();
  public formGroup = new FormGroup({
    title: new FormControl("", Validators.required),
    remainder: new FormControl<Date | undefined>(undefined),
    notes: new FormArray<FormGroup>([])
  });

  ngOnInit() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.listService
        .getToDoListById(id)
        .subscribe(response => {
          this.toDoList = response;
          this.patchForm();
        });
    }

  }

  save() {    
    if (this.toDoList.id) {
      this.toDoList.title = this.formGroup.controls['title'].value!;
      this.toDoList.owner = this.authService.instance.getAllAccounts()[0].username ;
      //this.toDoList.remainder = this.formGroup.controls['remainder'].value!;
      this.listService
        .updateToDoList(this.toDoList)
        .subscribe(_ => this.snackBar.open('saved!'));
    }
    else {
      this.toDoList.title = this.formGroup.controls['title'].value!;
      this.toDoList.owner = this.authService.instance.getAllAccounts()[0].username ;
      //this.toDoList.remainder = this.formGroup.controls['remainder'].value!;
      this.listService
        .createToDoList(this.toDoList)
        .subscribe(_ => this.snackBar.open('saved!'));
    }
  }


  get notes() {
    return this.formGroup.controls.notes;
  }

  get notesControls() {
    return this.notes.controls;
  }

  get completed() {
    return this.notesControls.filter(x => x.controls['isCompleted'].value === true);
  }

  get notCompleted() {
    return this.notesControls.filter(x => x.controls['isCompleted'].value === false);
  }

  private readonly patchForm = () => {
    this.formGroup.controls.title.patchValue(this.toDoList.title ?? '');
    this.formGroup.controls.remainder.patchValue(this.toDoList.remainder);
    this.formGroup.controls.notes.controls = this.toDoList.notes!.map(note => this.createFormGroup(note));
  }

  private readonly createFormGroup = (note: GetNotesModel): FormGroup =>
    new FormGroup({
      id: new FormControl<string | undefined>(note.id),
      content: new FormControl<string | undefined>(note.content),
      isCompleted: new FormControl<boolean>(note.isComplete),
      position: new FormControl<number>(note.position)
    });

  deleteNote(noteId: string) {
    this.toDoList.notes = this.toDoList.notes!.filter(item => item.id !== noteId);
    this.patchForm()
  }



  onOpenDialog() {

    const dialogRef = this.dialog.open(NoteDialogComponent,
      {
        data: this.toDoList.id,
        width: "300px",
        height: "150px",
        disableClose: true
      }
    );

    dialogRef.afterClosed().subscribe(
      result => {
        this.toDoList.notes?.push(result);
        this.notesControls.push(this.createFormGroup(result));
      }
    )
  }
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
 
}
