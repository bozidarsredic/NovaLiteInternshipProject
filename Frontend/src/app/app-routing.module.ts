import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RouteGuard } from './core/route.guard';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ToDoListShareComponent } from './to-do-list-share/to-do-list-share.component';
import { ToDoListComponent } from './to-do-list/to-do-list.component';

const routes: Routes = [
  { path: '', component: DashboardComponent, canActivate: [RouteGuard] },
  { path: 'to-do-list/:id', component: ToDoListComponent, canActivate: [RouteGuard] },
  { path: 'create', component: ToDoListComponent, canActivate: [RouteGuard] },
  { path: 'share/:id', component: ToDoListShareComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
