import {Room} from './room.model';
import {RoleEnum} from './role.model';

export class User {
    id?: number;
    firstName?: string;
    lastName?: string;
    password: string; 
    email: string;
    penalty?: number [];
    departmentId?: number;
    confirmPassword?: number;
    userRole?: RoleEnum[];
    isActive?: boolean; 
    resetPassCode?: number;
    dateExpire?: Date;
      

    hasPenalties(): boolean {
        return this.penalty && this.penalty.length > 0;
    }

    hasPenaltiesForRoom(mixedRoom: Room): boolean {
        let roomId;
        if(mixedRoom){
            roomId = mixedRoom.id
        }
        return this.hasPenalties() && this.penalty.indexOf(roomId) !== -1;
    }

    hasRole(role: RoleEnum): boolean {
        return this.userRole && this.userRole.length && this.userRole.indexOf(role) !== -1;
    }
}
