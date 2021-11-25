(function ($) {

$(document).ready(function() {
	
	// Find tables in content, or Views tables with the .views-table-responsive class
	var table = $('.field-items table, .views-table-responsive table');
	
/* --------------------- 
First, check the table 
structure and assign classes.
--------------------- */
	
	/* ----- None ----- */
	table.addClass('table-none');
	
	/* ----- Row ----- */
	var rowsHeader = $(table.has('tr:first-of-type th:nth-of-type(2)'));
	rowsHeader.attr('class', 'table-row');
		
	/* ----- Col ----- */
	var colHeader = $(table.has('tr:nth-of-type(2) th:nth-of-type(1)'));
	colHeader.attr('class', 'table-col');
	
	/* ----- Both ----- */
	var bothHeader = $(table.has('tr:nth-of-type(1) th:nth-of-type(2)').has('tr:nth-of-type(2) th:nth-of-type(1)'));
	bothHeader.attr('class', 'table-both');
	
	/* --- Colspan or Rowspan --- */
	var tdColFreeze = $(table.has('td[colspan]'));
	var thColFreeze = $(table.has('th[colspan]'));
	var tdRowFreeze = $(table.has('td[rowspan]'));
	var thRowFreeze = $(table.has('th[rowspan]'));
	tdColFreeze.attr('class', 'table-freeze');
	thColFreeze.attr('class', 'table-freeze');
	tdRowFreeze.attr('class', 'table-freeze');
	thRowFreeze.attr('class', 'table-freeze');

	/* --- All Responsive Tables --- */
	table.addClass('responsive-table');

/* --------------------- 
Now apply any jQuery needed
to make the tables responsive
--------------------- */

	/* ----- Row ----- */
	// Wrap cell content in a span for flexbox.
	$('.table-row td').wrapInner('<span class="cell-content"></span>');

	// Cycle through each row...
	$('.table-row tr').each(function() {
		// And cycle through each td in that row...
	 $(this).find('td').each(function(i) {
			// Find the content of the closest th...		
		 	var rowHeader = $(this).closest('.table-row').find('tr th')[i].innerHTML;
		 	// And add it as a span in the td.
		 	$(this).prepend('<span class="row-header" aria-hidden="true">' + rowHeader + '</span>')
		});
	});

	/* ----- BOTH ----- */
	// Wrap cell content in a span for flexbox.
	$('.table-both td').wrapInner('<span class="both-cell-content"></span>');

	// Cycle through each row...
	$('.table-both tr').each(function() {
		// And cycle through each td in that row...
		$(this).find('td').each(function(i) {
						
			// Find the content of the closest th...		
		 	var rowHeader = $(this).closest('.table-both').find('tr th:not(:first-of-type)')[i].innerHTML;
		 	// And add it as a span in the td.
		 	$(this).prepend('<span class="both-row-header" aria-hidden="true">' + rowHeader + '</span>')
		});
	});
});

})(jQuery);
