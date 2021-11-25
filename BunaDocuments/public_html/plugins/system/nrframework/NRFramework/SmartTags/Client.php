<?php

/**
 * @author          Tassos.gr
 * @link            http://www.tassos.gr
 * @copyright       Copyright © 2021 Tassos Marinos All Rights Reserved
 * @license         GNU GPLv3 <http://www.gnu.org/licenses/gpl.html> or later
*/

namespace NRFramework\SmartTags;

defined('_JEXEC') or die('Restricted access');

class Client extends SmartTag
{
    /**
     * Returns the device
     * 
     * @return  string
     */
    public function getDevice()
    {
        return \NRFramework\WebClient::getDeviceType();
    }

    /**
     * Returns the OS
     * 
     * @return  string
     */
    public function getOS()
    {
        return \NRFramework\WebClient::getOS();
    }

    /**
     * Returns the browser
     * 
     * @return  string
     */
    public function getBrowser()
    {
        return \NRFramework\WebClient::getBrowser()['name'];
    }
    
    /**
     * Returns the current user agent
     * 
     * @return  string
     */
    public function getUserAgent()
    {
        return \NRFramework\WebClient::getClient()->userAgent;
    }
}