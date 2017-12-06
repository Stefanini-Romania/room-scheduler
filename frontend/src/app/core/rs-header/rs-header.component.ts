import {Component} from '@angular/core';
import {User} from '../../shared/models/user.model';
import {AuthService} from '../../auth/shared/auth.service';
import {Router} from '@angular/router';
import {RoomService} from '../../rooms/shared/room.service';
import {Room} from '../../shared/models/room.model';

@Component({
    selector: 'rs-header',
    templateUrl: './rs-header.component.html',
})

export class RsHeaderComponent {
    currentUser: User = undefined;
    userIsPenalizedInRoom = false;

    languages = [
        {'name': 'English', 'code': 'en', 'icon': 'https://cdn2.iconfinder.com/data/icons/flags_gosquared/64/United-Kingdom_flat.png'},
        {'name': 'Română', 'code': 'ro', 'icon': 'https://cdn2.iconfinder.com/data/icons/flags_gosquared/64/Romania_flat.png'}
    ];

    constructor(private authService: AuthService, private router: Router, private roomService: RoomService) {
        // observe room changing
        roomService.selectedRoomChanged$.subscribe((room: Room) => {
            this.userIsPenalizedInRoom = this.currentUser && this.currentUser.hasPenaltiesForRoom(room);
        });

        // observe authentication status
        authService.user$.subscribe((user: User) => {
            this.currentUser = user;
        });
    }

    logout() {
        this.authService.logout();
        location.reload(true);
    }

    redirectToLogin() {
        return this.router.navigate(['/login']);
    }
}
