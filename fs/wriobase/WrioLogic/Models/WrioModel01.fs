namespace Wrio.Models

type Dimension(table: string, abbrev: string, joinSrcCol: string, 
                dstAbbrev: string, joinDstCol: string, joinDiv: int) =
    member this.Table: string = table
    member this.Abbrev: string = abbrev
    member this.JoinSrcCol: string = joinSrcCol
    member this.DstAbbrev: string = dstAbbrev
    member this.JoinDstCol: string = joinDstCol
    member this.JoinDiv: int = joinDiv

(*
class DtSet(object):
    def __init__(self):
        self.datasetId = -1
        self.factTable = ""
        self.factAbbrev = ""
        self.dimensions = []
*)    
type DtSet(datasetId: int, factTable: string, factAbbrev: string) =
    let mutable dimensions: List<Dimension> = []
    member this.DatasetId: int = datasetId
    member this.FactTable: string = factTable
    member this.FactAbbrev: string = factAbbrev
    member this.Dimenstions
        with get() : List<Dimension> = dimensions
        and set(v: List<Dimension>) = dimensions <- v


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


