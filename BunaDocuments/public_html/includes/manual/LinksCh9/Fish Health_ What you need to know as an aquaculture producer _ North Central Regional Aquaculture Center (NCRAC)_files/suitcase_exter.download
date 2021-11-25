/**
 * Function for adding a class to external links
 * so they can be styled.
 */

(function ($) {

	$(document).ready(function() {

		$('a').filter(function() {
			return this.hostname && this.hostname !== location.hostname;
		}).not('a:has(img)').addClass('external');
	});

})(jQuery);
