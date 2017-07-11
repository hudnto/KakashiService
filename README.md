# KakashiService
A service that clones other services. 
This project is for everyone that depends on a service that keep falling or wants to cache a service.
(Inspired in the ninja Hatake Kakashi from Naruto)

## Main Modules
### 1 - Read Module
Module responsible for reading the endpoint and understanding the types, complex types, operations and returns

### 2 - Create Module
Module responsible for creating the new project with desired endpoint, using cache and database connections.

### 3 - Build Module
Module responsible for building and starting the new endpoint, so it became online.

## Step-by-step

### Get Kakashi Service
Download or clone the repository. 

### Open with Visual Studio 2015 in Administrator Mode
Kakashi Service will work only on visual studio 2015. You need to open in Administrator Mode because there are features that will only work this way.

### Change Web.config paths
Change the following paths in the KakashiService.Web/Web.config to your own configuration:
- msbuildPath: path for your MSBuild. Usually there is no need to change this.
- iisPath: folder where your clone services will put their binaries and contract service. For each service, there will be a folder with his name.
- svcutilPath: path of your SvcUtil. This changes acordding to your framework installed, so check the folder before start.

### Install Redis (NoSQL Database)
Install Redis using the following link: https://github.com/MicrosoftArchive/redis/releases version 3.0.504. Redis is installed on windows as a service. Make sure it is running before using Kakashi Service.


### Publish to IIS and Go
Kakashi Service won't work correctly on IIS Express because it creates others instance. If you want to debug and test feature, put breakpoint before the Build Module. To use all features, deploy to you local IIS. After the deploy, open the website and use it.


