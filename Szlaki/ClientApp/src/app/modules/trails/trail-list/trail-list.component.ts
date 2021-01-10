import { Router } from '@angular/router';
import { element } from 'protractor';
import { TrailSerivce } from './../services/trail-service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Trail } from '../models/trail';
import {MatSnackBar} from '@angular/material/snack-bar';

export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
}

@Component({
  selector: 'app-trail-list',
  templateUrl: './trail-list.component.html',
  styleUrls: ['./trail-list.component.css']
})

export class TrailListComponent implements OnInit {
  displayedColumns: string[] = ['title', 'description', 'startDate', 'endDate', 'operation'];
  trails: Trail[] = [];
  dataSource: MatTableDataSource<Trail>;
  constructor(private trailService: TrailSerivce, private router: Router, private _snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.trailService.getTrails().subscribe(response => {
      this.trails = response;
      this.dataSource = new MatTableDataSource(this.trails);
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  getRecord(element: Trail) {
    localStorage.setItem('trailKey', element.id);
    this.router.navigate(['/trail-view']);
  }

  deleteRow(element: Trail) {
    this.trailService.deleteTrail(element.id).subscribe(response => {
      this._snackBar.open('Trip was delete successfully',null,{
        duration: 3000
      });
      this.trailService.getTrails().subscribe(response => {
        this.trails = response;
        this.dataSource = new MatTableDataSource(this.trails);
      });
    })
  }
}
