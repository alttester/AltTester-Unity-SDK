from AltTesterLibrary.AltTesterKeywords import AltTesterKeywords
from AltTesterLibrary.version import VERSION

__version__ = VERSION


class AltTesterLibrary(AltTesterKeywords):

    """ A keyword library for Robot Framework. It provides keywords for
        performing various operations using AltTester to interact with 
        Unity games and apps. See http://alttester.com/tools/
        for more details.

        This library uses the AltTester Driver Python Library from
            https://pypi.org/project/AltTester-Driver/

    """

    ROBOT_LIBRARY_SCOPE = 'GLOBAL'
    ROBOT_LIBRARY_VERSION = __version__
