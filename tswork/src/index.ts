import moment, { isMoment } from 'moment';
import {WrioDate, WrioMap, WrioValue, WrioRecord} from './mdl01';

console.log("Hello index.ts");

const v1 = 1234.567;
const v2 = 3456.7;

console.log(new Intl.NumberFormat('ja').format(v1));

// 四捨五入
// 小数点以下をどう扱いたいかもオプションで指定可能です。
// minimumFractionDigitsで最小桁数（足りない場合は 0 で埋める）、
// maximumFractionDigitsで最大桁数（超える場合はこの桁数に収まるよう四捨五入）
// を指定できます。

  let nf1 = new Intl.NumberFormat('ja', {style: 'decimal', minimumFractionDigits: 2, maximumFractionDigits: 2});

console.log("v1=" + nf1.format(v1));
console.log("v2=" + nf1.format(v2));
