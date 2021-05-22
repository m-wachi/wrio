import { DsColumn } from "./model";

export interface PivotData {
  //colNames: string[];
  colNames: DsColumn[];
  rows: number[][];
}

export interface OptPivotData {
  value: PivotData;
}

