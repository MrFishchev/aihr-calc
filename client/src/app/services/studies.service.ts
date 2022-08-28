import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Course } from './courses.service';

export interface Study {
  id: string | undefined,
  courses: Course[],
  startDate: Date,
  endDate: Date,
  hoursPerWeek: number | undefined
}

export interface EstimatedStudyTime {
  weeks: number,
  hoursPerDay: number,
  hoursPerWeek: number
}

@Injectable()
export class StudiesService {
  private studiesApi = '/api/studies'

  constructor(private http: HttpClient) { }

  getStuies(): Observable<Study[]> {
    return this.http.get<Study[]>(this.studiesApi)
      .pipe(catchError(this.errorHandler));
  }

  estimateStudyTime(study: Study): Observable<EstimatedStudyTime> {
    const headers = { 'content-type': 'application/json' }
    const body = JSON.stringify(study);
    return this.http.post<EstimatedStudyTime>(this.studiesApi, body, { headers: headers })
      .pipe(catchError(this.errorHandler));
  }

  private errorHandler(error: HttpErrorResponse) {
    return throwError(() => new Error('server error'));
  }
}
