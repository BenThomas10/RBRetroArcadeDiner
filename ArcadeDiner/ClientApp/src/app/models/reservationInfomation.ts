import { Time } from "@angular/common";

export interface reservationInformation {
    reservationId : number
    partyName : string,
    numberInParty : number,
    reservationDate : Date,
    reservationTime : Date,
    reservationNumber : number
    submissionDateTime : Date
    email : string
    isFulfilled : boolean,
    lastUpdateDate : Date
}
