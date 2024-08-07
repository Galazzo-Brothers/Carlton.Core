﻿using System.Globalization;
using System.Reflection;
using Carlton.Core.Components.Dropdowns;
using Carlton.Core.Components.Tables;
using Carlton.Core.Foundation.State;
using static Carlton.Core.Components.Tests.Tables.TableTestHelper;
namespace Carlton.Core.Components.Tests.Tables;

[Trait("Component", nameof(Table<int>))]
public class TableComponentTests : TestContext
{
	[Theory(DisplayName = "Markup Test")]
	[InlineAutoData(false, false)]
	[InlineAutoData(true, false)]
	[InlineAutoData(false, true)]
	[InlineAutoData(true, true)]
	public void Table_Markup_RendersCorrectly(
		bool expectedIsZebraStriped,
		bool expectedIsHoverable,
		IEnumerable<TableTestObject> expectedItems)
	{
		//Arrange
		var expectedMarkup = BuildExpectedMarkup(
			TableTestHeadingItems,
			RowTemplate,
			expectedItems,
			expectedIsZebraStriped,
			expectedIsHoverable,
			false,
			RowsPerPageOpts,
			0,
			0);

		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, TableTestHeadingItems)
			.Add(p => p.Items, expectedItems)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ZebraStripped, expectedIsZebraStriped)
			.Add(p => p.Hoverable, expectedIsHoverable)
			.Add(p => p.ShowPaginationRow, false));

		//Assert
		cut.MarkupMatches(expectedMarkup);
	}

	[Theory(DisplayName = "Markup Test")]
	[InlineAutoData(1, 0, false, false)] //15 Items, Page 1, 5 Rows PerPage 
	[InlineAutoData(2, 0, false, false)] //15 Items, Page 2, 5 Rows PerPage 
	[InlineAutoData(3, 0, false, false)] //15 Items, Page 3, 5 Rows PerPage 
	[InlineAutoData(1, 1, false, false)] //15 Items, Page 1, 10 Rows PerPage 
	[InlineAutoData(2, 1, false, false)] //15 Items, Page 2, 10 Rows PerPage 
	[InlineAutoData(1, 2, false, false)] //15 Items, Page 1, 15 Rows PerPage 
	[InlineAutoData(1, 0, true, false)] //15 Items, Page 1, 5 Rows PerPage 
	[InlineAutoData(2, 0, true, false)] //15 Items, Page 2, 5 Rows PerPage 
	[InlineAutoData(3, 0, true, false)] //15 Items, Page 3, 5 Rows PerPage 
	[InlineAutoData(1, 1, true, false)] //15 Items, Page 1, 10 Rows PerPage 
	[InlineAutoData(2, 1, true, false)] //15 Items, Page 2, 10 Rows PerPage 
	[InlineAutoData(1, 2, true, false)] //15 Items, Page 1, 15 Rows PerPage
	[InlineAutoData(1, 0, false, true)] //15 Items, Page 1, 5 Rows PerPage 
	[InlineAutoData(2, 0, false, true)] //15 Items, Page 2, 5 Rows PerPage 
	[InlineAutoData(3, 0, false, true)] //15 Items, Page 3, 5 Rows PerPage 
	[InlineAutoData(1, 1, false, true)] //15 Items, Page 1, 10 Rows PerPage 
	[InlineAutoData(2, 1, false, true)] //15 Items, Page 2, 10 Rows PerPage 
	[InlineAutoData(1, 2, false, true)] //15 Items, Page 1, 15 Rows PerPage 
	[InlineAutoData(1, 0, true, true)] //15 Items, Page 1, 5 Rows PerPage 
	[InlineAutoData(2, 0, true, true)] //15 Items, Page 2, 5 Rows PerPage 
	[InlineAutoData(3, 0, true, true)] //15 Items, Page 3, 5 Rows PerPage 
	[InlineAutoData(1, 1, true, true)] //15 Items, Page 1, 10 Rows PerPage 
	[InlineAutoData(2, 1, true, true)] //15 Items, Page 2, 10 Rows PerPage 
	[InlineAutoData(1, 2, true, true)] //15 Items, Page 1, 15 Rows PerPage 
	public void Table_Markup_WithPagination_RendersCorrectly(
		int expectedCurrentPage,
		int expectedRowsPerPageIndex,
		bool expectedIsZebraStriped,
		bool expectedIsHoverable)
	{
		//Arrange
		var tableInteractionState = new TableInteractionState
		{
			CurrentPage = expectedCurrentPage,
			SelectedRowsPerPageIndex = expectedRowsPerPageIndex
		};
		var items = GetItems().ToList();
		var expectedMarkup = BuildExpectedMarkup(
			TableTestHeadingItems,
			RowTemplate,
			items,
			expectedIsZebraStriped,
			expectedIsHoverable,
			true,
			RowsPerPageOpts,
			expectedCurrentPage,
			expectedRowsPerPageIndex);

		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, TableTestHeadingItems)
			.Add(p => p.Items, items)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ZebraStripped, expectedIsZebraStriped)
			.Add(p => p.Hoverable, expectedIsHoverable)
			.Add(p => p.ShowPaginationRow, true)
			.Add(p => p.RowsPerPageOptions, RowsPerPageOpts)
			.Add(p => p.TableInteractionState, tableInteractionState));

		//Assert
		cut.MarkupMatches(expectedMarkup);
	}

	[Theory(DisplayName = "Ordered Headers Markup Test")]
	[InlineAutoData("ID", 0, true)]
	[InlineAutoData("ID", 0, false)]
	[InlineAutoData("DisplayName", 1, true)]
	[InlineAutoData("DisplayName", 1, false)]
	[InlineAutoData("CreatedDate", 2, true)]
	[InlineAutoData("CreatedDate", 2, false)]
	public void Table_Markup_OrderedHeadersSelected_RendersCorrectly(
		string expectedOrderColumn,
		int expectedOrderColumnIndex,
		bool expectedOrderAscending,
		IEnumerable<TableTestObject> expectedItems,
		bool expectedIsZebraStriped,
		bool expectedIsHoverable,
		bool expectedShowPaginationRow,
		IEnumerable<int> expectedRowsPerPageOpts)
	{
		//Arrange
		var expectedOrderedItems = expectedOrderAscending ? OrderCollection(expectedItems, expectedOrderColumnIndex) : OrderCollectionDesc(expectedItems, expectedOrderColumnIndex);
		var selectedRowsPerPageIndex = RandomUtilities.GetRandomIndex(expectedRowsPerPageOpts.Count());
		var maxPages = Math.Max(1, (int)Math.Ceiling((double)expectedItems.Count() / expectedRowsPerPageOpts.ElementAt(selectedRowsPerPageIndex)));
		var currentPage = RandomUtilities.GetRandomNonZeroIndex(maxPages);
		var tableInteractionState = new TableInteractionState
		{
			OrderByColumn = expectedOrderColumn,
			IsAscending = expectedOrderAscending,
			CurrentPage = currentPage,
			SelectedRowsPerPageIndex = selectedRowsPerPageIndex
		};
		var expectedMarkup = BuildExpectedMarkup(
			TableTestHeadingItems,
			RowTemplate,
			expectedOrderedItems,
			expectedIsZebraStriped,
			expectedIsHoverable,
			false,
			expectedRowsPerPageOpts,
			currentPage,
			selectedRowsPerPageIndex,
			expectedOrderColumnIndex,
			expectedOrderAscending);

		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, TableTestHeadingItems)
			.Add(p => p.Items, expectedOrderedItems)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ZebraStripped, expectedIsZebraStriped)
			.Add(p => p.Hoverable, expectedIsHoverable)
			.Add(p => p.ShowPaginationRow, expectedShowPaginationRow)
			.Add(p => p.RowsPerPageOptions, expectedRowsPerPageOpts)
			.Add(p => p.TableInteractionState, tableInteractionState));

		//Assert
		cut.MarkupMatches(expectedMarkup);
	}

	[Theory(DisplayName = "Headings Parameter Test"), AutoData]
	public void Table_HeadingsParameter_RendersCorrectly(
		IEnumerable<TableHeadingItem> expectedHeadings,
		IEnumerable<TableTestObject> expectedItems,
		IEnumerable<int> rowsPerPageOpts)
	{
		//Arrange
		var expectedCount = expectedHeadings.Count();
		var expectedHeaders = expectedHeadings.Select(x => x.DisplayName);

		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, expectedHeadings)
			.Add(p => p.Items, expectedItems)
			.Add(p => p.RowsPerPageOptions, rowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true));

		var headerRowItems = cut.FindAll(".header-cell");
		var actualCount = headerRowItems.Count;
		var actualHeaders = cut.FindAll(".heading-text").Select(x => x.TextContent);

		//Assert
		actualCount.ShouldBe(expectedCount);
		actualHeaders.ShouldBe(expectedHeaders);
	}

	[Theory(DisplayName = "Items Parameter Test"), AutoData]
	public void Table_ItemsParameter_RendersCorrectly(
		IEnumerable<TableHeadingItem> expectedHeadings,
		IEnumerable<TableTestObject> expectedItems,
		IEnumerable<int> expectedRowsPerPageOpts)
	{
		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, expectedHeadings)
			.Add(p => p.Items, expectedItems)
			.Add(p => p.RowsPerPageOptions, expectedRowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, false));

		var tableRows = cut.FindAll(".table-row");
		var actualItemCount = tableRows.Count - 1; //Exclude the header row
		var expectedDisplayValues = expectedItems.Select(item => new TableTestObject(item.ID, item.DisplayName, item.CreatedDate))
			.SelectMany(_ => new[] { _.ID.ToString(), _.DisplayName, _.CreatedDate.ToString("d", CultureInfo.InvariantCulture) });
		var actualDisplayValues = cut.FindAll("span.table-cell").Select(x => x.TextContent);

		//Assert
		actualItemCount.ShouldBe(expectedItems.Count());
		actualDisplayValues.ShouldBe(expectedDisplayValues);
	}

	[Theory(DisplayName = "Items Parameter with Pagination Test")]
	[InlineAutoData(1, 0)] //15 Items, Page 1, 5 Rows PerPage 
	[InlineAutoData(2, 0)] //15 Items, Page 2, 5 Rows PerPage 
	[InlineAutoData(3, 0)] //15 Items, Page 3, 5 Rows PerPage 
	[InlineAutoData(1, 1)] //15 Items, Page 1, 10 Rows PerPage 
	[InlineAutoData(2, 1)] //15 Items, Page 2, 10 Rows PerPage 
	[InlineAutoData(1, 2)] //15 Items, Page 1, 15 Rows PerPage 
	public void Table_ItemsParameter_WithPagination_RendersCorrectly(
	   int expectedCurrentPage,
	   int expectedRowsPerPageIndex)
	{
		//Arrange
		var rowsPerPage = RowsPerPageOpts.ElementAt(expectedRowsPerPageIndex);
		var skip = expectedCurrentPage == 1 ? 0 : rowsPerPage * (expectedCurrentPage - 1);
		var items = GetItems().ToList();
		var expectedItems = items.Skip(skip).Take(rowsPerPage);
		var expectedCount = expectedItems.Count();
		var tableInteractionState = new TableInteractionState
		{
			CurrentPage = expectedCurrentPage,
			SelectedRowsPerPageIndex = expectedRowsPerPageIndex
		};

		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, items)
			.Add(p => p.RowsPerPageOptions, RowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true)
			.Add(p => p.TableInteractionState, tableInteractionState));

		var tableRows = cut.FindAll(".table-row");
		var actualItemCount = tableRows.Count - 2; //Exclude the header row and pagination row
		var expectedDisplayValues = expectedItems.Select(item => new TableTestObject(item.ID, item.DisplayName, item.CreatedDate))
			.SelectMany(_ => new[] { _.ID.ToString(), _.DisplayName, _.CreatedDate.ToString("d", CultureInfo.InvariantCulture) });
		var actualDisplayValues = cut.FindAll("span.table-cell").Select(x => x.TextContent);

		//Assert
		actualItemCount.ShouldBe(expectedCount);
		actualDisplayValues.ShouldBe(expectedDisplayValues);
	}

	[Theory(DisplayName = "RowTemplate Parameter Test")]
	[InlineAutoData(@"<div class=""table-row""><span>Template {0}{1}{2}</span></div>")]
	[InlineAutoData(@"<div class=""table-row""><span>{0}Test Template {1}{2}</span></div>")]
	[InlineAutoData(@"<div class=""table-row"">{0}<span>{1}More Test Templates {2}</span></div>")]
	public void Table_RowTemplateParameter_RendersCorrectly(
		string expectedRowTemplate,
		List<TableTestObject> expectedItems,
		IEnumerable<TableHeadingItem> expectedHeadings,
		IEnumerable<int> expectedRowsPerPageOpts)
	{
		//Arrange
		var expectedContent = expectedItems.Select(_ => string.Format(expectedRowTemplate, _.ID, _.DisplayName, _.CreatedDate.ToString("d", CultureInfo.InvariantCulture)));

		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, expectedHeadings)
			.Add(p => p.Items, expectedItems)
			.Add(p => p.RowsPerPageOptions, expectedRowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(expectedRowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, false));

		var rowElements = cut.FindAll(".table-row");
		var itemRowElements = rowElements.Skip(1);

		var actualContent = itemRowElements.Select(x => x.OuterHtml);

		//Assert
		actualContent.ShouldBe(expectedContent);
	}

	[Theory(DisplayName = "RowTemplate Parameter With Pagination Test")]
	[InlineAutoData(1, 0, @"<div class=""table-row""><span>Template {0}{1}{2}</span></div>")] //15 Items, Page 1, 5 Rows PerPage 
	[InlineAutoData(2, 0, @"<div class=""table-row""><span>Template {0}{1}{2}</span></div>")] //15 Items, Page 2, 5 Rows PerPage 
	[InlineAutoData(3, 0, @"<div class=""table-row""><span>Template {0}{1}{2}</span></div>")] //15 Items, Page 3, 5 Rows PerPage 
	[InlineAutoData(1, 1, @"<div class=""table-row""><span>Template {0}{1}{2}</span></div>")] //15 Items, Page 1, 10 Rows PerPage 
	[InlineAutoData(2, 1, @"<div class=""table-row""><span>Template {0}{1}{2}</span></div>")] //15 Items, Page 2, 10 Rows PerPage 
	[InlineAutoData(1, 2, @"<div class=""table-row""><span>Template {0}{1}{2}</span></div>")] //15 Items, Page 1, 15 Rows PerPage 
	[InlineAutoData(1, 0, @"<div class=""table-row""><span>{0}Test Template {1}{2}</span></div>")] //15 Items, Page 1, 5 Rows PerPage 
	[InlineAutoData(2, 0, @"<div class=""table-row""><span>{0}Test Template {1}{2}</span></div>")] //15 Items, Page 2, 5 Rows PerPage 
	[InlineAutoData(3, 0, @"<div class=""table-row""><span>{0}Test Template {1}{2}</span></div>")] //15 Items, Page 3, 5 Rows PerPage 
	[InlineAutoData(1, 1, @"<div class=""table-row""><span>{0}Test Template {1}{2}</span></div>")] //15 Items, Page 1, 10 Rows PerPage 
	[InlineAutoData(2, 1, @"<div class=""table-row""><span>{0}Test Template {1}{2}</span></div>")] //15 Items, Page 2, 10 Rows PerPage 
	[InlineAutoData(1, 2, @"<div class=""table-row""><span>{0}Test Template {1}{2}</span></div>")] //15 Items, Page 1, 15 Rows PerPage 
	[InlineAutoData(1, 0, @"<div class=""table-row"">{0}<span>{1}More Test Templates {2}</span></div>")] //15 Items, Page 1, 5 Rows PerPage 
	[InlineAutoData(2, 0, @"<div class=""table-row"">{0}<span>{1}More Test Templates {2}</span></div>")] //15 Items, Page 2, 5 Rows PerPage 
	[InlineAutoData(3, 0, @"<div class=""table-row"">{0}<span>{1}More Test Templates {2}</span></div>")] //15 Items, Page 3, 5 Rows PerPage 
	[InlineAutoData(1, 1, @"<div class=""table-row"">{0}<span>{1}More Test Templates {2}</span></div>")] //15 Items, Page 1, 10 Rows PerPage 
	[InlineAutoData(2, 1, @"<div class=""table-row"">{0}<span>{1}More Test Templates {2}</span></div>")] //15 Items, Page 2, 10 Rows PerPage 
	[InlineAutoData(1, 2, @"<div class=""table-row"">{0}<span>{1}More Test Templates {2}</span></div>")] //15 Items, Page 1, 15 Rows PerPage 
	public void Table_RowTemplateParameter_WithPagination_RendersCorrectly(
	  int expectedPage,
	  int expectedRowsPerPageIndex,
	  string expectedRowTemplate)
	{
		//Arrange
		var rowsPerPage = RowsPerPageOpts.ElementAt(expectedRowsPerPageIndex);
		var skip = expectedPage == 1 ? 0 : rowsPerPage * (expectedPage - 1);
		var items = GetItems().ToList();
		var expectedItems = items.Skip(skip).Take(rowsPerPage);
		var expectedContent = expectedItems.Select(_ => string.Format(expectedRowTemplate, _.ID, _.DisplayName, _.CreatedDate.ToString("d", CultureInfo.InvariantCulture)));
		var tableInteractionState = new TableInteractionState
		{
			CurrentPage = expectedPage,
			SelectedRowsPerPageIndex = expectedRowsPerPageIndex
		};

		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, items)
			.Add(p => p.RowsPerPageOptions, RowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(expectedRowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true)
			.Add(p => p.TableInteractionState, tableInteractionState));

		var rowElements = cut.FindAll(".table-row");
		var itemRowElements = rowElements.Skip(1).Take(rowElements.Count - 2); //Throw away header and pagination row
		var actualContent = itemRowElements.Select(x => x.OuterHtml);

		//Assert
		actualContent.ShouldBe(expectedContent);
	}

	[Theory(DisplayName = "ZebraStriped Parameter Test")]
	[InlineAutoData(true)]
	[InlineAutoData(false)]
	public void Table_ZebraStripedParameter_RendersCorrectly(
		bool expectedZebraStriped,
		string expectedRowTemplate,
		List<TableTestObject> expectedItems,
		IEnumerable<TableHeadingItem> expectedHeadings,
		IEnumerable<int> expectedRowsPerPage,
		bool expectedShowPaginationRow)
	{
		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, expectedHeadings)
			.Add(p => p.Items, expectedItems)
			.Add(p => p.ZebraStripped, expectedZebraStriped)
			.Add(p => p.RowsPerPageOptions, expectedRowsPerPage)
			.Add(p => p.RowTemplate, item => string.Format(expectedRowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, expectedShowPaginationRow));

		var tableElement = cut.Find(".table-container");
		var zebraClassPresent = tableElement.ClassList.Contains("zebra");

		//Assert
		zebraClassPresent.ShouldBe(expectedZebraStriped);
	}

	[Theory(DisplayName = "Hoverable Parameter Test")]
	[InlineAutoData(true)]
	[InlineAutoData(false)]
	public void Table_HoverableParameter_RendersCorrectly(
		bool expectedHoverable,
		bool expectedZebraStriped,
		string expectedRowTemplate,
		List<TableTestObject> expectedItems,
		IEnumerable<TableHeadingItem> expectedHeadings,
		IEnumerable<int> expectedRowsPerPage,
		bool expectedShowPaginationRow)
	{
		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, expectedHeadings)
			.Add(p => p.Items, expectedItems)
			.Add(p => p.ZebraStripped, expectedZebraStriped)
			.Add(p => p.Hoverable, expectedHoverable)
			.Add(p => p.RowsPerPageOptions, expectedRowsPerPage)
			.Add(p => p.RowTemplate, item => string.Format(expectedRowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, expectedShowPaginationRow));

		var tableElement = cut.Find(".table-container");
		var hoverableClassPresent = tableElement.ClassList.Contains("hoverable");

		//Assert
		hoverableClassPresent.ShouldBe(expectedHoverable);
	}

	[Theory(DisplayName = "ShowPaginationRow Parameter Test")]
	[InlineAutoData(true)]
	[InlineAutoData(false)]
	public void Table_ShowPaginationRowParameter_RendersCorrectly(
		bool expectedShowPaginationRow,
		IEnumerable<TableHeadingItem> expectedHeadings,
		IEnumerable<TableTestObject> expectedItems,
		IEnumerable<int> expectedRowsPerPage)
	{
		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, expectedHeadings)
			.Add(p => p.Items, expectedItems)
			.Add(p => p.RowsPerPageOptions, expectedRowsPerPage)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString(CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, expectedShowPaginationRow));

		var paginationRowExists = cut.HasComponent<TablePaginationRow<TableTestObject>>();

		//Assert
		paginationRowExists.ShouldBe(expectedShowPaginationRow);
	}

	[Theory(DisplayName = "CurrentPage Parameter Test")]
	[InlineAutoData(1, 0, "1-5 of 15")] //15 Items, Page 1, 5 Rows PerPage 
	[InlineAutoData(2, 0, "6-10 of 15")] //15 Items, Page 2, 5 Rows PerPage 
	[InlineAutoData(3, 0, "11-15 of 15")] //15 Items, Page 3, 5 Rows PerPage 
	[InlineAutoData(1, 1, "1-10 of 15")] //15 Items, Page 1, 10 Rows PerPage 
	[InlineAutoData(2, 1, "11-15 of 15")] //15 Items, Page 2, 10 Rows PerPage 
	[InlineAutoData(1, 2, "1-15 of 15")] //15 Items, Page 1, 15 Rows PerPage 
	public void Table_CurrentPageParameter_RendersCorrectly(
	  int expectedCurrentPage,
	  int expectedSelectedRowsPerPageIndex,
	  string expectedLabel)
	{
		//Arrange
		var tableInteractionState = new TableInteractionState
		{
			CurrentPage = expectedCurrentPage,
			SelectedRowsPerPageIndex = expectedSelectedRowsPerPageIndex
		};

		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, GetItems())
			.Add(p => p.RowsPerPageOptions, RowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString(CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true)
			.Add(p => p.TableInteractionState, tableInteractionState));

		var paginationLabelElement = cut.Find(".pagination-label");

		//Assert
		paginationLabelElement.TextContent.ShouldBe(expectedLabel);
	}

	[Theory(DisplayName = "RowsPerPageOpts Parameter Test"), AutoData]
	public void Table_RowsPerPageOptsParameter_RendersCorrectly(
		IEnumerable<TableTestObject> expectedItems,
		IEnumerable<int> expectedRowsPerPageOpts)
	{
		//Arrange
		var expectedCount = expectedRowsPerPageOpts.Count();
		var expectedRowsPerPageValues = expectedRowsPerPageOpts.Select(x => x);

		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, expectedItems)
			.Add(p => p.RowsPerPageOptions, expectedRowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString(CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true));

		var options = cut.FindAll(".option");
		var actualCount = options.Count;
		var actualValues = options.Select(_ => int.Parse(_.TextContent));

		//Assert
		actualCount.ShouldBe(expectedCount);
		actualValues.ShouldBe(expectedRowsPerPageValues);
	}


	[Theory(DisplayName = "SelectedRowsPerPageIndex Parameter Test")]
	[InlineAutoData(1, 0)] //15 Items, Page 1, 5 Rows PerPage 
	[InlineAutoData(2, 0)] //15 Items, Page 2, 5 Rows PerPage 
	[InlineAutoData(3, 0)] //15 Items, Page 3, 5 Rows PerPage 
	[InlineAutoData(1, 1)] //15 Items, Page 1, 10 Rows PerPage 
	[InlineAutoData(2, 1)] //15 Items, Page 2, 10 Rows PerPage 
	[InlineAutoData(1, 2)] //15 Items, Page 1, 15 Rows PerPage 
	public void Table_SelectedRowsPageIndexParameter_RendersCorrectly(
	   int expectedCurrentPage,
	   int expectedSelectedRowsPerPageIndex)
	{
		//Arrange
		var tableInteractionState = new TableInteractionState
		{
			CurrentPage = expectedCurrentPage,
			SelectedRowsPerPageIndex = expectedSelectedRowsPerPageIndex
		};

		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, GetItems())
			.Add(p => p.RowsPerPageOptions, RowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString(CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true)
			.Add(p => p.TableInteractionState, tableInteractionState));

		var dropdownComponent = cut.FindComponent<Dropdown<int>>();

		//Assert
		dropdownComponent.Instance.SelectedIndex.ShouldBe(expectedSelectedRowsPerPageIndex);
		dropdownComponent.Instance.SelectedValue.ShouldBe(RowsPerPageOpts.ElementAt(expectedSelectedRowsPerPageIndex));
	}

	[Theory(DisplayName = "SortableHeaders Enabled Parameter Test")]
	[InlineData(true)]
	[InlineData(false)]
	public void Table_Header_SortableHeadersEnabledParameter_RendersCorrectly(
		bool expectedSortableHeadersEnabled)
	{
		//Arrange
		var headerIndex = RandomUtilities.GetRandomIndex(Headings.Count);
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, GetItems())
			.Add(p => p.RowsPerPageOptions, RowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true)
			.Add(p => p.SortableHeadersEnabled, expectedSortableHeadersEnabled));

		var headerRowItems = cut.FindAll(".header-cell");
		var selectedHeaders = cut.FindAll(".header-cell.selected", true);

		//Act
		headerRowItems.ElementAt(headerIndex).Click();


		//Assert
		selectedHeaders.Any().ShouldBe(expectedSortableHeadersEnabled);
	}

	[Theory(DisplayName = "OrderColumn Parameter Test")]
	[InlineData("ID", true)]
	[InlineData("ID", false)]
	[InlineData("DisplayName", true)]
	[InlineData("DisplayName", false)]
	[InlineData("CreatedDate", true)]
	[InlineData("CreatedDate", false)]
	public void Table_OrderColumnParameter_RendersCorrectly(
		string expectedOrderColumn,
		bool expectedOrderAscending)
	{
		//Arrange
		var tableInteractionState = new TableInteractionState
		{
			OrderByColumn = expectedOrderColumn,
			IsAscending = expectedOrderAscending
		};

		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, GetItems())
			.Add(p => p.RowsPerPageOptions, RowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString(CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true)
			.Add(p => p.TableInteractionState, tableInteractionState));

		var tableHeader = cut.FindComponent<TableHeader<TableTestObject>>();

		//Assert
		tableHeader.Instance.SelectedOrderColumn.ShouldBe(expectedOrderColumn);
	}

	[Theory(DisplayName = "OrderAscending Parameter Test")]
	[InlineData("ID", true)]
	[InlineData("ID", false)]
	[InlineData("DisplayName", true)]
	[InlineData("DisplayName", false)]
	[InlineData("CreatedDate", true)]
	[InlineData("CreatedDate", false)]
	public void Table_OrderAscendingParameter_RendersCorrectly(
	   string expectedOrderColumn,
	   bool expectedOrderAscending)
	{
		//Arrange
		var tableInteractionState = new TableInteractionState
		{
			OrderByColumn = expectedOrderColumn,
			IsAscending = expectedOrderAscending
		};

		//Act
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, GetItems())
			.Add(p => p.RowsPerPageOptions, RowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString(CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true)
			.Add(p => p.TableInteractionState, tableInteractionState));

		var tableHeader = cut.FindComponent<TableHeader<TableTestObject>>();

		//Assert
		tableHeader.Instance.IsAscending.ShouldBe(expectedOrderAscending);
	}

	[Theory(DisplayName = "PageChangedCallback Parameter Test")]
	[InlineAutoData(1)] //15 Rows, Page 1, 5 Rows PerPage
	[InlineAutoData(2)] //15 Rows, Page 2, 5 Rows PerPage
	public void Table_PageChangedCallbackParameter_FiresEvent(
	   int expectedCurrentPage)
	{
		//Arrange
		var eventFired = false;
		var actualPage = 1;
		var tableInteractionState = new TableInteractionState
		{
			CurrentPage = expectedCurrentPage
		};
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, GetItems())
			.Add(p => p.RowsPerPageOptions, RowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString(CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true)
			.Add(p => p.TableInteractionState, tableInteractionState)
			.Add(p => p.OnTableInteraction, args =>
			{
				eventFired = true;
				actualPage = args.CurrentPage;
			}));

		var rightChevron = cut.Find(".mdi-chevron-right");

		//Act
		rightChevron.Click();

		//Assert
		eventFired.ShouldBeTrue();
		actualPage.ShouldBe(expectedCurrentPage + 1);
	}

	[Theory(DisplayName = "PageChangedCallback Parameter Test")]
	[InlineAutoData(3)] //15 Rows, Page 3, 5 Rows PerPage
	public void Table_OnLastPage_PageChangedCallbackParameter_DoesNotFireEvent(
	   int expectedCurrentPage)
	{
		//Arrange
		var eventFired = false;
		var tableInteractionState = new TableInteractionState
		{
			CurrentPage = expectedCurrentPage
		};
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, GetItems())
			.Add(p => p.RowsPerPageOptions, RowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString(CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true)
			.Add(p => p.TableInteractionState, tableInteractionState)
			.Add(p => p.OnTableInteraction, args =>
			{
				eventFired = true;
			}));

		var rightChevron = cut.Find(".mdi-chevron-right");

		//Act
		rightChevron.Click();

		//Assert
		eventFired.ShouldBeFalse();
	}

	[Theory(DisplayName = "RowsPerPageChangedCallback Parameter Test")]
	[InlineAutoData(0)] //5 Rows PerPage 
	[InlineAutoData(1)] //10 Rows PerPage 
	[InlineAutoData(2)] //15 Rows PerPage 
	public void Table_RowsPerPageChangedCallbackParameter_FiresEvent(int expectedIndexToSelect)
	{
		//Arrange
		var eventFired = false;
		var actualSelectedRowsPerPageIndex = -1;
		var selectedRowsPerPageCount = RowsPerPageOpts.ElementAt(expectedIndexToSelect);
		var tableState = new TableInteractionState
		{
			SelectedRowsPerPageIndex = expectedIndexToSelect
		};

		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, GetItems())
			.Add(p => p.RowsPerPageOptions, RowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString(CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true)
			.Add(p => p.TableInteractionState, tableState)
			.Add(p => p.OnTableInteraction, args =>
			{
				eventFired = true;
				actualSelectedRowsPerPageIndex = args.SelectedRowsPerPageIndex;
			}));

		var options = cut.FindAll(".option");
		var optToClick = options.First(_ => int.Parse(_.TextContent) == selectedRowsPerPageCount);

		//Act
		optToClick.Click();

		//Assert
		eventFired.ShouldBeTrue();
		actualSelectedRowsPerPageIndex.ShouldBe(expectedIndexToSelect);
	}

	[Theory(DisplayName = "ItemsOrderedChangedCallback Parameter Test")]
	[InlineAutoData(0)] //1st Header 
	[InlineAutoData(1)] //2nd Header 
	[InlineAutoData(2)] //3rd Header 
	public void Table_ItemsOrderedCallbackParameter_FiresEvent(
	   int expectedColumnIndex)
	{
		//Arrange
		var eventFired = false;
		var actualOrderColumn = string.Empty;
		var actualOrderAscending = false;
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, GetItems())
			.Add(p => p.RowsPerPageOptions, RowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString(CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true)
			.Add(p => p.OnTableInteraction, args =>
			{
				eventFired = true;
				actualOrderColumn = args.OrderByColumn;
				actualOrderAscending = args.IsAscending;
			}));

		var headerRowItems = cut.FindAll(".header-cell");
		var expectedOrderColumn = Headings.ElementAt(expectedColumnIndex).OrderingName;

		//Act
		headerRowItems.ElementAt(expectedColumnIndex).Click();

		//Assert
		eventFired.ShouldBeTrue();
		actualOrderColumn.ShouldBe(expectedOrderColumn);
		actualOrderAscending.ShouldBe(true);
	}

	[Theory(DisplayName = "ItemsOrderedChangedCallback Descending Parameter Test")]
	[InlineAutoData(0)] //1st Header 
	[InlineAutoData(1)] //2nd Header 
	[InlineAutoData(2)] //3rd Header 
	public void Table_ItemsOrderedCallbackParameter_Desc_FiresEvent(
	  int expectedColumnIndex)
	{
		//Arrange
		var eventFired = false;
		var actualOrderColumn = string.Empty;
		var actualOrderAscending = true;

		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, GetItems())
			.Add(p => p.RowsPerPageOptions, RowsPerPageOpts)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString(CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true)
			.Add(p => p.OnTableInteraction, args =>
			{
				eventFired = true;
				actualOrderColumn = args.OrderByColumn;
				actualOrderAscending = args.IsAscending;
			}));

		var headerRowItems = cut.FindAll(".header-cell");
		var expectedOrderColumn = Headings.ElementAt(expectedColumnIndex).OrderingName;

		//Act
		var heading = headerRowItems.ElementAt(expectedColumnIndex);
		heading.Click();

		heading = cut.FindAll(".header-cell").ElementAt(expectedColumnIndex);
		heading.Click();

		//Assert
		eventFired.ShouldBeTrue();
		actualOrderColumn.ShouldBe(expectedOrderColumn);
		actualOrderAscending.ShouldBe(false);
	}

	[Theory(DisplayName = "Header Click Once Test")]
	[InlineAutoData(0)] //1st Header 
	[InlineAutoData(1)] //2nd Header
	[InlineAutoData(2)] //3rd Header
	public void Table_Header_OnClickOnce_FiltersItemsAsc(
	   int expectedColumnIndex,
	   IEnumerable<TableTestObject> items,
	   IEnumerable<int> expectedRowsPerPage)
	{
		//Arrange
		var filteredItemsProp = typeof(Table<TableTestObject>).GetProperty("FilteredItems", BindingFlags.NonPublic | BindingFlags.Instance);
		var expectedItems = OrderCollection(items, expectedColumnIndex);
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, items)
			.Add(p => p.RowsPerPageOptions, expectedRowsPerPage)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true));

		var headerRowItems = cut.FindAll(".header-cell");

		//Act
		headerRowItems.ElementAt(expectedColumnIndex).Click();

		//Assert
		filteredItemsProp.GetValue(cut.Instance).ShouldBe(expectedItems);
	}

	[Theory(DisplayName = "Header Click Twice Test")]
	[InlineAutoData(0)] //1st Header 
	[InlineAutoData(1)] //2nd Header 
	[InlineAutoData(2)] //3rd Header 
	public async Task Table_Header_OnClickTwice_FiltersItemsDesc(
		int expectedColumnIndex,
		IEnumerable<TableTestObject> items,
		IEnumerable<int> expectedRowsPerPages)
	{
		//Arrange
		var filteredItemsProp = typeof(Table<TableTestObject>).GetProperty("FilteredItems", BindingFlags.NonPublic | BindingFlags.Instance);
		var expectedItems = OrderCollectionDesc(items, expectedColumnIndex);
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, items)
			.Add(p => p.RowsPerPageOptions, expectedRowsPerPages)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, true));

		//Act
		var itemToClick = cut.FindAll(".header-cell").ElementAt(expectedColumnIndex);
		await itemToClick.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
		cut.WaitForElement(".arrow-ascending");
		itemToClick = cut.FindAll(".header-cell").ElementAt(expectedColumnIndex);
		await itemToClick.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
		cut.WaitForElement(".arrow-descending");

		//Assert
		filteredItemsProp.GetValue(cut.Instance).ShouldBe(expectedItems);
	}

	[Theory(DisplayName = "Header Click Once, CSS Selected Class Test")]
	[InlineAutoData(0)] //1st Header 
	[InlineAutoData(1)] //2nd Header 
	[InlineAutoData(2)] //3rd Header 
	public void Table_HeaderItemOnClick_SelectedClass_RendersCorrectly(
		int selectedIndex,
		IEnumerable<TableTestObject> items,
		IEnumerable<int> rowsPerPage,
		bool showPaginationRow)
	{
		//Arrange
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, items)
			.Add(p => p.RowsPerPageOptions, rowsPerPage)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, showPaginationRow));

		var headerRowItems = cut.FindAll(".header-cell", true);
		var selectedItem = headerRowItems.ElementAt(selectedIndex);

		//Act
		selectedItem.Click();
		selectedItem = headerRowItems.ElementAt(selectedIndex);

		//Assert
		selectedItem.ClassList.ShouldContain("selected");
	}

	[Theory(DisplayName = "Header Click Once, CSS Ascending Class Test")]
	[InlineAutoData(0)] //1st Header 
	[InlineAutoData(1)] //2nd Header 
	[InlineAutoData(2)] //3rd Header 
	public void Table_HeaderItemOnClick_AscendingClass_RendersCorrectly(
		int selectedIndex,
		IEnumerable<TableTestObject> items,
		IEnumerable<int> rowsPerPage,
		bool showPaginationRow)
	{
		//Arrange
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, items)
			.Add(p => p.RowsPerPageOptions, rowsPerPage)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, showPaginationRow));

		var headerRowItems = cut.FindAll(".header-cell", true);
		var selectedItem = headerRowItems.ElementAt(selectedIndex);

		//Act
		selectedItem.Click();
		selectedItem = headerRowItems.ElementAt(selectedIndex);

		//Assert
		selectedItem.ClassList.ShouldContain("ascending");
	}

	[Theory(DisplayName = "Header Click Once, CSS Descending Class Test")]
	[InlineAutoData(0)] //1st Header 
	[InlineAutoData(1)] //2nd Header 
	[InlineAutoData(2)] //3rd Header 
	public void Table_HeaderItemOnClick_DescendingClass_RendersCorrectly(
		int selectedIndex,
		IEnumerable<TableTestObject> items,
		IEnumerable<int> rowsPerPage,
		bool showPaginationRow)
	{
		//Arrange
		var cut = RenderComponent<Table<TableTestObject>>(parameters => parameters
			.Add(p => p.Headings, Headings)
			.Add(p => p.Items, items)
			.Add(p => p.RowsPerPageOptions, rowsPerPage)
			.Add(p => p.RowTemplate, item => string.Format(RowTemplate, item.ID, item.DisplayName, item.CreatedDate.ToString("d", CultureInfo.InvariantCulture)))
			.Add(p => p.ShowPaginationRow, showPaginationRow));


		//Act
		cut.FindAll(".header-cell").ElementAt(selectedIndex).Click();
		cut.FindAll(".header-cell").ElementAt(selectedIndex).Click();
		var selectedItem = cut.FindAll(".header-cell").ElementAt(selectedIndex);

		//Assert
		selectedItem.ClassList.ShouldContain("descending");
	}
}