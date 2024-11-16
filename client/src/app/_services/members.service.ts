import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  members = signal<Member[]>([]);

  getMembers() {
    return this.http
      .get<Member[]>(
        this.baseUrl + 'users'
        // this.getHttpOptions()
      )
      .subscribe({
        next: (members) => this.members.set(members),
      });
  }

  getMember(username: string) {
    const member = this.members().find((x) => x.userName == username);
    if (member !== undefined) return of(member); // this will return the member as observable
    return this.http.get<Member>(
      this.baseUrl + 'users/' + username
      // this.getHttpOptions() no need, jwt interceptor handles this
    );
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      tap(() => {
        this.members.update((members) =>
          members.map((m) => (m.userName === member.userName ? member : m))
        );
      })
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

//A signal is a special variable that holds a value and automatically notifies any part of your app that depends on it whenever its value changes.
// It's used to reactively manage data and ensure your app updates efficiently when the state changes.
