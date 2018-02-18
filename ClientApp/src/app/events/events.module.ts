import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Route, RouterModule } from '@angular/router';

import { EventListComponent } from './event-list/event-list.component';
import { EventCreateComponent } from './event-create/event-create.component';
import { EventEditComponent } from './event-edit/event-edit.component';
import { EventDetailsComponent } from './event-details/event-details.component';

import { EventsService } from './events.service';

const eventRoutes: Route[] = [
  {
    path: 'events', children: [
      { path: '', component: EventListComponent },
      { path: 'create', component: EventCreateComponent },
      { path: ':eventid', component: EventDetailsComponent },
      { path: ':eventid/edit', component: EventEditComponent}
    ]
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(eventRoutes)
  ],
  exports: [
    RouterModule
  ],
  declarations: [
    EventListComponent,
    EventCreateComponent,
    EventEditComponent,
    EventDetailsComponent
  ],
  providers: [
    EventsService
  ]
})
export class EventsModule { }