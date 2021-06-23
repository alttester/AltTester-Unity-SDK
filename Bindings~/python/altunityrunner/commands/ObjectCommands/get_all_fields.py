import json

from loguru import logger

from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.tools import proofread_json


class GetAllFields(BaseCommand):

    def __init__(self, socket, request_separator, request_end, alt_object, component):
        super(GetAllFields, self).__init__(socket, request_separator, request_end)
        self.alt_object = alt_object
        self.component = component

    def simplify(self, list_of_fields):
        """ Parse list of dicts [{'name': 'm_Sprite', 'value': 'Button (UnityEngine.Sprite)'},...] to
        [{'m_Sprite':'Button (UnityEngine.Sprite)'...}"""

        fields = {}
        for item in list_of_fields:
            try:
                correct_json = proofread_json(item.get("value"))
                fields[item.get("name")] = json.loads(correct_json)
            except json.decoder.JSONDecodeError:
                fields[item.get("name")] = item.get("value")

        return fields

    def execute(self):
        if isinstance(self.component, dict):
            if bool(self.component.get('assemblyName', None)) & bool(self.component.get("componentName", None)):
                # I actually can't find where to put this. Documentations isn't helping.
                alt_assembly_name = self.component.get('assemblyName')  # noqa: F841
                alt_component_name = self.component.get('componentName')
        elif isinstance(self.component, str):
            alt_component_name = self.component
        else:
            logger.error("Component supplied: {} \nis missing something.".format(self.component))
            raise ValueError("Component supplied is missing something.")

        component_json = json.dumps({"componentName": alt_component_name})
        data = self.send_command("getAllFields", str(self.alt_object.id), component_json, "ALLFIELDS")

        try:
            parsed_data = json.loads(data)
            return self.simplify(parsed_data)
        except json.JSONDecodeError:
            logger.exception("Cannot parse the {}, this is not JSON.".format(data))
            raise
