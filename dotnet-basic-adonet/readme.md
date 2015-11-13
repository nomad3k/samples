# The Cheese Shop

This repository contains the main source code for www.thecheeseshop.com.

## Installation

### The database

The database is located in the .\database directory, and can be managed using the [RoundHouse](https://github.com/chucknorris/roundhouse "RoundHousE") database migration tool.

There are three environments setup:

1. LOCAL - The development database
2. TEST - The test/QA environment **DO NOT USE - Provided as an example only**
3. PRODUCTION - The live environment **DO NOT USE - Provided as an example only**

To create the initial development database use the following command:

    .\database\deployment\LOCAL.DBDeployment.DropCreate.bat

The migration scripts can be found in the following location:

    .\database\db

Please note that this application has been configured to use a SQL Express instance installed locally on localhost\SqlExpress.  If this isn't the case on your system then toy will need to edit the above batch file and also the web.config.

### The application

This application has been constructed using Visual Studio 2013 Community Edition, so there may be version compatibility issues with different versions of Visual Studio and MSBuild.  

**WARNING:** This release has not been tested under any other systems due to time constraints.  Please assume that in a production environment more time would have been taken on this.

The application can be compiled from the command line by executing:

    .\build.bat

This will compile the solution, run all the tests and the produce a versioned Zip file in the Release directory.

### Installation

Copy the release zip file onto your webserver and extract the contents.

To install the live application onto the production enironment use the following command.

    .\install.ps1 PRODUCTION CheeseShop

To install it onto the test application onto the production environment use the following command.  **Illustration purposes only.  This has not been implemented.**

    .\install.ps1 PRODUCTION CheeseShopTest

**NOTE:** This assumes that you have already configured an IIS website, associated security and application pools, which is running under c:\inetpub\CheeseShop.  

**WARNING:** This installer has not been tested on systems external to the development environment due to time constraints.  Please assume that in a production environment more time would have been taken on this.