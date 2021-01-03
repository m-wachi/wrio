namespace Wrio.Models

open System

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


type DsTable(pDsTableId: int, table: string, abbrev: string, tableType: int, joinDiv: int, pAryDsJoin: DsJoin array) =
    let mutable dsTableId = pDsTableId
    let mutable aryDsJoin = pAryDsJoin
    new() = DsTable(0, "", "", 0, 1, [||])
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

(*
module ModelFunc =
    let joinCondSql (srcAbbrev: string) (dsJoin: DsJoin) : string =
        //sprintf "%s.%s=%s.%s" dsJoin.DstAbbrev dsJoin.JoinDstCol srcAbbrev dsJoin.JoinSrcCol
        String.Format("{0}.{1}={2}.{3}", dsJoin.DstAbbrev, dsJoin.JoinDstCol, srcAbbrev, dsJoin.JoinSrcCol)
*)

