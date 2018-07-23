import { Component, Input, Inject, OnChanges, Output, EventEmitter } from '@angular/core';
import * as $ from "jquery";

import { NgForm } from '@angular/forms';
import { EventModule, Event } from './event.component';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Http } from '@angular/http';

@Component({
    selector: 'eventedit',
    templateUrl: './event.edit.component.html'
})
export class EventEditComponent {

}