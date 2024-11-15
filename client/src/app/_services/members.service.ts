import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  getMembers() {
    return this.http.get<Member[]>(
      this.baseUrl + 'users'
      // this.getHttpOptions()
    );
  }

  getMember(username: string) {
    return this.http.get<Member>(
      this.baseUrl + 'users/' + username
      // this.getHttpOptions() no need, jwt interceptor handles this
    );
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
