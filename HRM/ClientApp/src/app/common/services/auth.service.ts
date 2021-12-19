import { Injectable } from '@angular/core';
import { CanActivateChild } from '@angular/router';
import { Route } from '@angular/compiler/src/core';

@Injectable()

export class AuthService implements CanActivateChild {

  constructor(private authService: AuthService) { }

  canActivateChild() {
    var user = localStorage.getItem("user");
    console.log('User Login: ' + user);

    return true;
  }
}
