# Agenda Web API

A complete guideline for accessing the APIs

[[_TOC_]]

## Getting Started

API uses ```REST``` (Representational State Transfer) which is an architectural style that defines a set of constraints and properties based on HTTP.

The document will walk you through. The API is composed of several endpoints/modules to follow.

### Authentication

#### Login

Authenticates the client and returns token needed for the subsequent requests

| Method | URL |
|---|---|
| POST | [host]/signin |

**Request Body**

(*) Required

| Field | DataType | Desc |
|---|---|---|
| Email* | string | Email address |
| Password* | string | Password |

Sample

```json
{
	"email" : "a@y.com",
	"password" : "notyouraveragepassword"
}
```

**Response**

```json
eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6NzQ1LCJleHAiOjE1MjUzMDk1MjB9.77vz7s1Ai-SvOM6Xku61qVp-WyTCx4wk9bJlJ0aHipk
```

The token needed for the subsequent requests

### Account

#### Change Password

Updates client's password

| Method | URL |
|---|---|
| POST | [host]/account/changepassword |

A token must be present in ```Authorization``` header to proceed.

**Request Body**

(*) Required

| Field | DataType | Desc |
|---|---|---|
| CurrentPassword* | string | Current user password |
| NewPassword* | string | New password |
| ReEnterNewPassword* | string | New password |

Sample

```json
{
	"currentPassword" : "password123",
	"newPassword" : "password1",
	"reEnterNewPassword" : "password1"
}
```

**Response**

```
200 HTTP Status Code
```

### Member

#### Find

Searches members

| Method | URL |
|---|---|
| POST | [host]/member/find |

**Request Body**

(*) required

| Field | DataType | Description |
|--------|--------|---|
| Name | string | Complete name of member (FirstName LastName) |
| Email | string | Email |
| IsActive | boolean | Active or inactive |

A token must be present in ```Authorization``` header to proceed.

Sample

```json
{
	"name" : "ALVIN CHRISTIAN",
	"isActive" : true
}
```

**Response**

```json
[
    {
        "id": 1484,
        "name": "ALVIN CHRISTIAN T. CHUA",
        "nickName": "",
        "address": "Mandaluyong City",
        "mobile": "",
        "email": "actchua@periapsys.com",
        "birthDate": null,
        "remarks": "",
        "civilStatus": 2,
        "gender": 3,
        "invitedBy": 0,
        "isActive": true,
        "dateCreated": "2011-05-01T16:41:50",
        "createdBy": "admin",
        "dateModified": "2015-11-17T05:46:54.947",
        "modifiedBy": "admin",
        "communityId": 1,
        "endUser": null,
        "attendance": [],
        "groupMember": [],
        "registrant": []
    },
    {
        "id": 1484,
        .
        .
        .
        .
]
```

Returns array for members

#### Add

Adds a member which also becomes a user when ```email``` is present

| Method | URL |
|--------|--------|
| POST | [host]/api/member/new |

A token must be present in ```Authorization``` header to proceed.

**Request Body**

(*) required

| Field | DataType | Description |
|--------|--------|---|
| Name* | string | Complete name of member |
| NickName | string | Nick name of member |
| Address | string | Address of member |
| Mobile | string | Mobile |
| Email | string | Email |
| BirthDate | string (mm/dd/yyyy) | Birth date of member |
| Remarks | string | Remarks |
| CivilStatus | number (1 = Married; 2 = Single) | Civil status |
| Gender | number (3 = Male; 4 = Female) | Gender |

Sample

```json
{
	"name" : "ALVIN CHUA",
    "birthDate" : "04/01/1999",
	"email" : "actchua@periapsys.com"
}
```

**Response**

```
123
```

Returns the ```MemberId```; an email regarding authentication will be sent if the member becomes a user

#### Edit

Updates a member

| Method | URL |
|--------|--------|
| POST | [host]/api/member/edit |

A token must be present in ```Authorization``` header to proceed.

**Request Body**

(*) required

| Field | DataType | Description |
|--------|--------|---|
| MemberId* | number | ID of member |
| Name* | string | Complete name of member (FirstName LastName) |
| NickName | string | Nick name of member |
| Address | string | Address of member |
| Mobile | string | Mobile |
| Email | string | Email |
| BirthDate | string (mm/dd/yyyy) | Birth date of member |
| Remarks | string | Remarks |
| CivilStatus | number (1 = Married; 2 = Single) | Civil status |
| Gender | number (3 = Male; 4 = Female) | Gender |

Sample

```json
{
	"memberId" : 123,
	"name" : "ALVIN CHUA",
    "birthDate" : "04/01/2000",
	"email" : "actchua@periapsys.com"
}
```

**Response**

```
200 Status code
```

#### Get

Gets the detail of specific member

|Method|URL|
|---|---|
|GET|[host]/api/member/get/[memberId]/

A token must be present in ```Authorization``` header to proceed.

**Response**

```json
{
    "id": 26905,
    "name": "PEDRO PENDUCO",
    "nickName": null,
    "address": null,
    "mobile": null,
    "email": "b@y.com",
    "birthDate": null,
    "remarks": null,
    "civilStatus": null,
    "gender": null,
    "invitedBy": null,
    "isActive": true,
    "dateCreated": null,
    "createdBy": null,
    "dateModified": null,
    "modifiedBy": null,
    "communityId": 1,
    "endUser": null,
    "attendance": [],
    "groupMember": [],
    "registrant": []
}
```

### Event Category

#### Find

Searches event categories

|Method|URL|
|---|---|
|POST|[host]/api/eventcategory/find|

**Request Body**

(*) required

|Field|DataType|Description|
|---|---|---|
|Name|string|Name of the category|

A token must be present in ```Authorization``` header to proceed.

Sample

```json
{
	"name": "ANTIPOLO"
}
```

**Response**

```json
[
    {
        "id": 1038,
        "name": "ANTIPOLO Sunday Worship service",
        "events": 23,
        "minAttendees": 0,
        "averageAttendees": 42,
        "maxAttendees": 86
    },
    .
    .
    .
]
```

Returns an array of event categories

### Location

#### Find

Searches locations

|Method|URL|
|---|---|
|POST|[host]/api/location/find|

A token must be present in ```Authorization``` header to proceed.

**Request Body**

|Field|DataType|Desc|
|---|---|---|
|Name|string|Name of the location|

Sample

```json
{
	"name": "ANTIPOLO"
}
```

**Response**

```json
[
    {
        "id": 19,
        "name": "Antipolo City",
        "events": 23
    },
    .
    .
    .
]
```

Returns an array of locations

### Event

#### Find

Searches events

|Method|URL|
|---|---|
|POST|[host]/api/event/find|

A token must be present in ```Authorization``` header to proceed.

**Request Body**

(*) required

|Field|DataType|Description|
|---|---|---|
|Name|string|Name of event|
|EventCategoryId|number| ID of the event category |
|LocationID|number| ID of the location |
|DateTimeStart|string (mm/dd/yyyy hh:mm tt)| Event date/time |

Sample

```json
{
	"dateTimeStart":"05/06/2018"
}
```

**Response**

```json
[
    {
        "id": 12532,
        "eventCategoryId": 1,
        "category": "Special Event",
        "name": "Event1",
        "isActive": true,
        "dateTimeStart": "2018-05-06T00:00:00",
        "location": "GATEWAY",
        "attendance": 0,
        "isExclusive": null
    },
    .
    .
    .
]
```

Returns an array of events

#### Add

Adds an event

|Method|URL|
|---|---|
|POST|[host]/api/event/new|

A token must be present in ```Authorization``` header to proceed.

**Request Body**

(*) required

|Field|DataType|Description|
|---|---|---|
|Name*|string|Name of event|
|EventCategoryId*|number| ID of the event category |
|LocationID*|number| ID of the location |
|DateTimeStart*|string (mm/dd/yyyy hh:mm tt)| Event date/time |

Sample

```json
{
	"name": "Event1",
	"eventCategoryId": 1,
	"locationId": 1,
	"dateTimeStart": "05/06/2018 04:00 pm"
}
```

**Response**

```
123
```

Returns the ```EventId``` of the newly created event

#### Get

Gets the event detail

|Method|URL|
|---|---|
|GET|[host]/api/event/get/[eventId]|

A token must be present in ```Authorization``` header to proceed.

**Response**

Sample

```json
{
        "id": 12532,
        "eventCategoryId": 1,
        "category": "Special Event",
        "name": "Event1",
        "isActive": true,
        "dateTimeStart": "2018-05-06T00:00:00",
        "location": "GATEWAY",
        "attendance": 0,
        "isExclusive": null
    }
```

### Attendance

#### Add

Logs an attendee of an event

|Method|URL|
|---|---|
|POST|[host]/api/attendance/[eventId]/add|

A token must be present in ```Authorization``` header to proceed.

**Request Body**

(*) required

|Field|DataType|Description|
|---|---|---|
|EventID*|number|ID of the event|
|MemberID*|number|ID of the member|
|DateTimeLogged|string (mm/dd/yyyy hh:mm tt)| Event date/time |

```DateTimeLogged``` when empty will be defaulted to system-date.

Sample

```json
{
    "eventId" : 1,
    "memberId" : 123,
    "dateTimeLogged" : "05/31/2018 08:00 pm"
}
```

**Response**

```
200 Status Code
```

#### Delete

Deletes an attendee of an event

|Method|URL|
|---|---|
|POST|[host]/api/attendance/[eventId]/delete|

A token must be present in ```Authorization``` header to proceed.

**Request Body**

(*) required

|Field|DataType|Description|
|---|---|---|
|EventID*|number|ID of the event|
|MemberID*|number|ID of the member|

Sample

```json
{
    "eventId" : 1,
    "memberId" : 123
}
```

**Response**

```
200 Status Code
```

### Group Category

TBA

### Group

TBA

### Error Responses

| HTTP Response | Reason |
|---|---|
| 400 | Bad Request; didn't meet the required params |
| 401 | Unauthorized |
| 403 | Forbidden; params resulted conflict |
| 412 | Precondition Failed; token has expired |
| 417 | Expectation Failed; cannot verify token |
| 500 | Internal server error; unhandled error |


