<?php

/**
 * @author          Tassos.gr
 * @link            http://www.tassos.gr
 * @copyright       Copyright © 2021 Tassos Marinos All Rights Reserved
 * @license         GNU GPLv3 <http://www.gnu.org/licenses/gpl.html> or later
*/

namespace NRFramework\SmartTags;

defined('_JEXEC') or die('Restricted access');

class User extends SmartTag
{
    /**
     * Fetch a property from the User object
     *
     * @param   string  $key   The name of the property to return
     *
     * @return  mixed   Null if property is not found, mixed if property is found
     */
    public function fetchValue($key)
    {
        // Just in case, deny access to the 'password' property
        if ($key == 'password')
        {
            return;
        }

        $user_id = isset($this->options['user']) ? $this->options['user'] : null;
        $user = $this->factory->getUser($user_id);

        if (is_null($user) || $user->id == 0 || !isset($user->{$key}))
        {
            return;
        }

        return $user->{$key};
    }

    /**
     * Returns the name of the user capitalized
     * 
     * @return  string
     */
    public function getName()
    {
        return ucwords(strtolower($this->fetchValue('name')));
    }

    /**
     * Returns the user first name
     * 
     * @return  string
     */
    public function getFirstname()
    {
		// Set first name
        $nameParts = explode(' ', $this->getName(), 2);
        $firstname = trim($nameParts[0]);
        
        return $firstname;
    }

    /**
     * Returns the user last name
     * 
     * @return  string
     */
    public function getLastname()
    {
		// Set last name
    	$nameParts = explode(' ', $this->getName(), 2);
    	$lastname  = isset($nameParts[1]) ? trim($nameParts[1]) : $nameParts[0];
        
        return $lastname;
    }

    /**
     * Returns the user login
     * 
     * @deprecated Use {user.username}
     * 
     * @return  string
     */
    public function getLogin()
    {
        return $this->fetchValue('username');
    }

    /**
     * Returns the user register date
     * 
     * @return  string
     */
    public function getRegisterDate()
    {
        if (!$date = $this->fetchValue('registerDate'))
        {
            return;
        }

        return \JHtml::_('date', $date, \JText::_('DATE_FORMAT_LC5'));
    }
}