import { Photos } from './photoModel';
import { from } from "rxjs";

export class Trail {
    id: string;
    title: string;
    description: string;
    startDate: Date;
    endDate: Date;
    photos: any[];
    videos: any[];
    user: any;
    userId: any;
}