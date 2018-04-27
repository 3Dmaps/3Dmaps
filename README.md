# 3Dmaps
3DMaps in Augmented Reality. Software production project course / Univ. of Helsinki, spring 2018. The project has two repositories. This repository contains the main software (Unity, C#). The [mapcreator](https://github.com/3Dmaps/mapcreator) repository contains the source data processing tool (Python).

## Project background information and definition

In Labtool: https://studies.cs.helsinki.fi/ohtuprojekti/topic_descriptions/199

## Backlogs

In Google sheets: https://docs.google.com/spreadsheets/d/15aIlJD48ZQKQ7nGFM40B4Lvwt3_bHjMlxDbJSRVtRH8

## Project notes

In Google drive: https://drive.google.com/drive/folders/1dDglKWNlMnkj2in2mZXfWaNzyCIiN0KJ

## Definition of Done

In Google sheets: https://docs.google.com/spreadsheets/d/15aIlJD48ZQKQ7nGFM40B4Lvwt3_bHjMlxDbJSRVtRH8/edit#gid=2061537297

## Workflow
Basic workflow in Google sheets: https://docs.google.com/spreadsheets/d/15aIlJD48ZQKQ7nGFM40B4Lvwt3_bHjMlxDbJSRVtRH8/edit#gid=1868766105

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

* Android
Requirements: Unity 3D, Android device
Open the project in Unity 3D and build the project for Android. The resulting file can then be transferred to Android.

* iOS
Requirements: Unity 3D, iOS device, Macintosh computer, Xcode, Apple ID / Apple Developer ID
Connect the iOS device to the computer. Open the project in Unity 3D and build the project for iOS. Open the resulting package in Xcode, build the application using Apple ID or Apple Developer ID for the connected iOS device. The application will be transferred to the computer.



## Built With

* [Unity](https://unity3d.com/) - Game engine
* [C#/.NET](https://www.microsoft.com/net/)

## Versioning

Versioning. *Not defined yet.*

## Authors

* **Julius Laitala**
* **Ollipekka Väänänen**
* **Sami Ollila**
* **Mikko Kotola**
* **Lauri Mäntylä**
* **Jussi Rintala**

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments
University of Helsinki
