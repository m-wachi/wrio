import moment, {isMoment} from 'moment';


export type WrioValue = string | number | null | WrioDate | IWrioBase; // | moment.Moment;


export interface IWrioBase {
  getTypeName() : string;
  equals(v: any) : boolean;
}

export function equals(v1: WrioValue | undefined, v2: WrioValue | undefined) : boolean {
  if (v1 === undefined) {
    return (v2 === undefined);
  }

  if (v1 === null) {
    return (v2 === null);
  }

  if ("string" === typeof v1) {
    if ("string" === typeof v2) {
      return (v1 === v2);
    } else {
      return false;
    }
  }
  
  if ("number" === typeof v1) {
    if ("number" === typeof v2) {
      return (v1 === v2);
    } else {
      return false;
    }
  }

  const iwb = v1 as IWrioBase;
  return iwb.equals(v2);
  
}


export class WrioDate implements IWrioBase {
  private static TYPENAME = "WrioDate"; // do not change
  public momentValue: moment.Moment;

  constructor(sDateExp?: string) {
    if (undefined === sDateExp) {
      this.momentValue = moment();
    } else {
      this.momentValue = moment(sDateExp);
    }
  }

  getTypeName() : string {
    return WrioDate.TYPENAME;
  }

  equals(v: any) : boolean {
    /*
    let vTypeName = v.getTyepName();
    if (vTypeName === "WrioDate") {
    */
    if (v instanceof WrioDate) {
        return this.momentValue.isSame(v.momentValue);
    }
    return false;
  }

  public toString(): string {
    return this.momentValue.toString();
  }

}

export class WrioRecord implements IWrioBase {
  private static TYPENAME = "WrioRecord"; // do not change
  private map_ : Map<string, WrioValue>;

  constructor() {
    this.map_ = new Map();
  }

  public get(k: string) : WrioValue | undefined {
    const v = this.map_.get(k);
    return v;
  }
  public set(k: string, v: WrioValue) : void{
    this.map_.set(k, v);
  }

  public getSize() {
    return this.map_.size;
  }

  public entries() {
    return this.map_.entries();
  }

  public getTypeName() : string {
    return WrioRecord.TYPENAME;
  }

  public equals(wr: any) : boolean {
    if (!(wr instanceof WrioRecord)) {
      return false;
    }

    const wr2 = wr as WrioRecord;

    //size check
    if (this.getSize() !== wr2.getSize()) {
      return false;
    }

    const ite = this.map_.entries();

    let iteResult = ite.next();
    while(!iteResult.done) {
      const kvPair = iteResult.value;
      const k = kvPair[0];
      const v = kvPair[1];
      if (!equals(v, wr2.get(k))) {
        return false;
      }
      iteResult = ite.next();
    }
    return true;
  }

  public toString(): string {
    let lstKv : string[] = [];
    this.map_.forEach((v, k, mp) => {
      lstKv.push(String(k) + ": " + String(v));
    });
    return ("WrioRecord {" + lstKv.join(", ") + "}");
  }

}

export class WrioRecordPair implements IWrioBase {
  private static TYPENAME = "WrioRecordPair"; // do not change
  constructor(public rowHdr : WrioRecord, public colHdr: WrioRecord) {}

  public getTypeName() : string {
    return WrioRecordPair.TYPENAME;
  }

  public equals(wrp: any) : boolean {
    if (!(wrp instanceof WrioRecordPair)) {
      return false;
    }

    const wrp2 = wrp as WrioRecordPair;

    if (!wrp2.rowHdr.equals(this.rowHdr)) {
      return false;
    } else if (!wrp2.colHdr.equals(this.colHdr)) {
      return false;
    } else {
      return true;
    }
  }

  public toString() : string {
    return "WrioRecordPair { rowHdr: " + this.rowHdr.toString() + ", colHdr: " + this.colHdr.toString() + "}";
  }

}


export class WrioMap {
  private map_ : Map<WrioValue, any>;
  constructor() {
    this.map_ = new Map();
  }

  public get(theKey: WrioValue) : any {
    const sTypeOf = typeof theKey;
    if("string" === sTypeOf || "number" === sTypeOf) {
      return this.map_.get(theKey);
    }

    // use "equals()" if theKey is not string or number;
    const theKey2 = theKey as IWrioBase
    
    const ite = this.map_.entries();
    //for(const kvPair in this.map_.entries()) {
    let iteResult = ite.next();
    while(!iteResult.done) {
      const kvPair = iteResult.value;
      const k = kvPair[0];
      const v = kvPair[1];
      
      if (theKey2.equals(k)) {
        return v;
      }
      iteResult = ite.next();
    }
    return null;      
  }

  public set(theKey: WrioValue, theValue: any) : void {
    const sTypeOf = typeof theKey;
    if("string" === sTypeOf || "number" === sTypeOf) {
      this.map_.set(theKey, theValue);
      return;
    }

    // use "equals()" if theKey is not string or number;
    const theKey2 = theKey as IWrioBase
    const ite = this.map_.keys();
    let iteResult = ite.next();
    while(!iteResult.done) {
      const k = iteResult.value
      //console.log("k=%s", String(k));
      if (theKey2.equals(k)) {
        this.map_.set(k, theValue);
        return;
      }
      iteResult = ite.next();
    }
    //console.log("WrioMap.set() endset.")
    this.map_.set(theKey, theValue);

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
    //return String(this.map_);
    let lstKv : string[] = [];
    this.map_.forEach((v, k, mp) => {
      lstKv.push(String(k) + ": " + String(v));
    });
    return ("WrioMap {" + lstKv.join(", ") + "}");

  }

}

export class WrioSet {
  private set_ : Set<WrioValue>;
  constructor() {
    this.set_ = new Set<WrioValue>();
  }


  public add(v: WrioValue) : void {
    //this.set_.entries();

    //this.set_.
    const sTypeOf = typeof v;
    if("string" === sTypeOf || "number" === sTypeOf) {
      this.set_.add(v);
      return;
    }

    // use "equals()" if theKey is not string or number;
    const v2 = v as IWrioBase
    const ite = this.set_.values();
    let iteResult = ite.next();
    while(!iteResult.done) {
      const v3 = iteResult.value
      //console.log("k=%s", String(k));
      if (v2.equals(v3)) {
        return;
      }
      iteResult = ite.next();
    } 
    this.set_.add(v2);

  }

  public values() {
    return this.set_.values();
  }

  public toString(): string {
    //return String(this.map_);
    let lstV : string[] = [];
    this.set_.forEach((v) => {
      if (null === v) {
        lstV.push("(null)");
      } else {
        lstV.push(v.toString());
      }
    });
    return ("WrioSet [" + lstV.join(", ") + "]");

  }

}