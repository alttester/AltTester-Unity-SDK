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


# -- Project information -----------------------------------------------------

from recommonmark.parser import CommonMarkParser
from recommonmark.transform import AutoStructify


project = 'AltUnity Tools'
copyright = '2020, Altom Consulting'
author = 'Altom'

# The full version, including alpha/beta/rc tags
# displays version under project title
version = 'AltUnity Tester v.1.7.0-alpha'
release = 'v.1.7.0-alpha'


# -- General configuration ---------------------------------------------------

# Add any Sphinx extension module names here, as strings. They can be
# extensions coming with Sphinx (named 'sphinx.ext.*') or your custom
# ones.
extensions = [
    'sphinx_markdown_tables',
    'sphinx_tabs.tabs',
    'sphinx_rtd_theme',
    'recommonmark'
]

source_suffix = {'.rst': 'restructuredtext', '.md': 'markdown'}


# Add any paths that contain templates here, relative to this directory.
templates_path = ['_templates']

# List of patterns, relative to source directory, that match files and
# directories to ignore when looking for source files.
# This pattern also affects html_static_path and html_extra_path.
exclude_patterns = ['_build', 'Thumbs.db', '.DS_Store']


def setup(app):
    app.add_transform(AutoStructify)


# -- Options for HTML output -------------------------------------------------


# The theme to use for HTML and HTML Help pages.  See the documentation for
# a list of builtin themes.
#
html_theme = 'sphinx_rtd_theme'

# Add any paths that contain custom static files (such as style sheets) here,
# relative to this directory. They are copied after the builtin static files,
# so a file named "default.css" will overwrite the builtin "default.css".
html_static_path = ['_static']

html_logo = '_static/images/altUnity-512x512.png'
html_favicon = '_static/images/altUnity-512x512.png'
html_title = 'AltUnity Tester Documentation'

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
