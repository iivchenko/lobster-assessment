import { Component, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Edge, Node } from '@swimlane/ngx-graph';
import { Subject } from 'rxjs';

@Component({
  selector: 'poll-component',
  styleUrls: ['./poll.component.css'],
  templateUrl: './poll.component.html'
})
export class PollComponent {
  public poll: Poll = { id: null, name: null, description: null, rootQuestionId: null };;
  public question: Question = { id: null, text: null, answers: [] };
  public end: End = { id: null, text: null };
  public finish: boolean;

  public nodes: Node[] = [];
  public links: Edge[] = [];  
  public treeIsBuild: boolean = false;
  public layoutSettings = {
    orientation: 'TB'
  };
  public update$: Subject<boolean> = new Subject();
  public center$: Subject<boolean> = new Subject();
  public zoomToFit$: Subject<boolean> = new Subject();

  public history: HistoryItem[] = [];

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
    this.finish = false;
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');

    this.http.get<Poll>(this.baseUrl + 'api/polls/' + id).subscribe(result => {
      this.poll = result;

      this.pushHistory(this.poll.id, this.poll.name, 1);

      this.http.get<Question>(this.baseUrl + 'api/polls/' + id + '/questions/' + this.poll.rootQuestionId).subscribe(result => {
        this.question = result;

        this.pushHistory(this.question.id, this.question.text, 2);

      }, error => console.error(error));

    }, error => console.error(error));
  }

  giveAnswer(id: string) {
    this.http.get<Answer>(this.baseUrl + 'api/polls/' + this.poll.id + '/answers/' + id).subscribe(result => {

      this.pushHistory(result.id, result.text, 3);

      if (result.nextEntityType === 0) {
        this.http.get<Question>(this.baseUrl + 'api/polls/' + this.poll.id + '/questions/' + result.nextEntityId).subscribe(result => {
          this.question = result;

          this.pushHistory(this.question.id, this.question.text, 2);

        }, error => console.error(error));
      }
      else {
        this.http.get<End>(this.baseUrl + 'api/polls/' + this.poll.id + '/end/' + result.nextEntityId).subscribe(result => {
          this.end = result;

          this.pushHistory(this.end.id, this.end.text, 4);

        }, error => console.error(error));
        this.finish = true;
      }   
    }, error => console.error(error));
  }

  pushHistory(id, text, type) {
    const item: HistoryItem = {
      id: id,
      text: text,
      type: type
    };

    this.history.push(item)
  }

  buildTree() {
    this.buildTreeAsync().then(_ => {
      this.treeIsBuild = true;
      this.center$.next(true);
      this.zoomToFit$.next(true);
      this.update$.next(true);
    });
  }

  async buildTreeAsync() {
    const question = await this.http.get<Question>(this.baseUrl + 'api/polls/' + this.poll.id + '/questions/' + this.poll.rootQuestionId).toPromise();
    await this.handleQuestion(question);
  }

  async handleQuestion(question: Question) {
    const node: Node = {
      id: question.id,
      label: question.text,
      data: {
        selected: this.history.some(x => x.id === question.id),
        type: 'question'
      }
    };

    this.nodes.push(node);

    for (var answer of question.answers) {
      const link: Edge = {
        source: question.id,
        target: answer.id,
      };

      this.links.push(link);

      const fullAnswer = await this.http.get<Answer>(this.baseUrl + 'api/polls/' + this.poll.id + '/answers/' + answer.id).toPromise();

      await this.handleAnswer(fullAnswer);
    }
  }

  async handleAnswer(answer: Answer) {
    const node: Node = {
      id: answer.id,
      label: answer.text,
      data: {
        selected: this.history.some(x => x.id === answer.id),
        type: 'answer'
      }
    };

    this.nodes.push(node);

    switch (answer.nextEntityType) {
      case 0:
        let question = await this.http.get<Question>(this.baseUrl + 'api/polls/' + this.poll.id + '/questions/' + answer.nextEntityId).toPromise();

        const link1: Edge = {
          source: answer.id,
          target: question.id,
        };
        this.links.push(link1);

        await this.handleQuestion(question);
        break;
      case 1:
        const end = await this.http.get<End>(this.baseUrl + 'api/polls/' + this.poll.id + '/end/' + answer.nextEntityId).toPromise();

        const link2: Edge = {
          source: answer.id,
          target: end.id,
        };

        this.links.push(link2);

        const node: Node = {
          id: end.id,
          label: end.text,
          data: {
            selected: this.history.some(x => x.id === end.id),
            type: 'end'
          }
        };

        this.nodes.push(node);
        break;
    }
  }  
}

interface Poll {
  id: string;
  name: string;
  description: string;
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
  text: string;
}

interface HistoryItem {
  id: string;
  text: string;
  type: number;
}
