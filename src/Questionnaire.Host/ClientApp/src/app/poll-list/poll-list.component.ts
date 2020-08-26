import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'poll-list-component',
  templateUrl: './poll-list.component.html'
})
export class PollListComponent {
  public polls: PollSummary[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<PollSummary[]>(baseUrl + 'api/polls').subscribe(result => {
      this.polls = result;
    }, error => console.error(error));
  }
}

interface PollSummary {
  id: string;
  name: string;
  desciption: string;
}
