import {Injectable} from '@angular/core';
import { Response } from '@angular/http';
import { environment } from '../../../environments/environment';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import 'rxjs/Rx';
import {Observable} from 'rxjs/Observable';
import { Event } from "../../shared/models/event.model";

@Injectable()
export class EventService {

    constructor(private http: HttpClient) {}

    public listEvents(startDate: Date, endDate: Date, roomId?: number, hostId?: number) {
        const url = environment.apiUrl + '/event/list';
        let params = new HttpParams();

        let x:Date;

        x = startDate;
        params = params.append("startDate", new Date(Date.UTC(x.getFullYear(), x.getMonth(), x.getDate(), 0, 0, 0)).toJSON());

        if (endDate) {
            x = endDate;
            params = params.append("endDate", new Date(Date.UTC(x.getFullYear(), x.getMonth(), x.getDate(), 23, 59, 59)).toJSON());
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
        const url = environment.apiUrl + '/event/create';
        const body = JSON.stringify({
            startDate: event.startDate.toLocaleString(),
            endDate: event.endDate.toLocaleString(),
            eventType: event.eventType,
            roomId: event.roomId,
            hostId: event.hostId,
            attendeeId: event.attendeeId,
            eventStatus: event.eventStatus,
            notes: event.notes
        });
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

        return this.http.post(url, body, { headers: headers, withCredentials: true })
            .catch((error: any) => {
                return Observable.throw(error);
            })
            .map((response: Response) => {
                return response;
            });

    }

    private editEvent(event: Event) {
        const url = environment.apiUrl + '/event/edit/' + event.id;
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
        return this.http.put(url, body, {headers: headers})
            .catch((error: any) => Observable.throw(error.message))
            .map((response: Response) => {
                return response;
            });
    }

    public save(event: Event) {
        return event.id ? this.editEvent(event) : this.createEvent(event);
    }
}
