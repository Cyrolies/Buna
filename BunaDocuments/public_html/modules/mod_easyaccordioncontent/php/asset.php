<?php
/**
 * @package 	Easy Accordion Content
 * @version 	1.0
 * @author 		JoomBoost
 * @website		https://www.joomboost.com
 * @copyright 	Copyright (C) 2012 - 2016 JoomBoost
 * @license 	GNU/GPL http://www.gnu.org/copyleft/gpl.html
**/

// no direct access
defined( '_JEXEC' ) or die( 'Restricted access' );

jimport('joomla.form.formfield');

class JFormFieldAsset extends JFormField
{
	protected	$type = 'Asset';
	
	protected function getInput() {
	
        $jsFile = dirname(__FILE__) . DIRECTORY_SEPARATOR . ".." . DIRECTORY_SEPARATOR . "css" . DIRECTORY_SEPARATOR . "mod-style.css";
        $jsUrl = str_replace(JPATH_ROOT, JURI::root(true), $jsFile);
        $jsUrl = str_replace(DIRECTORY_SEPARATOR, "/", $jsUrl);
		$doc = JFactory::getDocument();	
		$doc->addStylesheet($jsUrl);
		return null;
	}
} 
?>