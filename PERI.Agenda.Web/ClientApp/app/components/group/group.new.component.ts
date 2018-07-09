import { Component, Input, Inject, OnChanges, Output, EventEmitter } from '@angular/core';
import * as $ from "jquery";

import { NgForm } from '@angular/forms';
import { GroupModule, Group } from './group.component';
import { MemberModule } from '../member/member.component';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Http } from '@angular/http';
import { GroupCategory, GroupCategoryModule } from '../groupcategory/groupcategory.component';

@Component({
    selector: 'groupnew',
    templateUrl: './group.new.component.html'
})
export class GroupNewComponent {
    private gm: GroupModule;
    private mm: MemberModule;
    private ex: ErrorExceptionModule;

    public group: Group;
    public groupCategories: GroupCategory[];

    @Output() change: EventEmitter<number> = new EventEmitter<number>();

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) {
        this.mm = new MemberModule();
        this.mm.http = http;
        this.mm.baseUrl = baseUrl;

        this.gm = new GroupModule();
        this.gm.http = http;
        this.gm.baseUrl = baseUrl;

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

    suggestionSelect(s: string, f: NgForm, fieldName: string) {
        this.suggestions.length = 0;

        f.controls[fieldName].setValue(s);
    }

    ngAfterViewInit() {
        var gc = new GroupCategoryModule();
        gc.http = this.http;
        gc.baseUrl = this.baseUrl;
        gc.find(new GroupCategory()).subscribe(result => { this.groupCategories = result });

        this.mm.allNames().subscribe(result => { this.names = result });
    }

    onNewSubmit(f: NgForm) {
        var g = new Group();
        g.groupCategoryId = f.controls['groupCategoryId'].value;
        g.leader = f.controls['leader'].value;
        g.name = f.controls['name'].value;

        this.gm.add(g).subscribe(result => {
            g.id = result;
            this.change.emit(g.id);

            alert('Added!');
            $('#modalNew').modal('toggle');
        }, err => this.ex.catchError(err));
    }
}