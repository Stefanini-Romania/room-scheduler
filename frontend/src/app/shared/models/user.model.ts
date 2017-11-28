import {Room} from './room.model';

export class User {
    id?: number;
    name: string;
    firstName?: string;
    lastName?: string;
    password: string;
    confirmPassword: string; // @TODO remove this since is NOT part of the User model and only used in registration
    email: string;
    penalty?: number [];
    departmentId?: number;
    roleId?: number;
    userRole?: number [];

    hasPenaltiesForRoom(mixedRoom: Room|number): boolean {
        const roomId = mixedRoom instanceof Room ? mixedRoom.id : mixedRoom;

        return this.penalty && this.penalty.length > 0 && this.penalty.indexOf(roomId) !== -1;
    }
}
