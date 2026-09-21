# Test reports with Allure

## Generate testing reports using Allure

### NUnit
#### Prerequisites

1. Allure installed on your system:
    - **Windows**: [Scoop installation](https://docs.qameta.io/allure/#_windows) or [Manual installation](https://docs.qameta.io/allure/#_manual_installation).
    - **MacOS**: use the following command in your terminal `brew install allure`.
2. NUnit project where all the test classes belong to a certain **namespace**.
3. (Not a must) VS Code installed with the [Live Server extention](https://marketplace.visualstudio.com/items?itemName=ritwickdey.LiveServer).

`*` surely you can use any other IDE if it has these features.
#### Setup

1. Add the Allure NUnit package to your project:
    ```
    dotnet add package Allure.NUnit --version 2.9.5-preview.1
    ```
* Other versions: https://www.nuget.org/packages/Allure.NUnit/
2. Create two folders called `allure-report` and `allure-results` under your project.
3. Add an `allureConfig.json` file at the following path `/bin/Debug/netcoreappX` (where X is the version of your dotnet)
    - Config file [example](https://github.com/allure-framework/allure-csharp/blob/main/Allure.NUnit.Examples/allureConfig.json).
    - the value of the `directory` property should be the full path to the `allure-results` previously created folder.
4. In the tests files, import the AllureNUnit adapter `using NUnit.Allure.Core`.
5. Use the attribute `[TestFixture]` and the `[AllureNUnit]` under it. 
    - Additionally, you can add more attributes that increase the diversity of your report. See more examples [here](https://github.com/allure-framework/allure-csharp/tree/main/Allure.NUnit.Examples).

#### How to run the tests to obtain an Allure report

1. Execute tests to generate the output in the `allure-results` folder by using the command:
    ```
    dotnet test --results-directory allure-results
    ```
2. Generate a report in the `allure-report` folder:
    ```
    allure generate allure-results -o allure-report
    ```
#### How to check the results

- Using VS Code and Live Server:
    - Navigate to the `allure-report` folder and open the `index.html` file with [Live server](https://www.alphr.com/vs-code-open-with-live-server/).
- Using an allure command:
    ```
    allure serve allure-results
    ```
This command will generate a new report but not in a specific output. To find the report's location, check the terminal output and there will be a message like `Report successfully generated to PATH` where the path is the report's location.

#### Examples
- [EXAMPLES-CSharp-AllureNUnit-AltTrashCat](https://github.com/alttester/EXAMPLES-CSharp-AllureNUnit-AltTrashCat).

More details related to Allure can be found at the official [Allure documentation](https://docs.qameta.io/allure/).

### Pytest
#### Prerequisites

1. Allure installed on your system:
    - **Windows**: [Scoop installation](https://docs.qameta.io/allure/#_windows) or [Manual installation](https://docs.qameta.io/allure/#_manual_installation).
    - **MacOS**: use the following command in your terminal `brew install allure`.
2. Pytest installed (`pip install pytest`)

#### Setup

1. Add the allure-pytest dependency to your project:
    ```
    pip install allure-pytest
    ```
2. Create a folder called `allure-report` by using the following command in your terminal:
    ```
    allure generate
    ```

#### How to run the tests to obtain an Allure report

1. Execute tests to generate the output in the `allure-report` folder by using the command:
    ```
    pytest -v --alluredir=allure-report/ test_suite.py
    ```
2. For viewing the allure report use the following command after the previous:
    ```
    allure serve allure-report/
    ```
#### How to obtain a single html report

In order to obtain a single html file with the whole report, you should use `allure-combine`. Please follow the steps:
1. Install allure-combine using the following command in your terminal:
    ```
    pip install allure-combine
    ```

2. Generate a non-combined report by using the follosing command:
    ```
    allure generate -c allure-report -o allure-results-html
    ```
3. Generate a single html file with the whole report:
    ```
    allure-combine ./allure-results-html
    ```

`!` For MacOS, you should replace `pip` with `pip3`.
The name of the combined report is `combined.html` and it is under `allure-results-html` folder.

#### Examples
- [EXAMPLES-Python-Standalone-AltTrashCat](https://github.com/alttester/EXAMPLES-Python-Standalone-AltTrashCat).

More details related to Allure can be found at the official [Allure documentation](https://docs.qameta.io/allure/).

### Java
#### Prerequisites

1. Allure installed on your system:
    - **Windows**: [Scoop installation](https://docs.qameta.io/allure/#_windows) or [Manual installation](https://docs.qameta.io/allure/#_manual_installation).
    - **MacOS**: use the following command in your terminal `brew install allure`.
2. Maven installed on your system:
    - **Windows**: [Guide from official Maven docs](https://maven.apache.org/install.html).
    - **MacOS**: `brew install maven`.
3. (Not a must) Allure-combine python package: `pip install allure-combine`.

#### Updating the `pom.xml`
1. Update the properties section of your `pom.xml` with the following info:
    ```
    <properties>
        <project.build.sourceEncoding>UTF-8</project.build.sourceEncoding>
        <junit.version>4.13.2</junit.version>
        <allure.junit4.version>2.14.0</allure.junit4.version>
        <maven.compiler.plugin.version>3.5.1</maven.compiler.plugin.version>
        <maven.compiler.source>11</maven.compiler.source>
        <maven.compiler.target>11</maven.compiler.target>
        <aspectj.version>1.9.6</aspectj.version>
        <maven-surefire-plugin-version>3.0.0-M5</maven-surefire-plugin-version>
    </properties>
    ```
2. Update the build section of your `pom.xml` with the following info:
    ```
    <build>
            
        <plugins>
    <!-- Compiler plug-in -->
    
            <plugin>
                    <groupId>org.apache.maven.plugins</groupId>
                    <artifactId>maven-compiler-plugin</artifactId>
                    <version>${maven.compiler.plugin.version}</version>
                    <configuration>
                        <source>${maven.compiler.source}</source> <!--For JAVA 8 use 1.8-->
                        <target>${maven.compiler.target}</target> <!--For JAVA 8 use 1.8-->
                    </configuration>
                </plugin>
                
        <!-- Added Surefire Plugin configuration to execute tests -->       
            <plugin>
                <groupId>org.apache.maven.plugins</groupId>
                <artifactId>maven-surefire-plugin</artifactId>
                <version>${maven-surefire-plugin-version}</version>
                <configuration>
                    <testFailureIgnore>false</testFailureIgnore>
                    <argLine>
                        -javaagent:"${settings.localRepository}/org/aspectj/aspectjweaver/${aspectj.version}/aspectjweaver-${aspectj.version}.jar"
                    </argLine>
                    <properties>
                        <property>
                            <name>listener</name>
                            <value>io.qameta.allure.junit4.AllureJunit4</value>
                        </property>
                    </properties>
                </configuration>
                <dependencies>
                    <dependency>
                        <groupId>org.aspectj</groupId>
                        <artifactId>aspectjweaver</artifactId>
                        <version>${aspectj.version}</version>
                    </dependency>
                </dependencies>
            </plugin>
        </plugins>
    </build>
    ```
3. Include in your dependecies section the following items:
    ```
    <dependencies>
            <dependency>
                <groupId>com.alttester</groupId>
                <artifactId>alttester</artifactId>
                <version>2.3.2</version>
            </dependency>
            <dependency>
                <groupId>junit</groupId>
                <artifactId>junit</artifactId>
                <version>${junit.version}</version>
                <scope>test</scope>
            </dependency>
            <dependency>
            <groupId>io.qameta.allure</groupId>
            <artifactId>allure-junit4</artifactId>
            <version>${allure.junit4.version}</version>
            <scope>test</scope>
        </dependency> 
        </dependencies>
    ```
For more information, check [QA Automation expert tutorial](https://qaautomation.expert/2021/08/06/integration-of-allure-report-with-selenium-and-junit4/).
#### How to run the tests to obtain an Allure report

1. Execute tests to generate the output in the `allure-results` folder by using the command:
    ```
    mvn test
    ```
2. For viewing the allure report use the following command after the previous:
    ```
    allure serve allure-results
    ```
#### How to obtain a single html report

In order to obtain a single html file with the whole report, you should use `allure-combine` which is a **python** package. Please follow the steps:
1. Install allure-combine using the following command in your terminal:
    ```
    pip install allure-combine
    ```

2. Generate a non-combined report by using the follosing command:
    ```
    allure generate -c allure-results -o allure-results-html
    ```
3. Generate a single html file with the whole report:
    ```
    allure-combine ./allure-results-html
    ```
`!` For MacOS, you should replace `pip` with `pip3`.
The name of the combined report is `combined.html` and it is under `allure-results-html` folder.
#### Examples
- [EXAMPLES-Java-Standalone-and-Android-AltTrashCat](https://github.com/alttester/EXAMPLES-Java-Standalone-and-Android-AltTrashCat).

More details related to Allure can be found at the official [Allure documentation](https://docs.qameta.io/allure/).
