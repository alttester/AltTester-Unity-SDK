errors = ['ERROR', 'FAILED']


def test_re_check(request):
    with open(str(request.config.getoption("--file_to_log"))) as f:
        content = f.read().upper()
        for error in errors:
            if error in content:
                print(error)
                assert False