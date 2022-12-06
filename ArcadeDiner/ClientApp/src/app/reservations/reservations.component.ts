import { Component } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http"
import { reservationInformation } from '../models/reservationInfomation';
import { Time } from '@angular/common';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
})

export class ReservationsComponent {
  visibleReservationCreation: boolean = false;
  visibleReservationUpdate: boolean = false;

  result: boolean = false;
  reservationInformation: reservationInformation[] = [];
  public partyNameVar: string = "";
  public numberInPartyVar: number = 1;
  public reservationDateVar: Date = new Date();
  public reservationTimeVar: Date = new Date();
  public reservationNumberVar: number = 0;
  public emailVar: string = "";
  public isFulfilledVar: boolean = false;
  public reservationIdVar: number = 0;


  viewReservations() {
    this.http.get<reservationInformation[]>('https://localhost:44391/api/ReservationInfo').subscribe(result => {
      this.reservationInformation = result;
    }, error => console.error(error));
  }

  constructor(private http: HttpClient) {
    this.viewReservations();
  }

  onSubmit(resInfo: reservationInformation) {
    this.http.post('https://localhost:44391/api/ReservationInfo', resInfo).subscribe(result => {
      console.log(result);
      this.visibleReservationCreation = false;
      this.viewReservations();
    }, error => console.error(error));
  }

  onUpdate(resInfo: reservationInformation) {
    this.http.put('https://localhost:44391/api/ReservationInfo', resInfo).subscribe(result => {
      console.log(result);
      this.visibleReservationUpdate = false;
      this.viewReservations();
    }, error => console.error(error));
  }

  onCancel() {
    this.visibleReservationCreation = false;
    this.visibleReservationUpdate = false;
    this.viewReservations();
  }

  createReservation() {
    this.visibleReservationCreation = true;
  }

  updateReservation(reservationId: number) {
    this.visibleReservationUpdate = true;
    const params = new HttpParams().set('reservationId', reservationId);
    this.http.get<reservationInformation[]>('https://localhost:44391/api/ReservationInfo/update', { params }).subscribe(result => {
      console.log(result);
      this.reservationInformation = result;
      this.partyNameVar = this.reservationInformation[0].partyName;
      this.numberInPartyVar = this.reservationInformation[0].numberInParty;
      this.reservationDateVar = this.reservationInformation[0].reservationDate;
      this.reservationTimeVar = this.reservationInformation[0].reservationTime;
      this.reservationNumberVar = this.reservationInformation[0].reservationNumber;
      this.emailVar = this.reservationInformation[0].email;
      this.isFulfilledVar = this.reservationInformation[0].isFulfilled;
      this.reservationIdVar = this.reservationInformation[0].reservationId;
    }, error => console.error(error));
  }

  deleteReservation(reservationId: number) {
    const params = new HttpParams().set('reservationId', reservationId);
    this.http.delete('https://localhost:44391/api/ReservationInfo', { params }).subscribe(result => {
      console.log(result);
      this.viewReservations();
    }, error => console.error(error));
  }

 
}


