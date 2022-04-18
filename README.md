# Introduction 
A small proof of concept for using Dapper with GraphQl on a .Net platform. The main goal was to create dynamic SQL query based on the the passed in GraphQL Query.
HotChocolate takes away having to write GraphQL Schemas and instead generates them for you based on your Query.cs file. 
You are able to access your Entity properties in your GraphQL Query by inheriting a common abstract class or interface.
This allows you to have your Query.cs file be generic and return any Entity or Collection of Entities and retrieve as many nested relationships as you want without having to write seperate implementations.

# Getting Started
Utilizes the following:

	1.	GraphQL - HotChocolate by ChilliCream (https://chillicream.com/docs/hotchocolate)
	2.	ORM - Dapper (https://github.com/DapperLib/Dapper)
	3.	SQL Building - Dapper.SqlBuilder (https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder)
	4.	Mapping to POCOs - Slapper.AutoMapper (https://github.com/SlapperAutoMapper/Slapper.AutoMapper)

# Desired Changes
I do not recommend using Dapper.SqlBuilder and plan to remove it when time permits. 
It is not as powerful as you would hope and does not remove any real complexity. 
	
	Pros:
		1.	Managed object for your various SQL statements that can then build your SQL by adding a template
		2.	SELECT statement generation handles adding commas for you which reduces code.
	Cons:
		1.	WHERE statements require explicit naming since they use anonymous types, so whatever your @filterName is, has to be the name of your anonymous type field.
			This creates difficulties when you want to add dynamic parameters. For example if you had a where statement like:
			User.Id = @id then your passed anonymous type would need to be "new { id = 1234 }". You end up having to just use a DynamicParameter object anyways if you want to add where statements that are not based on an ID.
		2.	They have known issues on their github about using ".Where()" together with ".OrWhere" not generating the correct SQL.
		3.	Does not do a whole lot in the long run. You are still having to write your sql statements. All the method calls provide is removing you having to write the literal words "inner join" or "left join" etc. This library
			is something that could easily be recreated and would reduce reliance on an unneeded third party.