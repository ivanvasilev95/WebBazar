import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {
  baseUrl = environment.apiUrl + 'photos/';

  constructor(private http: HttpClient) { }

  setMainPhoto(id: number) {
    return this.http.put(this.baseUrl + 'setMain/' + id, {});
  }

  deletePhoto(id: number) {
    return this.http.delete(this.baseUrl + id);
  }
}
