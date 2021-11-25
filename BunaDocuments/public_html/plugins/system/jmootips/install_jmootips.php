<?php
/**
 # jmootips Joomla! Plugin
 #
 # installation-script "install_jmootips.php"
 #
 # @author Joachim Schmidt - joachim.schmidt@jschmidt-systemberatung.de
 # @copyright Copyright (C) 2014 Joachim Schmidt. All rights reserved.
 # @license	 http://www.gnu.org/licenses/gpl-2.0.html GNU/GPL
 #
 # change activity:
 # 5.02.2015: Release V1.2.2 for Joomla 3.x
 # 1.10.2018: Release V1.2.4 for Joomla 3.8
 */
defined('_JEXEC') or die('Restricted access');
use Joomla\CMS\Factory;
use Joomla\CMS\Language\Text;

class plgsystemjmootipsInstallerScript
{

    function postflight ($parent, $type)
    {
         // Enable plugin
        $db = Factory::getDBO();
        $sql = " UPDATE `#__extensions` SET `enabled`=1 WHERE `type`='plugin' AND `element`='jmootips' AND folder='system' LIMIT 1;";
        $db->setQuery($sql);
        $db->execute();

        // disable content plugin if found
        $sql = " SELECT element from #__extensions WHERE type='plugin' AND element='jmootips' AND folder='content' and enabled='1' LIMIT 1;";
        $db->setQuery($sql);
        $db->execute();
        $result = $db->loadResult();
        if ($result)
        {
            $sql = " UPDATE #__extensions SET enabled = '0'  WHERE type='plugin' AND element='jmootips' AND folder='content';";
            $db->setQuery($sql);
            $db->execute();
            Factory::getApplication()->enqueueMessage(Text::_('PLG_DISABLE_MSG'), 'notice');
        }
    }
}
?>