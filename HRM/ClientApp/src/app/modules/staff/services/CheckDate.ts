import { FormGroup, AbstractControl, Validator, NG_VALIDATORS } from "@angular/forms";

export function CheckDate(control: AbstractControl): { [key: string]: boolean } | null {

  let DateValueCompare = new Date(control.value);
  let minDateString = '1900-01-01';
  let minDate = new Date(minDateString);
  let maxDateString = '9999-12-31';
  let maxDate = new Date(maxDateString);
  if (control.value == '' || (DateValueCompare > minDate && DateValueCompare <= maxDate)) {
    return null;
  }
  return { 'DateValueValid': true };
};
