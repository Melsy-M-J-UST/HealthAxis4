import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  if (authService.isLoggedIn()) {
    return true;
  }
  if (authService.isLoggedIn() && !authService.isTokenExpired()) {
    return true;
  }
  router.navigate(["/login"], {
    queryParams: { returnUrl: state.url }
  });
  return false;
};
export const roleGuard: CanActivateFn = (route, state) => {

  const authService = inject(AuthService);
  const router = inject(Router);

  const roles = authService.getUserRoles();
  const requiredRoles = route.data?.['roles'] as string[];

  if (!requiredRoles || requiredRoles.length === 0) {
    return true;
  }

  const hasAccess = roles.some(r => requiredRoles.includes(r));

  if (hasAccess) {
    return true;
  }

  router.navigate(['/login']);
  return false;
};
