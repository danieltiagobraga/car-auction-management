# Car Auction Management System

## Overview
This project implements a simple Car Auction Management System using C#. The system manages different types of vehicles: Hatchback, Sedans, SUVs, and Trucks, and allows users to add vehicles, search for them, and manage auctions.

## Features

1. **Add Vehicles**: Add vehicles to the auction inventory with their respective attributes.
2. **Search Vehicles**: Search for vehicles by type, manufacturer, model, or year.
3. **Manage Auctions**: Start and close auctions for vehicles, and place bids during active auctions.
4. **Error Handling**: Comprehensive error handling for scenarios such as duplicate vehicle IDs, invalid bids, and more.

## Design Decisions
The project utilizes principles of object-oriented design to create a flexible and maintainable codebase.
- **Design Patterns**:
- **Vehicle Factory**: Encapsulate the vehicle creation logic and make it easier to extend in the future if new vehicle types are added.
- **Repository Pattern**: For data access abstraction.
- **Unit of Work Pattern**: To manage transactions.

## Project Structure
The project tries to follow a Domain Driven Design architecture 

├── CarAuctions.Domain  <br>
│ ├── Vehicles/  <br>
│ │ ├── CargoVehicle.cs  <br>
│ │ ├── Hatchback.cs  <br>
│ │ ├── IVehicle.cs  <br>
│ │ ├── PassengerVehicle.cs  <br>
│ │ ├── Suv.cs  <br>
│ │ ├── Truck.cs  <br>
│ │ ├── Vehicle.cs  <br>
│ ├── Auction.cs  <br>
│ ├── Bid.cs  <br>
│ ├── User.cs  <br>
├── CarAuctions.Infra  <br>
│ ├── Data/  <br>
│ │ ├── DataContext.cs  <br>
│ ├── Repositories/  <br>
│ │ ├── IRepository.cs  <br>
│ │ ├── Repository.cs  <br>
│ │ ├── AuctionRepository.cs  <br>
│ │ ├── BidRepository.cs  <br>
│ │ ├── VehicleRepository.cs  <br>
│ ├── Transactions/  <br>
│ │ ├── UnitOfWork.cs  <br>
├── CarAuctions.Services  <br>
│ ├── Constants/  <br>
│ │ ├── ValidationMessage.cs  <br>
│ ├── Resources/  <br>
│ │ ├── ValidationMessage.en-GB.resx  <br>
│ │ ├── ValidationMessage.pt-PT.resx  <br>
│ ├── AuctionManagerService.cs  <br>
│ ├── VehicleFactory.cs  <br>
├── CarAuctions.Services.Tests  <br>
│ ├── AuctionManagerServiceTests.cs  <br>
├── CarAuctions.UI  <br>
│ ├── Program.cs  <br>





## Setup and Installation

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.
- [xUnit](https://xunit.net/) for running unit tests.
- [Moq](https://github.com/moq/moq4) for mocking dependencies in tests.

### Installation

1. **Clone the repository:**

    ```bash
    git clone https://github.com/danieltiagobraga/car-auction-management.git
    cd car-auction-management
    ```

2. **Build the project:**

    ```bash
    dotnet build
    ```

3. **Run the tests:**

    ```bash
    dotnet test
    ```

## Usage

### Adding a Vehicle

```csharp
var vehicle1 = VehicleFactory.CreatePassengerVehicle(nameof(Suv), Vehicle1Id, manufacturer: "BMW", model: "X1", 2010, 4);
AuctionManagerService auctionManagerService = new(uw);
auctionManagerService.AddVehicle(vehicle1);
```

### Searching for Vehicles
```csharp
var sedanList = auctionManagerService.SearchVehicles(nameof(Sedan), null, null, null);
```

### Starting an Auction
```csharp
var auction1Id = auctionManagerService.StartAuction(Vehicle1Id, startingBid: 10000);
```

### Placing a Bid
```csharp
AuctionManagerService auctionManagerService = new(uw);
auctionManagerService.PlaceBid(Vehicle1Id, new Bid(id: "1", amount: 10100, User1Id, auction1Id, Vehicle1Id));
```

### Close an Auction
```csharp
AuctionManagerService auctionManagerService = new(uw);
auctionManagerService.CloseAuction(Vehicle1Id);
```

## Error Handling
The system handles various error scenarios, such as:
- Adding a vehicle with a duplicate ID.
- Starting an auction for a vehicle that does not exist or is already in an auction.
- Placing a bid that is lower than the current highest bid or when the auction is not active.

## Multi-Language Support
To add multi-language support, you can use resource files (.resx) to store validation messages in different languages and retrieve the appropriate messages based on the current culture.

## Future improvements
- Logging: Implement logging namely for AuctionManagerService.cs and Repository.cs
- References: Use Nuget packages instead using project references
- Scalability: The current implementation works for a basic scenario, but for handle a larger number of auctions and concurrent bids in a production system we should consider different approaches. For example, distributed databases, queuing mechanisms, cache, a microservice architeture, ...
- UI Considerations: Explore web-based or mobile interfaces
