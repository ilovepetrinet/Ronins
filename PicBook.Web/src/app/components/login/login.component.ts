import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { AuthenticationService } from "../../services/authentication.service";
import { Logger } from "../../services/logger.service";
import { NotificationsService } from "../../services/notifications.service";

import { User } from "../../models/user";
import { Notification } from "../../models/notification";

@Component({
  selector: "login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"]
})

export class LoginComponent implements OnInit {

  constructor(
    private _authService: AuthenticationService,
    private _logger: Logger,
    private _router: Router,
    private _notifier: NotificationsService
  ) { }

  ngOnInit() {
    
  }

  public login(provider: string): void {
    this._authService.getExternalAccessToken(provider).subscribe(
      data => {
        const user = new User();
        const extRes = data as any;

        user.email = extRes.email;
        user.name = extRes.name;
        user.provider = extRes.provider;
        user.userIdentifier = extRes.uid;

        this._authService.login(user).subscribe(
          res => {
            this._notifier.add(new Notification("success", "Login successful."));
            this._logger.debug("0x020700", "Login successful.", res);

            localStorage.setItem("uid", extRes.uid);
            localStorage.setItem("acc_token", extRes.token);
            localStorage.setItem("external_login_provider", extRes.provider);

            this._router.navigate([""]);
          }
        );
      }, err => {
        this._logger.error("Ex020700", "Error occured in external login.", err);
        this._notifier.add(new Notification("error", "Error occured while logging in."));
      }
    );
  }
}
