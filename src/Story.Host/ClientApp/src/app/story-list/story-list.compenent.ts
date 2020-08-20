import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'story-list-component',
  styleUrls: ['./story-list.component.css'],
  templateUrl: './story-list.component.html'
})
export class StoryListComponent {
  public stories: StorySummary[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<StorySummary[]>(baseUrl + 'api/stories').subscribe(result => {
      this.stories = result;
    }, error => console.error(error));
  }
}

interface StorySummary {
  id: string;
  name: string;
  desciption: string;
}
