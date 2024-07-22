"""
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
"""

from setuptools import setup

NAME = "alttester-robotframework-library"
DESCRIPTION = (
    "Robot Framework Library for the AltTester® framework. "
    "AltTester® is an open-source UI driven test "
    "automation tool that helps you find objects in your game "
    "and interacts with them."
)
URL = "https://alttester.com/docs/sdk/latest/"
EMAIL = "contact@alttester.com"
AUTHOR = "Altom Consulting"
REQUIRES_PYTHON = ">=3.4.0"
LICENSE = "GNU GPLv3"

with open("src/AltTesterLibrary/version.py") as f:
    for line in f.readlines():
        if "VERSION = " in line:
            VERSION = line.replace("VERSION = ", "") \
                          .replace('"', "") \
                          .replace("\n", "")

with open("requirements.txt") as f:
    REQUIRED = f.read().splitlines()

with open("README.md") as f:
    README = f.read()

setup(
    name=NAME,
    version=VERSION,
    description=DESCRIPTION,
    long_description=README,
    long_description_content_type="text/markdown",
    license=LICENSE,
    url=URL,
    project_urls={
        "Issues": "https://github.com/alttester/AltTester-Unity-SDK/issues",
        "Documentation": "https://alttester.com/docs/sdk/latest",
        "Source": "https://github.com/alttester/AltTester-Unity-SDK",
    },
    author=AUTHOR,
    author_email=EMAIL,
    zip_safe=False,
    python_requires=REQUIRES_PYTHON,
    setup_requires=REQUIRED,
    install_requires=REQUIRED,
    include_package_data=True,
    keywords="robot framework testing test automation alttester",
    package_dir={"": "src"},
    packages=["AltTesterLibrary"],
    classifiers=[
        "Development Status :: 5 - Production/Stable",
        "Intended Audience :: Developers",
        "Intended Audience :: Other Audience",
        "License :: OSI Approved :: GNU General Public License v3 (GPLv3)",
        "Programming Language :: Python :: 3.10",
        "Operating System :: OS Independent",
        "Topic :: Games/Entertainment",
        "Topic :: Software Development :: Libraries",
        "Topic :: Software Development :: Testing",
        "Topic :: Software Development :: Testing :: Acceptance",
        "Topic :: Software Development :: Testing :: Unit",
    ],
)
