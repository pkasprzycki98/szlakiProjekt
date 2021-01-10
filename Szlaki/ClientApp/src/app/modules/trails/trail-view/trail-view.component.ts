import { Trail } from './../models/trail';
import { TrailSerivce } from './../services/trail-service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-trail-view',
  templateUrl: './trail-view.component.html',
  styleUrls: ['./trail-view.component.css']
})
export class TrailViewComponent implements OnInit {

  trail: Trail
  id: string;

  constructor(private trailService: TrailSerivce, private router: Router) {     
     }

  ngOnInit(): void {
      this.id = localStorage.getItem('trailKey');
      this.trailService.getTrail(this.id).subscribe(response => {
        this.trail = response;
      },error => {
        console.log(error);
      })
  }
  
}
