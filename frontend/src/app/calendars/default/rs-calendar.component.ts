import { Component, ChangeDetectionStrategy, ViewEncapsulation } from '@angular/core';
import { CalendarEvent } from 'angular-calendar';
import { setHours, setMinutes, subDays, addDays } from 'date-fns';
import { RSCalendarEventColors } from './rs-calendar-event-colors';

@Component({
  selector: 'rs-calendar-component',
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
  templateUrl: './rs-calendar.component.html'
})

export class RSCalendarComponent {
  view: string = 'day';

  viewDate: Date = new Date();

  // CODED EVENTS
  events: CalendarEvent[] = [
    {
      title: 'Event1',
      start: new Date(),
      color: RSCalendarEventColors.blue
    },
    {
      title: 'Event9',
      start: setHours(setMinutes(new Date(), 0), 11),
      color: RSCalendarEventColors.blue
    },
    {
      title: 'Event5',
      start: setHours(setMinutes(new Date(), 30), 9),
      color: RSCalendarEventColors.yellow
    }
  ];

  // EXCLUDE WEEKENDS
  excludeDays: number[] = [0, 6];
  skipWeekends(direction: 'back' | 'forward'): void {
    if (this.view === 'day') {
      if (direction === 'back') {
        while (this.excludeDays.indexOf(this.viewDate.getDay()) > -1) {
          this.viewDate = subDays(this.viewDate, 1);
        }
      } else if (direction === 'forward') {
        while (this.excludeDays.indexOf(this.viewDate.getDay()) > -1) {
          this.viewDate = addDays(this.viewDate, 1);
        }
      }
    }
  }
}