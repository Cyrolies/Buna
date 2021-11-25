<?php
/**
 * @package 	Easy Accordion Content
 * @version 	1.0
 * @author 		JoomBoost
 * @website		https://www.joomboost.com
 * @copyright 	Copyright (C) 2012 - 2016 JoomBoost
 * @license 	GNU/GPL http://www.gnu.org/copyleft/gpl.html
**/

//no direct access

defined('_JEXEC') or die('Direct Access to this location is not allowed.');
require_once dirname(__FILE__).'/color.php';

// Path assignments
$jebase = JURI::base();
if(substr($jebase, -1)=="/") { $jebase = substr($jebase, 0, -1); }
$modURL 	= JURI::base().'modules/mod_easyaccordioncontent';

// get parameters 
$jQuery = $params->get("jQuery");
$ReadMoreText = $params->get('ReadMoreText','Read More...');

$titleBg = $params->get('titleBg','#9aa5b3');
$titleText = $params->get('titleText','#ffffff');
$contentBg = $params->get('contentBg','#F7F7F7');
$contentText = $params->get('contentText','#333333');
$fontStyle = $params->get('fontStyle','Open+Sans');


$Image[]= $params->get( '!', "" );
$Title[]= $params->get( '!', "" );
$Text[]= $params->get( '!', "" );
$Link[]= $params->get( '!', "" );

for ($j=1; $j<=30; $j++) {

	$Image[]		= $params->get( 'Image'.$j , "" );
	$Title[]		= $params->get( 'Title'.$j , "" );
	$Text[]	= $params->get( 'Text'.$j , "" );
	$Link[]	= $params->get( 'Link'.$j , "" );	

}

// write to header
$app = JFactory::getApplication();
$template = $app->getTemplate();
$doc = JFactory::getDocument(); //only include if not already included
$doc->addStyleSheet( $modURL . '/css/style.css');
$doc->addStyleSheet( 'http://fonts.googleapis.com/css?family='.$fontStyle.'');
$fontStyle = str_replace("+"," ",$fontStyle);
$fontStyle = explode(":",$fontStyle);
$style = '
#jeAccordion'.$module->id.'.jeAccordion { background:'.$contentBg.'; color:'.$contentText.';  }
#jeAccordion'.$module->id.' .jeAcc-title { border-bottom:1px solid '.jeAccColor($titleBg,'-20').'; background:'.$titleBg.';  color:'.$titleText.';font-family: "'.$fontStyle[0].'", Arial, Helvetica, sans-serif ;}
#jeAccordion'.$module->id.' .jeAcc-title.active, #jeAccordion'.$module->id.' .jeAcc-title:hover { background:'.jeAccColor($titleBg,'-10').'; color:'.jeAccColor($titleText,'40').' }
'; 
$doc->addStyleDeclaration( $style );
if ($params->get('jQuery')) {$doc->addScript ('http://code.jquery.com/jquery-latest.pack.js');}
$doc = JFactory::getDocument();
$js = "
jQuery(document).ready(function() {
    function close_accordion_section() {
        jQuery('#jeAccordion".$module->id." .jeAcc-title').removeClass('active');
        jQuery('#jeAccordion".$module->id." .jeAcc-content').slideUp(300).removeClass('open');
    }
    jQuery('#jeAccordion".$module->id." .jeAcc-title').click(function(e) {
        // Grab current anchor value
        var currentAttrValue = jQuery(this).attr('href');
 
        if(jQuery(e.target).is('.active')) {
            close_accordion_section();
        }else {
            close_accordion_section();
 
            // Add active class to section title
            jQuery(this).addClass('active');
            // Open up the hidden content panel
            jQuery('#jeAccordion".$module->id." ' + currentAttrValue).slideDown(300).addClass('open');
        }
        e.preventDefault();
    });
});
";
$doc->addScriptDeclaration($js);


?>
<div id="jeAccordion<?php echo $module->id;?>" class="jeAccordion">
<?php for ($i=0; $i<=30; $i++){ if ($Title[$i] != null) { ?>
    <div class="jeAcc-section">
        <a class="jeAcc-title" href="#jeAcc-<?php echo $i ?>"><?php echo $Title[$i] ?></a>
        <div id="jeAcc-<?php echo $i ?>" class="jeAcc-content">
            <p><?php echo $Text[$i] ?></p>
            <?php if ($Link[$i] != null) {echo '<a href="'.$Link[$i].'" class="jeAcc-readmore">'.$ReadMoreText.'</a>';}?>
        </div>
    </div>
<?php }};  ?>   
</div>