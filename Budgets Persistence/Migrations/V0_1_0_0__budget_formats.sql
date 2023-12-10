if (col_length('budgets_budgets.tblbudgets', 'formats') is null)
begin
    alter table budgets_budgets.tblbudgets
        add formats NVARCHAR(255) null;
end
