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

// no direct access
defined('_JEXEC') or die('Restricted access');
defined('DS') or define('DS', DIRECTORY_SEPARATOR);

$version = new JVersion;
$isJoomla4 = version_compare($version->getShortVersion(), '4', '>=');

if($isJoomla4) {
	require_once(dirname(__FILE__).'/helpers/ModMenuHelper.php');
} else {
	require_once JPATH_ROOT.'/modules/mod_menu/helper.php';
}

// Include the syndicate functions only once
require_once 'helperversion.php';

modDJMegaMenuHelper::parseParams($params);

$params->set('module_id', $module->id);
$startLevel = $params->get('startLevel');
$endLevel = $params->get('endLevel');

$list		= modDJMegaMenuHelper::getList($params);
$subwidth	= modDJMegaMenuHelper::getSubWidth($params);
$subcols	= modDJMegaMenuHelper::getSubCols($params);
$expand		= modDJMegaMenuHelper::getExpand($params);
$active		= modDJMegaMenuHelper::getActive($params);
$active_id	= $active->id;
$path		= $active->tree;

$showAll	= $params->get('showAllChildren');
$class_sfx	= ($params->get('hasSubtitles') ? 'hasSubtitles ':'') . htmlspecialchars($params->get('moduleclass_sfx'));

if(!count($list)) return;

$app = JFactory::getApplication();
$doc = JFactory::getDocument();
$direction = $doc->direction;
//$app->enqueueMessage("<pre>".print_r($parents, true)."</pre>");

$canDefer = preg_match('/(?i)msie [6-9]/', @$_SERVER['HTTP_USER_AGENT']) ? false : true;
$defercss = array();

JHTML::_('jquery.framework');

// direction integration with joomla monster templates
if ($app->input->get('direction') == 'rtl'){
	$direction = 'rtl';
} else if ($app->input->get('direction') == 'ltr') {
	$direction = 'ltr';
} else {
	if (isset($_COOKIE['jmfdirection'])) {
		$direction = $_COOKIE['jmfdirection'];
	} else {
		$direction = $app->input->get('jmfdirection', $direction);
	}
}

$ver = modDJMegaMenuHelper::getVersion($params);

$minified = ( true ) ? '.min' : '';

modDJMegaMenuHelper::addTheme($params, $direction);

if($params->get('moo',1)) {

	$doc->addStyleSheet(JURI::root(true).'/modules/mod_djmegamenu/assets/css/animations.css', array('version' => $ver));
	$defercss['animate_min_css'] = JURI::root(true).'/media/djextensions/css/animate.min.css';
	$defercss['animate_ext_css'] = JURI::root(true).'/media/djextensions/css/animate.ext.css';

	$doc->addScript(JURI::root(true).'/modules/mod_djmegamenu/assets/js/jquery.djmegamenu' . $minified . '.js', array('version' => $ver), array('defer' => $canDefer));
	
	if(!is_numeric($openDelay = $params->get('openDelay'))) $openDelay = 250;
	if(!is_numeric($closeDelay = $params->get('closeDelay'))) $closeDelay = 500;
	
	$wrapper_id = $params->get('wrapper');
	$animIn = $params->get('animation_in');
	$animOut = $params->get('animation_out');
	$animSpeed = $params->get('animation_speed');
	$open_event = $params->get('event', 'mouseenter');
	$close_event = $params->get('eventClose', 'mouseleave');
	$fixed = $params->get('fixed', 0);
	$fixed_offset = $params->get('fixed_offset', 0);
	$theme = $params->get('theme');
	$wcag = $params->get('wcag', 1);
	$overlay = $params->get('overlay', 0);
	$custom_colors = ($params->get('customColors', 0)) ? 'dj-megamenu-custom' : '';
	
	$options = json_encode(array('wrap' => $wrapper_id, 'animIn' => $animIn, 'animOut' => $animOut, 'animSpeed' => $animSpeed, 'openDelay' => $openDelay, 'closeDelay' => $closeDelay, 
		'event' => $open_event, 'eventClose' => $close_event, 'fixed' => $fixed, 'offset' => $fixed_offset, 'theme' => $theme, 'direction' => $direction, 'wcag' => $wcag, 'overlay' => $overlay ));
}

$mobilemenu = (int) $params->get('select', 0);
if($mobilemenu) {

	$doc->addStyleDeclaration("
		@media (min-width: ".((int)$params->get('width')+1)."px) {
			#dj-megamenu$module->id"."mobile { display: none; }
		}
		@media (max-width: ".(int)$params->get('width')."px) {
			#dj-megamenu$module->id, #dj-megamenu$module->id"."sticky, #dj-megamenu$module->id"."placeholder { display: none !important; }
		}
	");

	if($mobilemenu == 2) {
		$position = $params->get('offcanvas_pos', 'left') == 'right' ? '_right':'';
		$doc->addStyleSheet(JURI::root(true).'/modules/mod_djmegamenu/assets/css/offcanvas'.$position.'.css', array('version' => $ver));
		$offmodules = array();
		if($params->get('pro')) {
			$offmodules['top'] = modDJMegaMenuHelper::loadModules('dj-offcanvas-top', $params->get('offcanvas_topmod_style','xhtml'));
			$offmodules['bottom'] = modDJMegaMenuHelper::loadModules('dj-offcanvas-bottom', $params->get('offcanvas_botmod_style','xhtml'));
		}
	}

	if($mobilemenu > 0) {
		$doc->addScript(JURI::root(true).'/modules/mod_djmegamenu/assets/js/jquery.djmobilemenu' . $minified . '.js', array('version' => $ver), array('defer' => $canDefer));
		modDJMegaMenuHelper::addMobileTheme($params, $direction);
	}
}

//font awesome
$fa = $params->get('fa', 1);

if( $fa == 5 ) {
	$doc->addStyleSheet("https://use.fontawesome.com/releases/v5.15.2/css/all.css");
	$doc->addStyleSheet("https://use.fontawesome.com/releases/v5.15.2/css/v4-shims.css");
} else if( $fa ) {
	$doc->addStyleSheet('//maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css');
}

//temporary fix for dj-icon
if($params->get('theme')=='_override'|| $params->get('theme')=='override') {
	$doc->addStyleDeclaration(".dj-megamenu-override li.dj-up a.dj-up_a span.dj-icon { display: inline-block; }");
}

if(count($defercss)) {
	$js = "
	(function(){
		var cb = function() {
			var add = function(css, id) {
				if(document.getElementById(id)) return;
				var l = document.createElement('link'); l.rel = 'stylesheet'; l.id = id; l.href = css;
				var h = document.getElementsByTagName('head')[0]; h.appendChild(l);
			};";
		foreach($defercss as $id => $css) {
			$js .= "add('$css', '$id');";
		}
	$js .= "
		}
		var raf = requestAnimationFrame || mozRequestAnimationFrame || webkitRequestAnimationFrame || msRequestAnimationFrame;
		if (raf) raf(cb);
		else window.addEventListener('load', cb);
	})();";
	$doc->addScriptDeclaration($js);
}

require(JModuleHelper::getLayoutPath('mod_djmegamenu', $params->get('layout', 'default')));
