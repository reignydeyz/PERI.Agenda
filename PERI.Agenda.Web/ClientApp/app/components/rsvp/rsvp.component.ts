﻿import { Http } from "@angular/http";
import { ErrorExceptionModule } from "../errorexception/errorexception.component";
import { Rsvp } from "../../models/rsvp";

export class RsvpModule {
    public http: Http;
    public baseUrl: string;

    public ex: ErrorExceptionModule;

    public add(r: Rsvp) {
        return this.http.put(this.baseUrl + 'api/rsvp/add', {
            memberId: r.memberId,
            eventId: r.eventId,
            isGoing: r.isGoing
        });
    }

    public delete(r: Rsvp) {
        return this.http.post(this.baseUrl + 'api/rsvp/delete', {
            memberId: r.memberId,
            eventId: r.eventId
        });
    }
}