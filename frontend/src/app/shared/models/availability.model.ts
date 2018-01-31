export class Availability{
    id?: number;
    startDate: Date;
    endDate?: Date;
    daysOfWeek: number;
    availabilityType: number;
    roomId?: number;
    hostId: number;
    occurrence: number;
}