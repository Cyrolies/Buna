<?php
/**
 * @package Joomla template Framework
 * @author Ltheme https://www.ltheme.com
 * @copyright Copyright (c) Ltheme
 * @license http://www.gnu.org/licenses/gpl-2.0.html GNU/GPLv2 or Later
*/

defined ('_JEXEC') or die();

?>

<ol itemscope itemtype="https://schema.org/BreadcrumbList" class="breadcrumb">
	<?php if ($params->get('showHere', 1)) : ?>
		<li class="float-left">
			<?php echo JText::_('MOD_BREADCRUMBS_HERE'); ?>&#160;
		</li>
	<?php else : ?>
		<li class="float-left">
			<span class="divider fa fa-location" aria-hidden="true"></span>
		</li>
	<?php endif; ?>

	<?php
	// Get rid of duplicated entries on trail including home page when using multilanguage
	for ($i = 0; $i < $count; $i++)
	{
		if ($i === 1 && !empty($list[$i]->link) && !empty($list[$i - 1]->link) && $list[$i]->link === $list[$i - 1]->link)
		{
			unset($list[$i]);
		}
	}

	// Find last and penultimate items in breadcrumbs list
	end($list);
	$last_item_key   = key($list);
	prev($list);
	$penult_item_key = key($list);

	// Make a link if not the last item in the breadcrumbs
	$show_last = $params->get('showLast', 1);

	// Generate the trail
	foreach ($list as $key => $item) :
		if ($key !== $last_item_key) :
			if (!empty($item->link)) :
				$breadcrumbItem = '<a itemprop="item" href="' . $item->link . '" class="pathway"><span itemprop="name">' . $item->name . '</span></a>';
			else :
				$breadcrumbItem = '<span itemprop="name">' . $item->name . '</span>';
			endif;
			// Render all but last item - along with separator ?>
			<li itemprop="itemListElement" itemscope itemtype="https://schema.org/ListItem" class="breadcrumb-item"><?php echo $breadcrumbItem; ?>
				<meta itemprop="position" content="<?php echo $key + 1; ?>">
			</li>
		<?php elseif ($show_last) :
			$breadcrumbItem = '<span itemprop="name">' . $item->name . '</span>';
			// Render last item if reqd. ?>
			<li itemprop="itemListElement" itemscope itemtype="https://schema.org/ListItem" class="breadcrumb-item active"><?php echo $breadcrumbItem; ?>
				<meta itemprop="position" content="<?php echo $key + 1; ?>">
			</li>
		<?php endif;
	endforeach; ?>
</ol>
