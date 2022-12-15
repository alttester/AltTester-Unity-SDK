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
# import os
# import sys
# sys.path.insert(0, os.path.abspath('.'))

from recommonmark.transform import AutoStructify
from recommonmark.parser import CommonMarkParser


# -- Project information -----------------------------------------------------

project = 'AltTester'
copyright = '2022, Altom Consulting'
author = 'Altom'

# The full version, including alpha/beta/rc tags
# displays version under project title
version = 'AltTester Unity SDK v1.8.1'
release = 'v1.8.1'


# -- General configuration ---------------------------------------------------

# Add any Sphinx extension module names here, as strings. They can be
# extensions coming with Sphinx (named 'sphinx.ext.*') or your custom
# ones.
extensions = [
    'sphinx.ext.autosectionlabel',
    'sphinx_markdown_tables',
    'sphinx_tabs.tabs',
    'sphinx_rtd_theme',
    'recommonmark'
]

source_suffix = ['.rst', '.md']
source_parsers = {
    '.md': CommonMarkParser,
}


# Add any paths that contain templates here, relative to this directory.
templates_path = ['_templates']

# List of patterns, relative to source directory, that match files and
# directories to ignore when looking for source files.
# This pattern also affects html_static_path and html_extra_path.
exclude_patterns = ['Thumbs.db', '.DS_Store']

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
html_title = 'AltTester Unity SDK Documentation'

html_css_files = [
    'css/custom.css',
]

html_js_files = [
    'js/custom.js'
]

html_theme_options = {
    'collapse_navigation': False,
    'navigation_depth': 5
}


# -- AutoStructify options ---------------------------------------------------

def setup(app):
    app.add_transform(AutoStructify)
