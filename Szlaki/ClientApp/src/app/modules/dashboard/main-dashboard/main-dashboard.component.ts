import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-main-dashboard',
  templateUrl: './main-dashboard.component.html',
  styleUrls: ['./main-dashboard.component.css']
})
export class MainDashboardComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  modules: any = [
    {
      name: 'Trails',
      id: 'trails',
      icon: 'book_online',
      description: 'Module whehre you can display list of your trips',
      routing: '/trail',
      roleRequired: 'any'
    },
    {
      name: 'Add new Trail',
      id: 'addtrail',
      icon: 'arrow_right_alt',
      description: 'Module where you can add your new trip',
      routing: '/trail/trail-add',
      roleRequired: 'any'
    }
  ]
}
