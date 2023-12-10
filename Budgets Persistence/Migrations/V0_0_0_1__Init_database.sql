if (schema_id('budgets_users') is null) 
begin
    exec('create schema budgets_users authorization dbo')
end

if (object_id('budgets_users.tblusers', 'U') is null)
begin
    create table budgets_users.tblusers (
        pk UNIQUEIDENTIFIER not null,
        externalid NVARCHAR(255) not null unique check(externalid <> ''),
        email NVARCHAR(255) not null unique check(email <> ''),
        [level] NVARCHAR(255) not null,
        fullname NVARCHAR(255) not null check(fullname <> ''),
        [state] NVARCHAR(255) not null,
        whencreated DATETIME2 not null,
        whocreated NVARCHAR(255) not null check(whocreated <> ''),
        whenupdated DATETIME2 not null,
        whoupdated NVARCHAR(255) not null check(whoupdated <> ''),
        primary key (pk)
    )
end
