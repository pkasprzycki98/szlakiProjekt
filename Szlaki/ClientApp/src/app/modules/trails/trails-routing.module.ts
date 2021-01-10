import { TrailViewComponent } from './trail-view/trail-view.component';
import { TrailEditComponent } from './trail-edit/trail-edit.component';
import { TrailAddComponent } from './trail-add/trail-add.component';
import { TrailListComponent } from './trail-list/trail-list.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [ 
  {
    path: "",
    component: TrailListComponent,  
  },
  {
    path: 'trail-add',
    component: TrailAddComponent
  },
  {
    path: 'trail-edit',
    component: TrailEditComponent
  },
  {
    path: 'trail-view',
    component: TrailViewComponent
  },
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TrailsRoutingModule {}