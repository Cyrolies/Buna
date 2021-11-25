<?php
/**
 # jmootips Joomla! Plugin
 # @author Joachim Schmidt - joachim.schmidt@jschmidt-systemberatung.de
 # @copyright Copyright (C) 2013 Joachim Schmidt. All rights reserved.
 # @license	 http://www.gnu.org/licenses/gpl-2.0.html GNU/GPL
 #
 # change activity:
 # 1.06.2013: Release V1.0.0 for Joomla 2.5 and 3.0
 */
defined('_JEXEC') or die('Restricted access');
use Joomla\CMS\Factory;

class plgeditorsxtdtipbuttonInstallerScript
{

    function postflight ($parent, $type)
    {
        // Enabled plugin
        $db = Factory::getDBO();
        $sql = " UPDATE `#__extensions` SET `enabled`=1 WHERE `type`='plugin' AND `element`='tipbutton' and `folder`='editors-xtd';";
        $db->setQuery($sql);
        $db->execute();
    }
}
?>