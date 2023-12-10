using Krypton.Budgets.Domain._Base.Entities;
using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain._Impl.Exceptions;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Impl.Utils;
using Krypton.Budgets.Domain.Budgets.Group_;
using Krypton.Budgets.Domain.Users.User_;
using Krypton.Budgets.Shared;
using System.Collections.Immutable;

namespace Krypton.Budgets.Domain.Budgets.Budget_;

internal class Budget : BaseEntity, IBudget
{
    private enum EntityErrors
    {
        UPDATE_BUDGET_NOT_OPEN,
        MANAGE_BUDGET_NOT_LOCKED,
        GROUPS_WRONG_LENGTH,
        GROUPS_UNKNOWN_ID,
        DELTE_BUDGET_NOT_OPEN
    }

    private readonly ISet<Group> groups = new HashSet<Group>();

    protected Budget() { }

    protected Budget(IContext context) : base(context) { }

    public Budget(IContext context, string code, bool isTemplate, DateOnly budgetDate, string title) : this(context)
    {
        Code = code;
        IsTemplate = isTemplate;
        BudgetDate = budgetDate;
        Title = title;
        State = BudgetState.OPEN;

        context.Persistence.PersistAsync(this).GetAwaiter().GetResult();
    }

    [Required]
    [Unique]
    public virtual string Code { get; protected set; } = "";

    [Required]
    public virtual bool IsTemplate { get; protected set; } = false;

    [Required]
    public virtual DateOnly BudgetDate { get; protected set; } = DateOnly.MinValue;

    [Required]
    public virtual string Title { get; protected set; } = "";

    [Required]
    public virtual BudgetState State { get; protected set; } = BudgetState.OPEN;

    public virtual string? FilmDate { get; protected set; }
    public virtual string? FinalClient { get; protected set; }
    public virtual string? Product { get; protected set; }
    public virtual string? Agency { get; protected set; }
    public virtual string? Director { get; protected set; }
    public virtual string? Producer { get; protected set; }
    public virtual string? TVAgency { get; protected set; }
    public virtual string? Rights { get; protected set; }
    public virtual string? Formats { get; protected set; }

    public virtual string? StudioDays { get; protected set; }
    public virtual string? LocationDays { get; protected set; }
    public virtual string? OutsideDays { get; protected set; }
    public virtual string? WeekendHolidays { get; protected set; }

    public virtual string? PostProdDuration { get; protected set; }
    public virtual string? PostProdVersions { get; protected set; }
    public virtual string? PostProdSound { get; protected set; }
    public virtual string? PostProdNVoices { get; protected set; }

    public virtual string? Comments { get; protected set; }
    public virtual string? CommentsEnglish { get; protected set; }

    public virtual decimal? ProducerPercent { get; protected set; }
    public virtual decimal? BCAPercent { get; protected set; }

    public virtual int? NWeatherDays { get; protected set; }
    public virtual decimal? WeatherTotal { get; protected set; }
    public virtual decimal? WeatherPercent { get; protected set; }

    public virtual IEnumerable<Group> Groups => groups.OrderBy(g => g.Order).ToImmutableList();
    [External]
    IEnumerable<IGroup> IBudget.Groups => groups;

    public virtual User? Manager { get; protected set; }

    public virtual void Recode(string code) => Code = code;

    public virtual void AssignTo(User? manager) => Manager = manager;

    public virtual async Task<Budget> CloneAsync(string newCode, bool asTemplate, DateOnly newBudgetDate, string? newTitle)
    {
        var result = new Budget(context)
        {
            Code = newCode,
            IsTemplate = asTemplate,
            BudgetDate = newBudgetDate,
            State = BudgetState.OPEN,

            Title = newTitle ?? (Title + " (clone)"),

            FilmDate = FilmDate,
            FinalClient = FinalClient,
            Product = Product,
            Agency = Agency,
            Director = Director,
            Producer = Producer,
            TVAgency = TVAgency,
            Rights = Rights,
            Formats = Formats,

            StudioDays = StudioDays,
            LocationDays = LocationDays,
            OutsideDays = OutsideDays,
            WeekendHolidays = WeekendHolidays,

            PostProdDuration = PostProdDuration,
            PostProdVersions = PostProdVersions,
            PostProdSound = PostProdSound,
            PostProdNVoices = PostProdNVoices,

            Comments = Comments,
            CommentsEnglish = CommentsEnglish,

            ProducerPercent = ProducerPercent,
            BCAPercent = BCAPercent,

            NWeatherDays = NWeatherDays,
            WeatherTotal = WeatherTotal,
            WeatherPercent = WeatherPercent
        };

        await context.Persistence.PersistAsync(result);

        result.groups.UnionWith(await Task.WhenAll(groups.Select(c => c.CloneAsync(result))));

        return result;
    }

    public virtual void UpdateFrontData(BudgetFrontData data)
    {
        if (State != BudgetState.OPEN)
        {
            throw new ConsistencyException(TypeHelper.GetProperty((Budget i) => i.Groups),
                EntityErrors.UPDATE_BUDGET_NOT_OPEN, "Update Budget: budget not open");
        }

        BudgetDate = data.BudgetDate;

        Title = data.Title;

        FilmDate = data.FilmDate;
        FinalClient = data.FinalClient;
        Product = data.Product;
        Agency = data.Agency;
        Director = data.Director;
        Producer = data.Producer;
        TVAgency = data.TVAgency;
        Rights = data.Rights;
        Formats = data.Formats;

        StudioDays = data.StudioDays;
        LocationDays = data.LocationDays;
        OutsideDays = data.OutsideDays;
        WeekendHolidays = data.WeekendHolidays;

        PostProdDuration = data.PostProdDuration;
        PostProdVersions = data.PostProdVersions;
        PostProdSound = data.PostProdSound;
        PostProdNVoices = data.PostProdNVoices;

        Comments = data.Comments;
        CommentsEnglish = data.CommentsEnglish;
    }

    public virtual void UpdateFinalData(BudgetFinalData data)
    {
        if (State != BudgetState.OPEN)
        {
            throw new ConsistencyException(TypeHelper.GetProperty((Budget i) => i.Groups),
                EntityErrors.UPDATE_BUDGET_NOT_OPEN, "Update Budget: budget not open");
        }

        ProducerPercent = data.ProducerPercent;
        BCAPercent = data.BCAPercent;

        NWeatherDays = data.NWeatherDays;
        WeatherTotal = data.WeatherTotal;
        WeatherPercent = data.WeatherPercent;
    }

    public virtual async Task UpdateDefinitionAsync(BudgetDefData def)
    {
        if (State != BudgetState.OPEN)
        {
            throw new ConsistencyException(TypeHelper.GetProperty((Budget i) => i.Groups),
                EntityErrors.UPDATE_BUDGET_NOT_OPEN, "Update Budget: budget not open");
        }

        await UpdateGroupDefsAsync(def.GroupDefData);
    }

    public virtual void UpdateData(BudgetData data)
    {
        if (State != BudgetState.OPEN)
        {
            throw new ConsistencyException(TypeHelper.GetProperty((Budget i) => i.Groups),
                EntityErrors.UPDATE_BUDGET_NOT_OPEN, "Update Budget: budget not open");
        }

        if (data.GroupData.Count() != groups.Count)
        {
            throw new InvalidArgsException(TypeHelper.GetProperty((Budget i) => i.Groups),
                EntityErrors.GROUPS_WRONG_LENGTH, "Update Groups: list of groups is the wrong length");
        }

        var groupBuffer = groups.ToDictionary(v => v.Order);
        foreach (var kv in data.GroupData.Select((v, i) => (i, v)))
        {
            groupBuffer[kv.i + 1].UpdateData(kv.v);
        }
    }

    public virtual async Task UpdateManagementAsync(int? nWeatherDays, BudgetData data)
    {
        if (State != BudgetState.LOCKED)
        {
            throw new ConsistencyException(TypeHelper.GetProperty((Budget i) => i.Groups),
                EntityErrors.MANAGE_BUDGET_NOT_LOCKED, "Manage Budget: budget not locked");
        }

        if (data.GroupData.Count() != groups.Count)
        {
            throw new InvalidArgsException(TypeHelper.GetProperty((Budget i) => i.Groups),
                EntityErrors.GROUPS_WRONG_LENGTH, "Update Groups: list of groups is the wrong length");
        }

        NWeatherDays = nWeatherDays;

        var groupBuffer = groups.ToDictionary(v => v.Order);
        foreach (var kv in data.GroupData.Select((v, i) => (i, v)))
        {
            await groupBuffer[kv.i + 1].UpdateManagementAsync(kv.v);
        }
    }

    public virtual void Lock()
    {
        if (IsTemplate)
        {
            throw new ConsistencyException(
                TypeHelper.GetProperty((Budget b) => b.State),
                EntityErrors.UPDATE_BUDGET_NOT_OPEN,
                "Attempt to lock template budget"
            );
        }

        State = State == BudgetState.OPEN ? BudgetState.LOCKED : State;
    }

    public virtual void Unlock() => State = State == BudgetState.LOCKED ? BudgetState.OPEN : State;

    public virtual void Close() => State = State == BudgetState.LOCKED ? BudgetState.CLOSED : State;

    public virtual void Reopen() => State = State == BudgetState.CLOSED ? BudgetState.LOCKED : State;

    public virtual void RemoveGroup(Group group)
    {
        groups.Remove(group);
    }

    public virtual async Task DeleteAsync()
    {
        if (State != BudgetState.OPEN)
        {
            throw new ConsistencyException(
                TypeHelper.GetProperty((Budget b) => b.State),
                EntityErrors.DELTE_BUDGET_NOT_OPEN,
                "Attempt to delete " + State.ToString() + "budget"
            );
        }

        foreach (var group in Groups)
        {
            await group.DeleteAsync();
        }

        await context.Persistence.DeleteAsync(this);
    }

    private async Task UpdateGroupDefsAsync(IEnumerable<GroupDefData> groupDefData)
    {
        var groupIds = groupDefData.Select(g => g.Id).ToHashSet();
        var toDelete = groups.Where(i => !groupIds.Contains(i.Id)).ToList();
        await Task.WhenAll(toDelete.Select(i => i.DeleteAsync()));

        var groupBuffer = groups.ToDictionary(v => v.Id);
        foreach (var kv in groupDefData.Select((v, i) => (i, v)))
        {
            if (groupBuffer.TryGetValue(kv.v.Id, out var group))
            {
                group.ReRank(kv.i + 1);
                await group.UpdateDefinitionAsync(kv.v);
            }
            else
            {
                CreateGroup(kv.v).ReRank(kv.i + 1);
            }
        }
    }

    private Group CreateGroup(GroupDefData def)
    {
        var group = new Group(context, this, groups.Count + 1, def);

        groups.Add(group);

        return group;
    }
}
