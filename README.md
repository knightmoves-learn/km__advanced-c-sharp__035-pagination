# 035 Pagination

## Lecture

[![# Pagination)](https://img.youtube.com/vi/1E4uIz_DkU4/0.jpg)](https://www.youtube.com/watch?v=1E4uIz_DkU4)

## Instructions

# In `HomeEnergyApi/Pagination/PaginatedResult.cs`

NOTE: At the start of this lesson `HomeEnergyApi/Pagination/PaginatedResult.cs` and `HomeEnergyApi/Models/IPaginatedReadRepository.cs` have been created for you.

In `HomeEnergyApi/Models/HomeRepository.cs`
- Implement the class `IPaginatedReadRepository<int, Home>` on `HomeRepository`
- Create a public method named `FindPaginated` that returns `PaginatedResult<Home>` and takes two parameters: a page number of type `int` and a page size of type `int`
    - Set the page number to `1` if it is less than `1`, otherwise keep its current value
    - Set the page size to `10` if it is less than `1`, otherwise keep its current value
    - Create a variable and assign it the result of calling `Count()` on the `Homes` property of the context
    - Create a variable and assign it the result of a LINQ query on the `Homes` property of the context:
        - Call `Include()` with a lambda expression accessing the `HomeUsageData` property
        - Call `Include()` with a lambda expression accessing the `HomeUtilityProviders` property
        - Call `OrderBy()` with a lambda expression accessing the `Id` property
        - Call `Skip()` with the calculation `(page number - 1) * page size`
        - Call `Take()` with the page size
        - Call `ToList()`
    - Return a new `PaginatedResult<Home>` with properties set to:
        - The `Items` property set to the variable containing the LINQ query results
        - The `TotalCount` property set to the variable containing the count of `Homes`
        - The `PageNumber` property set to the page number
        - The `PageSize` property set to the page size
        - The `TotalPages` property set to the result of casting `Math.Ceiling()` of the division of the total count variable cast to `double` by the page size to `int`
        - The `HasNextPage` property set to the boolean result of checking if the multiplication of the page number and page size is less than the total count variable
- Create a public method named `FindPaginatedByDate` that returns `PaginatedResult<Home>` and takes three parameters: one of type `string`, a page number of type `int`, and a page size of type `int`
    - Set the page number to `1` if it is less than `1`, otherwise keep its current value
    - Set the page size to `10` if it is less than `1`, otherwise keep its current value
    - Create a variable and assign it the result of calling `Count()` on the `Homes` property of the context
    - Create a variable and assign it the result of a LINQ query on the `Homes` property of the context:
        - Call `Where()` with a lambda expression that checks if the `OwnerLastName` property equals the `string` parameter
        - Call `Include()` with a lambda expression accessing the `HomeUsageData` property
        - Call `Include()` with a lambda expression accessing the `HomeUtilityProviders` property
        - Call `OrderBy()` with a lambda expression accessing the `Id` property
        - Call `Skip()` with the calculation `(page number - 1) * page size`
        - Call `Take()` with the page size
        - Call `ToList()`
    - Return a new `PaginatedResult<Home>` with properties set to:
        - The `Items` property set to the variable containing the query results
        - The `TotalCount` property set to the variable containing the count
        - The `PageNumber` property set to the page number
        - The `PageSize` property set to the page size
        - The `TotalPages` property set to the result of casting `Math.Ceiling()` of the division of the total count variable cast to `double` by the page size to `int`
        - The `HasNextPage` property set to the boolean result of checking if the multiplication of the page number and page size is less than the total count variable

In `HomeEnergyApi/Controllers/HomeController.cs`
- Replace the properties for the `IReadRepository` and `IOwnerLastNameQueryable` with the `IPaginatedReadRepository` and update the constructor accordingly
- Modify the `Get()` method
    - Add the following two `[FromQuery]` parameters
        - `pageNumber` of type `int` defaulting to `1`
        - `pageSize` of type `int` defaulting to `10`
    - Modify the body of the method to instead return a `PaginatedResult<Home>` from the `IPaginatedReadRepository` depending on whether an `OwnerLastName` was provided.
    - Add a new return statement, outside of the if/else block, that returns the following properties/values
        - `Homes` set to the `Items` from the `PaginatedResult`
        - `PageNumber` set to the passed `int` page number
        - `PageSize` set to the passed `int` page size
        - `TotalItems` set to the `TotalCount` of the `PaginatedResult`
        - `TotalPages` set to the `TotalPages` of the `PaginatedResult`
        = `Next Page` set to the result of the expression `Url.Action(nameof(Get), new { pageNumber = pageNumber + 1, pageSize })` if the page number is less than the total pages, or `null` otherwise

In `HomeEnergyApi/Program.cs`
- Similar to how you added a scoped service for your other repositories, add a scoped service to the `builder` for `IPaginatedReadRepository`

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
