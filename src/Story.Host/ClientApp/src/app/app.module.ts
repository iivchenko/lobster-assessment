import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgxGraphModule } from '@swimlane/ngx-graph'

import { AppComponent } from './app.component';
import { PollListComponent } from './poll-list/poll-list.component'
import { PollComponent } from './poll/poll.component'

@NgModule({
  declarations: [
    AppComponent,
    PollListComponent,
    PollComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: PollListComponent, pathMatch: 'full' },
      { path: 'poll/:id', component: PollComponent }
    ]),
    BrowserAnimationsModule,
    NgxGraphModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
