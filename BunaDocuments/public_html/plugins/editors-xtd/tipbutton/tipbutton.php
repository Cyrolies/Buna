<?php
/**
 # jmootips Joomla! Plugin - Editor tooltip button
 # @author Joachim Schmidt - joachim.schmidt@jschmidt-systemberatung.de
 # @copyright Copyright (C) 2013 Joachim Schmidt. All rights reserved.
 # @license	 http://www.gnu.org/licenses/gpl-2.0.html GNU/GPL
 #
 # change activity:
 # 15.05.2013: Release V1.0.0 for Joomla 2.5 and 3.0
 */
// no direct access
defined('_JEXEC') or die();

use Joomla\CMS\Plugin\CMSPlugin;
use Joomla\CMS\Factory;
use Joomla\CMS\Language\Text;
use Joomla\CMS\Object\CMSObject;
use Joomla\CMS\Session\Session;
use Joomla\CMS\Uri\Uri;
use Joomla\CMS\Version;
use Joomla\CMS\HTML\HTMLHelper;

class plgButtonTipbutton extends CMSPlugin
{

    private $joomla_version;

    protected $autoloadLanguage = true;

    public function __construct (& $subject, $config)
    {
        parent::__construct($subject, $config);
        $this->loadLanguage();
        JLoader::import('joomla.version');
        $this->joomla_version = new Version();
    }

    function onDisplay ($name)
    {
        $doc = Factory::getDocument();

        if (version_compare($this->joomla_version->getShortVersion(), '3.10', '>='))
        {
            $url = URI::root() . 'media/plg_system_jmootips/js/tipbutton.min.js';
            $doc->addScript($url);

            $js = "
		    function jSelectTooltip( id, title, catid, object, link, lang ) {
    		var tag = '{tip id=\"' +id +'\"}' + title + '{/tip}';
			jInsertEditorText(tag, '" . $name . "');
            jQuery('#TooltipModal').modal('toggle');
  		 }";
        }
        else
        {
            $js = "
    		function jSelectTooltip( id, title, catid, object, link, lang ) {
        		var tag = '{tip id=\"' +id +'\"}' + title + '{/tip}';
    			jInsertEditorText(tag, '" . $name . "');
    			SqueezeBox.close();
    		}";
            HTMLHelper::_('behavior.modal');
        }

        $doc->addScriptDeclaration($js);
        $link = "index.php?option=com_content&amp;view=articles&amp;layout=modal&amp;tmpl=component&amp;" . Session::getFormToken() . '=1' . "&amp;function=jSelectTooltip";
        $button = new CMSObject();
        $button->modal = true;
        $button->class = 'btn';
        $button->link = $link;
        $button->text = 'Tooltip';
        $button->title = Text::_('PLG_TIPBUTTON_BUTTON_TOOLTIP');
        $button->name = 'article';
        $button->options = "{ handler: 'iframe', size: {x: 600, y: 400} }";

        return $button;
    }
}

