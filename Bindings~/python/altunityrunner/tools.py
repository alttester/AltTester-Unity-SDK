def proofread_json(input_str):
    """ Currently server sends poor JSON responses, which cannot be parsed due to typos.
    This function corrects that behaviour"""
    input_str = input_str.replace("True", 'true')  # Server sends "True", which is bad JSON
    input_str = input_str.replace("False", 'false')  # Server sends "False", which is bad JSON
    # More of this to come in case there is more unknown typing errors
    return input_str
