<?php
/**
 * @version     1.7.0
 * @package     sellacious
 *
 * @copyright   Copyright (C) 2012-2019 Bhartiy Web Technologies. All rights reserved.
 * @license     SPL Sellacious Private License; see http://www.sellacious.com/spl.html
 * @author      Izhar Aazmi <info@bhartiy.com> - http://www.bhartiy.com
 */

// No direct access to this file
defined('_JEXEC') or die('Restricted access');

jimport('sellacious.loader');

/**
 * Create an empty class to meet the situation where sellacious backoffice is not installed yet.
 * In this case however, the backoffice part of the component will not be processed and only the joomla frontend and backend files, and the datanse will be installed.
 */
if (!class_exists('SellaciousInstallerComponent'))
{
    class SellaciousInstallerComponent
    {
    }
}

/**
 * Script file of test component.
 *
 * The name of this class is dependent on the component being installed.
 * The class name should have the component's name, directly followed by
 * the text InstallerScript (ex:. com_testInstallerScript).
 *
 * This class will be called by Joomla!'s installer, if specified in your component's
 * manifest file, and is used for custom automation actions in its installation process.
 *
 * In order to use this automation script, you should reference it in your component's
 * manifest file as follows:
 * <scriptfile>script.php</scriptfile>
 *
 * @package     Joomla.Administrator
 * @subpackage  com_test
 *
 * @copyright   Copyright (C) 2005 - 2015 Open Source Matters, Inc. All rights reserved.
 * @license     GNU General Public License version 2 or later; see LICENSE.txt
 */
class Com_importerInstallerScript extends SellaciousInstallerComponent
{
}
