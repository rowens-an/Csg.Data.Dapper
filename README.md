# Csg.Data.Dapper
[Dapper](https://github.com/stackexchange/dapper) provides extension methods for IDbConnection. This library provides those
same extensions for the Query Builder (IDbQueryBuilder) from [Csg.Data](https://www.github.com/csgsolutions/csg.data).

# CI and Pre-Release Feed

[![Build status](https://ci.appveyor.com/api/projects/status/j1l9gafvjaxjlec1?svg=true)](https://ci.appveyor.com/project/jusbuc2k/csg-data-dapper)

Early releases can be found on the [CSG Public MyGet feed](https://www.myget.org/feed/csgsolutions/package/nuget/Csg.Data.Dapper).

# Get Started
Install the [NuGet package](https://www.nuget.org/packages/Csg.Data.Dapper/)

# Example Usage

```csharp
var activeProducts = await connection.QueryBuilder("dbo.Product")
    .Where(x => x.FieldEquals<bool>("IsActive", true))
    .QueryAsync<Product>();
```

# Provided Dapper Methods
  * Query
  * QueryAsync
  * QueryFirst
  * QueryFirstAsync
  * QueryFirstOrDefault
  * QueryFirstOrDefaultAsync
  * QuerySingle
  * QuerySingleAsync
  * QuerySingleOrDefault
  * QuerySingleOrDefaultAsync

# Other Methods

 ToDapperCommand() provides a way to build a custom Dapper execution.

```csharp
var dapperCmd = connection.QueryBuilder("dbo.Product")
    .Where(x => x.FieldEquals<bool>("IsActive", true))
    .ToDapperCommand();

// do something with native Dapper extension methods.
var data = await connection.QueryAsync(dapperCmd);
// etc
```