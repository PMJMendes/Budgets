﻿using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Budgets.Budget_.LockBudget;

public interface ILockBudgetResults : IOpResults
{
    BudgetState State { get; }
}
