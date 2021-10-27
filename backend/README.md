# CarRentalApp backend

# Run development
* ```cd CarRental```
* ```dotnet user-secrets init```
* ```dotnet user-secrets set "DevelopDB:Login" "{dev username}" ```
* ```dotnet user-secrets set "DevelopDB:Password" "{dev password}" ```
* ```cd ..```
* ```dotnet run --project CarRental```

# Run Docker
```docker build -t car-rental-app-backend CarRental```
```docker build -t car-rental-app-backend CarRental```

```docker run car-rental-app-backend --expose=5000```