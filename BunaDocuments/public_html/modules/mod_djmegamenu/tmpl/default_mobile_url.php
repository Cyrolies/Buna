<?php
/**
 * @version $Id$
 * @package DJ-MegaMenu
 * @copyright Copyright (C) 2017 DJ-Extensions.com LTD, All rights reserved.
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

defined('_JEXEC') or die;

// Note. It is important to remove spaces between elements.
$title = $item->anchor_title ? 'title="'.$item->anchor_title.'" ' : '';

$subtitle = '';
$aclass = '';

if($params->get('subtitles') > 1) {
	$subtitle = $item_params->get('djmegamenu-subtitle');
	if(!empty($subtitle)) {
		$subtitle = '<br/><small class="subtitle">'.$subtitle.'</small>';
		$aclass = ' withsubtitle';
	}
}
$linktype = $item->title . $subtitle;

$class = $item->anchor_css || !empty($aclass) ? 'class="'.$aclass.' '.$item->anchor_css.'" ' : '';

if($params->get('icons') > 1) {
	$faicon = $item_params->get('djmegamenu-fa','');
	if(!empty($faicon)) {
		if($item_params->get('menu_text', 1)) {
			$linktype = '<span class="'.$faicon.'" aria-hidden="true"></span><span class="image-title">'. $item->title . $subtitle .'</span>';
		} else {
			$linktype = '<span class="'.$faicon.'" aria-hidden="true" title="'.htmlspecialchars($item->title).'"></span>';
			$title .= 'aria-label="'.htmlspecialchars($item->title).'" ';
		}
	} else if($item->menu_image) {
		$item_params->get('menu_text', 1) ?
		$linktype = '<img src="'.$item->menu_image.'" alt="'.$item->title.'" /><span class="image-title">'. $linktype .'</span>' :
		$linktype = '<img src="'.$item->menu_image.'" alt="'.$item->title.'" />';
	}
}

switch ($item->type) :
		case 'heading':
		case 'separator':
			$item->browserNav = 3;
			break;
		case 'component':
		case 'url':
		default:
			$flink = JFilterOutput::ampReplace(htmlspecialchars($item->flink, ENT_COMPAT, 'UTF-8', false));
			break;
endswitch;

$custom_html_pos = (bool) $item_params->get('djmegamenu-html-pos', '0');
$attr = $item_params->get('djmegamenu-custom-attr', '');
$custom_attr = ( !empty($attr) ) ? ' ' . $attr : '';

$html_before = $item_params->get('djmegamenu-html-before','');
if( !empty($html_before) && !$custom_html_pos ) {
	$linktype = '<span class="html-before">' . $html_before . '</span>'.$linktype;
}

$html_after = $item_params->get('djmegamenu-html-after','');
if( !empty($html_after) && !$custom_html_pos ) {
	$linktype = $linktype.'<span class="html-after">' . $html_after . '</span>';
}

if( !empty($html_before) && $custom_html_pos ) {
	echo '<div class="html-before">' . $html_before . '</div>';
}

switch ($item->browserNav) :
	default:
	case 0:
		?><a <?php echo $class; ?>href="<?php echo $flink; ?>" <?php echo $title; ?><?php echo $custom_attr; ?>><?php echo $linktype; ?></a><?php
		break;
	case 1:
		// _blank
		?><a <?php echo $class; ?>href="<?php echo $flink; ?>" target="_blank" <?php echo $title; ?><?php echo $custom_attr; ?>><?php echo $linktype; ?></a><?php
		break;
	case 2:
		// window.open
		$options = 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,'.$params->get('window_open');
		?><a <?php echo $class; ?>href="<?php echo $flink; ?>" onclick="window.open(this.href,'targetWindow','<?php echo $options;?>');return false;" <?php echo $title; ?><?php echo $custom_attr; ?>><?php echo $linktype; ?></a><?php
		break;
	case 3:
		?><a <?php echo $class; ?> <?php echo $title; ?> tabindex="0"<?php echo $custom_attr; ?>><?php echo $linktype; ?></a><?php
endswitch;

if( !empty($html_after) && $custom_html_pos ) {
	echo '<div class="html-after">' . $html_after . '</div>';
}
