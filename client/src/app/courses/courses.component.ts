import { Component, OnInit } from '@angular/core';
import { CoursesService, Course } from '../services/courses.service';
import { StudiesService, Study, EstimatedStudyTime } from '../services/studies.service';
import { FormGroup, FormControl } from '@angular/forms';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.sass'],
  providers: [CoursesService, StudiesService, DatePipe]
})

export class CoursesComponent implements OnInit {
  range = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
  });
  courses: Course[] | undefined;
  studies: Study[] | undefined;
  estimatedStudyTime: EstimatedStudyTime | undefined;

  constructor(
    private coursesService: CoursesService,
    private studiesService: StudiesService,
    public datepipe: DatePipe
  ) { }

  ngOnInit(): void {
    this.coursesService.getCourses()
      .subscribe(result => this.courses = result);
    this.studiesService.getStuies()
      .subscribe(result => this.studies = result);
  }

  get selectedCourses(): Course[] | undefined {
    return this.courses?.filter(x => x.selected);
  }

  get selectedCoursesTotalDuration(): number | undefined {
    return this.selectedCourses?.map(x => x.duration).reduce((total, duration) => total + duration, 0);
  }

  selectCourse(course: Course): void {
    course.selected = !course.selected;
  }

  calculateStudyTime(): void {
    const courses = this.selectedCourses;
    const startDate = this.range.value.start;
    const endDate = this.range.value.end;

    if (!startDate || !endDate || courses?.length === 0) {
      return;
    }

    const study = {
      courses: courses!,
      startDate: startDate!,
      endDate: endDate!,
    }

    this.studiesService.estimateStudyTime(study as Study)
      .subscribe(result => this.estimatedStudyTime = result);
  }

  clear(): void {
    this.courses?.forEach(x => x.selected = false);
    this.estimatedStudyTime = undefined;
    this.range.reset();
  }

  getDate(date: Date): string {
    return this.datepipe.transform(date, 'dd-MM-yyyy') ?? '';
  }
}
