import {Room} from './room.model';
import {RoleEnum} from './role.model';

export class User {
    id?: number;
    name: string;
    firstName?: string;
    lastName?: string;
    password: string; 
    email: string;
    penalty?: number [];
    departmentId?: number;
    confirmPassword?: number;
    userRole?: RoleEnum[];
    isActive?: boolean; 
      

    hasPenalties(): boolean {
        return this.penalty && this.penalty.length > 0;
    }

    hasPenaltiesForRoom(mixedRoom: Room|number): boolean {
        const roomId = mixedRoom instanceof Room ? mixedRoom.id : mixedRoom;
        return this.hasPenalties() && this.penalty.indexOf(roomId) !== -1;
    }

    hasRole(role: RoleEnum): boolean {
        return this.userRole && this.userRole.length && this.userRole.indexOf(role) !== -1;
    }
}
