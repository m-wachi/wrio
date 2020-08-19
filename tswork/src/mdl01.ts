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
        if (sDateExp) {
            this.momentValue = moment(sDateExp);
        } else {
            this.momentValue = moment();
        }
    }

    getTypeName() : string {
        return WrioDate.TYPENAME;
    }

    equals(v: any) : boolean {
        let vTypeName = v.getTyepName();
        if (vTypeName === "WrioDate") {
            return this.momentValue.isSame(v.momentValue);
        }
        return false;
    }



}



export class WrioMap {

}
