export interface DsColumn {
  colName: string;
  colType: string;
}

export interface DsJoin {
    joinSrcCol: string;
    dstAbbrev: string;
    joinDstCol: string;
}

export interface DsTable {
  dsTableId: number;
  table: string;
  abbrev: string;
  tableType: number;
  joinDiv: number;
  dsJoins: DsJoin[];
  columns: DsColumn[];
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
    dataSet: DtSet;
}

export interface OptPivot {
  value: Pivot;
}
