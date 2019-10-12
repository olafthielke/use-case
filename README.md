# use-case

This solution comprises two projects: BusinessLogic and WebApi.

The primary purpose of the BusinessLogic project is to house the RegisterCustomerUseCase 
class. This class acts as an example for simple high-level business logic ought to look in 
an object-oriented language like C#. Please note that this use case class or any code in 
the BusinessLogic project refer to anything web, service or database-specific. This project 
contains only pure unadulterated business logic.

The RegisterCustomerUseCaseTests.cs file contains the readily readable unit tests of the 
use case functionality. As always, unit tests should be more readable and understandable
than even the implementation code since they convey the business requirements.

The other project, WebApi, is a project that demonstrates how to plug in and use the 
RegisterCustomerUseCase class. All actual heavy lifting with regards to business logic is 
deferred to the use case class and the Register controller method in CustomerController
only performs simple validation, conversion and calling the use case method.

The WebApi project should be runnable right away. Via Postman (or similar testing HTTP 
client) a customer's registration details can be POSTed to the Register method which will, 
if validation passes, put the new customer in the InMemoryCustomerRepository. 

TODO: Demonstrate with a second and even third client project (Windows Forms, console app, 
etc.) how the RegisterCustomerUseCase class can simply be plugged into any of these without
having tochange any code inside of the use case class. 
