# Bondora developer home assignment

## 1. Objective

Assess skills in OO analysis, system design and .Net programming.  
The task should be completed by agreed term and sent to Bondora.

## 2. The task – Online construction equipment rental

We want to create a self-service system for renting construction equipment.  
This rental site has potential to become a multinational and multilingual platform with hundreds of thousands of users, so the system must be ready to accommodate growth and be scalable.

### 2.1. Use cases

A customer must be able to:

* See the list of equipment
* For individual machines, enter the number of days for how long he wishes to rent it, and click "Add to Cart"
* Get an invoice

### 2.2. Inventory

There are three types of equipment available:

* Heavy equipment
* Regular equipment
* Specialized equipment

Example:

| name                  | type        |
|-----------------------|-------------|
| Caterpillar bulldozer | Heavy       |
| KamAZ truck           | Regular     |
| Komatsu crane         | Heavy       |
| Volvo steamroller     | Regular     |
| Bosch jackhammer      | Specialized |

Inventory can be static and stored in any convenient way (simple file is OK).

#### 2.2.1. Price calculation

The price of rentals is based on equipment type and rental length.  
There are three different fees:

* One-time rental fee – 100€
* Premium daily fee – 60€/day
* Regular daily fee – 40€/day

The price calculation for different types of equipment is:

* Heavy – rental price is one-time rental fee plus premium fee for each day rented.
* Regular – rental price is one-time rental fee plus premium fee for the first 2 days plus regular fee for the number of days over 2.
* Specialized – rental price is premium fee for the first 3 days plus regular fee times the number of days over 3.

#### 2.2.2. Loyalty points

Customers get loyalty points when renting equipment. A heavy machine gives 2 points and other types give one point per rental (regardless of the time rented).

### 2.3. Invoice

Customers can ask for an invoice that must be generated as a text file.  
The file must contain:
* Title
* Line items for every rental, displaying name and rental price
* Summary displaying total price and number of bonus points earned

Because of a legal requirement, the system must not store intermediate customer balances, only individual machine names. Prices and points must be calculated at the time of invoice generation.

### 2.4. Unit tests

Implement a set of unit tests that you think is reasonable for this application. Make sure they can be run inside Visual Studio.

### 2.5. Other considerations

Do not worry about user management for time being. Assuming a single user is fine for demonstration purposes.

Also, persistence is not mandatory. It is OK to keep the program state in memory only.

## 3. The implementation

* Solution must be implemented as a two-tier system: a web front-end, and a separate backend service handling the business logic.
* Web front-end can be implemented as ASP.NET MVC or ASP.NET Core project.
* Backend service may be implemented in any suitable manner. A console application is fine.
* You can use any convenient way of inter-process communication, but keep in mind scalability and extensibility needs.
* Extra points will be awarded for demonstrating:
  * Message-based architecture
  * IoC / DI
  * Logging
  * Decent-looking UI
  * Caching
  * Class and interaction diagrams

## 4. Assessment

Package the solution as follows:
* One Visual Studio solution containing all the needed projects.
* README file containing any instructions on how to run the system.

Upload the code to github or some other online source code repository, and send us a link. During the in-person interview you will be asked to walk through and discuss your solution.

We will look at:
* Whether the code works.
* The design of the solution, also regarding the ambitions of the project described in section 2.
* Whether the code is neat.
* Whether the extra-point-awarding skills are displayed.
