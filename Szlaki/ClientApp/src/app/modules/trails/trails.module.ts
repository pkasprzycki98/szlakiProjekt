import { TrailSerivce } from './services/trail-service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrailListComponent } from './trail-list/trail-list.component';
import { TrailAddComponent } from './trail-add/trail-add.component';
import { TrailEditComponent } from './trail-edit/trail-edit.component';
import { TrailViewComponent } from './trail-view/trail-view.component';
import { MatTableModule } from '@angular/material/table';
import {MatInputModule} from '@angular/material/input';
import {TrailsRoutingModule} from './trails-routing.module';
import {MatGridListModule} from '@angular/material/grid-list';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { NgxMatFileInputModule } from '@angular-material-components/file-input';
import {MatIconModule} from '@angular/material/icon';
import { MatNativeDateModule } from '@angular/material/core';
import {MatButtonModule} from '@angular/material/button';
import {MatSnackBarModule} from '@angular/material/snack-bar';

@NgModule({
  declarations: [TrailListComponent, TrailAddComponent, TrailEditComponent, TrailViewComponent],
  providers: [
    TrailSerivce,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  imports: [
    CommonModule,
    MatTableModule,
    MatInputModule,
    TrailsRoutingModule,
    MatGridListModule,
    ReactiveFormsModule,
    MatCardModule,
    MatDatepickerModule,
    MatNativeDateModule,
    NgxMatFileInputModule,
    MatIconModule,
    MatButtonModule,
    MatSnackBarModule
  ]
})
export class TrailsModule { }
