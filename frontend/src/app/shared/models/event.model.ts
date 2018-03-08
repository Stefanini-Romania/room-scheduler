export class Event{
    startDate: Date;
    endDate: Date;
    id: number;
    roomId: number;
    hostId?: number;
    eventType: number;
    attendeeId: number;
    eventStatus: number;
    notes?: string;
    availabilityType: number;
    dateCreated: Date;
    hostName: string;
    attendeeName: string;
}

export enum EventStatusEnum {
    present = 0,
    absent,
    cancelled,
    waiting,
    absentChecked,
    waitingReminder
}

export enum EventTypeEnum {
    massage = 0,
    availability
}
