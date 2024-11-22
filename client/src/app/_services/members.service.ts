import {
  HttpClient,
  HttpParams,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  // members = signal<Member[]>([]); remove it cause everything will be stored in paginatedResult
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);
  memberCache = new Map();

  getMembers(userParams: UserParams) {
    const response = this.memberCache.get(Object.values(userParams).join('-'));
    if (response) return this.setPaginatedResponse(response);
    let params = this.setPaginationHeaders(
      userParams.pageNumber,
      userParams.pageSize
    );

    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return this.http
      .get<Member[]>(
        this.baseUrl + 'users',
        { observe: 'response', params }
        // this.getHttpOptions()
      )
      .subscribe({
        // next: (members) => this.members.set(members),
        next: (response) => {
          this.setPaginatedResponse(response);
          this.memberCache.set(Object.values(userParams).join('-'), response);
        },
      });
  }

  private setPaginatedResponse(response: HttpResponse<Member[]>) {
    this.paginatedResult.set({
      items: response.body as Member[],
      pagination: JSON.parse(response.headers.get('pagination')!),
    });
  }
  private setPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();

    if (pageNumber && pageSize) {
      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', pageSize);
    }
    return params;
  }

  getMember(username: string) {
    // const member = this.members().find((x) => x.userName == username);
    // if (member !== undefined) return of(member); // this will return the member as observable
    const member: Member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.body), [])
      .find((m: Member) => m.userName === username);
    if (member) return of(member);
    return this.http.get<Member>(
      this.baseUrl + 'users/' + username
      // this.getHttpOptions() no need, jwt interceptor handles this
    );
  }

  updateMember(member: Member) {
    return this.http
      .put(this.baseUrl + 'users', member)
      .pipe
      // tap(() => {
      //   this.members.update(
      //     (
      //       members // After the server confirms the update, the local state (members signal) is updated to reflect the changes, ensuring the app stays in sync with the server
      //     ) => members.map((m) => (m.userName === member.userName ? member : m))
      //   );
      // })
      ();
  }

  setMainPhoto(photo: Photo) {
    return this.http
      .put(this.baseUrl + 'users/set-main-photo/' + photo.id, {})
      .pipe
      // tap(() => {
      //   this.members.update((members) =>
      //     members.map((m) => {
      //       if (m.photos.includes(photo)) {
      //         m.photoUrl = photo.url;
      //       }
      //       return m;
      //     })
      //   );
      // })
      ();
  }

  deletePhoto(photo: Photo) {
    return this.http
      .delete(this.baseUrl + 'users/delete-photo/' + photo.id)
      .pipe
      // tap(() => {
      //   this.members.update((members) =>
      //     members.map((m) => {
      //       if (m.photos.includes(photo)) {
      //         m.photos = m.photos.filter((x) => x.id !== photo.id);
      //       }
      //       return m;
      //     })
      //   );
      // })
      ();
  }
  getHttpOptions() {
    return {
      // creates an object containing the HTTP headers, specifically the Authorization header with a Bearer token.
      // headers: new HttpHeaders({ now no need for it because of the jwt interceptor
      //   Authorization: `Bearer ${this.accountService.currentUser()?.token}`,
      // }),
    };
  }
}

// The HTTP headers are passed with the request to authenticate the user. The Authorization header allows the server to verify the user's identity by checking the token, ensuring that only authenticated users can access certain resources.

//A signal is a special variable that holds a value and automatically notifies any part of your app that depends on it whenever its value changes.
// It's used to reactively manage data and ensure your app updates efficiently when the state changes.

// The pipe function allows adding additional operations to the observable stream returned by http.put().
// The tap operator performs a side effect (updating the local state) without changing the response from the observable.
