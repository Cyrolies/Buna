<?php
/**
 * @package Joomla template Framework
 * @author Ltheme https://www.ltheme.com
 * @copyright Copyright (c) Ltheme
 * @license http://www.gnu.org/licenses/gpl-2.0.html GNU/GPLv2 or Later
*/

defined ('JPATH_BASE') or die();

use Joomla\CMS\Language\Text;
use Joomla\CMS\Router\Route;
$title = $this->escape($displayData['item']->category_title);
?>
<span class="category-name" title="<?php echo Text::sprintf('COM_CONTENT_CATEGORY', $title); ?>">
	<?php if ($displayData['params']->get('link_category') && $displayData['item']->catslug) : ?>
		<a href="<?php echo Route::_(ContentHelperRoute::getCategoryRoute($displayData['item']->catslug)); ?>"><?php echo $title; ?></a>
	<?php else : ?>
		<?php echo $title; ?>
	<?php endif; ?>
</span>
