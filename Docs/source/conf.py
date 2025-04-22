# Configuration file for the Sphinx documentation builder.
#
# This file only contains a selection of the most common options. For a full
# list see the documentation:
# https://www.sphinx-doc.org/en/master/usage/configuration.html

# -- Path setup --------------------------------------------------------------

# If extensions (or modules to document with autodoc) are in another directory,
# add these directories to sys.path here. If the directory is relative to the
# documentation root, use os.path.abspath to make it absolute, like shown here.
#
# import sys
# sys.path.insert(0, os.path.abspath('.'))

import os
from recommonmark.parser import CommonMarkParser
from recommonmark.transform import AutoStructify
from pygments.lexers.robotframework import RobotFrameworkLexer

# -- Project information -----------------------------------------------------

copyright = '2025, Altom Consulting'
author = 'Altom'
project = 'AltTester® Unity SDK'

# Before creating a new tag, add the new one here and update 'latest' with the new SDK tag in index.rst line 44
# Currently there is no way to add in toctree a dynamic link based on the version.
# https://github.com/sphinx-doc/sphinx/issues/1836
# https://github.com/sphinx-doc/sphinx/issues/500

TAGS = ["1.8.1", "2.0.0", "2.0.1", "2.0.2",
        "2.0.3", "2.1.0", "2.1.1", "2.1.2", "2.2.0", "2.2.2", "2.2.4", "2.2.5"]
LATEST_VERSION = 'master'
BRANCHES = ['master']

smv_branch_whitelist = r'^master$'
smv_tag_whitelist = r'^\d+\.\d+\.\d+$'
smv_remote_whitelist = r'^.*$'

github_ref = os.getenv('GITHUB_REF_NAME')

if github_ref == 'development':
    smv_branch_whitelist = r'^development$'
    BRANCHES = ['development']
    LATEST_VERSION = 'development'

smv_latest_version = LATEST_VERSION
smv_rename_latest_version = 'latest'

version = TAGS[len(TAGS) - 1]
release = version

desktop_release_version = 'v.' + release
sdk_release_version = version.replace('.', '_')

alttester_sdk_docs_link = "https://alttester.com/docs/desktop/" + \
    desktop_release_version + "/%s"
alttester_unreal_docs_link = "https://alttester.com/docs/unreal-sdk/latest/%s"

extlinks = {
    "alttesterpage":                    ("https://alttester.com/%s", None),
    "alttesterdesktopdocumentation":    (alttester_sdk_docs_link, None),
    "alttesterunrealdocumentation":     (alttester_unreal_docs_link, None),
    "alttesteriphoneblog":              ("https://alttester.com/testing-ios-applications-using-java-and-altunity-tester/%s", None)
}

# -- General configuration ---------------------------------------------------

# Add any Sphinx extension module names here, as strings. They can be
# extensions coming with Sphinx (named 'sphinx.ext.*') or your custom
# ones.
extensions = ['sphinx.ext.autosectionlabel',
              'sphinx_markdown_tables',
              'sphinx_tabs.tabs',
              'sphinx_rtd_theme',
              'recommonmark',
              'sphinx_multiversion',
              'sphinx.ext.extlinks']


source_suffix = {'.rst': 'restructuredtext', '.md': 'markdown'}

# Add any paths that contain templates here, relative to this directory.
templates_path = ['_templates']

# List of patterns, relative to source directory, that match files and
# directories to ignore when looking for source files.
# This pattern also affects html_static_path and html_extra_path.
exclude_patterns = ['_build', 'Thumbs.db', '.DS_Store']

# If true, Sphinx will warn about all references where the target cannot
# be found.
nitpicky = True

# True to prefix each section label with the name of the document it is in,
# followed by a colon.
autosectionlabel_prefix_document = True


# -- Options for HTML output -------------------------------------------------

# The theme to use for HTML and HTML Help pages.  See the documentation for
# a list of builtin themes.
#
html_theme = 'sphinx_rtd_theme'

# Add any paths that contain custom static files (such as style sheets) here,
# relative to this directory. They are copied after the builtin static files,
# so a file named "default.css" will overwrite the builtin "default.css".
html_static_path = ['_static']


# -- RTD Theme ---------------------------------------------------------------

html_logo = '_static/logo/AltTester-512x512.png'
html_favicon = '_static/logo/AltTester-512x512.png'
html_css_files = [
    'css/custom.css',
]
html_js_files = [
    'js/custom.js',
    ('https://cdn.usefathom.com/script.js',
     {'defer': 'defer', 'data-site': 'RTZVKYOQ'})
]
html_theme_options = {
    'collapse_navigation': False,
    'navigation_depth': 5
}


class AltTesterRobotFrameworkLexer(RobotFrameworkLexer):
    name = 'Robot'
    aliases = ['robot']
    mimetypes = ['text/x-robotframework']


def setup(app):
    app.add_transform(AutoStructify)
    app.add_lexer('robot', AltTesterRobotFrameworkLexer)
