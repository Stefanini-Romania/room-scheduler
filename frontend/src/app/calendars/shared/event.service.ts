import {Injectable} from '@angular/core';
import {Response} from '@angular/http';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import 'rxjs/Rx';
import {Observable} from 'rxjs/Observable';
import {Event} from "../../shared/models/event.model";

@Injectable()
export class EventService {

    constructor(private http: HttpClient) {}

    public listEvents(startDate: Date, endDate: Date, roomId?: number, hostId?: number) {
        const url = 'http://fctestweb1:88/event/list';
        let params = new HttpParams();
        params = params.append("startDate", startDate.toUTCString());
        if (endDate) {
            params = params.append("endDate", endDate.toUTCString());
        }
        if (roomId > 0) {
            params = params.append("roomId", roomId.toString());
        }
        if (hostId > 0) {
            params = params.append("hostId", hostId.toString());
        }

        const body = JSON.stringify(params);
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.get(url, {params: params});

    }

    private createEvent(event: Event) {
        const url = 'http://fctestweb1:88/event/create';
        const body = JSON.stringify({
            startDate: event.startDate,
            endDate: event.endDate,
            eventType: event.eventType,
            roomId: event.roomId,
            hostId: event.hostId,
            attendeeId: event.attendeeId,
            eventStatus: event.eventStatus,
            notes: event.notes
        });
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(url, body, {headers: headers})
                        .catch((error: any) => Observable.throw(error))
                        .map((response: Response) => {

                return response;
            });

    }

    private editEvent(event: Event) {
        const url = 'http://fctestweb1:88/event/create';
        const body = JSON.stringify({
            startDate: event.startDate,
            endDate: event.endDate,
            eventId: event.id,
            eventType: event.eventType,
            roomId: event.roomId,
            hostId: event.hostId,
            attendeeId: event.attendeeId,
            eventStatus: event.eventStatus,
            notes: event.notes
        });

        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(url, body, {headers: headers})
            .catch((error: any) => Observable.throw(error.message))
            .map((response: Response) => {

                return response;
            });
    }

    public save(event: Event) {
        return event.id ? this.editEvent(event) : this.createEvent(event);
    }
}
