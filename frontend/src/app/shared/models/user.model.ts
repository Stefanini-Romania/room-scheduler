import {Room} from './room.model';
import {RoleEnum} from './role.model';

export class User {
    id?: number;
    name: string;
    firstName?: string;
    lastName?: string;
    password: string;
    confirmPassword: string;    email: string;
    penalty?: number [];
    departmentId?: number;
    userRoles?: RoleEnum[];

    hasPenalties(): boolean {
        return this.penalty && this.penalty.length > 0;
    }

    hasPenaltiesForRoom(mixedRoom: Room|number): boolean {
        const roomId = mixedRoom instanceof Room ? mixedRoom.id : mixedRoom;

        return this.hasPenalties() && this.penalty.indexOf(roomId) !== -1;
    }

    hasRole(role: RoleEnum): boolean {
        return this.userRoles && this.userRoles.length && this.userRoles.indexOf(role) !== -1;
    }
}
