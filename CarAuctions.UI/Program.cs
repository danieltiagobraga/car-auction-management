using CarAuctions.Domain;
using CarAuctions.Services;
using CarAuctions.Infra.Data;
using CarAuctions.Domain.Vehicles;
using CarAuctions.Infra.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

const string Vehicle1Id = "vehicle1";
const string Vehicle2Id = "vehicle2";
const string Vehicle3Id = "vehicle3";
const string Vehicle4Id = "vehicle4";

const string User1Id = "00001";

// 1. Setup

// Set the Culture
Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB"); 

DataContext context = new(new DbContextOptions<DataContext>() { });
var uw = new UnitOfWork(context);

AuctionManagerService auctionManagerService = new(uw);

// 2. Add 4 different vehicles
var vehicle1 = VehicleFactory.CreatePassengerVehicle(nameof(Suv), Vehicle1Id, manufacturer: "BMW", model: "X1", 2010, 4);
var vehicle2 = VehicleFactory.CreatePassengerVehicle(nameof(Sedan), Vehicle2Id, manufacturer: "MERCEDES", model: "C220", 2000, 4);
var vehicle3 = VehicleFactory.CreatePassengerVehicle(nameof(Hatchback), Vehicle3Id, manufacturer: "CITROEN", model: "C3", 2020, 4);
var vehicle4 = VehicleFactory.CreateCargoVehicle(nameof(Truck), Vehicle4Id, manufacturer: "VOLVO", model: "FH", 2024, 4000);

auctionManagerService.AddVehicle(vehicle1);
auctionManagerService.AddVehicle(vehicle2);
auctionManagerService.AddVehicle(vehicle3);
auctionManagerService.AddVehicle(vehicle4);

// 3. Lets start the auction for each vehicle
var auction1Id = auctionManagerService.StartAuction(Vehicle1Id, startingBid: 10000);
var auction2Id = auctionManagerService.StartAuction(Vehicle2Id, startingBid: 7600);
var auction3Id = auctionManagerService.StartAuction(Vehicle3Id, startingBid: 1500);
var auction4Id = auctionManagerService.StartAuction(Vehicle4Id, startingBid: 50000);

// 4. Lets start the bidding
auctionManagerService.PlaceBid(Vehicle1Id, new Bid(id: "1", amount: 10100, User1Id, auction1Id, Vehicle1Id));
auctionManagerService.PlaceBid(Vehicle2Id, new Bid(id: "2", amount: 7610, User1Id, auction2Id, Vehicle2Id));
auctionManagerService.PlaceBid(Vehicle3Id, new Bid(id: "3", amount: 1600, User1Id, auction3Id, Vehicle3Id));
auctionManagerService.PlaceBid(Vehicle4Id, new Bid(id: "4", amount: 50010, User1Id, auction4Id, Vehicle4Id));
//auctionManagerService.PlaceBid(Vehicle4Id, new Bid(id: "5", amount: 50005, User1Id, auction4Id, Vehicle4Id)); // Throws Exception

// 5. Lets close the auctions
auctionManagerService.CloseAuction(Vehicle1Id);
auctionManagerService.CloseAuction(Vehicle2Id);
auctionManagerService.CloseAuction(Vehicle3Id);
auctionManagerService.CloseAuction(Vehicle4Id);
//auctionManagerService.PlaceBid(Vehicle4Id, new Bid(id: "5", amount: 50005, User1Id, auction4Id, Vehicle4Id));  // Throws Exception

// 6. Find vehicles
var sedanList = auctionManagerService.SearchVehicles(nameof(Sedan), null, null, null);
var suvList = auctionManagerService.SearchVehicles(nameof(Suv), null, null, null);


Console.ReadLine();