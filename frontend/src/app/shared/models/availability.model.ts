export class Availability{
    id?: number;
    startDate: Date;
    endDate?: Date;
    daysOfWeek: any;
    availabilityType: number;
    roomId?: number;
    roomName?: string;
    hostId: number;
    occurrence: number;
    status: number;
}