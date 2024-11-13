// interceptor handles errors for all HTTP requests for the client side

import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toastr = inject(ToastrService);
  return next(req).pipe(
    catchError((error) => {
      if (error) {
        switch (error.status) {
          case 400:
            if (error.error.errors) {
              const modalStateErrors = [];
              for (const key in error.error.errors) {
                if (error.error.errors[key]) {
                  modalStateErrors.push(error.error.errors[key]);
                }
              }
              throw modalStateErrors.flat();
            } else {
              toastr.error(error.error, error.status);
            }
            break;

          case 401:
            toastr.error('unauthorised', error.status);
            break;
          case 404:
            router.navigateByUrl('/not-found');
            break;
          case 500:
            //navigationExtras passes the error details to the "server-error" page via the state object.
            const navigationExtras: NavigationExtras = {
              state: { error: error.error },
            };
            router.navigateByUrl('/server-error', navigationExtras);
            break;
          default:
            toastr.error('somthing went wrong');
            break;
        }
      }
      throw error;
    })
  );
};
// interceptors  modify outgoing requests (e.g., add headers), handle responses, or catch errors globally.
// next(req): here where the request is passed to the next step in the HTTP request pipeline (i.e., making the actual request).
// pipe(): method to chain operators on the observable.
// error.status : returns status code
