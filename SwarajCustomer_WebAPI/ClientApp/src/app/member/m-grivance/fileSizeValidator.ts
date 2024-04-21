import { AbstractControl, FormControl } from "@angular/forms";
export function fileSizeValidator(files: FileList) {
  return function(control: FormControl) {
    // return (control: AbstractControl): { [key: string]: any } | null => {
    const file = control.value;
    if (file) {
      var path = file.replace(/^.*[\\\/]/, "");
      const fileSize = files.item(0).size;
      const fileSizeInMB = Math.round(fileSize / 1048576);
      if (fileSizeInMB > 2) {
        return {
          fileSizeValidator: true
        };
      } else {
        return null;
      }
    }
    return null;
  };
}