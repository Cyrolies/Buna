<?php
/**
 * @version $Id: legacy.php 22 2020-07-05 13:04 m.maciejewski $
 * @package DJ-MegaMenu
 * @copyright Copyright (C) 2017  DJ-Extensions.com LTD, All rights reserved.
 * @license http://www.gnu.org/licenses GNU/GPL
 * @author url: http://dj-extensions.com
 * @author email contact@dj-extensions.com
 *
 * DJ-Cleaning is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * DJ-Cleaning is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with DJ-Cleaning. If not, see <http://www.gnu.org/licenses/>.
 *
 */

/**
 * @package DJ-Catalog2
 * @copyright Copyright (C) DJ-Extensions.com, All rights reserved.
 * @license http://www.gnu.org/licenses GNU/GPL
 * @author url: http://dj-extensions.com
 * @author email contact@dj-extensions.com
 */
class DJLegacyHelper
{

    protected static $JOOMLA_VERSION;

    public static function getJoomlaVersion()
    {

        if (!isset(self::$JOOMLA_VERSION)) {
            $version = new JVersion;
            self::$JOOMLA_VERSION = $version->getShortVersion();
        }

        return self::$JOOMLA_VERSION;
    }


    /*
     * Include js library legacy
     */
    public static function BehaviorTooltip()
    {
        if (version_compare(self::getJoomlaVersion(), '4.0.0-alpha10', '<')) {
            JHtml::_('behavior.tooltip');
        } else {
            // Joomla 4 code
        }
    }


    /*
     * Check user is logged on backend
     */
    public static function isAdmin()
    {
        $app = JFactory::getApplication();

        if (version_compare(self::getJoomlaVersion(), '4.0.0-alpha10', '<')) {
            return $app->isAdmin();
        } else {
            return $app->isClient('admin');
        }
    }

    /*
     * Check user is logged on frontend
     */
    public static function isClient()
    {
        $app = JFactory::getApplication();

        if (version_compare(self::getJoomlaVersion(), '4.0.0-alpha10', '<')) {
            return $app->isSite();
        } else {
            return $app->isClient('site');
        }
    }
}
