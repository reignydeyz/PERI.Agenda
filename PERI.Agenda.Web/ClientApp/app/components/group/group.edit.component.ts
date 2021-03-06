﻿import { Component, Input, Inject, OnChanges, Output, EventEmitter } from '@angular/core';
import * as $ from "jquery";

import { NgForm } from '@angular/forms';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Http } from '@angular/http';
import { MemberService } from '../../services/member.service';
import { Group } from '../../models/group';
import { GroupService } from '../../services/group.service';
import { GroupCategory } from '../../models/groupcategory';
import { GroupCategoryService } from '../../services/groupcategory.service';

@Component({
    selector: 'groupedit',
    templateUrl: './group.edit.component.html'
})
export class GroupEditComponent {
    private ex: ErrorExceptionModule;
    
    public groupCategories: GroupCategory[];

    @Input('group') group: Group;
    @Input('isAdmin') isAdmin: boolean;
    @Output() change: EventEmitter<number> = new EventEmitter<number>();

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string,
    private mm: MemberService, private gm: GroupService, private gcm: GroupCategoryService) {
        this.ex = new ErrorExceptionModule();
        this.ex.http = http;
        this.ex.baseUrl = baseUrl;
    }

    public names: string[];
    suggestions: string[] = [];

    suggest(fc: any) {
        if (fc.value.length > 0) {
            this.suggestions = this.names
                .filter(c => c.startsWith(fc.value.toUpperCase()))
                .slice(0, 5);
        }
        else {
            this.suggestions.length = 0;
        }
    }

    suggestionSelect(s: string) {
        this.suggestions.length = 0;

        this.group.leader = s;
    }

    async ngAfterViewInit() {
        this.groupCategories = await this.gcm.find(new GroupCategory());
        this.names = await this.mm.allNames();
    }

    onEditSubmit(g: Group) {
        this.gm.edit(g).then(() => {
            this.change.emit(g.id);

            alert('Success!');
            $('#modalEdit').modal('toggle');
        });
    }
}