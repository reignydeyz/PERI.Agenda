import { Activity } from "./activity";

export class Member {
    id: number;
    name: string;
    nickName: string;
    birthDate: any;
    gender: number;
    email: string;
    address: string;
    mobile: string;
    isActive?: boolean;
    roleId: number;
    leading: number;
    following: number;
    invites: number;
    activities: Activity[];
    invitedBy: number;
    invitedByMemberName: string;
    remarks: string;
}