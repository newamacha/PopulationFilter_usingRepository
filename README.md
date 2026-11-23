# Population Filter

An ASP.NET Core MVC app (.NET 8) for recording population figures and exploring them by country, city and age group. Built as a practice project to work through the repository pattern with Entity Framework Core and SQL Server.

## What it does

- **Record population data**: male and female counts per country, city and age group (Old, Young, Child). Submitting the same combination again updates the existing row instead of adding a duplicate.
- **Summary page**: totals grouped by age group, plus the top three countries by total population.
- **Filter without reloading**: pick a country, city or age group and the summary updates through small JSON endpoints (`GetCities`, `FilterData`).
- **Manage reference data**: screens for adding countries and cities.
- **Login screen** in front of the app.

## How it is built

```
Controllers  ->  Repository interfaces  ->  Repositories  ->  EF Core DbContext  ->  SQL Server
```

- **Controllers** (`Auth`, `Countries`, `Cities`, `Populations`) stay thin and take their dependencies through constructor injection.
- **Repositories** sit behind interfaces (`ICountriesRepository`, `ICitiesRepository`, `IPopulationRepository`, `IAuthRepository`) and are registered as scoped services in `Program.cs`.
- **DTOs** carry form input and the grouped query results, so views never bind directly to entities.
- **Queries** use LINQ with `Include`, `GroupBy` and `Sum`, translated to SQL by EF Core.
- **Schema** is code-first, tracked with EF Core migrations in `Migrations/`.

## Run it locally

You need the .NET 8 SDK and a SQL Server instance (Express is fine).

1. Point `ConnectionStrings:DefaultConnection` in `appsettings.json` at your server.
2. Create the database:
   ```
   dotnet ef database update
   ```
3. Add one row to the `Users` table so you can sign in.
4. Start the app:
   ```
   dotnet run
   ```

## Notes

This is a learning project, not production code. In particular the login is a plain lookup against the `Users` table, with no password hashing or session handling.

An earlier version without the repository layer is in [PopulationFilter](https://github.com/newamacha/PopulationFilter).
