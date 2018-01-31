export class Availability{
    availabilityId: number;
    startDate: Date;
    endDate: Date;
    dayOfWeek: number;
    availabilityType: number;
    roomId?: number;
    hostId: number;
    occurrence: number;
}