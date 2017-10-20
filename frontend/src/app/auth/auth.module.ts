import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent} from './login/login.component';
import { FormsModule } from '@angular/forms';

const routes: Routes = [
    { path: 'login', component: LoginComponent }
];

@NgModule({
  imports: [
      FormsModule,
      RouterModule.forRoot(routes),
  ],
  providers: [],
  declarations: [LoginComponent],
  exports: [LoginComponent],
})
export class AuthModule {}