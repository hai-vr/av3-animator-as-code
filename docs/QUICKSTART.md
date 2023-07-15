# Quick Start Guide
!> This documentation currently only applies to the use case of VRChat avatars! The system will still work for other use cases, but the documentation is not yet complete for those use cases.
### Terminology
Before reading the rest of this guide, it is important to understand the terminology used throughout the guide.
- **AAC**: The Animator As Code system itself.
- **AAC Repository**: The repository containing the Animator As Code system.
- **VCC**: The [VRChat Creator Companion](https://vcc.docs.vrchat.com/), which is used to launch Unity projects and manage project packages.
- **VRC**: VRChat, the game itself.

## Requirements
- Make sure you have the latest version of the [VRChat Creator Companion](https://vcc.docs.vrchat.com/) installed.
  - For Windows 10, this is the regular UI version.
  - For MacOS and Linux, the only available version that VRChat provides is the [CLI Version](https://vcc.docs.vrchat.com/vpm/cli)
- Avatar project is already migrated into the VCC.
  - This guide assumes you have already migrated your avatar project into the VCC. If you have not done this yet, please follow the [VCC Migration Guide](https://vcc.docs.vrchat.com/vpm/migrating) before continuing.

## Adding the AAC Repository to the VCC
The steps for loading the AAC repository into the VCC are different depending on the type of VCC install you are using. This process only needs to be done once, and will allow you to use the AAC system in any of your projects.
<!-- tabs:start -->
#### **GUI**
For the graphical based VCC, all you need to do is click [Add To VCC](vcc://vpm/addRepo?url=https%3A%2F%2Fwww.matthewherber.com%2Fav3-animator-as-code%2Findex.json). This will show a popup on your VCC asking you to confirm the installation of the repository. Click yes, and the repository will be available for use in any of your projects.

?> If you are having issues with the Add To VCC link not working, your VCC is likely out of date
#### **CLI**
For the CLI based VCC, you will need to run the following command in your terminal:
```bash
vpm add repo https://www.matthewherber.com/av3-animator-as-code/index.json
```
<!-- tabs:end -->

## Installing the AAC Package into a project
Once you have the AAC repository loaded into your VCC, you can now install the AAC package into one of your projects.  

!> IMPORTANT! Adding AAC to an existing project *may* update some of the other packages in that project to a newer version. This should not cause any issues, but if you feel inclined, now would be a good time to make a backup of your project.
<!-- tabs:start -->
#### **GUI**
Navigate to the Projects tab of the VCC, find your project, and click on the "Manage Projects" Button associated with it. This will open a window with all of the packages that your project currently has installed. Find or search for the package labelled "Animator As Code", and click the plus button to the right of it. This will install the AAC system into your project, along with any other dependencies that are required. You can now open the project and continue with the next steps.

#### **CLI**
Navigate to the root directory of your project in your terminal, and run the following command:
```bash
vpm add package com.happyrobot33.animatorascode
```
This will install the AAC system into your project, along with any other dependencies that are required. You can now open the project and continue with the next steps.
<!-- tabs:end -->
