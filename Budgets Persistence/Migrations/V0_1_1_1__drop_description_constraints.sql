DECLARE @sql NVARCHAR(MAX) = N'';

select @sql += N'ALTER TABLE ' + TC.TABLE_SCHEMA+ + '.' + TC.TABLE_NAME + ' DROP CONSTRAINT ' + TC.Constraint_Name + ';'
from information_schema.table_constraints TC
inner join information_schema.constraint_column_usage CC on TC.Constraint_Name = CC.Constraint_Name
where Column_Name = 'description';

EXEC sp_executesql @sql;
