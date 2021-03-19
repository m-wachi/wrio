import { type } from 'os';
import { WrioValueType, WrioValue, WrioDate } from './model02';


export interface PivotTableFieldDef {
    format(v: WrioValue): string;
}

export function getPivotTableFieldDef(typeEnum: WrioValueType): PivotTableFieldDef {
    switch (typeEnum) {
        case WrioValueType.STRING:
            return new PivotTableStringFieldDef();
        case WrioValueType.NUMBER:
            return new PivotTableNumberFieldDef();
        case WrioValueType.DATE:
            return new PivotTableDateFieldDef();
    }
}


export class PivotTableStringFieldDef implements PivotTableFieldDef {
    format(v: string): string {
        return v;
    }
}

export class PivotTableNumberFieldDef implements PivotTableFieldDef {

    private numfmt: Intl.NumberFormat = null;
    private fractMin: number = 0;
    private fractMax: number = 0;

    constructor() {}

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
        return this.numfmt.format(v);
    }

}

export class PivotTableDateFieldDef implements PivotTableFieldDef {

    private fmtStr: string = "";

    constructor() {}

    public setFormatString(formatString: string) {
        this.fmtStr = formatString;
    }
    
    public format(v: WrioDate): string {
        return v.format(this.fmtStr);
    }

}

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
