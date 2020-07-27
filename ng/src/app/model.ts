export interface DtJoin {
    joinSrcCol: string;
    dstAbbrev: string;
    joinDstCol: string;
}

export interface DsTable {
  dsJoins: DtJoin[];
  dsTableId: number;
  table: string;
  abbrev: string;
  tableType: number;
  joinDiv: number;
}

export interface DtSet {
  dimensions: DsTable[];
  datasetId: number;
  fact: DsTable;
}

export interface CellVal {
  colName: string;
  abbrev: string;
  aggFuncDiv: number;
}

export interface PivotSetting {
  datasetId: number;
  colHdr: string[];
  rowHdr : string[];
  cellVal: CellVal[];
  rowOdr: string[];
  colOdr: string[];
}

export interface Pivot {
    pivotId: number;
    datasetId: number;
    setting: PivotSetting;
    DtSet: DtSet;
}

export interface OptPivot {
  value: Pivot;
}
