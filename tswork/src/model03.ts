import * as dayjs from 'dayjs';
import { formats } from 'dayjs/locale/*';
import { IWrioBase, WrioValue } from './model02';
import { PivotTableCell } from './pivottable';

export class PtcRecord {
  constructor(public nmIdxPairs: [string, number][], public aryPtc: PivotTableCell[]) {}

  get(fieldName: string) : PivotTableCell | undefined {
    const f = (x: [string, number]) => {
      if (x[0] === fieldName) return true;
      else false;
    }

    let filtered = this.nmIdxPairs.filter(f)
    if (0 === filtered.length) {
      return undefined;
    }
    else {
      const idx = filtered[0][1];
      return this.aryPtc[idx];
    }

  }

  getFieldCount(): number {
    return this.aryPtc.length;
  }
}
