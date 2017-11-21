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
}

export enum EventStatusEnum {
    present = 0,
    absent,
    cancelled,
    waiting
}

export enum EventTypeEnum {
    massage = 0,
    availability
}
