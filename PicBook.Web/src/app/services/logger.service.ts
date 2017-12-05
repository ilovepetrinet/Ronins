import { Injectable } from "@angular/core";

import { Configuration } from "../app.constants";

@Injectable()
export class Logger {

  constructor(
    private _configuration: Configuration
  ) { }

  public debug(code: string, message: string, obj?: any): void {
    if (this._configuration.IsDevEnvironment) {
      let format = this.getLocalDateTime() + ": [" + code + "] - " + message;

      if (obj) {
        console.log(format, obj);
      } else {
        console.log(format);
      }
    }
  }

  public info(code: string, message: string, obj?: any): void {
    let format = this.getLocalDateTime() + ": [" + code + "] - " + message;

      if (obj) {
        console.info(format, obj);
      } else {
        console.info(format);
      }
  }

  public warn(code: string, message: string, obj?: any): void {
    let format = this.getLocalDateTime() + ": [" + code + "] - " + message;

      if (obj) {
        console.warn(format, obj);
      } else {
        console.warn(format);
      }
  }

  public error(code: string, message: string, obj?: any): void {
    let format = this.getLocalDateTime() + ": [" + code + "] - " + message;

      if (obj) {
        console.error(format, obj);
      } else {
        console.error(format);
      }
  }

  private getLocalDateTime(): string {
    const date = new Date();
    return date.toLocaleTimeString();
  }
}
