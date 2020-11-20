from setuptools import setup
with open("altunityrunner/__version__.py") as version_file:
    VERSION = version_file.readline().replace("VERSION = ", "").replace("\"", "")

setup(name='altunityrunner',
      version=VERSION,
      description='Python Binding to allow Appium tests to be run against Unity games and apps using AltUnityTester',
      long_description='This package includes the Python bindings needed for tests to be run against Unity games and apps using AltUnityTester. \n\nFor more information, visit https://gitlab.com/altom/altunitytester',
      url='https://gitlab.com/altom/altunity/altunitytester',
      author='Altom Consulting',
      author_email='altunitytester@altom.fi',
      license='GNU General Public License v3.0',
      packages=['altunityrunner',
                'altunityrunner.commands',
                'altunityrunner.commands.FindObjects',
                'altunityrunner.commands.InputActions',
                'altunityrunner.commands.ObjectCommands',
                'altunityrunner.commands.UnityCommands'],
      zip_safe=False,
      install_requires=[
          'deprecated',
          'pure-python-adb',
          'loguru'
      ])
