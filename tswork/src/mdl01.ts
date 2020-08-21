import moment, {isMoment} from 'moment';


export type WrioValue = string | number | IWrioBase; // | moment.Moment;

export interface IWrioBase {
  getTypeName() : string;
  equals(v: any) : boolean;
}

//export type ObjectGroup = WrioDate | 

//export interface Wrio

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

  toString(): string {
    return this.momentValue.toString();
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
    let retVal: any = null;
    for(const kvPair in this.map_.entries()) {
      const k = kvPair[0];
      const v = kvPair[1];
      
      if (theKey2.equals(k)) {
        return v;
      }
    }
    return null;      

  }

    //public set

}
