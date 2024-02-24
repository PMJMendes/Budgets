using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ManageBudget;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;
using Krypton.Budgets.Shared;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage._Core;

public class GroupModel : IGroup
{
	private GroupModel(Guid id, string description, string? descEnglish, IEnumerable<CategoryModel> categories)
	{
		Id = id;

		Description = description;
		DescEnglish = descEnglish;

		Categories = categories.ToList();
	}

	public Guid Id { get; private init; }

	public string Description { get; set; }
	public string? DescEnglish { get; set; }

	public List<CategoryModel> Categories { get; private init; }

	IEnumerable<ICategory> IGroup.Categories => Categories;

	public void WriteToCSVStream(StreamWriter sw, int index)
	{
		sw.WriteLine("" + index + ";\"" + Description + "\";;;;;;;;;;;");

		Categories.ForEach(c => c.WriteToCSVStream(sw));
	}

	public bool IsValid => !string.IsNullOrWhiteSpace(Description);

	public GroupArgs AsUpdateArgs() => new(
		Id,
		Description,
		DescEnglish,
		Categories.Select(c => c.AsUpdateArgs()).ToList()
	);

	public MGroupArgs AsManageArgs() => new(
		Id,
		Categories.Select(c => c.AsManageArgs()).ToList()
	);

	public CategoryModel AddCategory(CategoryModel category)
	{
		Categories.Add(category);
		return category;
	}

	public void RemoveCategory(CategoryModel category) => Categories.Remove(category);

	public GroupModel WithManageResults(PGroupItem? item) => new(
		Id,
		Description,
		DescEnglish,
		UpdateCategoryProdData(item?.Categories?.ToList() ?? new())
	);

	public GroupModel Clone() => new(
		Id,
		Description,
		DescEnglish,
		Categories.Select(c => c.Clone())
	);

	public static GroupModel Empty() => new(
		Guid.Empty,
		string.Empty,
		null,
		Array.Empty<CategoryModel>()
	);

	public static GroupModel FromService(GroupItem? item) => new(
		item?.Id ?? Guid.Empty,
		item?.Description ?? string.Empty,
		item?.DescEnglish,
		item?.Categories?.Select(c => CategoryModel.FromService(c)) ?? Array.Empty<CategoryModel>()
	);

	public static GroupModel FromService(PGroupItem? item) => new(
		item?.Id ?? Guid.Empty,
		item?.Description ?? string.Empty,
		item?.DescEnglish,
		item?.Categories?.Select(c => CategoryModel.FromService(c)) ?? Array.Empty<CategoryModel>()
	);

	private IEnumerable<CategoryModel> UpdateCategoryProdData(List<PCategoryItem?> categories) =>
		Categories.Select((c, i) => i < categories.Count && categories[i] is PCategoryItem item && c.Id == item.Id ?
			c.WithManageResults(item) :
			c
		);
}
