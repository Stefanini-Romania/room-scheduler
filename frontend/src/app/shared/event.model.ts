export class Event{
    startDate: Date;
    endDate: Date;
    roomId: number;
    hostId?: number;
    eventType: number;
    attendeeId: number;
    eventStatus: number;
    notes?: string;
}