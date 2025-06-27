# 034 Rate Limiting

## Lecture

[![# Rate Limiting)](https://img.youtube.com/vi/1E4uIz_DkU4/0.jpg)](https://www.youtube.com/watch?v=1E4uIz_DkU4)

## Instructions

In `HomeEnergyApi/Middleware/RateLimitingService.cs`
- Create a new public class named `RateLimitingService`
    - Add a private readonly property of type `Dictionary<string, List<DateTime>>` and initialize it to a new empty dictionary
    - Add a private readonly property of type `int` and initialize it to the value `20
    - Add a private readonly property of type `TimeSpan` and initialize it with a value representing 1 second
    - Create a public method named `IsRequestAllowed` that returns `bool` and takes one parameter of type `string`
        - Create an `if` statement that checks if the `Dictionary<string, List<DateTime>>` property does NOT contain a key using `ContainsKey()` with the `string` parameter representing the client key
            - Inside the `if` block, assign a new `List<DateTime>` to the `Dictionary<string, List<DateTime>>` property using the `string` parameter as the key
        - Call `RemoveAll()` on the `List<DateTime>` accessed from the `Dictionary<string, List<DateTime>>` property using the `string` parameter as the key, passing a lambda expression that checks if each request is less than `DateTime.UtcNow` minus the `TimeSpan` property
        - Create an `if` statement that checks if the `Count` of the `List<DateTime>` accessed from the `Dictionary<string, List<DateTime>>` property using the `string` parameter as the key is greater than or equal to the `int` property representing the maximum requests
            - Inside the `if` block, return `false`
        - Call `Add()` on the `List<DateTime>` accessed from the `Dictionary<string, List<DateTime>>` property using the `string` parameter as the key, passing `DateTime.UtcNow`
        - Return `true` from the method
        - NOTE: You may want more or less strict rate limiting parameters on a real application, the values of 20 requets in one second were chosen to make autograding this assignment possible

In `HomeEnergyApi/Services/RateLimitingMiddleware.cs`
- Create a new public class named `RateLimitingMiddleware`
    - Add a private readonly property of type `RequestDelegate`
    - Add a private readonly property of type `RateLimitingService`
    - Add a private readonly property of type `ILogger<RateLimitingMiddleware>`
    - Create a public constructor that accepts three parameters with the following types
        - `RequestDelegate`
        - `RateLimitingService`
        - `ILogger<RateLimitingMiddleware>`
        - In the constructor body assign each parameter to the appropriate property.
    - Create a public async method named `InvokeAsync*()` that returns `Task` and takes one parameter of type `HttpContext`
        - At the beginning of the method, call `LogDebug()` on the `ILogger<RateLimitingMiddleware>` property with the message `"RateLimitingMiddleware Started"`
        - Create a variable and assign it the value of calling `ToString()` on the `RemoteIpAddress` property from the `Connection` property of the `HttpContext`
        - Create an `if` statement that checks if the request is NOT allowed by calling `IsRequestAllowed()` on the `RateLimitingService` property with the previous `ToString()` result as the argument
            - Inside the `if` block, set the `StatusCode` property of the `Response` property on the `HttpContext` to `StatusCodes.Status429TooManyRequests`
            - Call `await WriteAsync()` on the `Response` property of the `HttpContext` with the message `"Slow down! Too many requests."`
            - Add a `return` statement to exit the method
        - After the `if` block, call `await` on the `RequestDelegate` property, passing the `HttpContext` to pass control to the next middleware in the pipeline

In `HomeEnergyApi/Program.cs
- Add the line `builder.Services.AddSingleton<RateLimitingService>();`
- Add the line `app.UseMiddleware<RateLimitingMiddleware>();`
    - This should be added BEFORE the `ApiKeyMiddleware` and be called regardless of the requested endpoint

## Additional Information

- Do not remove or modify anything in `HomeEnergyApi.Tests`
- Some Models may have changed for this lesson from the last, as always all code in the lesson repository is available to view
- Along with `using` statements being added, any packages needed for the assignment have been pre-installed for you, however in the future you may need to add these yourself

## Building toward CSTA Standards:

- Document design decisions using text, graphics, presentations, and/or demonstrations in the development of complex programs (3A-AP-23) https://www.csteachers.org/page/standards
- Systematically design and develop programs for broad audiences by incorporating feedback from users (3A-AP-19) https://www.csteachers.org/page/standards

## Resources

- https://swagger.io/
- https://github.com/dotnet/aspnet-api-versioning/wiki/API-Documentation

Copyright &copy; 2025 Knight Moves. All Rights Reserved.
