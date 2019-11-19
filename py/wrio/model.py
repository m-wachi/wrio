import json


class MainTbl(object):
    def __init__(self):
        self._jsonObj = {"id": 45, "name": 'Bye Bye Keen!'}

    def getJSONObj(self):
        return self._jsonObj
    

class DtSet(object):
    def __init__(self):
        self.datasetId = -1
        self.factTable = ""
        self.factAbbrev = ""
        self.dimensions = []

    def getJSONObj(self):
        jo = {"datasetId": self.datasetId,
              "factTable": self.factTable,
              "factAbbrev": self.factAbbrev,
              "dimensions": [x.getJSONObj() for x in self.dimensions]}
        
        return jo
        
    def __str__(self):
        s = "datasetId: " + self.datasetId
        s += "factTable: " + self.factTable
        s += ", factAbbrev: " + self.factAbbrev
        return s
    

class Dimension(object):
    def __init__(self):
        self.table = ""
        self.abbrev = ""
        self.joinCond = ""

    def getJSONObj(self):
        jo = {"table": self.table,
              "abbrev": self.abbrev,
              "joinCond": self.joinCond}
        return jo
        
    def __str__(self):
        s = "table: " + self.table
        s += ", abbrev: " + self.abbrev
        s += ", joinCond: " + self.joinCond
        return s
    
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
        
