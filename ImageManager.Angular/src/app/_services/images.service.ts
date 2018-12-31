import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Image } from '@/_models';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ImagesService {
    constructor(private http: HttpClient) { }

    getAll() {
        return this.http.get<Image[]>(`${config.apiUrl}/images/getAll`);
    }

    public upload(image: File): Observable<any> {
        const formData = new FormData();
        formData.append('file', image);
        return this.http.post(`${config.apiUrl}/images/upload`, formData);
    }

    delete(image: Image) {
        return this.http.delete(`${config.apiUrl}/images/delete/` + image.name);
    }

    deleteAll() {
        return this.http.delete(`${config.apiUrl}/clearImages/deleteAll`);
    }
}