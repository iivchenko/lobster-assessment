import { Component, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'story-component',
  templateUrl: './story.component.html'
})
export class StoryComponent {
  public story: Story;
  public question: Question;
  public end: End;
  public finish: boolean;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
    this.finish = false;
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');

    this.http.get<Story>(this.baseUrl + 'api/stories/' + id).subscribe(result => {
      this.story = result;

      this.http.get<Question>(this.baseUrl + 'api/stories/' + id + '/questions/' + this.story.rootQuestionId).subscribe(result => {
        this.question = result;
      }, error => console.error(error));

    }, error => console.error(error));
  }

  giveAnswer(id: string) {
    this.http.get<Answer>(this.baseUrl + 'api/stories/' + this.story.id + '/answers/' + id).subscribe(result => {

      if (result.nextEntityType === 0) {
        this.http.get<Question>(this.baseUrl + 'api/stories/' + this.story.id + '/questions/' + result.nextEntityId).subscribe(result => {
          this.question = result;
        }, error => console.error(error));
      }
      else {
        this.http.get<End>(this.baseUrl + 'api/stories/' + this.story.id + '/end/' + result.nextEntityId).subscribe(result => {
          this.end = result;
        }, error => console.error(error));
        this.finish = true;
      }   
    }, error => console.error(error));
  }
}

interface Story {
  id: string;
  name: string;
  desciption: string;
  rootQuestionId: string;
}

interface Question {
  id: string;
  text: string;
  answers: Answer[]
}

interface Answer {
  id: string;
  text: string;
  nextEntityType: number;
  nextEntityId: string;
}

interface End {
  id: string;
  message: string;
}
