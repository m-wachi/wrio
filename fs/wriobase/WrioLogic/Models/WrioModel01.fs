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

type Pivot {
    DatasetId : int
    SettingJson : string
    DtSet : DtSet
}
(*
type Pivot(datasetId : int, settingJson : string, dtSet : Dataset)
    member this.DatasetId : int = datasetId
    member this.SettingJson : string = settingJson
    member this.Dataset : DtSet = dtSet
*)