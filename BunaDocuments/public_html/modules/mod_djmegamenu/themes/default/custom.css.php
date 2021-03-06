<?php

/**
 * @version $Id$
 * @package DJ-MegaMenu
 * @copyright Copyright (C) 2020 DJ-Extensions.com LTD, All rights reserved.
 * @license http://www.gnu.org/licenses GNU/GPL
 * @author url: http://dj-extensions.com
 * @author email contact@dj-extensions.com
 * @developer Szymon Woronowski - szymon.woronowski@design-joomla.eu
 *
 * DJ-MegaMenu is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * DJ-MegaMenu is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with DJ-MegaMenu. If not, see <http://www.gnu.org/licenses/>.
 *
 */

// no direct access
defined('_JEXEC') or die('Restricted access');

$djmegamenu = '#dj-megamenu' . $params->get('module_id');

$megabg = $params->get('megabg'); //menu background
$megacolor = $params->get('megacolor'); //menu text
$megastcolor = $params->get('megastcolor'); //menu subtitle
$megabg_a = $params->get('megabg_a'); //active menu background
$megacolor_a = $params->get('megacolor_a'); //active menu text
$megastcolor_a = $params->get('megastcolor_a'); //active menu subtitle
$megasubbg = $params->get('megasubbg'); //submenu background
$megasubcolor = $params->get('megasubcolor'); //submenu text
$megasubstcolor = $params->get('megasubstcolor'); //submenu subtitle
$megasubbg_a = $params->get('megasubbg_a'); //active submenu background
$megasubcolor_a = $params->get('megasubcolor_a'); //active submenu text
$megasubstcolor_a = $params->get('megasubstcolor_a'); //active submenu subtitle
$megamodcolor = $params->get('megamodcolor'); //module text
$megaoverlaycolor = $params->get('megaoverlaycolor'); //overlay background

?>
<?php if ( !empty($megabg) ) { ?>
	<?php echo $djmegamenu; ?>,
	<?php echo $djmegamenu; ?>sticky {
		background: <?php echo $megabg; ?>;
	}

	<?php echo $djmegamenu; ?> li a.dj-up_a {
		border-right-color: <?php echo adjustBrightness($megabg, 0.8); ?>;
		border-left-color: <?php echo adjustBrightness($megabg, 1.2); ?>;
	}

	<?php echo $djmegamenu; ?>.verticalMenu li a.dj-up_a {
		border-bottom-color: <?php echo adjustBrightness($megabg, 1.2); ?>;
		border-top-color: <?php echo adjustBrightness($megabg, 0.8); ?>;
	}

	<?php if ($direction == 'rtl') { ?>
		<?php echo $djmegamenu; ?> li a.dj-up_a {
			border-left-color: <?php echo adjustBrightness($megabg, 0.8); ?>;
			border-right-color: <?php echo adjustBrightness($megabg, 1.2); ?>;
		}
	<?php } ?>
<?php } ?>

<?php if ( !empty($megacolor) ) { ?>
	<?php echo $djmegamenu; ?> li a.dj-up_a {
		color: <?php echo $megacolor; ?>;
	}
<?php } ?>

<?php if ( !empty($megastcolor) ) { ?>
	<?php echo $djmegamenu; ?> li a.dj-up_a small.subtitle {
		color: <?php echo $megastcolor; ?>;
	}
<?php } ?>

<?php if ( !empty($megabg_a) ) { ?>
	<?php echo $djmegamenu; ?> li:hover a.dj-up_a,
	<?php echo $djmegamenu; ?> li.hover a.dj-up_a,
	<?php echo $djmegamenu; ?> li.active a.dj-up_a {
		background: <?php echo $megabg_a; ?>;
		border-right-color: <?php echo adjustBrightness($megabg_a, 0.8); ?>;
		border-left-color: <?php echo adjustBrightness($megabg_a, 1.2); ?>;
	}

	<?php if ($direction == 'rtl') { ?>
	<?php echo $djmegamenu; ?> li:hover a.dj-up_a,
	<?php echo $djmegamenu; ?> li.hover a.dj-up_a,
	<?php echo $djmegamenu; ?> li.active a.dj-up_a {
		border-left-color: <?php echo adjustBrightness($megabg_a, 0.8); ?>;
		border-right-color: <?php echo adjustBrightness($megabg_a, 1.2); ?>;
	}
	<?php } ?>
<?php } ?>

<?php if ( !empty($megacolor_a) ) { ?>
	<?php echo $djmegamenu; ?> li:hover a.dj-up_a,
	<?php echo $djmegamenu; ?> li.hover a.dj-up_a,
	<?php echo $djmegamenu; ?> li.active a.dj-up_a {
		color: <?php echo $megacolor_a; ?>;
	}
<?php } ?>

<?php if ( !empty($megastcolor_a) ) { ?>
	<?php echo $djmegamenu; ?> li:hover a.dj-up_a small.subtitle,
	<?php echo $djmegamenu; ?> li.hover a.dj-up_a small.subtitle,
	<?php echo $djmegamenu; ?> li.active a.dj-up_a small.subtitle {
		color: <?php echo $megastcolor_a; ?>;
	}
<?php } ?>

<?php if ( !empty($megasubbg) ) { ?>
	<?php echo $djmegamenu; ?> li:hover div.dj-subwrap,
	<?php echo $djmegamenu; ?> li.hover div.dj-subwrap {
		background: <?php echo $megasubbg; ?>;
	}

	<?php echo $djmegamenu; ?> li:hover div.dj-subwrap li:hover > div.dj-subwrap,
	<?php echo $djmegamenu; ?> li.hover div.dj-subwrap li.hover > div.dj-subwrap {
		background: <?php echo $megasubbg; ?>;
	}

	<?php echo $djmegamenu; ?> li ul.dj-submenu > li {
		border-top-color: <?php echo adjustBrightness($megasubbg, 1.2); ?>;
	}

	<?php echo $djmegamenu; ?> .djsubrow_separator {
		border-bottom-color: <?php echo adjustBrightness($megasubbg, 1.2); ?>;
	}
<?php } ?>

<?php if ( !empty($megasubcolor) ) { ?>
	<?php echo $djmegamenu; ?> li ul.dj-submenu > li > a {
		color: <?php echo $megasubcolor; ?>;
	}

	<?php echo $djmegamenu; ?> li ul.dj-subtree > li > a {
		color: <?php echo $megasubcolor; ?>;
	}
<?php } ?>

<?php if ( !empty($megasubstcolor) ) { ?>
	<?php echo $djmegamenu; ?> li ul.dj-submenu > li > a small.subtitle {
		color: <?php echo $megasubstcolor; ?>;
	}
	<?php echo $djmegamenu; ?> li ul.dj-subtree > li {
		color: <?php echo $megasubstcolor; ?>;
	}
	<?php echo $djmegamenu; ?> li ul.dj-subtree > li > a small.subtitle {
		color: <?php echo $megasubstcolor; ?>;
}
<?php } ?>

<?php if ( !empty($megasubbg_a) ) { ?>
	<?php echo $djmegamenu; ?> li ul.dj-submenu > li > a:hover,
	<?php echo $djmegamenu; ?> li ul.dj-submenu > li > a.active,
	<?php echo $djmegamenu; ?> li ul.dj-submenu > li.hover:not(.subtree) > a {
		background: <?php echo $megasubbg_a; ?>;
	}
<?php } ?>

<?php if ( !empty($megasubcolor_a) ) { ?>
	<?php echo $djmegamenu; ?> li ul.dj-submenu > li > a:hover,
	<?php echo $djmegamenu; ?> li ul.dj-submenu > li > a.active,
	<?php echo $djmegamenu; ?> li ul.dj-submenu > li.hover:not(.subtree) > a {
		color: <?php echo $megasubcolor_a; ?>;
	}

	<?php echo $djmegamenu; ?> li ul.dj-subtree > li > a:hover {
		color: <?php echo $megasubcolor_a; ?>;
	}
<?php } ?>

<?php if ( !empty($megasubstcolor_a) ) { ?>
	<?php echo $djmegamenu; ?> li ul.dj-submenu > li > a:hover small.subtitle,
	<?php echo $djmegamenu; ?> li ul.dj-submenu > li > a.active small.subtitle,
	<?php echo $djmegamenu; ?> li ul.dj-submenu > li.hover:not(.subtree) > a small.subtitle {
		color: <?php echo $megasubstcolor_a; ?>;
	}

	<?php echo $djmegamenu; ?> li ul.dj-subtree > li > a:hover small.subtitle {
		color: <?php echo $megasubstcolor_a; ?>;
	}
<?php } ?>

<?php if ( !empty($megamodcolor) ) { ?>
<?php echo $djmegamenu; ?> .modules-wrap {
	color: <?php echo $megamodcolor; ?>;
}
<?php } ?>

<?php if ( !empty($megaoverlaycolor) ) { ?>
body .dj-megamenu-overlay-box {
	background: <?php echo $params->get('megaoverlaycolor'); ?>;
}
<?php } ?>