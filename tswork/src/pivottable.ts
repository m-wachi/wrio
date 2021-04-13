import { type } from 'os';
import { Pivot } from './model';
import { WrioValueType, WrioValue, WrioDate, getWrioValueType, equals } from './model02';

export interface IPivotTableFieldDef {
  format(v: WrioValue): string;
  textAlign: string; //left, right, center
  fieldName: string;
}


//
// PivotTableFieldDef factory function
//
export function getPivotTableFieldDef(fieldName: string, typeEnum: WrioValueType): IPivotTableFieldDef {
  switch (typeEnum) {
    case WrioValueType.STRING:
      return new PivotTableStringFieldDef(fieldName);
    case WrioValueType.NUMBER:
      return new PivotTableNumberFieldDef(fieldName);
    case WrioValueType.DATE:
      return new PivotTableDateFieldDef(fieldName);
    default:
      return new PivotTableStringFieldDef(fieldName);
  }
}


export class PivotTableStringFieldDef implements IPivotTableFieldDef {
  public textAlign: string = "left";
  constructor(public fieldName: string) {}
  format(v: string): string {
    return v;
  }
}

export class PivotTableNumberFieldDef implements IPivotTableFieldDef {

  private numfmt?: Intl.NumberFormat = undefined;
  private fractMin: number = 0;
  private fractMax: number = 0;
  public textAlign: string = "right";

  constructor(public fieldName: string) {
    this.setNumFmt();
  }

  private setNumFmt() {
    this.numfmt = new Intl.NumberFormat('ja', 
        {style: 'decimal', minimumFractionDigits: this.fractMin, maximumFractionDigits: this.fractMax});
  }

  public setFractMin(pFractMin: number) {
    this.fractMin = pFractMin;
    this.setNumFmt();
  }

  public setFractMax(pFractMax: number) {
    this.fractMax = pFractMax;
    this.setNumFmt();
  }

  public setFract(pFractMin: number, pFractMax: number) {
    this.fractMin = pFractMin;
    this.fractMax = pFractMax;
    this.setNumFmt();
  }

  public format(v: number): string {
    if(null == this.numfmt)
      return "";
    else
      return this.numfmt.format(v);
  }

}

export class PivotTableDateFieldDef implements IPivotTableFieldDef {

  private fmtStr: string = "YYYY-MM-DD";
  public textAlign: string = "left";

  constructor(public fieldName: string) {}

  public setFormatString(formatString: string) {
    this.fmtStr = formatString;
  }
   
  public format(v: WrioDate): string {
    return v.format(this.fmtStr);
  }
}

/* これはいらない
export function getPivotTableCell(v: WrioValue) {
    const nWVType = getWrioValueType(v);
    const fieldDef = getPivotTableFieldDef(nWVType);
    return new PivotTableCell(v, fieldDef);
}
*/

export class PivotTableCell {
  constructor(protected wrioValue: WrioValue, protected fieldDef: IPivotTableFieldDef) {}
  text(): string {
    return this.fieldDef.format(this.wrioValue);
  }

  valEquals(ptc: PivotTableCell): boolean {
    return equals(this.wrioValue, ptc.wrioValue);
  }

  textAlign() {
    return this.fieldDef.textAlign;
  }

  fieldName() {
    return this.fieldDef.fieldName;
  }
}

export class PivotTableCells {
  constructor(protected aryPtc: PivotTableCell[]) {}
  valEquals(ptcs: PivotTableCells): boolean {
    let len = this.aryPtc.length;
    if(len !== ptcs.aryPtc.length) return false;
    for (let i = 0; i<len; i++) {
      if (!this.aryPtc[i].valEquals(ptcs.aryPtc[i]))
        return false;
    }
    return true;
  }


  filterByFieldIndexes(aryFieldIndex: number[]): PivotTableCells {

    /*
    let aryPtc2: PivotTableCell[] = [];
    for (let fieldIndex of aryFieldIndex) {
      aryPtc2.push(this.aryPtc[fieldIndex]);
    }
    */
    return new PivotTableCells(aryFieldIndex.map((x) => {return this.aryPtc[x];}));
  }


  /*
  filterFieldNames(aryFieldName: string[]): PivotTableCells {

    const f = (x: PivotTableCell) => {
      for (const fieldName of aryFieldName) {
        if (x.fieldName() === fieldName) return true;
      }
      return false;
    }

    return new PivotTableCells(this.aryPtc.filter(f));
  }
  */

  getFieldNames(): string[] {
    return this.aryPtc.map((x) => {return x.fieldName();});
  }

  getPtc(fieldName: string): PivotTableCell | undefined {
    let aryPtc2 = this.aryPtc.filter((x) => {
      return (x.fieldName() === fieldName);
    });
    if (aryPtc2.length === 0) {
      return undefined;
    }
    return aryPtc2[0];
  }
}
export class PtcsPair {
  
  constructor(public rowPtcs: PivotTableCells, public colPtcs: PivotTableCells) {}

  toArray(): PivotTableCells[] {
    return [this.rowPtcs, this.colPtcs];
  }
  valEquals(ptcp: PtcsPair): boolean {
      if (!this.rowPtcs.valEquals(ptcp.rowPtcs)) return false;
      if (!this.colPtcs.valEquals(ptcp.colPtcs)) return false;
      return true;
  }
}
export class PtcMap {
  private map_ : Map<PtcsPair, PivotTableCells>;
  constructor() {
    this.map_ = new Map();
  }
  
  public get(ptcspKey: PtcsPair) : PivotTableCells | undefined {

    const ite = this.map_.entries();
    let iteResult = ite.next();
    while(!iteResult.done) {
      const kvPair = iteResult.value;
        const k = kvPair[0];
        const v = kvPair[1];
        
        if (ptcspKey.valEquals(k)) {
          return v;
        }
        iteResult = ite.next();
      }
      return undefined;      
    }
  
    public set(ptcspKey: PtcsPair, ptcsValue: PivotTableCells) : void {
    
      const ite = this.map_.keys();
      let iteResult = ite.next();
      while(!iteResult.done) {
        const k = iteResult.value
        //console.log("k=%s", String(k));
        if (ptcspKey.valEquals(k)) {
          this.map_.set(k, ptcsValue);
          return;
        }
        iteResult = ite.next();
      }      
      this.map_.set(ptcspKey, ptcsValue);
    }
  
    public entries() {
      return this.map_.entries();
    }
  
    public keys() {
      return this.map_.keys();
    }
  
    public getSize() {
      return this.map_.size;
    }
  
    public toString(): string {

      let lstKv : string[] = [];
      this.map_.forEach((v, k, mp) => {
        lstKv.push(String(k) + ": " + String(v));
      });
      return ("PtcMap {" + lstKv.join(", ") + "}");
  
    }
  
  }
  
  
/*
export class PivotTableStringCell {
    constructor(protected v: string, protected fieldDef: PivotTableStringFieldDef) { }

    toString(): string{
        return this.fieldDef.format(this.v)
    }
}

export class PivotTableNumberCell {
    constructor(protected v: number, protected fieldDef: PivotTableNumberFieldDef) { }

    toString(): string{
        return this.fieldDef.format(this.v)
    }
}
*/


/*
export class PivotTableFieldDef {

    public format: string = "";

    private numfmt: Intl.NumberFormat = null;

    private typeName = "";

    constructor(public value: WrioValue) {
        this.typeName = getTypeName(this.value);


        if ("number" === typeName) {
            this.numfmt = new Intl.NumberFormat('ja', 
                {style: 'decimal', minimumFractionDigits: 2, maximumFractionDigits: 2});

        }

    }

    public toString(): string {

        if ("string" === this.typeName) return this.value as string;
        
        if ("number" === this.typeName) 
            return this.value.toString();

        //if ("WrioDate" === ) {

        //}
        this.value.toString();
    }

    

}
*/
