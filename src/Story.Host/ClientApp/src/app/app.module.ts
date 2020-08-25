import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgxGraphModule } from '@swimlane/ngx-graph'

import { AppComponent } from './app.component';
import { StoryListComponent } from './story-list/story-list.compenent'
import { StoryComponent } from './story/story.compenent'

@NgModule({
  declarations: [
    AppComponent,
    StoryListComponent,
    StoryComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: StoryListComponent, pathMatch: 'full' },
      { path: 'story/:id', component: StoryComponent },
    ]),
    BrowserAnimationsModule,
    NgxGraphModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
