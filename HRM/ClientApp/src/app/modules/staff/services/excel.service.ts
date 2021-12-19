
import { Injectable } from '@angular/core';
import * as FileSaver from 'file-saver';
import * as XLSX from 'xlsx';
import { DatePipe } from '@angular/common';
import { Observable } from 'rxjs';
//import { release } from 'os';

const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
const EXCEL_EXTENSION = '.xlsx';

@Injectable()
export class ExcelService {
  constructor(
    private datePipe: DatePipe) { }

  public exportAsExcelFile(json: any[], excelFileName: string): void {
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(json);
    const workbook: XLSX.WorkBook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    this.saveAsExcelFile(excelBuffer, excelFileName);
  }

  private saveAsExcelFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], { type: EXCEL_TYPE });
    FileSaver.saveAs(data, fileName + '_export_' + new Date().getTime() + EXCEL_EXTENSION);
  }

  parseExcel(file, listDateProperty: Array<string>, callback) {
    let reader = new FileReader();
    reader.onload = (e) => {
      let data = (<any>e.target).result;
      let workbook = XLSX.read(data, {
        type: 'binary', cellDates: false
      });

      //workbook.SheetNames.forEach((function (sheetName) {
      // Here is your object  

      let XL_row_object = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { raw: true, defval: "" });
      var self = this;
      XL_row_object.forEach(function (row: any) {
        listDateProperty.forEach(function (key: string) {
          console.log(row[key]);
          row[key] = self.excelDateToDate(row[key]);
          
        });
        row["phone"] = self.CheckPhone(row["phone"]);
      }.bind(this));
      console.log(XL_row_object);
      callback(XL_row_object);
    };

    reader.onerror = function (ex) {
      console.log(ex);
    };
    reader.readAsBinaryString(file);
  }

  excelDateToDate(excelDateNumber): any {
    var checkNumber = Number(excelDateNumber)
    if (excelDateNumber == "" || isNaN(checkNumber))
      return new Date(Date.UTC(1900, 0, 1, 0, 0, 0));
    else
      return new Date(Math.round((excelDateNumber - 25569) * 86400 * 1000));
  }
  CheckPhone(phone): any {
    var checkNumber = Number(phone)
    if (phone == "" || isNaN(checkNumber))
      return "";
    else return phone;
  }
}
