import { Http } from '@angular/http';

export class ErrorExceptionModule {
    public http: Http;
    public baseUrl: string;

    public catchError(error: any) {
        console.error(error);

        if (error.status >= 400 && error.status < 500) {
            window.location.replace(this.baseUrl + 'authentication/signout');
        }
        else {
            alert('Oops! Unknown error has occured.');
        }
    }
}