# What is MicroMapper?
MicroMapper is a very small, very opinionated library that helps you map property values from two different sources. It is built with only two features:
1. Map one object to another object
2. Map a json document to another object

These are the two scenarios that come up over and over in the type of middle-ware projects that I work on frequently.

# Why did you build it?
I work a lot in middleware applications that are consumed by web/mobile user interfaces and that connect with many 3rd party service apis. These days, most apis return json data and I ultimately need to get that json data into my internal business classes. Once I'm ready to return that data out of the api, I don't want to return my internal business classes. Instead I want to return "resources" or "models" that represent my business classes but are not the actual classes so that I'm free to evolve the business classes without breaking the api. These two requirements (construct an object from a json document and mapping from a business class to a "model" class) happen so much that I wanted to automate it.

# Why not just use AutoMapper and Json.Net?
Those are both amazing libraries, well more advanced that what MicroMapper sets out to be. I have two scenarios that make it difficult to work with those libraries.
1. I want to map a json document (returned from remote api) to an **internal** business object and I don't want to decorate my business class with Json.net attributes.
2. I want to inject dependencies into my business classes and therefore don't want to have to include a parameterless contructor if doing so would leave my object in an invalid state.

# How do I get it?
 First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then from the package manager console:
 
 `PM> Install-Package MicroMapper`