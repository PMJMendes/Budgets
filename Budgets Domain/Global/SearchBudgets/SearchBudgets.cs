using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Domain.Budgets.Budget_;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Global.SearchBudgets;

internal class SearchBudgets : BaseQuery<SearchBudgets, ISearchBudgetsArgs, ISearchBudgetsItem>, ISearchBudgets
{
	public SearchBudgets(IContext context, ILogger<SearchBudgets> logger) : base(context, logger) { }

	protected override void AssertPermissions() => AssertIsLoggedIn();

	protected override IAsyncEnumerable<V> InnerFetch<V>(ISearchBudgetsArgs? args, Func<ISearchBudgetsItem, V> itemFactory)
	{
		var query = context.Persistence.Query<Budget>().
			Where(b => !b.IsTemplate);

		if (!context.Security.HasSecurityLevel(SecurityLevel.Producer))
		{
			if (context.Security.HasSecurityLevel(SecurityLevel.Accounting))
			{
				query = query.Where(b => b.State != BudgetState.OPEN);
			}
			else
			{
				query = query.Where(b => b.State == BudgetState.LOCKED && b.Manager == context.CurrentUser);
			}
		}

		if (args?.FreeText?.ToLower() is string text)
		{
			query = query.Where(b =>
				b.Code.ToLower().Contains(text) ||
				b.Title.ToLower().Contains(text) ||
				(b.FilmDate != null && b.FilmDate.ToLower().Contains(text)) ||
				(b.FinalClient != null && b.FinalClient.ToLower().Contains(text)) ||
				(b.Product != null && b.Product.ToLower().Contains(text)) ||
				(b.Agency != null && b.Agency.ToLower().Contains(text)) ||
				(b.Director != null && b.Director.ToLower().Contains(text)) ||
				(b.Producer != null && b.Producer.ToLower().Contains(text)) ||
				(b.TVAgency != null && b.TVAgency.ToLower().Contains(text)) ||
				(b.Rights != null && b.Rights.ToLower().Contains(text)) ||
				(b.Formats != null && b.Formats.ToLower().Contains(text)) ||
				(b.Comments != null && b.Comments.ToLower().Contains(text)) ||
				(b.CommentsEnglish != null && b.CommentsEnglish.ToLower().Contains(text))
			);
		}

		if (args?.State is BudgetState state)
		{
			query = query.Where(b => b.State == state);
		}

		if (args?.BudgetDateFrom is DateOnly dateFrom)
		{
			query = query.Where(b => b.BudgetDate >= dateFrom);
		}

		if (args?.BudgetDateTo is DateOnly dateTo)
		{
			query = query.Where(b => b.BudgetDate <= dateTo.AddDays(1));
		}

		query = query.OrderByDescending(b => b.Code);

		return query.ToAsyncEnumerable().
			Select(b => itemFactory(new Item(
				b.Id,
				b.Code,
				b.BudgetDate,
				b.Title,
				b.State,
				b.FinalClient,
				b.Product
			)));
	}

	private record struct Item(
		Guid Id,
		string Code,
		DateOnly BudgetDate,
		string Title,
		BudgetState State,
		string? FinalClient,
		string? Product
	) : ISearchBudgetsItem;
}
