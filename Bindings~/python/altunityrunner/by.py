class By(object):
    NAME = 1
    TAG = 2
    LAYER = 3
    COMPONENT = 4
    ID = 5
    PATH = 6
    TEXT = 7

    @staticmethod
    def return_enum_string(value):
        if value == 1:
            return "NAME"
        elif value == 2:
            return "TAG"
        elif value == 3:
            return "LAYER"
        elif value == 4:
            return "COMPONENT"
        elif value == 5:
            return "ID"
        elif value == 6:
            return "PATH"
        elif value == 7:
            return "TEXT"
