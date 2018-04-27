import { Http } from '@angular/http';

export class ErrorExceptionModule {
    public http: Http;
    public baseUrl: string;

    public catchError(error: any) {
        console.error(error);

        switch (error.status) {
            case 403:
                var msg = error._body.replace(/"/g, '');
                alert(msg.replace(/\\n/g, "\n"));
                break;
            case 400:
            case 401:
            case 412:
            case 417:
                window.location.replace(this.baseUrl + 'authentication/signout');
                break;
            default:
                alert('Oops! Unknown error has occured.');
                break;
        }
    }
}