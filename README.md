# Project Management API
---

## Project Description
This API was made using C#, ASP.NET Core Web API and Microsoft SQL Server and Azure SQL. The application lets a user perform CRUD-based operations on entities such as: `Bugs`, `Projects`, `Project Managers` and `Employees`. ADO.NET was used to establish a connection to the SQLite database, and each entity has it's own repository data-access class and interface that defines the different data-access implementations available for each repository class.

---
## Demo of functionalities
`Swagger UI Homepage`
<img src="https://www.justintruong.studio/images/AllApiMethods.png">

`Get all bugs API call`
<img src="https://www.justintruong.studio/images/GetAllBugs.png">

`Get bug by id API call`
<img src="https://www.justintruong.studio/images/GetBugById.png">

`Delete bug by id API call`

<img src="https://www.justintruong.studio/images/DeleteBugById.png">


---
## How to run the application
1. The application is deployed and hosted on Microsoft Azure App Service, and can be accessed by visiting [https://projectsmanagementapi.azurewebsites.net/swagger/index.html](https://projectsmanagementapi.azurewebsites.net/swagger/index.html).


---
## How to use the project
1. Navigate to the application on any browser using the provided link

2. To execute a <ins>GET request on an entity.</ins>
   - Click on the tab of the action you want to execute such as /api/Bugs/GetAllBugs to return a JSON object of all bugs.
     - Click the `Try it out` button that appears, provide the required parameters if needed, and click the blue `execute` button.
       
   - To <ins>add a record to an entity</ins>, navigate to a POST request tab such as /api/Bugs/AddBug to create a record from the Bugs table.
     - Click on the POST tab.
     - Click the `Try it out` button that appears, provide the required parameters, and click the blue `execute` button.

   - To <ins>update a record of an entity</ins>, navigate to a PUT request tab such as /api/Bugs/UpdateBug to update a record from the Bugs table.
     - Click on the PUT tab.
     - Click the `Try it out` button that appears, provide the required parameters, and click the blue `execute` button.
       - If the update was successful, a 200 Status Code will be returned indicating the POST request was successful.
       - If the update was not successful, you will get a 500 Internal Server Error indicating the POST request was unsuccessful
      
   - To <ins>delete a record of an entity</ins>, navigate to a DELETE request tab such as /api/Bugs/DeleteBug to delete a record from the Bugs table.
     - Click on the DELETE tab.
     - Click the `Try it out` button that appears, provide the required parameter such as an {id}, and click the blue `execute` button.
       - If the delete was successful, a 200 Status Code will be returned indicating the DELETE request was successful.
       - If the delete was not successful, you will get a 404 Not Found indicating that the inputted {id} does not exist within the entity table.


---
## Technology used
1. C#, .NET 7.0, ASP.NET Core Web API
2. Microsoft SQL Server and Azure SQL as the database engine and hosting service
3. Azure App Service to host the API
4. ADO.NET as the data-access library


