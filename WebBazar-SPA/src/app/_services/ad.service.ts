import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Ad } from '../_models/ad';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AdService {
  baseUrl = environment.apiUrl + 'ads/';

  constructor(private http: HttpClient) { }

  getAds(currentPage: number, itemsPerPage: number, adParams: any = null): Observable<PaginatedResult<Ad[]>> {
    const params = this.addHttpParamsForAds(currentPage, itemsPerPage, adParams);

    return this.http.get<Ad[]>(this.baseUrl, { observe: 'response', params})
    .pipe(
      map(response => {
        const paginatedResult: PaginatedResult<Ad[]> = new PaginatedResult<Ad[]>();

        paginatedResult.result = response.body;

        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }

        return paginatedResult;
      })
    );
  }

  private addHttpParamsForAds(currentPage: number, itemsPerPage: number, adParams: any): HttpParams {
    let params = new HttpParams();

    params = params.append('pageNumber', currentPage.toString());
    params = params.append('pageSize', itemsPerPage.toString());

    if (adParams !== null) {
      if (adParams.searchText !== '') {
        params = params.append('searchText', adParams.searchText);
      }
      params = params.append('categoryId', adParams.categoryId);
      params = params.append('sortCriteria', adParams.sortCriteria);
    }

    return params;
  }

  getAd(adId: number): Observable<Ad> {
    return this.http.get<Ad>(this.baseUrl + adId);
  }

  getUserAds(): Observable<Ad[]> {
    return this.http.get<Ad[]>(this.baseUrl + 'mine');
  }

  getUserLikedAds() {
    return this.http.get(this.baseUrl + 'liked');
  }

  createAd(ad: Ad): Observable<number> {
    return this.http.post<number>(this.baseUrl, ad);
  }

  updateAd(adId: number, ad: Ad) {
    return this.http.put(this.baseUrl + adId, ad);
  }

  deleteAd(adId: number) {
    return this.http.delete(this.baseUrl + adId);
  }
}
