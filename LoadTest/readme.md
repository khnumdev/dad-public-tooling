# Load testing tool

**NOTE** This tool is based on the work of https://github.com/MicrosoftDocs/mslearn-hotel-reservation-system/tree/master/src/HotelReservationSystemTestClient

The purpose of the tool is to raise several queries to stress the site.

## Usage

In `Program.cs` there are two variables that should be modified to run the application:

```cs
private static string endpoint = "";
private static int workers = 100;
```

- Endpoint: Should be something like https://mywebapp.azurewebsites.net/
- Workers: Can be an integer from 1 to N. That depends on your machine.