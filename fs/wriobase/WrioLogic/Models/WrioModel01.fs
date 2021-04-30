namespace Wrio.Models

open System

type WrioValueType = 
    | NULL = 0 | STRING = 1 | NUMBER = 2 | DATE = 3 | OTHER = 4

// Dataset Column 
type DsColumn(pColName: string, pColType: WrioValueType) =
    let mutable colName: string = pColName
    let mutable colType: WrioValueType = pColType
    new() = DsColumn("", WrioValueType.STRING)
    member this.ColName
        with get(): string = colName
        and set(v: string) = colName <- v
    member this.ColType
        with get(): WrioValueType = colType
        and set(v: WrioValueType) = colType <- v
    override this.ToString(): string =
        sprintf "DsColumn {colName: %s, colType=%A}" colName colType

type DsJoin(pJoinSrcCol: string, pDstAbbrev: string, pJoinDstCol: string) = 
    let mutable joinSrcCol: string = pJoinSrcCol
    let mutable dstAbbrev: string = pDstAbbrev
    let mutable joinDstCol: string = pJoinDstCol
    new() = DsJoin("", "", "")
    member this.JoinSrcCol
        with get(): string = joinSrcCol
        and set(v: string) = joinSrcCol <- v
    member this.DstAbbrev
        with get(): string = dstAbbrev
        and set(v: string) = dstAbbrev <- v
    member this.JoinDstCol
        with get(): string = joinDstCol
        and set(v: string) = joinDstCol <- v


type DsTable(pDsTableId: int, table: string, abbrev: string, tableType: int, 
             joinDiv: int, pAryDsJoin: DsJoin array, pAryColumn: DsColumn array) =
    let mutable dsTableId = pDsTableId
    let mutable aryDsJoin = pAryDsJoin
    let mutable aryColumn = pAryColumn
    new() = DsTable(0, "", "", 0, 1, [||], [||])
    member this.DsTableId
        with get() : int = dsTableId
        and set(v: int) = dsTableId <- v
    member this.Table: string = table
    member this.Abbrev: string = abbrev
    member this.TableType: int = tableType
    member this.JoinDiv: int = joinDiv
    member this.DsJoins
        with get() : DsJoin array = aryDsJoin
        and set(v: DsJoin array) = aryDsJoin <- v
    member this.Columns
        with get() : DsColumn array = aryColumn
        and set(v: DsColumn array) = aryColumn <- v
    override this.ToString(): string =
        sprintf "DsTable { DsTableId=%d, LstDsJoin=%A }" dsTableId aryDsJoin

type DtSet(datasetId: int, fact: DsTable, pDimensions: DsTable array) =
    let mutable dimensions: DsTable array = pDimensions
    new() = DtSet(-1, DsTable(), [||])
    member this.DatasetId: int = datasetId
    member this.Fact: DsTable = fact
    member this.Dimensions
        with get() : DsTable array = dimensions
        and set(v: DsTable array) = dimensions <- v
    override this.ToString(): string =
        sprintf "DtSet datasetId=%d, fact=%A, dimensions=%A" datasetId fact dimensions

type CellVal(pColName: string, pAbbrev: string, pAggFuncDiv: int) =
    let mutable colName: string = pColName
    let mutable abbrev: string = pAbbrev
    let mutable aggFuncDiv: int = pAggFuncDiv
    new() = CellVal("", "", -1)
    member this.ColName
        with get() = colName
        and set(v) = colName <- v
    member this.Abbrev
        with get() = abbrev
        and set(v) = abbrev <- v
    member this.AggFuncDiv
        with get() = aggFuncDiv
        and set(v) = aggFuncDiv <- v
    override this.ToString(): string =
        sprintf "CellVal { ColName=%s, AggFuncDiv=%d }" colName aggFuncDiv

type PivotSetting() =  
    let mutable datasetId: int = -1
    let mutable colHdr : string array = [||]
    let mutable rowHdr : string array = [||] 
    let mutable cellVal: CellVal array = [||]
    let mutable rowOdr : string array = [||]
    let mutable colOdr : string array = [||]
    member this.DatasetId 
        with get() : int = datasetId
        and set(v: int) = datasetId <- v
    member this.ColHdr 
        with get() : string array = colHdr
        and set(v : string array) = colHdr <- v
    member this.RowHdr 
        with get() = rowHdr
        and set(v) = rowHdr <- v
    member this.CellVal
        with get() = cellVal
        and set(v) = cellVal <- v
    member this.RowOdr 
        with get() : string array = rowOdr
        and set(v : string array) = rowOdr <- v
    member this.ColOdr 
        with get() : string array = colOdr
        and set(v : string array) = colOdr <- v

    override this.ToString(): string =
        sprintf "PivotSetting { datasetId=%d, colHdr=%A, rowHdr=%A, CellVal=%A, rowOdr=%A, colOdr=%A }" datasetId colHdr rowHdr cellVal rowOdr colOdr

(*
type Pivot = {
    PivotId : int
    DatasetId : int
    Setting : PivotSetting
    DtSet : DtSet
}
*)

type Pivot(pPivotId : int, pDatasetId : int, pSetting : PivotSetting, pDataSet : DtSet) =
    let mutable pivotId : int = pPivotId
    let mutable datasetId : int = pDatasetId
    let mutable setting : PivotSetting = pSetting
    let mutable dataSet : DtSet = pDataSet
    new() = Pivot(-1, -1, PivotSetting(), DtSet())
    member this.PivotId
        with get() : int = pivotId
        and set(v : int) = pivotId <- v
    member this.DatasetId
        with get() : int = datasetId
        and set(v : int) = datasetId <- v
    member this.Setting
        with get() : PivotSetting = setting
        and set(v : PivotSetting) = setting <- v
    member this.DataSet 
        with get() : DtSet = dataSet
        and set(v : DtSet) = dataSet <- v

type PivotData(pColNames: string array, pRows: (obj array) array) = 
    let mutable colNames: string array = pColNames
    let mutable rows: (obj array) array = pRows
    new() = PivotData([||], [||])
    member this.ColNames
        with get() = colNames
        and set(v) = colNames <- v
    member this.Rows
        with get() = rows
        and set(v) = rows <- v
    override this.ToString(): string =
        sprintf "PivotData { ColNames=%A, Rows=%A }" colNames rows

module MdlFunc =
    let getDsColumn(dtSet: DtSet) (colName1: string) : DsColumn option =
        let sepColName1 = colName1.Split(".")
        let abbrev1 = sepColName1.[0]
        let rawColName = sepColName1.[1]

        let lookupDsc (dsTable1: DsTable): DsColumn option = 
            let dsColumns1 = Array.filter (fun (x: DsColumn) -> x.ColName = rawColName)  dsTable1.Columns
            if dsColumns1.Length > 0 
                then Some dsColumns1.[0] 
                else None

        if dtSet.Fact.Abbrev = abbrev1 
            then  
                lookupDsc dtSet.Fact
            else
                let b = Array.filter (fun (x: DsTable) -> x.Abbrev = abbrev1) dtSet.Dimensions
                if b.Length > 0 
                    then lookupDsc b.[0]
                    else None

    //TODO 
    // write test
    let getPivotColumns2 (pvtSetting: PivotSetting) (dtSet: DtSet) : DsColumn array =
        let pvtCols1: string array = Array.append pvtSetting.RowHdr pvtSetting.ColHdr

        let colName1: string = pvtCols1.[0]
        
        let pvtCols2: string array = 
            Array.append pvtCols1 
                         (Array.map (fun (x: CellVal) -> x.Abbrev + "." + x.ColName) 
                                    pvtSetting.CellVal)
       
        let consOpt (ary: DsColumn list) (x: DsColumn option) =
            match x with
            | Some y -> y :: ary
            | None -> ary

        let lstOptDsColumn = List.map (getDsColumn dtSet) (Array.toList pvtCols2)

        let lstDsColumnRev = List.fold consOpt [] lstOptDsColumn
        
        List.rev lstDsColumnRev |> List.toArray

(*
    let joinCondSql (srcAbbrev: string) (dsJoin: DsJoin) : string =
        //sprintf "%s.%s=%s.%s" dsJoin.DstAbbrev dsJoin.JoinDstCol srcAbbrev dsJoin.JoinSrcCol
        String.Format("{0}.{1}={2}.{3}", dsJoin.DstAbbrev, dsJoin.JoinDstCol, srcAbbrev, dsJoin.JoinSrcCol)
*)

