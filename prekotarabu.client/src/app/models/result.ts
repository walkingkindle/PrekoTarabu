import {Error} from "./error";

export interface Result {
  isSuccess: boolean;
  error: Error
}
