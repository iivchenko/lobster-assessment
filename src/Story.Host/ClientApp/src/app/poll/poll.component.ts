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
  public end: End = { id: null, message: null };
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

          this.pushHistory(this.end.id, this.end.message, 4);

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
    this.http.get<FullPoll>(this.baseUrl + 'api/polls/' + this.poll.id + '/full').subscribe(result => {
      this.handleQuestion(result.root);
      this.treeIsBuild = true;
      this.center$.next(true);
      this.zoomToFit$.next(true);
      this.update$.next(true);
      
    }, error => console.error(error));
  }

  handleQuestion(question: FullQuestion) {
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
    }

    for (var answer of question.answers) {
      this.handleAnswer(answer);
    }
  }

  handleAnswer(answer: FullAnswer) {
    const node: Node = {
      id: answer.id,
      label: answer.text,
      data: {
        selected: this.history.some(x => x.id === answer.id),
        type: 'answer'
      }
    };

    this.nodes.push(node);

    switch (answer.nextType) {
      case 0:
        const question: FullQuestion = answer.next;
        const link1: Edge = {
          source: answer.id,
          target: question.id,
        };
        this.links.push(link1);

        this.handleQuestion(question);

        break;
      case 1:
        const end: FullEnd = answer.next;
        const link2: Edge = {
          source: answer.id,
          target: end.id,
        };
        this.links.push(link2);

        const node: Node = {
          id: end.id,
          label: end.message,
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
  message: string;
}

interface HistoryItem {
  id: string;
  text: string;
  type: number;
}

interface FullEnd {
  id: string;
  message: string;
}

interface FullAnswer {
  id: string;
  text: string;
  nextType: number;
  next: any;
}

interface FullQuestion {
  id: string;
  text: string;
  answers: FullAnswer[]
}

interface FullPoll {
  id: string;
  name: string;
  root: FullQuestion;
}
