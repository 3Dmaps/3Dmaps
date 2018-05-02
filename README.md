# 3Dmaps
3DMaps in Augmented Reality. Software production project course / Univ. of Helsinki, spring 2018. The project has two repositories. This repository contains the main software (Unity, C#). The [mapcreator](https://github.com/3Dmaps/mapcreator) repository contains the source data processing tool (Python).

## Project definition

The goal of the project is to create a 3D visualisation component for a smartphone hiking app that shows hiking routes in national parks. 3DMaps generates a 3D model of a terrain, adds a satellite image on it as a texture and adds routs and points of interest on top of the model. The user can then explore the model by using gestures on the phone screen.

Background information in Labtool: https://studies.cs.helsinki.fi/ohtuprojekti/topic_descriptions/199

## What the software does (in a nutshell)
**Read source data**

<img src="/images/Sample_satellite_image.jpg" width="480">
![Height map](/images/Sample_height_map.png =480x)
![Open Street Map data](/images/Sample_XML.png =480x)

**Preprocess source data in mapcreator**

![Mapcreator](/images/Sample_mapcreator.png =360x)

**Create a 3d-model on IOS/Android device**
*Image of 3dmap on iPhone here*

## How to use the application
You can pinch to zoom. The map moves depending where you move your finger. If you click point of interest icon the name of the point of interest will appear. Button tilt will enter to tilt mode. Button swap will change the map texture between satellite data and color map data. 
## Source data

3DMaps uses files generated by the [mapcreator](https://github.com/3Dmaps/mapcreator) console application, consisting of a height map in binary format, an XML file containing [Open Street Map data](https://github.com/3Dmaps/3Dmaps/blob/master/osm.md) and a satellite image in .png format.

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
Requirements: Unity 3D with Android module, Android device, computer with Android SDK installed.

Open the project in Unity 3D and build the project for Android. The resulting .apk file is either automatically transferred to the Android device or can be moved to the device manually. The Android device may need to have its developer settings activated for the application to be launched.

* iOS
Requirements: Unity 3D with iOS device, iOS device, Macintosh computer, Xcode, Apple ID / Apple Developer ID. 

Connect the iOS device to the computer. Open the project in Unity 3D and build the project for iOS. Open the resulting package in Xcode, select the connected iOS device as the build target and then build the application using Apple ID or Apple Developer ID for the device. The application will be transferred to the iOS device. To launch the application, you may need to accept the device developer ID in security settings.

## Built With

* [Unity](https://unity3d.com/) - Game engine
* [C#/.NET](https://www.microsoft.com/net/)

## Versioning

Version 1.0: Released May 2nd 2018.

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
