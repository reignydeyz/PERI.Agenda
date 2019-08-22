import { Component, Input, Inject, OnChanges, Output, EventEmitter } from '@angular/core';
import * as $ from "jquery";

import { NgForm } from '@angular/forms';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Http } from '@angular/http';
import { GroupCategoryModule } from '../groupcategory/groupcategory.component';
import { MemberService } from '../../services/member.service';
import { Group } from '../../models/group';
import { GroupService } from '../../services/group.service';
import { GroupCategory } from '../../models/groupcategory';

@Component({
    selector: 'groupnew',
    templateUrl: './group.new.component.html'
})
export class GroupNewComponent {
    private ex: ErrorExceptionModule;
    
    public groupCategories: GroupCategory[];

    @Input('group') group: Group;
    @Input('isAdmin') isAdmin: boolean;
    @Output() change: EventEmitter<number> = new EventEmitter<number>();

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string,
    private mm: MemberService, private gm: GroupService) {
        this.ex = new ErrorExceptionModule();
        this.ex.http = http;
        this.ex.baseUrl = baseUrl;

        if (this.group == null || this.group == undefined) {
            this.group = new Group();
        }
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
        var gc = new GroupCategoryModule();
        gc.http = this.http;
        gc.baseUrl = this.baseUrl;
        gc.find(new GroupCategory()).subscribe(result => { this.groupCategories = result });

        this.names = await this.mm.allNames();
    }

    async onNewSubmit(g: Group) {
        await this.gm.add(g).then(r => {
            g.id = r;
            this.change.emit(g.id);

            alert('Added!');
            $('#modalNew').modal('toggle');
        });
    }
}