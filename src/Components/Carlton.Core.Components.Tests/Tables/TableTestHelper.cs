﻿using AutoFixture;
using Carlton.Core.Components.Tables;
using System.Globalization;
namespace Carlton.Core.Components.Tests.Tables;

public static class TableTestHelper
{
	public const int ItemCount = 15;
	public static readonly int[] RowsPerPageOpts = [5, 10, 15];

	public static IEnumerable<TableTestObject> GetItems()
	{
		var fixture = new Fixture();
		return Enumerable.Range(0, 15).Select(i => fixture.Create<TableTestObject>());
	}

	public static readonly IEnumerable<TableHeadingItem> TableTestHeadingItems =
		new List<TableHeadingItem>
		{
			new("ID"),
			new("DisplayName"),
			new("CreatedDate")
		};

	public record TableTestObject(int ID, string DisplayName, DateTime CreatedDate);

	public const string RowTemplate =
	@"
    <div class=""table-row"">
        <span class=""table-cell"">{0}</span>
        <span class=""table-cell"">{1}</span>
        <span class=""table-cell"">{2}</span>
    </div>";


	public static string BuildExpectedMarkup(
		IEnumerable<TableHeadingItem> headings,
		string rowTemplate,
		IEnumerable<TableTestObject> items,
		bool isZebraStriped,
		bool isHoverable,
		bool includePaginationRow,
		IEnumerable<int> rowsPerPageOpts,
		int currentPage,
		int selectedRowsPerPageIndex,
		int expectedOrderColumnIndex = -1,
		bool isAscending = true)
	{
		var rowsPerPage = rowsPerPageOpts.ElementAt(selectedRowsPerPageIndex);
		var skip = currentPage == 1 ? 0 : rowsPerPage * (currentPage - 1);

		var itemRows = includePaginationRow ? BuildExpectedPaginatedItemRows(items, rowTemplate, skip, rowsPerPage) :
			BuildAllExpectedItemRows(items, rowTemplate);

		return @$"
        <div class=""table-container {(isZebraStriped ? "zebra" : string.Empty)} {(isHoverable ? "hoverable" : string.Empty)}"">
            {BuildExpectedHeaderMarkup(headings, isAscending, expectedOrderColumnIndex)}
        <div class=""table-body"">
            {itemRows}
        </div>
            {(includePaginationRow ? BuildExpectedPaginationRow(items.Count(), currentPage, rowsPerPageOpts, selectedRowsPerPageIndex) : string.Empty)}";
	}

	public static string BuildExpectedHeaderMarkup(IEnumerable<TableHeadingItem> headings, bool isAscending, int selectedOrderIndex = -1)
	{
		var headerCells = string.Join(Environment.NewLine, headings.Select((item, i) =>
		@$"
            <div class=""header-cell table-cell {(i == selectedOrderIndex ? "selected" : string.Empty)} {(isAscending ? "ascending" : "descending")} heading-{i}"">
                <div class=""heading-container"">
                    <span class=""heading-text"">{item.DisplayName}</span>
                     <div class=""sort-arrows"">
                        <span class=""arrow-ascending mdi mdi-arrow-up""></span>
                        <span class=""arrow-descending mdi mdi-arrow-down""></span>
                     </div>
                </div>
        </div>"));

		return $@"<div class=""header-row table-row"">{headerCells}</div>";
	}

	public static string BuildAllExpectedItemRows(IEnumerable<TableTestObject> items, string rowTemplate)
	{
		return string.Join(Environment.NewLine, items.Select(item => string.Format(rowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture))));
	}

	public static string BuildExpectedPaginatedItemRows(IEnumerable<TableTestObject> items, string rowTemplate, int skip, int rowsPerPage)
	{
		return string.Join(Environment.NewLine, items.Skip(skip).Take(rowsPerPage).Select(item => string.Format(rowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture))));
	}

	public static string BuildExpectedPaginationRow(int itemTotal, int currentPage, IEnumerable<int> rowsPerPage, int selectedRowsPerPageIndex)
	{
		var selectedRowsPerPage = rowsPerPage.ElementAt(selectedRowsPerPageIndex);
		var numOfPages = Math.Ceiling((decimal)itemTotal / selectedRowsPerPage);
		var leftDisabled = currentPage == 1;
		var rightDisabled = currentPage == numOfPages;

		var optionsMarkup = string.Join(Environment.NewLine, rowsPerPage.Select(_ => $@"<div class=""option"">{_}</div>"));
		var startPageCount = 1 + ((currentPage - 1) * selectedRowsPerPage);
		var endPageCount = Math.Min((selectedRowsPerPage * currentPage), itemTotal);

		return
@$"
    <div class=""pagination-row table-row"">
        <div class=""rows-per-page"">
            <span class=""rows-per-page-label"">Rows Per Page</span>
            <div class=""dropdown"">
                <input readonly class=""dropdown-input"" placeholder="" "" value=""{selectedRowsPerPage}"" />
                <div class=""label""></div>
                <div class=""options"">
                    {optionsMarkup}
                </div>
            </div>
        </div>
        <div class=""page-number"">
            <span class=""pagination-label"">{startPageCount}-{endPageCount} of {itemTotal}</span>
        </div>
        <div class=""page-chevrons"">
          <span class=""mdi mdi-18px mdi-page-first {(leftDisabled ? "disabled" : string.Empty)}""></span>
          <span class=""mdi mdi-18px mdi-chevron-left {(leftDisabled ? "disabled" : string.Empty)}""></span>
          <span class=""mdi mdi-18px mdi-chevron-right {(rightDisabled ? "disabled" : string.Empty)}""></span>
          <span class=""mdi mdi-18px mdi-page-last {(rightDisabled ? "disabled" : string.Empty)}""></span>
        </div>
    </div>
    ";
	}

	public static readonly IReadOnlyCollection<TableHeadingItem> Headings = new List<TableHeadingItem>
	{
			new(nameof(TableTestObject.ID)),
			new(nameof(TableTestObject.DisplayName)),
			new(nameof(TableTestObject.CreatedDate))
	};

	public static IOrderedEnumerable<TableTestObject> OrderCollection(IEnumerable<TableTestObject> items, int columnIndex)
	{
		return columnIndex switch
		{
			0 => items.OrderBy(p => p.ID),
			1 => items.OrderBy(p => p.DisplayName),
			2 => items.OrderBy(p => p.CreatedDate),
			_ => throw new Exception(),
		};
	}

	public static IEnumerable<TableTestObject> OrderCollectionDesc(IEnumerable<TableTestObject> items, int columnIndex)
	{
		return columnIndex switch
		{
			0 => items.OrderByDescending(p => p.ID),
			1 => items.OrderByDescending(p => p.DisplayName),
			2 => items.OrderByDescending(p => p.CreatedDate),
			_ => throw new Exception(),
		};
	}
}
