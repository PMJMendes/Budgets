if (schema_id('budgets_budgets') is null) 
begin
    exec('create schema budgets_budgets authorization dbo')
end

if (object_id('budgets_budgets.tblbudgets', 'U') is null)
begin
	create table budgets_budgets.tblbudgets (
		pk UNIQUEIDENTIFIER not null,
		code NVARCHAR(255) not null unique check( code <> ''),
		istemplate BIT not null,
		budgetdate DATE not null,
		title NVARCHAR(255) not null check( title <> ''),
		[state] NVARCHAR(255) not null,
		filmdate NVARCHAR(255) null,
		finalclient NVARCHAR(255) null,
		product NVARCHAR(255) null,
		agency NVARCHAR(255) null,
		director NVARCHAR(255) null,
		producer NVARCHAR(255) null,
		tvagency NVARCHAR(255) null,
		rights NVARCHAR(255) null,
		studiodays NVARCHAR(255) null,
		locationdays NVARCHAR(255) null,
		outsidedays NVARCHAR(255) null,
		weekendholidays NVARCHAR(255) null,
		postprodduration NVARCHAR(255) null,
		postprodversions NVARCHAR(255) null,
		postprodsound NVARCHAR(255) null,
		postprodnvoices NVARCHAR(255) null,
		comments NVARCHAR(MAX) null,
		commentsenglish NVARCHAR(MAX) null,
		producerpercent DECIMAL(19,5) null,
		bcapercent DECIMAL(19,5) null,
		nweatherdays INT null,
		weathertotal DECIMAL(19,5) null,
		weatherpercent DECIMAL(19,5) null,
		whencreated DATETIME2 not null,
		whocreated NVARCHAR(255) not null check( whocreated <> ''),
		whenupdated DATETIME2 not null,
		whoupdated NVARCHAR(255) not null check( whoupdated <> ''),
		primary key (pk)
	)
end

if (object_id('budgets_budgets.tblgroups', 'U') is null)
begin
	create table budgets_budgets.tblgroups (
		pk UNIQUEIDENTIFIER not null,
		fk_owner UNIQUEIDENTIFIER not null,
		[order] INT not null,
		description NVARCHAR(255) not null check( description <> ''),
		descenglish NVARCHAR(255) null,
		whencreated DATETIME2 not null,
		whocreated NVARCHAR(255) not null check( whocreated <> ''),
		whenupdated DATETIME2 not null,
		whoupdated NVARCHAR(255) not null check( whoupdated <> ''),
		primary key (pk),
		unique (fk_owner, [order]),
		unique (fk_owner, description),
		constraint fk_owner_group_budget foreign key (fk_owner) references budgets_budgets.tblbudgets
	)
end

if (object_id('budgets_budgets.tblcategories', 'U') is null)
begin
	create table budgets_budgets.tblcategories (
		pk UNIQUEIDENTIFIER not null,
		fk_owner UNIQUEIDENTIFIER not null,
		[order] INT not null,
		formula NVARCHAR(255) not null check( formula <> ''),
		description NVARCHAR(255) not null check( description <> ''),
		descenglish NVARCHAR(255) null,
		whencreated DATETIME2 not null,
		whocreated NVARCHAR(255) not null check( whocreated <> ''),
		whenupdated DATETIME2 not null,
		whoupdated NVARCHAR(255) not null check( whoupdated <> ''),
		primary key (pk),
		unique (fk_owner, [order]),
		unique (fk_owner, description),
		constraint fk_owner_category_group foreign key (fk_owner) references budgets_budgets.tblgroups
	)
end

if (object_id('budgets_budgets.tblvaluedefs', 'U') is null)
begin
	create table budgets_budgets.tblvaluedefs (
		pk UNIQUEIDENTIFIER not null,
		fk_owner UNIQUEIDENTIFIER not null,
		[order] INT not null,
		type NVARCHAR(255) not null,
		description NVARCHAR(255) not null check( description <> ''),
		descenglish NVARCHAR(255) null,
		bcaformula NVARCHAR(255) null,
		whencreated DATETIME2 not null,
		whocreated NVARCHAR(255) not null check( whocreated <> ''),
		whenupdated DATETIME2 not null,
		whoupdated NVARCHAR(255) not null check( whoupdated <> ''),
		primary key (pk),
		unique (fk_owner, [order]),
		unique (fk_owner, description),
		constraint fk_owner_valuedef_category foreign key (fk_owner) references budgets_budgets.tblcategories
	)
end

if (object_id('budgets_budgets.tblitems', 'U') is null)
begin
	create table budgets_budgets.tblitems (
		pk UNIQUEIDENTIFIER not null,
		fk_owner UNIQUEIDENTIFIER not null,
		[order] INT not null,
		excludefrombase BIT not null,
		canbepercent BIT not null,
		description NVARCHAR(255) not null check( description <> ''),
		descenglish NVARCHAR(255) null,
		[percent] DECIMAL(19,5) null,
		bcapercent DECIMAL(19,5) null,
		whencreated DATETIME2 not null,
		whocreated NVARCHAR(255) not null check( whocreated <> ''),
		whenupdated DATETIME2 not null,
		whoupdated NVARCHAR(255) not null check( whoupdated <> ''),
		primary key (pk),
		unique (fk_owner, [order]),
		unique (fk_owner, description),
		constraint fk_owner_item_category foreign key (fk_owner) references budgets_budgets.tblcategories
	)
end

if (object_id('budgets_budgets.tblvalues', 'U') is null)
begin
	create table budgets_budgets.tblvalues (
		pk UNIQUEIDENTIFIER not null,
		fk_owner UNIQUEIDENTIFIER not null,
		fk_def UNIQUEIDENTIFIER not null,
		numbervalue DECIMAL(19,5) null,
		textvalue NVARCHAR(255) null,
		textenglish NVARCHAR(255) null,
		prodvalue DECIMAL(19,5) null,
		whencreated DATETIME2 not null,
		whocreated NVARCHAR(255) not null check( whocreated <> ''),
		whenupdated DATETIME2 not null,
		whoupdated NVARCHAR(255) not null check( whoupdated <> ''),
		primary key (pk),
		unique (fk_owner, fk_def),
		constraint fk_owner_value_item foreign key (fk_owner) references budgets_budgets.tblitems,
		constraint fk_def_value_valuedef foreign key (fk_def) references budgets_budgets.tblvaluedefs
	)
end
