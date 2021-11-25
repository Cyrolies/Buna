<?php

/**
 * @author          Tassos Marinos <info@tassos.gr>
 * @link            http://www.tassos.gr
 * @copyright       Copyright © 2021 Tassos Marinos All Rights Reserved
 * @license         GNU GPLv3 <http://www.gnu.org/licenses/gpl.html> or later
*/

namespace NRFramework\SmartTags;

defined('_JEXEC') or die('Restricted access');

use Joomla\Registry\Registry;

abstract class SmartTag
{
	/**
	 * Factory Class
	 *
	 * @var object
	 */
    protected $factory;

    /**
	 * Joomla Application object
	 *
	 * @var object
     */
    protected $app;

    /**
	 * Joomla Document
	 *
	 * @var object
     */
    protected $doc;

    /**
     * Useful data used by a Smart Tag
     * 
     * @var  array
     */
    protected $data;

    /**
     * Smart Tags Configuration Options
     * 
     * @var  array
     */
    protected $options;

    public function __construct($factory = null, $options = null)
    {
        if (!$factory)
        {
            $factory = new \NRFramework\Factory();
        }
        $this->factory = $factory;
        
		$this->app = $this->factory->getApplication();
        $this->doc = $this->factory->getDocument();
        
        $this->options = $options;
    }

    /**
     * Set the data
     * 
     * @param   array  $data
     * 
     * @return  void
     */
    public function setData($data)
    {
        $this->data = $data;
    }
}