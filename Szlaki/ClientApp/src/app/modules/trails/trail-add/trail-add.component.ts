import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Trail } from './../models/trail';
import { Router } from '@angular/router';
import { TrailSerivce } from './../services/trail-service';
import { Component, OnInit } from '@angular/core';
import {FileModel} from './../models/file';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-trail-add',
  templateUrl: './trail-add.component.html',
  styleUrls: ['./trail-add.component.css']
})
export class TrailAddComponent implements OnInit {

  trailForm: FormGroup;
  trail: Trail;
  addedTrailId: string;

  constructor(private trailService: TrailSerivce, private router: Router, private _snackBar: MatSnackBar) {
    this.trail = new Trail();
  }

  ngOnInit(): void {
    this.trailForm = new FormGroup({
      title: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      startDate: new FormControl('', Validators.required),
      endDate: new FormControl('', Validators.required),
      trailPhotos: new FormControl(''),
      trailVideo: new FormControl('')
    })
  }

  public onFormSubmit() {
    this.trail.title = this.trailForm.value.title;
    this.trail.description = this.trailForm.value.description;
    this.trail.startDate = this.trailForm.value.startDate;
    this.trail.endDate = this.trailForm.value.endDate;

    this.trailService.postTrail(this.trail).subscribe(response => {
      this.addedTrailId = response.data.id;
      let fileModel: FileModel =  {
        TrailId:  this.addedTrailId,
        TrailPhoto: this.trailForm.value.trailVideo
      };
      
    if(this.trailForm.value.trailPhotos != '') {
      this.trailForm.value.trailPhotos.forEach(element => {
        let model: FileModel =  {
          TrailId:  this.addedTrailId,
          TrailPhoto: element
        };
        this.trailService.postPhoto(this.toFormData(model)).subscribe();
      });
    }
   
    if(this.trailForm.value.trailVideo != '') {
      this.trailService.postVideo(this.toFormData(fileModel)).subscribe(response => {
        this._snackBar.open('Trip was added successfully',null,{
          duration: 3000
        });
      });
      this.router.navigate(['/dashboard']);
    }
    }, error => {

    });
  }

  toFormData<T>(formValue: T) {
    const formData = new FormData();
    for (const key of Object.keys(formValue)) {
      const value = formValue[key];
      formData.append(key, value);
    }
    return formData;
  }
}
