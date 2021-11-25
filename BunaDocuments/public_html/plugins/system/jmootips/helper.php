<?php
/**
 # helper class for jmootips Joomla! Plugin
 #
 # @author Joachim Schmidt - joachim.schmidt@jschmidt-systemberatung.de
 # @copyright Copyright (C) 2013 Joachim Schmidt. All rights reserved.
 # @license  http://www.gnu.org/licenses/gpl-2.0.html GNU/GPL
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
 # 13.03.2014: Code chnged to support extension "falang"
 # 01.06.2014: Release 1.2.0
 #             changed code for system plugin type (was a content plugin)
 # 05.02.2015: changed ajax-processings for parm "ajax4id"
 # 06.03-2015: added options for auto-positioning
 # 02.10-2018: changed to new namespaced Joomla API
 # 01.08.2020: changed to support jooomla 4
 # 22.09.2020: use namespaced classes
 # 30.06.2021: corrected version checking to run on Joomal 3.10
 */
namespace jmootips\plugin;
defined('_JEXEC') or die('Restricted access');

use Joomla\CMS\Version;
use Joomla\CMS\Factory;
use Joomla\CMS\Language\Text;
use Joomla\CMS\Uri\Uri;
use Joomla\CMS\HTML\HTMLHelper;

class plgSystemjmootipsHelper
{

    private $joomla_version;

    private $jmootips_version;

    public function __construct ()
    {
        $this->joomla_version = new Version();
        $this->jmootips_version = (object) array(
                'PRODUCT' => 'jmootips',
                'MINOR_VERSION' => '1.2.5',
                'PATCH_VERSION' => '2'
        );
    }

    function convert2Array ($param_line)
    {
        $matches = array();
        preg_match_all('/(\w+)(\s*=\s*\".*?\")/s', $param_line, $matches);
        $count = count($matches[1]);
        $parms = array();

        for ($i = 0; $i < $count; $i ++)
        {
            $value = ltrim($matches[2][$i], " \n\r\t=");
            $value = trim($value, '"');
            $parm = $matches[1][$i];
            $parms[$parm] = $value;
        }
        return $parms;
    }

    function getParam ($parms, $attribute, $default = null)
    {
        if (array_key_exists($attribute, $parms))
            return $parms[$attribute];
        else
            return $default;
    }

    function addStyle ($str, $style)
    {
        $tmp = explode('style=', $str);
        return $tmp[0] . "style=" . substr($tmp[1], 0, 1) . $style . substr($tmp[1], 1);
    }

    function checkUrl ($url)
    {
        if (filter_var($url, FILTER_VALIDATE_URL) === false)
            return Text::_('INVALID_URL');

        if (! function_exists("get_headers"))
            return true;

        $hdrs = @get_headers($url);
        if (is_array($hdrs) ? preg_match('/^HTTP\\/\\d+\\.\\d+\\s+2\\d\\d\\s+.*$/', $hdrs[0]) : false)
            return (true);
        elseif (is_array($hdrs) ? preg_match('/^HTTP\/.*\s+(300|301|302|303|307|308)/', $hdrs[0]) : false)
            return (true);
        elseif (is_array($hdrs))
            return ("<b>HTTP Response: </b>" . $hdrs[0]);
        else
            return Text::_('INVALID_URL');
    }
    
    function addMediaResource($name, $url, $type)
    {
        $document = Factory::getDocument();
        $base = URI::base(true) . "/";
        $joomlav4 = false;
        if (version_compare($this->joomla_version->getShortVersion(), '4.0', '>='))
        {
          $joomlav4 = true;
          $wa = Factory::getApplication()->getDocument()->getWebAssetManager();
        }
        
          switch ($type)
          {
              case "script":
                  if (!$joomlav4)
                     $document->addScript($base . $url);
                   else
                     $wa->registerAndUseScript($name, $url);
                  break;
                  
              case "style":
                  if (!$joomlav4)
                     $document->addStyleSheet($base . $url);
                   else
                     $wa->registerAndUseStyle($name, $url);
                  break;
                  
              case "inlinescript":
                  if (!$joomlav4)
                    $document->addScriptDeclaration($url);
                   else
                    $wa->addInlineScript($url);
                  break;
                  
              case "inlinestyle":
                  if (!$joomlav4)
                      $document->addStyleDeclaration($url);
                   else
                     $wa->addInlineStyle($url);
                  break;
                  
              default:
                  return;
          }
    }

    function addResources ($bootstrap = false, $params)
    {
        $lang = Factory::getLanguage();
        if ($lang->getTag() != 'en-GB')
        {
            $lang->load('plg_system_jmootips', JPATH_ADMINISTRATOR, 'en-GB');
        }
        $lang->load('plg_system_jmootips', JPATH_ADMINISTRATOR, null, 1);

        if (! $bootstrap)
        {
            if ( $params->get('bootstrap') != "1" )
                $this->add_mootool_scripts();

            if ($params->get('jmootips_cssfile') != "0")
                $this->addMediaResource('jmootips.css', 'media/plg_system_jmootips/css/jmootips.css', 'style');
               
            $sticky = $params->get('sticky', '0');
            if ($sticky == "1")
                $sticky = "true";
            else
                $sticky = "false";

            $openOnClickOption = $params->get('openonclick', '0');

            if ($openOnClickOption == "1")
            {
                $openOnClickOption = "true";
                $sticky = "true";
            }
            else
                $openOnClickOption = "false";

            $autoposition = $params->get('autoposition', 1);
            $css_class = $params->get('css_class', 'jmootips');
            $position = $params->get('position', - 1);
            $java_script = "/* start jmootips script */
// <![CDATA[
 if (window.jQuery) jQuery.noConflict();
   window.addEvent('load', function() {
  new jmootips({
  ToolTipClass: '" . $css_class . "',
  toolTipPosition:" . $position . ",
  autoPosition:" . $autoposition . ",
  sticky: " . $sticky . ",
  openOnClick: " . $openOnClickOption . ",
  closeMsg: '" . Text::_('CLOSE_TITLE') . "',
  loadingMsg: '" . Text::_('AJAX_LOADING_MSG') . "',
  failMsg: '" . Text::_('AJAX_ERROR_MSG') . "'
 });
});
// ]]>
/* end jmootips script */";
            
           $this->addMediaResource("jmootips.script", $java_script, "inlinescript");
         }
        else
        {
            HtmlHelper::_('jquery.framework');

            if ($params->get('jmootips_cssfile') != "0")
                $this->addMediaResource('jmootips.css', 'media/plg_system_jmootips/css/jmootips.css', 'style');
     
            $position_parm = $params->get('position', - 1);
            if ($position_parm == "1" || $position_parm == "below")
                $position = "bottom";
            elseif ($position_parm == "-1" || $position_parm == "above")
                $position = "top";
            elseif ($position_parm == "-2" || $position_parm == "right")
                $position = "right";
            elseif ($position_parm == "2" || $position_parm == "left")
                $position = "left";
            else
                $position = "top";

            $trigger_option = $params->get('openonclick', "0");
            $sticky_option = $params->get('sticky', "0");

            if ($trigger_option == "1")
                $trigger = "click";
            elseif ($sticky_option == "1")
                $trigger = "sticky-hover";
            else
                $trigger = "hover";
            if (version_compare($this->joomla_version->getShortVersion(), '3.4', '<'))
                $this->addMediaResource('bootstrap-v331', 'media/plg_system_jmootips/js/bootstrap331.min.js', 'script');
             $this->addMediaResource('jsbootstrap', 'media/plg_system_jmootips/js/jsbootstrap.min.js', 'script');
        
            $java_script = "\n/* start jmootips script */
   jQuery(document).ready(function() {  createjsTips('$position', '$trigger'); });
/* end jmootips script */\n";
           
            $this->addMediaResource("jmootips.script", $java_script, "inlinescript");
        }
    }

    function removeResources (&$buffer)
    {
        $buffer = preg_replace('#\s*<' . 'link [^>]*href="[^"]*/system/jmootips/css/[^"]*\.css[^"]*"[^>]* />#s', '', $buffer);
        $buffer = preg_replace('#\s*<' . 'script [^>]*src="[^"]*/system/jmootips/js/[^"]*\.js[^"]*"[^>]*></script>#s', '', $buffer);
        $buffer = preg_replace('#/\* start jmootips .*?/\* end jmootips [a-z]* \*/\s*#s', '', $buffer);
        return;
    }

    static function getBody ($html)
    {
        if (strpos($html, '<body') === false || strpos($html, '</body>') === false)
            return array(
                    '',
                    $html,
                    ''
            );

        $tmp = explode('<body', $html, 2);
        $before_body = $tmp[0];
        $body = $tmp[1];
        $tmp = explode('</body>', $body);
        $after_body = '</body>' . $tmp[1];
        $body = "<body" . $tmp[0];

        return array(
                $before_body,
                $body,
                $after_body
        );
    }

    function add_mootool_scripts ()
    {
        if (version_compare($this->joomla_version->getShortVersion(), '4.0', '<'))
        {
            $document = Factory::getDocument();
            $base = URI::root(true);
            $document->addScript($base . "/media/plg_system_jmootips/js/mootools-core.js");
            $document->addScript($base . "/media/plg_system_jmootips/js/mootools-more-jmootips.js");
            $document->addScript($base . '/media/plg_system_jmootips/js/jmootips.min.js');
        }
        else
        {
            $wa = Factory::getApplication()->getDocument()->getWebAssetManager();
            $wa->registerScript('mootools.core', 'media/plg_system_jmootips/js/mootools-core.js');
            $wa->registerScript('mootools.more', 'media/plg_system_jmootips/js/mootools-more-jmootips.js');
            $wa->registerScript('jmootips', 'media/plg_system_jmootips/js/jmootips.min.js');

            $wa->useScript('mootools.core')
                ->useScript('mootools.more')
                ->useScript('jmootips')
                ->useScript('jquery');
        }
    }

    function build4mootools (&$buffer, &$matches, $count, $regex, $params)
    {
        // $current_id = substr(md5(microtime()), rand(0, 26), 2);
        $current_id = "";

        $lang = Factory::getLanguage();
        /* $lang_subtag = explode("-", $lang->getTag()); */
        $rc = $lang->load('plg_system_jmootips', JPATH_ADMINISTRATOR, null, true, true);
        if (! $rc)
        {
            $lang->load('plg_system_jmootips', JPATH_ADMINISTRATOR, "en-GB");
            // $lang_subtag[0] = "en";
        }

        if ($params->get('remote_host') == "1")
        {
            $allow_remotehost = true;
            // set timeout for url requests
            ini_set('default_socket_timeout', 10);
        }
        else
            $allow_remotehost = false;
        if ($params->get('allow_url') == "1")
            $allow_url = true;
        else
            $allow_url = false;

        $cursor_style = "cursor:pointer;";
        $openOnClickOption = $params->get('openonclick', '0');

        if ($openOnClickOption == "1")
        {
            $openOnClickOption = "true";
            $sticky = "true";
        }
        else
            $openOnClickOption = "false";

        $replace = "";
        $tip_container = "\n<!--Tooltip container - contents of tooltips:  -->";
        $tip_container .= "\n<div id='tooltip_container" . $current_id . "' style='display: none;'>";

        for ($i = 0; $i < $count; $i ++)
        {
            $tooltip_target = $matches[2][$i];
            $param_line = $this->convert2Array($matches[1][$i]);
            $open_parm = $this->getParam($param_line, 'openonclick');
            $sticky_parm = $this->getParam($param_line, 'sticky');
            $position_parm = $this->getParam($param_line, 'position');
            $autoposition_parm = $this->getParam($param_line, 'autoposition');
            $width_parm = $this->getParam($param_line, 'width');
            $articleId = $this->getParam($param_line, 'id');
            $url_parm = $this->getParam($param_line, 'url');
            $image_parm = $this->getParam($param_line, 'image');
            $title_parm = $this->getParam($param_line, 'title');
            $text_parm = $this->getParam($param_line, 'text');
            $ajax_parm = $this->getParam($param_line, 'ajax');
            $ajax4id = $this->getParam($param_line, 'ajax4id', $params->get('ajax4id', '0'));

            if ($tooltip_target != '')
            {
                /* --------------------------------------------------------------------- */
                /* process parameters for jmootips object                                */
                /* --------------------------------------------------------------------- */
                $options = array();
                $tip_content = "";
                $tooltip_id = "id='tooltip" . $current_id . "_" . $i . "' ";

                if ($open_parm != "")
                {
                    if ($open_parm == "1")
                    {
                        $openOnClick = true;
                        $sticky_parm = "1";
                        $style = "class='jmootipper' style='" . $cursor_style . "'";
                    }
                    else
                    {
                        $openOnClick = false;
                        $style = "class='jmootipper'";
                        $options['sticky'] = false;
                    }
                    $options['openOnClick'] = $openOnClick;
                }
                elseif ($openOnClickOption == "false")
                    $style = "class='jmootipper'";
                elseif ($openOnClickOption == "true")
                    $style = "class='jmootipper' style='" . $cursor_style . "'";

                if ($sticky_parm != "")
                {
                    if ($sticky_parm == "1")
                        $sticky = true;
                    else
                        $sticky = false;
                    $options['sticky'] = $sticky;
                }

                if ($autoposition_parm != "")
                {
                    if ($autoposition_parm != "1")
                        $options['autoposition'] = "0";
                    else
                        $options['autoposition'] = "1";
                }

                if ($position_parm != "")
                {
                    if ($position_parm == "1" || $position_parm == "below")
                        $position = 1;
                    elseif ($position_parm == "-1" || $position_parm == "above")
                        $position = - 1;
                    elseif ($position_parm == "-2" || $position_parm == "right")
                        $position = - 2;
                    elseif ($position_parm == "2" || $position_parm == "left")
                        $position = 2;
                    else
                        $position = - 1;
                    $options['position'] = $position;
                }

                if ($title_parm != "")
                    $options['title'] = htmlentities($title_parm, ENT_QUOTES, 'utf-8');

                if ($width_parm != "")
                    $options['width'] = $width_parm;

                if ($articleId != "")
                {
                    $db = Factory::getDBO();
                    if (! array_key_exists('title', $options))
                    {
                        $sql = "SELECT id,title FROM #__content WHERE state='1' and access='1' and id = " . intval($articleId);
                        $db->setQuery($sql);
                        $result = $db->loadAssocList();

                        if ($result)
                            if (array_key_exists('title', $result[0]))
                                $options['title'] = str_replace("'", "&#39;", $result[0]['title']);
                    }

                    if ($ajax4id == "1" && version_compare($this->joomla_version->getShortVersion(), '3.2', '>='))
                        $sql = "SELECT id FROM #__content WHERE state='1' and access='1' and id = " . intval($articleId);
                    else
                        $sql = "SELECT id,introtext FROM #__content WHERE state='1' and access='1' and id = " . intval($articleId);
                    $db->setQuery($sql);
                    $result = $db->loadAssocList();
                    if ($result)
                    {
                        if (array_key_exists('id', $result[0]))
                        {
                            if ($ajax4id == "1" && version_compare($this->joomla_version->getShortVersion(), '3.2', '>='))
                            {
                                $args = "&amp;format=raw" . "&amp;articleid=" . $articleId;
                                $options['ajax'] = URI::root(true) . "/index.php?option=com_ajax&amp;plugin=jmootips&amp;group=system" . $args;
                            }
                            else
                                $tip_content = $result[0]['introtext'];
                        }
                        else
                        {
                            $options['title'] = Text::_('ERROR_TITLE');
                            $tip_content = sprintf(Text::_('EMPTY_ARTICLE'), $articleId);
                        }
                    }
                    else
                    {
                        $options['title'] = Text::_('ERROR_TITLE');
                        $tip_content = sprintf(Text::_('EMPTY_ARTICLE'), $articleId);
                    }
                }

                elseif ($text_parm != "")
                    $tip_content = $text_parm;

                elseif ($url_parm != "")
                {
                    if ($allow_url)
                    {
                        if (filter_var($url_parm, FILTER_VALIDATE_URL, FILTER_FLAG_HOST_REQUIRED))
                        {
                            $url = parse_url($url_parm);
                            if ($url['host'] != $_SERVER['HTTP_HOST'] && $allow_remotehost == false)
                            {
                                $options['title'] = Text::_('ERROR_TITLE');
                                $tip_content = "&quot;$url_parm&quot;  " . sprintf(Text::_('REMOTE_NOT_ALLOWED'), $_SERVER['HTTP_HOST']);
                            }
                            else
                            {
                                $rc = $this->checkUrl($url_parm);
                                if ($rc === true)
                                {
                                    $tip_content = @file_get_contents($url_parm);
                                    if ($tip_content === false)
                                        $tip_content = "&quot;$url_parm&quot; - " . Text::_('URL_NOT_FOUND');
                                }
                                else
                                {
                                    $options['title'] = Text::_('ERROR_TITLE');
                                    $tip_content = "&quot;$url_parm&quot; - " . Text::_('URL_NOT_FOUND') . "<br /> " . $rc;
                                }
                            }
                        }
                        else
                        {
                            if (strpos($url_parm, "http") === false)
                                $url_parm = URI::base() . ltrim($url_parm, "/");
                            $rc = $this->checkUrl($url_parm);
                            if ($rc === true)
                            {
                                $tip_content = @file_get_contents($url_parm);
                                if ($tip_content === false)
                                    $tip_content = "&quot;$url_parm&quot; - " . Text::_('URL_NOT_FOUND');
                            }
                            else
                            {
                                $options['title'] = Text::_('ERROR_TITLE');
                                $tip_content = "&quot;$url_parm&quot; - " . Text::_('URL_NOT_FOUND') . "<br /> " . $rc;
                            }
                        }
                    }
                    else
                    {
                        $options['title'] = Text::_('ERROR_TITLE');
                        $tip_content = "&quot;$url_parm&quot; - " . Text::_('URL_NOT_ALLOWED');
                    }
                }

                elseif ($image_parm != "")
                {
                    if (file_exists(JPATH_SITE . $image_parm))
                        $tip_content = "<img src='" . $image_parm . "' alt=''/>";
                    else
                    {
                        $options['title'] = Text::_('ERROR_TITLE');
                        $tip_content = sprintf(Text::_('FILE_NOT_FOUND'), $image_parm);
                    }
                }
                elseif ($ajax_parm != "")
                {
                    if ($allow_url)
                    {
                        $file = explode("?", $ajax_parm);
                        if (filter_var($ajax_parm, FILTER_VALIDATE_URL, FILTER_FLAG_HOST_REQUIRED))
                        {
                            $url = parse_url($ajax_parm);
                            $rc = $this->checkUrl($ajax_parm);
                            if ($url['host'] != $_SERVER['HTTP_HOST'])
                            {
                                $options['title'] = Text::_('ERROR_TITLE');
                                $tip_content = sprintf(Text::_('INVALID_AJAX_URL'), $ajax_parm, $_SERVER['HTTP_HOST']);
                            }
                            elseif ($rc === true)
                                $options['ajax'] = $ajax_parm;
                            else
                            {
                                $options['title'] = Text::_('ERROR_TITLE');
                                $tip_content = "&quot;$ajax_parm&quot; <br>" . Text::_('URL_NOT_FOUND') . "<br /> " . $rc;
                            }
                        }
                        elseif (! file_exists(JPATH_SITE . $file[0]))
                        {
                            $options['title'] = Text::_('ERROR_TITLE');
                            $tip_content = "&quot;$ajax_parm&quot; <br>" . Text::_('URL_NOT_FOUND');
                        }
                        else
                            $options['ajax'] = $ajax_parm;
                    }
                    else
                    {
                        $options['title'] = Text::_('ERROR_TITLE');
                        $tip_content = "&quot;$ajax_parm&quot; " . Text::_('URL_NOT_ALLOWED');
                    }
                }
                else
                {
                    $options['title'] = Text::_('ERROR_TITLE');
                    $tip_content = Text::_('NO_CONTENT');
                }

                /* -------------------------------------------------------------------- */
                /* create contents for tooltips                                         */
                /* -------------------------------------------------------------------- */
                if (version_compare($this->joomla_version->getShortVersion(), '3.2', '<'))
                    $ajax4id = "0";

                $qq = "'";
                if (stristr($tooltip_target, "style=") !== false && stristr($style, "style=") !== false)
                {
                    $style = "class='jmootipper'";
                    $tooltip_target = $this->addStyle($tooltip_target, $cursor_style);
                }
                if (($ajax_parm == '' && $ajax4id != '1') || ($ajax4id == '1' && $articleId == "") || (($ajax_parm != "" || $ajax4id == '1') && $tip_content != ""))
                {
                    $options['content'] = "tooltip_content" . $current_id . "_" . $i;
                    $tip_container .= "\n <div id='tooltip_content" . $current_id . "_" . $i . "'>" . $tip_content . "</div>";
                }
                if (stristr($tooltip_target, "<img") !== false && stristr($tooltip_target, "class=") === false)
                {
                    $tmp = "<img " . $style . " data-jmootips=" . $qq . json_encode($options) . $qq . " ";
                    $replace = str_replace("<img ", $tmp, $tooltip_target);
                }
                elseif (stristr($tooltip_target, "<area") !== false && stristr($tooltip_target, "class=") === false)
                {
                    $tmp = "<area " . $style . " data-jmootips=" . $qq . json_encode($options) . $qq . " ";
                    $replace = str_replace("<area ", $tmp, $tooltip_target);
                }
                elseif (stristr($tooltip_target, "<a ") !== false && stristr($tooltip_target, "class=") === false)
                {
                    $tmp = "<a " . $style . " data-jmootips=" . $qq . json_encode($options) . $qq . " ";
                    $replace = str_replace("<a ", $tmp, $tooltip_target);
                }
                else
                    $replace = "<span " . $tooltip_id . $style . " data-jmootips=" . $qq . json_encode($options) . $qq . ">" . $tooltip_target . "</span>";
            }

            $buffer = str_replace($matches[0][$i], $replace, $buffer);
        }
        if (substr_count($tip_container, 'div') > 1)
            $buffer .= $tip_container . "\n</div>\n<!-- End of Tooltip container   -->";

        return;
    }

    function build4bootstrap (&$buffer, &$matches, $count, $regex, $params)
    {
        $current_id = "";
        $lang = Factory::getLanguage();
        $rc = $lang->load('plg_system_jmootips', JPATH_ADMINISTRATOR, null, true, true);
        if (! $rc)
        {
            $lang->load('plg_system_jmootips', JPATH_ADMINISTRATOR, "en-GB");
        }

        $cursor_style = "cursor:pointer;";
        $trigger_option = $params->get('openonclick', "0");
        $sticky_option = $params->get('sticky', "0");

        if ($trigger_option == "1")
        {
            $style = "class='jmootipper' style='" . $cursor_style . "'";
        }
        elseif ($sticky_option == "1" && $trigger_option == "0")
        {
            $style = "class='jmootipper'";
        }
        else
        {
            $style = "class='jmootipper'";
        }

        if ($params->get('remote_host') == "1")
        {
            $allow_remotehost = true;
            ini_set('default_socket_timeout', 10);
        }
        else
            $allow_remotehost = false;
        if ($params->get('allow_url') == "1")
            $allow_url = true;
        else
            $allow_url = false;

        if ($params->get('autoposition', '1') == "1")
            $autoposition = "1";
        else
            $autoposition = "0";

        $replace = "";
        $tip_container = "\n<!--Tooltip container - contents of tooltips:  -->";
        $tip_container .= "\n<div id='tooltip_container" . $current_id . "' style='display: none;'>";

        for ($i = 0; $i < $count; $i ++)
        {
            $tooltip_target = $matches[2][$i];
            $param_line = $this->convert2Array($matches[1][$i]);
            $position_parm = $this->getParam($param_line, 'position');
            $trigger_parm = $this->getParam($param_line, 'openonclick');
            $width_parm = $this->getParam($param_line, 'width');
            $articleId = $this->getParam($param_line, 'id');
            $url_parm = $this->getParam($param_line, 'url');
            $image_parm = $this->getParam($param_line, 'image');
            $title_parm = $this->getParam($param_line, 'title');
            $text_parm = $this->getParam($param_line, 'text');
            $ajax_parm = $this->getParam($param_line, 'ajax');
            $sticky_parm = $this->getParam($param_line, 'sticky');
            $autoposition_parm = $this->getParam($param_line, 'autoposition');
            $ajax4id = $this->getParam($param_line, 'ajax4id', $params->get('ajax4id', '0'));

            if ($tooltip_target != '')
            {
                /* --------------------------------------------------------------------- */
                /* process parameters for jmootips object                                */
                /* --------------------------------------------------------------------- */
                $options = array();
                $tip_content = "";
                $tooltip_id = "id='tooltip" . $current_id . "_" . $i . "' ";

                if ($autoposition_parm == "")
                    $options['autoposition'] = $autoposition;
                else
                    $options['autoposition'] = $autoposition_parm;

                if ($position_parm == "")
                    $position_parm = $params->get('position');

                if ($position_parm == "1" || $position_parm == "below")
                    $options['position'] = "bottom";
                elseif ($position_parm == "-1" || $position_parm == "above")
                    $options['position'] = "top";
                elseif ($position_parm == "-2" || $position_parm == "right")
                    $options['position'] = "right";
                elseif ($position_parm == "2" || $position_parm == "left")
                    $options['position'] = "left";
                else
                    $options['position'] = "top";

                if ($trigger_parm != "")
                {
                    if ($trigger_parm == "1")
                    {
                        $options['trigger'] = "click";
                        $style = "class='jmootipper' style='" . $cursor_style . "'";
                    }
                    else
                    {
                        $options['trigger'] = "hover";
                        $style = "class='jmootipper'";
                    }
                }
                elseif ($sticky_parm != "")
                {
                    if ($sticky_parm == "1" && isset($options['trigger']))
                    {
                        if ($options['trigger'] != "1")
                            $options['trigger'] = "sticky-hover";
                    }
                    elseif ($sticky_parm == "1")
                    {
                        $options['trigger'] = "sticky-hover";
                    }
                }

                elseif ($trigger_option == "1")
                    $style = "class='jmootipper' style='" . $cursor_style . "'";
                elseif ($trigger_option != "1")
                    $style = "class='jmootipper'";

                if ($title_parm != "")
                    $options['title'] = htmlentities($title_parm, ENT_QUOTES, 'utf-8');
                else
                    $options['title'] = "";

                if ($width_parm != "")
                    $options['width'] = $width_parm;

                if ($articleId != "")
                {
                    $db = Factory::getDBO();
                    if ($options['title'] == "")
                    {
                        $sql = "SELECT id,title FROM #__content WHERE state='1' and access='1' and id = " . intval($articleId);
                        $db->setQuery($sql);
                        $result = $db->loadAssocList();

                        if ($result)
                        {
                            if (array_key_exists('title', $result[0]))
                                $options['title'] = $result[0]['title'];
                        }
                    }

                    if ($ajax4id == "1" && version_compare($this->joomla_version->getShortVersion(), '3.2', '>='))
                        $sql = "SELECT id FROM #__content WHERE state='1' and access='1' and id = " . intval($articleId);
                    else
                        $sql = "SELECT id,introtext FROM #__content WHERE state='1' and access='1' and id = " . intval($articleId);
                    $db->setQuery($sql);
                    $result = $db->loadAssocList();
                    if ($result)
                    {
                        if (array_key_exists('id', $result[0]))
                        {
                            if ($ajax4id == "1" && version_compare($this->joomla_version->getShortVersion(), '3.2', '>='))
                            {
                                $args = "&amp;format=raw" . "&amp;articleid=" . $articleId;
                                $ajax_url = URI::root(true) . "/index.php?option=com_ajax&amp;plugin=jmootips&amp;group=system" . $args;

                                $tip_content = '{ "ajax" : "' . $ajax_url . '" }';
                            }
                            else
                                $tip_content = $result[0]['introtext'];
                        }
                        else
                        {
                            $options['title'] = Text::_('ERROR_TITLE');
                            $tip_content = sprintf(Text::_('EMPTY_ARTICLE'), $articleId);
                        }
                    }
                    else
                    {
                        $options['title'] = Text::_('ERROR_TITLE');
                        $tip_content = sprintf(Text::_('EMPTY_ARTICLE'), $articleId);
                    }
                }

                elseif ($text_parm != "")
                    $tip_content = $text_parm;

                elseif ($url_parm != "")
                {
                    if ($allow_url)
                    {
                        if (filter_var($url_parm, FILTER_VALIDATE_URL, FILTER_FLAG_HOST_REQUIRED))
                        {
                            $url = parse_url($url_parm);
                            if ($url['host'] != $_SERVER['HTTP_HOST'] && $allow_remotehost == false)
                            {
                                $options['title'] = Text::_('ERROR_TITLE');
                                $tip_content = "&quot;$url_parm&quot;  " . sprintf(Text::_('REMOTE_NOT_ALLOWED'), $_SERVER['HTTP_HOST']);
                            }
                            else
                            {
                                $rc = $this->checkUrl($url_parm);
                                if ($rc === true)
                                {
                                    $tip_content = @file_get_contents($url_parm);
                                    if ($tip_content === false)
                                        $tip_content = "&quot;$url_parm&quot; - " . Text::_('URL_NOT_FOUND');
                                }
                                else
                                {
                                    $options['title'] = Text::_('ERROR_TITLE');
                                    $tip_content = "&quot;$url_parm&quot; - " . Text::_('URL_NOT_FOUND') . "<br /> " . $rc;
                                }
                            }
                        }
                        else
                        {
                            if (strpos($url_parm, "http") === false)
                                $url_parm = URI::base() . ltrim($url_parm, "/");
                            $rc = $this->checkUrl($url_parm);
                            if ($rc === true)
                            {
                                $tip_content = @file_get_contents($url_parm);
                                if ($tip_content === false)
                                    $tip_content = "&quot;$url_parm&quot; - " . Text::_('URL_NOT_FOUND');
                            }
                            else
                            {
                                $options['title'] = Text::_('ERROR_TITLE');
                                $tip_content = "&quot;$url_parm&quot; - " . Text::_('URL_NOT_FOUND') . "<br /> " . $rc;
                            }
                        }
                    }
                    else
                    {
                        $options['title'] = Text::_('ERROR_TITLE');
                        $tip_content = "&quot;$url_parm&quot; - " . Text::_('URL_NOT_ALLOWED');
                    }
                }

                elseif ($image_parm != "")
                {
                    if (file_exists(JPATH_SITE . $image_parm))
                        $tip_content = "<img src='" . $image_parm . "' alt=''/>";
                    else
                    {
                        $options['title'] = Text::_('ERROR_TITLE');
                        $tip_content = sprintf(Text::_('FILE_NOT_FOUND'), $image_parm);
                    }
                }

                elseif ($ajax_parm != "")
                {
                    if ($allow_url)
                    {
                        $file = explode("?", $ajax_parm);
                        if (filter_var($ajax_parm, FILTER_VALIDATE_URL, FILTER_FLAG_HOST_REQUIRED))
                        {
                            $rc = $this->checkUrl($url_parm);
                            $url = parse_url($ajax_parm);
                            if ($url['host'] != $_SERVER['HTTP_HOST'])
                            {
                                $options['title'] = Text::_('ERROR_TITLE');
                                $tip_content = sprintf(Text::_('INVALID_AJAX_URL'), $ajax_parm, $_SERVER['HTTP_HOST']);
                            }
                            elseif ($rc === true)
                                $tip_content = '{ "ajax": ' . '"' . $ajax_parm . '" }';
                            else
                            {
                                $options['title'] = Text::_('ERROR_TITLE');
                                $tip_content = "&quot;$ajax_parm&quot; <br>" . Text::_('URL_NOT_FOUND') . "<br /> " . $rc;
                            }
                        }
                        elseif (! file_exists(JPATH_SITE . $file[0]))
                        {
                            $options['title'] = Text::_('ERROR_TITLE');
                            $tip_content = "&quot;$ajax_parm&quot; <br>" . Text::_('URL_NOT_FOUND');
                        }
                        else
                            $tip_content = '{ "ajax": ' . '"' . $ajax_parm . '" }';
                    }
                    else
                    {
                        $options['title'] = Text::_('ERROR_TITLE');
                        $tip_content = "&quot;$ajax_parm&quot; " . Text::_('URL_NOT_ALLOWED');
                    }
                }

                else
                {
                    $options['title'] = Text::_('ERROR_TITLE');
                    $tip_content = Text::_('NO_CONTENT');
                }

                /* -------------------------------------------------------------------- */
                /* create contents for tooltips                                         */
                /* -------------------------------------------------------------------- */
                $tip_container .= "\n <div id='tooltip" . $current_id . "_" . $i . "_content'>";
                if (array_count_values($options))
                    $tip_container .= "\n   <div class='tooltipOptions'>" . json_encode($options) . "</div>";
                $tip_container .= "\n   <div id='tooltipContent_" . $i . "' class='tooltipContent'>$tip_content</div>";
                $tip_container .= "\n </div>";

                if (stristr($tooltip_target, "style=") !== false && stristr($style, "style=") !== false)
                {
                    $style = "class='jmootipper'";
                    $tooltip_target = $this->addStyle($tooltip_target, $cursor_style);
                }

                if (stristr($tooltip_target, "<a ") !== false && stristr($tooltip_target, "class=") === false)
                {
                    $tmp = "<a " . $tooltip_id . $style . " ";
                    $replace = str_replace("<a ", $tmp, $tooltip_target);
                }
                elseif (stristr($tooltip_target, "<img") !== false && stristr($tooltip_target, "class=") === false)
                {
                    $tmp = "<img " . $tooltip_id . $style . " ";
                    $replace = str_replace("<img ", $tmp, $tooltip_target);
                }
                elseif (stristr($tooltip_target, "<area") !== false && stristr($tooltip_target, "class=") === false)
                {
                    $tmp = "<area " . $tooltip_id . $style . " ";
                    $replace = str_replace("<area ", $tmp, $tooltip_target);
                }
                else
                    $replace = "<span " . $tooltip_id . $style . " >" . $tooltip_target . "</span>";
            }

            $buffer = str_replace($matches[0][$i], $replace, $buffer);
        }
        if (substr_count($tip_container, 'div') > 1)
            $buffer .= $tip_container . "\n</div>\n<!-- End of Tooltip container   -->";

        return;
    }
}