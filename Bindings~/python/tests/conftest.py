def pytest_addoption(parser):
    parser.addoption(
        "--file_to_log", action="store", default=None,
        help="The path to the output file of the browserstack sdk command"
    )
