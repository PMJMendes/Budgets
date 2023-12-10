if (object_id('budgets_budgets.tblcosts', 'U') is null)
begin
   create table budgets_budgets.tblcosts (
        pk UNIQUEIDENTIFIER not null,
        fk_owner UNIQUEIDENTIFIER not null,
        [order] INT not null,
        costvalue DECIMAL(19,5) not null,
        supplier NVARCHAR(255) null,
        [text] NVARCHAR(255) null,
        whencreated DATETIME2 not null,
        whocreated NVARCHAR(255) not null check( whocreated <> '') ,
        whenupdated DATETIME2 not null,
        whoupdated NVARCHAR(255) not null check( whoupdated <> '') ,
        primary key (pk),
        unique (fk_owner, [order]),
        constraint fk_owner_cost_item foreign key (fk_owner) references budgets_budgets.tblitems
    )
end

if (object_id('budgets_budgets.tblinvoices', 'U') is null)
begin
    create table budgets_budgets.tblinvoices (
        pk UNIQUEIDENTIFIER not null,
        fk_owner UNIQUEIDENTIFIER not null,
        [order] INT not null,
        invoicedvalue DECIMAL(19,5) not null,
        invoicenumber NVARCHAR(255) null,
        supplier NVARCHAR(255) null,
        [text] NVARCHAR(255) null,
        whencreated DATETIME2 not null,
        whocreated NVARCHAR(255) not null check( whocreated <> '') ,
        whenupdated DATETIME2 not null,
        whoupdated NVARCHAR(255) not null check( whoupdated <> '') ,
        primary key (pk),
        unique (fk_owner, [order]),
        constraint fk_owner_invoice_item foreign key (fk_owner) references budgets_budgets.tblitems
    )
end
