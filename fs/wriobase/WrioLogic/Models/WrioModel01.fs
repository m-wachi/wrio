namespace Wrio.Models

open System

(*
type DtJoin(joinSrcCol: string, dstAbbrev: string, joinDstCol: string, 
        joinDiv: int) =
    JoinSrcCol: string
    DstAbbrev: string
    JoinDstCol: string
    JoinDiv: int
*)
type DsJoin = {
    //DsTableId: int
    //SeqNo: int
    JoinSrcCol: string
    DstAbbrev: string
    JoinDstCol: string
    //JoinDiv: int
}


type Dimension(table: string, abbrev: string, joinDiv: int, lstDsJoin: DsJoin array) =
    new() = Dimension("", "", 1, [||])
    // joinSrcCol: string, dstAbbrev: string, joinDstCol: 
    member this.Table: string = table
    member this.Abbrev: string = abbrev
    //member this.JoinSrcCol: string = joinSrcCol //will be deleted
    //member this.DstAbbrev: string = dstAbbrev   //will be deleted
    //member this.JoinDstCol: string = joinDstCol //will be deleted
    // member this.JoinDiv: int = joinDiv          //will be deleted
    member this.LstDsJoin: DsJoin array = lstDsJoin


    // 暫定コード
    member this.JoinCond: string = 
        String.Format("{0}.{1} = {2}.{3}", lstDsJoin.[0].DstAbbrev, lstDsJoin.[0].JoinDstCol, abbrev, lstDsJoin.[0].JoinSrcCol)
        //"%s.%s = %s.%s" % (row[4], row[5], dim1.abbrev, row[3])

type DsTable(dsTableId: int, table: string, abbrev: string, tableType: int, joinDiv: int, pAryDsJoin: DsJoin array) =
    let mutable aryDsJoin = pAryDsJoin
    new() = DsTable(0, "", "", 0, 1, [||])
    member this.DsTableId: int = dsTableId
    member this.Table: string = table
    member this.Abbrev: string = abbrev
    member this.TableType: int = tableType
    member this.JoinDiv: int = joinDiv
    member this.DsJoins
        with get() : DsJoin array = aryDsJoin
        and set(v: DsJoin array) = aryDsJoin <- v
    override this.ToString(): string = 
        sprintf "DsTable DsTableId=%d, LstDsJoin=%A" dsTableId aryDsJoin

(*
class DtSet(object):
    def __init__(self):
        self.datasetId = -1
        self.factTable = ""
        self.factAbbrev = ""
        self.dimensions = []
*)    
type DtSet(datasetId: int, factTable: string, factAbbrev: string) =
    let mutable dimensions: Dimension array = [||]
    new() = DtSet(-1, "", "")
    member this.DatasetId: int = datasetId
    member this.FactTable: string = factTable
    member this.FactAbbrev: string = factAbbrev
    member this.Dimensions
        with get() : Dimension array = dimensions
        and set(v: Dimension array) = dimensions <- v


type PivotSetting() = 
    let mutable datasetId: int = -1
    let mutable colHdr : string array = [||]
    let mutable rowHdr : string array = [||] //Array.empty<string>
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
    member this.RowOdr 
        with get() : string array = rowOdr
        and set(v : string array) = rowOdr <- v
    member this.ColOdr 
        with get() : string array = colOdr
        and set(v : string array) = colOdr <- v


(*
class Pivot(object):
    def __init__(self):
        self.datasetId = -1
        self.settingJson = None
        self.dataset = None
        self.jsonObj = None  #will delete

    def getJSONObj(self):
        jo = {"datasetId": self.datasetId,
              "settingJson": self.settingJson,
              "dataset": self.dataset.getJSONObj()}
        return jo

    def __str__(self):
        s = "datasetId: " + str(self.datasetId)
        s += (", settingJson: " + json.dumps(self.settingJson))
        return s
*)

type Pivot = {
    PivotId : int
    DatasetId : int
    Setting : PivotSetting
    DtSet : DtSet
}
(*
type Pivot(datasetId : int, settingJson : string, dtSet : Dataset)
    member this.DatasetId : int = datasetId
    member this.SettingJson : string = settingJson
    member this.Dataset : DtSet = dtSet
*)


