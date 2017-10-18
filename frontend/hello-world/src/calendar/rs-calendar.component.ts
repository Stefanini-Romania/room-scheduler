import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CalendarEvent } from 'angular-calendar';
import { setHours, setMinutes } from 'date-fns';
import { RSCalendarEventColors } from './rs-calendar-event-colors';

@Component({
  selector: 'mwl-demo-component',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: 'rs-calendar.component.html'
})
export class RSCalendarComponent {
  view: string = 'day';

  viewDate: Date = new Date();

  events: CalendarEvent[] = [
    {
      title: 'No event end date',
      start: setHours(setMinutes(new Date(), 0), 3),
      color: RSCalendarEventColors.blue
    },
    {
      title: 'No event end date',
      start: setHours(setMinutes(new Date(), 0), 5),
      color: RSCalendarEventColors.yellow
    }
  ];
}