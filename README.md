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
- [.Net Core 2.1](https://www.microsoft.com/net/download/windows)
- [.Net Standard 2.0](#)
- [EntityFramework Core](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.Net Identity](https://www.asp.net/identity)
- [Angular 4](https://angular.io)
- [Chart.js](https://www.chartjs.org/)

### Solution Structure

- PERI.Agenda.BLL
	- The Business Layer of the system
- PERI.Agenda.Core
	- Common functions
- PERI.Agenda.EF
	- Contains the EntityFramework module
	- All of the data-manipulations were done here
- PERI.Agenda.Web
	- The main project

### Database

TBA

### Application Setting

1.Create ```appsettings.json``` that contains your connection. Follow the format below and edit the parameter/s accordingly:

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

2.Place the setting inside ```PERI.Agenda.Web``` project.