<?php
/**
 # jmootips Joomla! Plugin
 #
 # @author Joachim Schmidt - joachim.schmidt@jschmidt-systemberatung.de
 # @copyright Copyright (C) 2013 Joachim Schmidt. All rights reserved.
 # @license	 http://www.gnu.org/licenses/gpl-2.0.html GNU/GPL
 #
 # change activity:
 # 15.05.2013: Release V1.0.0 for Joomla Version 2.5 and Version 3.x
 # 27.06.2013: Release V1.1.0 for Joomla Version 2.5 and Version 3.x
 #             Support for bootstrap framework (popover) added as alternative to
 #             Mootools framework
 # 15.08.2013: Release 1.1.1
 #             add area tag support
 # 06.01.2014: Release V1.1.2
 #             add option to open tooltip with mouseclick
 # 11.01.2014: Code Cleanup (e.g. use of json_encode instead of coding json-objects directly)
 # 14.01.2014: Corrected problem with duplicate style parm if option "openonlick" used
 # 13.03.2014: Code chnged to support Joomla! extension "Falang"
 # 01.06.2014: Release 1.2.0
 #             changed plugin type (now a system plugin)
 # 10.06.2014: added check for enabled jmootips content plugin
 # 12.12.2014: Release 1.2.1
 #             Added option "width" for explicitly setting tooltip width
 # 05.02.2015: added ajax-option to process ajax-event for parm "ajax4id"
 # 02.10.2018: use namespaced (new) joomla API
 # 22.09.2020: use namespaced (own) classes
 */
// no direct access
defined('_JEXEC') or die('Restricted access');

use Joomla\CMS\Factory;
use Joomla\CMS\Language\Text;
use Joomla\CMS\Plugin\CMSPlugin;
use Joomla\CMS\Plugin\PluginHelper;
use Joomla\String\StringHelper;
use jmootips\plugin\plgSystemjmootipsHelper;

class plgSystemjmootips extends CMSPlugin
{

    var $init_script = true;

    function construct_jmootips (&$subject, $params)
    {
        parent::__construct($subject, $params);
        $this->_plugin = PluginHelper::getPlugin('system', 'jmootips');
    }

    function onAjaxJmootips ()
    {
        $app = Factory::getApplication();
        $input = $app->input;
        $articleId = $input->get('articleid');

        $db = Factory::getDBO();
        $sql = "SELECT id,introtext FROM #__content WHERE state='1' and access='1' and id = " . intval($articleId);
        $db->setQuery($sql);
        $result = $db->loadAssocList();
        if ($result)
        {
            if (array_key_exists('introtext', $result[0]))
                return $result[0]['introtext'];
        }
        else
            return (sprintf(Text::_('EMPTY_ARTICLE'), $articleId));
    }

    function onAfterRoute ()
    {
        if (Factory::getApplication()->isClient('administrator') && Factory::getApplication()->input->get('option') != 'com_login')
        {
            if (PluginHelper::isEnabled("content", 'jmootips'))
            {
                $lang = Factory::getLanguage();
                $rc = $lang->load('plg_system_jmootips', JPATH_ADMINISTRATOR, null, true, true);
                if (! $rc)
                    $lang->load('plg_system_jmootips', JPATH_ADMINISTRATOR, "en-GB");

                $db = Factory::getDBO();
                $sql = " UPDATE #__extensions SET enabled = '0'  WHERE type='plugin' AND element='jmootips' AND folder='content';";
                $db->setQuery($sql);
                $db->execute();
                Factory::getApplication()->enqueueMessage(Text::_('PLG_DISABLE_MSG'), 'warning');
            }
        }

        return true;
    }

    function onAfterDispatch ()
    {
        $document = Factory::getDocument();
        if ($document->getType() !== 'html' || Factory::getApplication()->input->getInt('print', 0))
            return true;

        if (Factory::getApplication()->isClient('administrator'))
            return true;

        if ($this->init_script)
        {
            include_once 'helper.php';
            $this->helper = new plgSystemjmootipsHelper();
            $this->init_script = false;
            $this->helper->addResources($this->params->get('bootstrap'), $this->params);
        }
    }

    function onAfterRender ()
    {
        $matches = array();
        $app = Factory::getApplication();

        if (Factory::getDocument()->getType() !== 'html')
            return true;

        if ($app->isClient('administrator'))
            return true;

        if ($app->input->get('layout') == 'edit')
            return true;

        $buffer = $app->getBody();

        if ($buffer == '')
            return true;

        include_once 'helper.php';
        $this->helper = new plgSystemjmootipsHelper();
        list ($before_body, $body, $end_body) = $this->helper->getBody($buffer);

        if (StringHelper::strpos($body, '{tip') === false)
        {
            $this->helper->removeResources($before_body);
            $app->setBody($before_body . $body . "\n" . $end_body);
            return true;
        }
        else
        {
            $regex = "#{tip\\s*(.*?)}(.*?){/tip}#s";
            preg_match_all($regex, $body, $matches);
            $count = count($matches[0]);
            if ($count)
            {
                if ($this->params->get('bootstrap') !== "1")
                    $this->helper->build4mootools($body, $matches, $count, $regex, $this->params);
                else
                    $this->helper->build4bootstrap($body, $matches, $count, $regex, $this->params);

                $app->setBody($before_body . $body . "\n" . $end_body);
            }
            else
                return true;
        }
    }
}

?>