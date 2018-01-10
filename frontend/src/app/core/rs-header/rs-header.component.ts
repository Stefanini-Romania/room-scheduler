import {Component, OnInit} from '@angular/core';
import {User} from '../../shared/models/user.model';
import {RoleEnum} from './../../shared/models/role.model';
import {AuthService} from '../../auth/shared/auth.service';
import {Router, NavigationEnd, ActivatedRoute} from '@angular/router';
import {RoomService} from '../../rooms/shared/room.service';
import {Room} from '../../shared/models/room.model';


@Component({
    selector: 'rs-header',
    templateUrl: './rs-header.component.html',
})

export class RsHeaderComponent {
    currentUser: User = undefined;
    userIsPenalizedInRoom = false;
    checkIfloginRoute = false;

    languages = [
        {'name': 'English', 'code': 'en', 'icon': 'https://cdn2.iconfinder.com/data/icons/flags_gosquared/64/United-Kingdom_flat.png'},
        {'name': 'Română', 'code': 'ro', 'icon': 'https://cdn2.iconfinder.com/data/icons/flags_gosquared/64/Romania_flat.png'}
    ];

    constructor(private authService: AuthService,public route: ActivatedRoute, public router: Router, private roomService: RoomService) {
        router.events
        .filter(e => e instanceof NavigationEnd)
        .forEach(e => {

        if(route.root.firstChild.snapshot.data['name']=="login"){
            this.checkIfloginRoute=true;
        }  
        });
        
        // observe room changing
        roomService.selectedRoomChanged$.subscribe((room: Room) => {
            this.userIsPenalizedInRoom = this.currentUser && this.currentUser.hasPenaltiesForRoom(room);
        });

        // observe authentication status
        authService.user$.subscribe((user: User) => {
            this.currentUser = user;
        });

        if (authService.isLoggedIn()){
            this.currentUser = authService.getLoggedUser();
        } 
    }

    isAdmin(currentUser: User): boolean {
        return (currentUser && currentUser.userRole.length != 0  && currentUser.userRole.indexOf(RoleEnum.admin) !== -1);
    }

    logout() {
        this.authService.logout();  
        if(this.route.root.firstChild.snapshot.data['name']=="calendar") {
            window.location.reload();
        } 
        else {
            return this.router.navigate(['/calendar']); 
        }
    }

    redirectToLogin() {
        return this.router.navigate(['/login']);
    }
}
