# 3Dmaps
3DMaps in augmented reality. Software production project course / Univ. of Helsinki, spring 2018

## Backlogs

Google sheets: https://docs.google.com/spreadsheets/d/15aIlJD48ZQKQ7nGFM40B4Lvwt3_bHjMlxDbJSRVtRH8

## Project notes

Google drive: https://drive.google.com/drive/folders/1dDglKWNlMnkj2in2mZXfWaNzyCIiN0KJ

## Definition of Done

Google sheets: https://docs.google.com/spreadsheets/d/15aIlJD48ZQKQ7nGFM40B4Lvwt3_bHjMlxDbJSRVtRH8/edit#gid=2061537297

## Installing

To get a development env running:
* Install the latest version of [Unity](https://unity3d.com/get-unity/download).
* Clone or download the master branch from the 3Dmaps project's github repository to a local directory
* Open the Unity project folder with Unity

### Prerequisites

* Unity currently only supports **Windows** and **Mac** environments.
* The project is written in C# (which is the main language supported by Unity). Install [.NET/C#](https://www.microsoft.com/net/download/) if the .NET-framework (including C#) is not already installed on your computer.
* Install an editor that supports C# and integrates with Unity, e.g. [Visual Studio 2017](https://www.microsoft.com/fi-fi/store/b/visualstudio)

## Running the tests

To locally run the automated tests, open the "Test runner" tab in Unity and click "Run all tests".

## Continuous integration

The project uses Unity Cloud Build as its continous integration platform. Unity Cloud BUild automatically builds and tests the following branches:
* Master - the master branch
* Staging - a staging branch used for used for building and testing a feature branch before final code review and merging into master.
* Sandbox - a sandbox branch that can freely be used for building and testing branches during development.

## Deployment

Add additional notes about how to deploy this on a live system. *Not defined yet.*

## Built With

Tool list.
* [Unity](https://unity3d.com/) - Game engine
* [C#/.NET](https://www.microsoft.com/net/)

## Versioning

Versioning. *Not defined yet.*

## Authors

* **Julius Laitala**
* **OlliPekka Väänänen**
* **Sami Ollila**
* **Mikko Kotola**
* **Lauri Mäntylä**
* **Jussi Rintala**

## Contributing

List of contributors.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments
University of Helsinki
