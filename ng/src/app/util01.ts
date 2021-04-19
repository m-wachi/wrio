export const myZip = <T1, T2>(ary1: T1[], ary2: T2[]): [T1, T2][] => {
  let aryRet:[T1, T2][] = [];
  for(let i=0; i<ary1.length; i++) {
    aryRet.push([ary1[i], ary2[i]]);
  }
  return aryRet;
}

