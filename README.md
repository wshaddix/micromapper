# What is MicroMapper?
MicroMapper is a very small, very opinionated library that helps you map property values from two different sources. It is built with only two features in mind:

1. Map a source object to destination object's internal properties
2. Map a json document to an internally scoped object

These are the two scenarios that come up over and over in the type of middle-ware projects that I work on frequently.

# Why did you build it?
The various mappers (object/json) are built to work on public classes and/or public properties when they map. This is fine for many scenarios but in just about all of the middle-ware applications that I work on, I need to make public objects to my internally scoped business objects or I need to map a json document to my internally scoped business objects. I don't want to expose my business objects, which are internal to my domain, to the outside world just so they can be mapped to/from. 

# How is it different?
1. By default, it will map to the destination object's internal properties. This allows you to map to your internally scoped business entities.
2. You provide both the source and destination object. Because many times the destination object will have dependencies injected into it's constructor, the Mapper cannot know or care about those dependencies. Instead, you construct your destination object and inject dependencies, then you map properties onto it.

# How do I get it?
 First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then from the package manager console:
 
 ```
PM> Install-Package MicroMapper
```

# How do I use it?
## To map an internal business entity to a public model (view model or api resource)

```csharp
var customer = new Customer // <-- This is an internally scoped class
{
    AgeInYears = 45,
    CreatedOnUtc = DateTime.UtcNow,
    FirstName = "First",
    IsPreferredMember = true,
    LastName = "Last",
    Nicknames = new List<string> { "Nickname 1", "Nickname 2" }
};

var customerVm = new CustomerViewModel(); // <-- This is a publicly scoped class

var mapper = new Mapper<Customer, CustomerViewModel>(customer, customerVm);
mapper.Execute();

// customerVm now has the same property values as Customer

```

## To map properties when the names don't match

```csharp
var customer = new Customer
{
    AgeInYears = 45,
    CreatedOnUtc = DateTime.UtcNow,
    FirstName = "First",
    IsPreferredMember = true,
    LastName = "Last",
    Nicknames = new List<string> { "Nickname 1", "Nickname 2" }
};

var personVm = new PersonViewModel();
var mapper = new Mapper<Customer, PersonViewModel>(customer, personVm);

mapper
    .MapProperty(vm => vm.HowOld, c => c.AgeInYears)
    .MapProperty(vm => vm.WhenCreated, c => c.CreatedOnUtc)
    .MapProperty(vm => vm.FullName, c => $"{customer.FirstName} {customer.LastName}")
    .MapProperty(vm => vm.Preferred, c => c.IsPreferredMember)
    .MapProperty(vm => vm.Aliases, c => c.Nicknames)
    .MapProperty(vm => vm.SomeProperty, c => "any value works")
    .Execute();

// personVm now has the same property values as Customer
```

## To ignore mapping properties onto your destination object

```csharp
 var customer = new Customer
{
    AgeInYears = 33,
    CreatedOnUtc = DateTime.UtcNow,
    FirstName = "First",
    IsPreferredMember = true,
    LastName = "Last",
    Nicknames = new List<string> { "Nickname 1", "Nickname 2" }
};

var customerVm = new CustomerViewModel();
customerVm.AgeInYears = 0;
customerVm.FirstName = "First should be ignored";

var mapper = new Mapper<Customer, CustomerViewModel>(customer, customerVm);

mapper
    .ReadOnlyPublicPropertiesFromSource()
    .Ignore(vm => vm.AgeInYears)
    .Ignore(vm => vm.FirstName)
    .Execute();

// customerVm now has the same property values as Customer EXCEPT for the AgeInYears and FirstName properties (they were not mapped)
```

## To only map the PUBLIC properties from the source object

```csharp
var customer = new Customer // <-- This is an internally scoped class
{
    AgeInYears = 45,
    CreatedOnUtc = DateTime.UtcNow,
    FirstName = "First",
    IsPreferredMember = true,
    LastName = "Last",
    Nicknames = new List<string> { "Nickname 1", "Nickname 2" }
};

var customerVm = new CustomerViewModel(); // <-- This is a publicly scoped class

var mapper = new Mapper<Customer, CustomerViewModel>(customer, customerVm);
mapper
	.ReadOnlyPublicPropertiesFromSource() // <-- only public properties of the Customer will be mapped onto the CustomerVm
	.Execute();

// customerVm now has the same property values as Customer

```