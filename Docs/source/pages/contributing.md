# Contributing

First off, thank you for considering contributing to AltUnityTester.


## Leave a review

It would help us enormously if you would send a review to [our email](mailto:alttester@altom.com).


## Did you find a bug?

Ensure the bug was not already reported by searching all issues.
If you’re unable to find an open issue addressing the problem, open a [new issue](https://github.com/alttester/AltTester-Unity-SDK/issues/new?assignees=&labels=bug&template=bug-report.md&title=).

**When you ask a question about a problem you will get a much better/quicker answer if you provide a code sample that can be used to reproduce the problem.**

Try to:

* Use as little code as possible that still produces the same problem.
* Provide all parts needed to reproduce the problem (code and model if needed).
* Test the code you’re about to provide to make sure it reproduces the problem.


## How to suggest a feature or enhancement?

If you find yourself wishing for a feature that doesn’t exist in AltUnityTester:

* Ensure the enhancement was not already reported by searching all issues.
* Open a [new issue](https://github.com/alttester/AltTester-Unity-SDK/issues/new?assignees=&labels=&template=feature-request.md&title=). Be sure to include a clear description of the feature you would like to see, as much relevant information as possible:
  * Why you need it?
  * How should it work?


## Contributing changes

When you create a merge request take in consideration the following:

* Respect the project structure
* If it is a new feature like a new command try to make it for all three languages (Java, C# and Python) and also add tests
* If it is a bugfix then write a test to show that the bug is no longer reproducible


### Preparing your Fork

* Click ‘Fork’ on GitHub, creating e.g. `yourname/alttester-unity-sdk`.
* Clone your project: `git clone git@github.com:yourname/altester-unity-sdk`.
* `cd alttester-unity-sdk`

For a more detailed tutorial check out the [GitHub Documentation](https://docs.github.com/en/get-started/quickstart/contributing-to-projects)


### Preparing a Merge Request

After forking a project and applying your local changes, complete the following steps to create a merge request from your fork to contribute back to the main project:

* Go to **Projects > Your Projects** and select your fork of the repository.
* In the left menu, go to **Merge Requests**, and click **New Merge Request**.
* In the *Source branch* drop-down list box, select your branch in your forked repository as the source branch.
* In the *Target branch* drop-down list box, select the `master` branch from the `altunitytester` repository as the target branch.
* Assign a user to review your changes, and click **Submit merge request**.

For a more detailed tutorial check out the [GitLab Documentation](https://docs.gitlab.com/ee/user/project/merge_requests/creating_merge_requests.html#when-you-work-in-a-fork).
