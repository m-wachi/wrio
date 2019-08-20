

class MainTbl(object):
    def __init__(self):
        self._jsonObj = {"id": 45, "name": 'Bye Bye Keen!'}

    def getJSONObj(self):
        return self._jsonObj
    

class DtSrc(object):
    def __init__(self):
        self.factTable = ""
        self.factAbbrev = ""
        self.dimensions = []

    def __str__(self):
        s = "factTable: " + self.factTable
        s += ", factAbbrev: " + self.factAbbrev
        return s

class Dimension(object):
    def __init__(self):
        self.table = ""
        self.abbrev = ""
        self.joinCond = ""

    def __str__(self):
        s = "table: " + self.table
        s += ", abbrev: " + self.abbrev
        s += ", joinCond: " + self.joinCond
        return s
    
