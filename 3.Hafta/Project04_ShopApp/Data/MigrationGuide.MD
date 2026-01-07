# Migration Guide
1. Make changes to the Entity Model. Go to the Product.cs file and add a new property.
2. Create a migration. dotnet ef migrations add MigrationName
3. Review the new migration file created in the Migrations folder, especially the Up method!
4. Apply the migration. dotnet ef database update (this command will run the Up method)
5. The database is now up to date and compatible with your C# code.
- Note: If you realise you've made a mistake immediately after creating the migration, but before deploying it, you can roll back the migration with dotnet ef migrations remove