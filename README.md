# PaymentGateway
This is a simple project to demonstrate design of a RESTful API backend service. For simplicity we'll call it gateway or GW.  
In the imaginary system, the GW sits between merchants and a bank.

Merchants <---> Gateway <---> Bank

The GW publicly exposes a number of end-points so that merchants can hit them with requests over the Internet.

The logic within the GW then persists requests for later reference, performs some validations and, if valid, forwards them to the bank.


# Assumptions

### Gateway
It exposes three end-points.
- Healthcheck (get): In real-life these are used for monitoring purposes and metric collection. For instance, your load-balancers in production might want to know health status of the service instances. In our case, this end-point only returns a literal string 'OK' with status of 200.
- Payment (post): Accepts a payment request with details in the payload. It returns one of the values: Invalid, Declined, Success in addition to a unique id for future reference.
Whether or not the request is valid depends on the logic in the GW. Whether a valid request is successful or declined is determined by the bank. One might think we need other values to represent status of a request but these three are mutually exclusive and cover all possibilities from a logical point of view. For example the 'Valid' state is unenecessary. Can you tell why?
- Payment (get): Accepts a query about earlier payment requests and returns a report. The query must provide the id of the merchant and the id of the payment request it enquires about.
The merchant id is necessary here to determine whether the original request was made by the merchant. Otherwise an invalid response is sent back. This is intentionally chosen as opposed to an 'unauthorised' or 'access-denied' response because this doesn't even say whether or not such request id exists in the system. From the security perspective we should give out the minimum necessary information.
A valid query will get a response with details of the payment request except the card number. We obscure the card numbers in the reports by replacing the first 12 characters with asterisks.
- There are assumed arbitrary values in the logic that form the basis for decisions e.g valid range of CVV, the oldest allowed card, the range of acceptable request amount, the maximum payment amout that the bank allows, etc.

### Bank
Its internal operation is no concern of the GW. All that the GW needs to know is the interface via which the GW can communicate with the bank. We abstract away all behaviour of the bank in the form of an interface called IBank.  
In our solution we simulate behaviour of the bank in a concrete class called FakeBank. The FakeBank performs a payment request if it's less than an arbitrary threshold and declines it otherwise.
In real life scenarios there's usually an API comminication between our GW and the bank. That communication has an async nature, so does the IBank interface. Howerver turning all operation to async and faking them takes a lot of time and brings little value to our design demo so we leave that out.


# Design

## Model Binding and DTOs
Request payloads can be an attack vector agains your api so we should allow absolute minimum data from them to enter our logic. Therefore, instead of binding payloads to our model types we bind them to simple DTOs. Prevent over-posting!
The reports we return in response to queries also need to convey minimum information. That's why the Report class is effectively a DTO type.
Our DTOs have the following characteristics:
- They are absolutely dumb! No logic, no behaviour, no complex operation.
- They are open to getters and setters since they are merely data carriers.
- They flatten our data model to keep things simple. With every level of flattening they prepend a prefix to the lower level values. e.g Report.Card.Number becomes Report.Card_Number with an underscore in between.
- By definition, the dumbest objects need no unit testing, right?

## Immutable Models
Based on the encapsulation principle and to limit possibility of inadvertent change to the state of our model objects, they are either immutable namely Card, Date and Response or mostly immutable e.g Request.
The only changeable property of a request is its status which is determined at a point after initial creation of the object.

## Repository
Data persistence is responsibility of a repository class. Since the design is open to further development, I kept things broad and general. That's why the repository interface is generic. It only declares two pieces of behaviour namely Save and Get.
Current implementation of request repository is faked to keep things simple. Database operation in real-life is usually async. That's another simplification.
Our fake repository keeps a collection of objects in memory and returns them upon request.
Why is the collection a Dictionary rather than a List? Ok, the fact that most developers use List all the time doesn't mean that's the best choice. They usually use SingleOrDefault() and FirstOrDefault() to retrieve objects from the collection. Those methods iterate through the entire collection with linear runtime i.e O(n) but a key-value collection with built-in hash-mapping e.g Dictionary has a retrieval cost of constant time i.e O(1).
Data handling in batches of numerous objects usually calls for nested loops. That's where a List with its linear cost noticeably drops the performance.

## Dependency Injection
When it comes to injecting dependencies into different areas of our code we need to pay attention to the nature of the dependency. All injected types in our design are stateless services so they can be transient except the FakeRepository. Since our fake repository maintains an in-memory collection of objects we need a reference to the same instance every time! That's why we inject it as a Singleton.

## Factory methods
Our immutable objects are produced using their static factory methods. One might argue that in our case constructors could perform the factory job. That's to some extent true but:
- The operation in the factory methods is a bit complex or will protentially grow complex in the future.
- There's persistence operation in Request.Create() method. If you implement a real persistence layer with async nature you can simply change the factory method to async which is not the case with constructors. At the time of writing this c# doesn't support async constructors or I'm unaware of such a thing.

## Factory classes
There are types whose only purpose is to produce an object based on some criteria e.g RequestFactory, ResponseFactory and ReportFactory. The naming convension clearly reflects their purpose and lets them encapsulate the logic involved.

## Arbitrary values
There are some constant values here and there in the logic that form the basis for decisions. These values determine the behaviour of our GW logic and need to be highly configurable from a central point. That's why we usually read them from config files rather than hard-coding them. This is left for the future.

# Local setup

### Preparation
There's no SLN file for VisualStudio in this repository since we don't tie our work to a specific tool-set. The three projects can be built and run using the minimal setup that follows.
If you're on an ArchLinux machine, then you're in luck. Just run the setup script in the Scripts directory and let it do the work. Otherwise, please install the packages specified in that script.

### Use
To launch the GW just navigate to the Api directory and run: **dotnet run**  
If you have access to any Bash environment e.g on a linux machine or in GitBash you can run the test scripts in the scripts directory to hit the end-points and observe the result in terminal.
Otherwise, one can take json values from those scripts and use any other tool e.g Postman to send them to the end-points.

# Unit tests
In order to run unit tests please navigate to the Test directory and run: **dotnet test**


# Further steps
There's always room for more development and improvement. For instance we could model the business in more detail. Also security needs to be built-in to the product during the development rather than bolt-on later. Financial systems can never get enough of validation. Some steps for future work:

- Async behaviour for the repository layer
- Async behaviour for the bank
- Timestamp the requests
- Add PIN to card details for authorisation
- Validate query payload
- Validate card number
- Config files for central control of constant values
- Decide what to return from the health-check end-point
- Logs for auditing
- Authentication and authorisation of the request source
- Https redirection for security of traffic
- More unit testing
- Randomise dummy data in unit tests like what's in RequestTest
- Automated integration testing to hammer the controllers
