if (col_length('budgets_budgets.tblbudgets', 'fk_manager') is null)
begin
    alter table budgets_budgets.tblbudgets
        add fk_manager UNIQUEIDENTIFIER null;
end

if (object_id('budgets_budgets.fk_manager_budget_user', 'F') is null)
begin
    alter table budgets_budgets.tblbudgets
        add constraint fk_manager_budget_user
        foreign key (fk_manager)
        references budgets_users.tblusers;
end
