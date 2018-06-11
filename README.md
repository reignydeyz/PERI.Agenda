# Agenda

Just a simple event manager

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- [Visual Studio](https://www.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2016)
- [SQL Server Management Studio](https://msdn.microsoft.com/en-us/library/mt238290.aspx)
- [Node.js](https://nodejs.org)

## People to blame

The following personnel is/are responsible for managing this project.

- [actchua@periapsys.com](mailto:actchua@periapsys.com)

## Developer's Guide

You may need to understand the nature of ASP-MVC before going through the entire solution. Because of abstraction and frequent changing of schema, we choose MVC, for it suits best.

### Technology Used

- [MVC Core](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/?view=aspnetcore-2.1)
- [.Net Core 2.0](https://www.microsoft.com/net/download/windows)
- [.Net Standard 2.0](#)
- [EntityFramework Core](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.Net Identity](https://www.asp.net/identity)
- [Angular 4](https://angular.io)
- [Chart.js](https://www.chartjs.org/)
- [XUnit](https://xunit.github.io)

### Solution Structure

- PERI.Agenda.BLL
	- The Business Layer of the system
- PERI.Agenda.Core
	- Common functions
- PERI.Agenda.EF
	- Contains the EntityFramework module
	- All of the data-manipulations were done here
- PERI.Agenda.Repository
    - Repository pattern
- PERI.Agenda.Test
    - Unit test project
- PERI.Agenda.Web
	- The main project

### Database

1. Create a database using SSMS.
2. Execute the ff accordingly
   - 01 - AARS tables.sql
   - 02 - AARS.LookUp defaults.sql
   - 10 - Agenda schema.sql
   - 11 - Agenda tables.sql
   - 12 - Agenda.Role defaults.sql

### Application Setting

1. Create ```appsettings.json``` that contains your connection. Follow the format below and edit the parameter/s accordingly:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Trace",
      "Microsoft": "Information"
    }
  },

  "ConnectionStrings": {
    "DefaultConnection": "[CONNECTION_STRING]"
  },

  "JWT": {
    "Secret": "[SECRET_KEY]"
  },
  
  "SmtpClient": {
    "DisplayName": "[DISPLAY_NAME]",
    "DisplayEmail": "[EMAIL]",
    "SmtpServer": "[SMTP_SERVER]",
    "SmtpPort": "[PORT]",
    "UseSsl": [true/false],
    "SmtpUser": "[SMTP_USERNAME]",
    "SmtpPassword": "[SMTP_USER_PASSWORD]"
  },

  "GoogleReCaptcha": {
    "PublicKey": "[YOUR_GOOGLE_RECAPTCHA_PUBLIC_KEY]",
    "PrivateKey": "[YOUR_GOOGLE_RECAPTCHA_PRIVATE_KEY]"
  }
}

```

2. Place the setting inside ```PERI.Agenda.Web``` & ```PERI.Agenda.Test``` project folders.

### Unit Test

You can test the business logics in the project,```PERI.Agenda.Test```. A good knowledge in performing unit testing is required, especially in XUnit.

#### Configuration

The ```appsettings.json``` must be placed in the project folder. In ```Configuration Properties``` make sure ```Copy to Output Directory``` is set to ```Copy if newer```.

#### Sample Data

Sample data can be found in ```TestDataGenerator.cs``` and is used by several test classes.

### Tips

- ```Community``` is an organization or main group that is required in sign-up page. Because the system supports multiple parties, it is important to identify the ```Community``` when signing-up.

- After generating all the DB scripts, you need to add a default ```Community```. To do this, open DB via SSMS and look for ```Community``` table and manually add your default community.

- The newly registered member will have the role, ```User```.

- Because there's no way to create ```Admin``` at first, you need to use SSMS and manually edit your role after signing-up. You can see your account info in ```prompt.EndUser``` table. Change the ```RoleId``` to ```1``` to make your account admin.
