/**
 * @version     1.7.0
 * @package     sellacious
 *
 * @copyright   Copyright (C) 2012-2019 Bhartiy Web Technologies. All rights reserved.
 * @license     GNU General Public License version 2 or later; see LICENSE.txt
 * @author      Izhar Aazmi <info@bhartiy.com> - http://www.bhartiy.com
 */
jQuery(document).ready(function ($) {
	$('.btn-install').click(function () {
		var url = $(this).data('url');
		var code = $(this).data('lang');
		$(this).closest('form').append($('<input>', {type: 'hidden', name: 'install_url', value: url}));
		$(this).closest('form').append($('<input>', {type: 'hidden', name: 'lang_code', value: code}));
		Joomla.submitbutton('install.install');
	});
});
