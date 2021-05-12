from setuptools import setup, find_packages

NAME = 'altunityrunner'
DESCRIPTION = 'Python Binding to allow Appium tests to be run against Unity games and apps using AltUnityTester.'
URL = 'https://gitlab.com/altom/altunity/altunitytester'
EMAIL = 'altunitytester@altom.fi'
AUTHOR = 'Altom Consulting'
REQUIRES_PYTHON = '>=3.4.0'
LICENSE = 'GNU GPLv3'


with open("altunityrunner/__version__.py") as f:
    VERSION = f.readline().replace("VERSION = ", "").replace("\"", "").replace("\n", "")


with open('requirements.txt') as f:
    REQUIRED = f.read().splitlines()


with open('README.md') as f:
    README = f.read()


setup(
    name=NAME,
    version=VERSION,
    description=DESCRIPTION,
    long_description=README,
    long_description_content_type='text/markdown',
    license=LICENSE,
    url=URL,
    project_urls={
        "Bug Tracker": "https://gitlab.com/altom/altunity/altunitytester/-/issues",
        "Documentation": "https://altom.gitlab.io/altunity/altunitytester",
        "Source": "https://gitlab.com/altom/altunity/altunitytester",
    },

    author=AUTHOR,
    author_email=EMAIL,

    zip_safe=False,
    python_requires=REQUIRES_PYTHON,
    setup_requires=REQUIRED,
    install_requires=REQUIRED,
    packages=find_packages(exclude=['tests']),
    include_package_data=True,

    classifiers=[
        "Development Status :: 5 - Production/Stable",

        "Intended Audience :: Developers",
        "Intended Audience :: Other Audience",

        "License :: OSI Approved :: GNU General Public License v3 (GPLv3)",

        "Programming Language :: C#",
        "Programming Language :: Java",
        "Programming Language :: Cython",
        "Programming Language :: Python :: Implementation",
        "Programming Language :: Python :: Implementation :: CPython",
        "Programming Language :: Python :: 3 :: Only",
        "Programming Language :: Python :: 3.4",
        "Programming Language :: Python :: 3.5",
        "Programming Language :: Python :: 3.6",
        "Programming Language :: Python :: 3.7",
        "Programming Language :: Python :: 3.8",
        "Programming Language :: Python :: 3.9",

        "Operating System :: OS Independent",

        "Topic :: Games/Entertainment",
        "Topic :: Software Development :: Libraries",
        "Topic :: Software Development :: Testing",
        "Topic :: Software Development :: Testing :: Acceptance",
        "Topic :: Software Development :: Testing :: Unit",
    ],
    keywords="unity testing tests",
)
