using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ManageBudget;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;
using Krypton.Budgets.Shared;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage._Core;

public class CategoryModel : ICategory
{
	private CategoryModel(Guid id, string formula, string description, string? descEnglish,
		IEnumerable<ValueDefModel> defs, IEnumerable<ItemModel> items)
	{
		Id = id;

		Formula = formula;
		Description = description;
		DescEnglish = descEnglish;

		Defs = defs.Select((d, i) => d.WithOrder(i + 1)).ToList();
		Items = items.Select(i => i.WithOwner(this)).ToList();
	}

	public Guid Id { get; private init; }

	public string Formula { get; set; }
	public string Description { get; set; }
	public string? DescEnglish { get; set; }

	public List<ValueDefModel> Defs { get; private set; }
	public List<ItemModel> Items { get; private set; }

	IEnumerable<IItem> ICategory.Items => Items;

	public void WriteToCSVStream(StreamWriter sw)
	{
		sw.Write(";\"" + Description + "\";");

		Defs.ForEach(d => sw.Write("\"" + d.Description + "\";"));

		for (int i = 0; i < 9 - Defs.Count; i++)
		{
			sw.Write(';');
		}

		sw.WriteLine("\"Valor\";");

		Items.ForEach(i => i.WriteToCSVStream(sw));
	}

	public bool IsValid => !string.IsNullOrWhiteSpace(Description) &&
		!string.IsNullOrWhiteSpace(Formula);

	public CategoryArgs AsUpdateArgs() => new(
		Id,
		Formula,
		Description,
		DescEnglish,
		Defs.OrderBy(d => d.Order).Select(d => d.AsArgs()).ToList(),
		Items.Select(i => i.AsUpdateArgs()).ToList()
	);

	public MCategoryArgs AsManageArgs() => new(
		Id,
		Items.Select(i => i.AsManageArgs()).ToList()
	);

	public ValueDefModel AddDef(ValueDefModel def)
	{
		var result = def.WithOrder(Defs.Count + 1);
		Defs.Add(result);
		foreach (var item in Items)
		{
			item.AddValue(result);
		}
		return result;
	}

	public void RemoveDef(ValueDefModel def)
	{
		Defs.Remove(def);
		Defs = Defs.Select((d, i) => d.WithOrder(i + 1)).ToList();
		foreach (var item in Items)
		{
			item.RemoveValue(def);
		}
	}

	public ItemModel AddItem(ItemModel item)
	{
		var result = item.WithOwner(this);
		Items.Add(result);
		return result;
	}

	public void RemoveItem(ItemModel item) => Items.Remove(item);

	public void ItemUp(ItemModel item)
	{
		var index = Items.IndexOf(item);
		if (index < 1)
		{
			return;
		}

		Items[index] = Items[index - 1];
		Items[index - 1] = item;
	}

	public void ItemDown(ItemModel item)
	{
		var index = Items.IndexOf(item);
		if (index < 0 || index == Items.Count - 1)
		{
			return;
		}

		Items[index] = Items[index + 1];
		Items[index + 1] = item;
	}

	public CategoryModel WithManageResults(PCategoryItem? item) => new(
		Id,
		Formula,
		Description,
		DescEnglish,
		Defs,
		UpdateItemProdData(item?.Items?.ToList() ?? new())
	);

	public CategoryModel Clone() => new(
		Id,
		Formula,
		Description,
		DescEnglish,
		Defs.Select(d => d.Clone()),
		Items.Select(i => i.Clone())
	);

	public static CategoryModel Empty() => new(
		Guid.Empty,
		string.Empty,
		string.Empty,
		null,
		Array.Empty<ValueDefModel>(),
		Array.Empty<ItemModel>()
	);

	public static CategoryModel FromService(CategoryItem? item) => new(
		item?.Id ?? Guid.Empty,
		item?.Formula ?? string.Empty,
		item?.Description ?? string.Empty,
		item?.DescEnglish,
		item?.Defs?.Select(d => ValueDefModel.FromService(d)) ?? Array.Empty<ValueDefModel>(),
		item?.Items?.Select(i => ItemModel.FromService(i)) ?? Array.Empty<ItemModel>()
	);

	public static CategoryModel FromService(PCategoryItem? item) => new(
		item?.Id ?? Guid.Empty,
		item?.Formula ?? string.Empty,
		item?.Description ?? string.Empty,
		item?.DescEnglish,
		item?.Defs?.Select(d => ValueDefModel.FromService(d)) ?? Array.Empty<ValueDefModel>(),
		item?.Items?.Select(i => ItemModel.FromService(i)) ?? Array.Empty<ItemModel>()
	);

	private IEnumerable<ItemModel> UpdateItemProdData(List<PItemItem?> items) =>
		Items.Select((i, n) => n < items.Count && items[n] is PItemItem item && i.Id == item.Id ?
			i.WithManageResults(item) :
			i
		);
}
