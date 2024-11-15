import { HttpInterceptorFn } from '@angular/common/http';
import { AccountService } from '../_services/account.service';
import { inject } from '@angular/core';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService);

  // custom function to handle HTTP requests and attach the JWT (JSON Web Token) to the headers of outgoing requests
  if (accountService.currentUser()) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accountService.currentUser()?.token}`,
      },
    });
  }
  return next(req);
};
// The main purpose of this interceptor is to automatically add the JWT token (if the user is logged in) to the HTTP requests made by the Angular application
// clone() is a method used to create a copy of the original request while allowing modifications to it
