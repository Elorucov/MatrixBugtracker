# MatrixBugtracker

A simple bug-tracker project created for demonstration in Matrix Academy. Inspired by [VK's bug-tracker](https://vk.com/testing). 

This is a platform where testers help developers and managers in the company find and fix problems in their products.

The company's __employees__ create and manage __products__ in the platform. __User__ as testers create __reports__ in the products. __Moderators__ checks the reports for the fact of reproducing the bug specified in the reports and change its status.

It is possible to leave __comments__ on reports, attach __files__ to reports and comments, and testers can mark other reports as "reproduced".

There is a notification system. Sending notifications by e-mail to testers has been implemented.

## Technologies and principles used

* ASP.NET 8 Web API
* Entity Framework Core
* Serilog
* AutoMapper
* FluentValidator
* _JWT_ and _refresh_ tokens
* DDD architecture
* Design following SOLID Principles
* Repository and Unit-of-Work pattern
* Soft-delete 