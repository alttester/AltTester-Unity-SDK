import os
errors = ['ERROR', 'FAILED']


def test_file_not_empty(request):
    check_file = os.path.getsize(str(request.config.getoption("--file_to_log")))
    assert check_file != 0


def test_file_does_not_contains_errors(request):
    with open(str(request.config.getoption("--file_to_log"))) as f:
        content = f.read().upper()
        for error in errors:
            if error in content:
                print("The tests on BrowserStack failed >>>>>>>>>>> " + error)
                assert False
