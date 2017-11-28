export class User {
    name: string;
    firstName?: string;
    lastName?: string;
    password: string;
    confirmPassword: string;
    email: string;
    id?: number;
    penalty?: number [];  
    departmentId?: number;
    roleId?: number;
}
