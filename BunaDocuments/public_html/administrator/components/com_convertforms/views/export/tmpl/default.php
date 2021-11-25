<?php

/**
 * @package         Convert Forms
 * @version         2.8.5 Free
 * 
 * @author          Tassos Marinos <info@tassos.gr>
 * @link            http://www.tassos.gr
 * @copyright       Copyright © 2020 Tassos Marinos All Rights Reserved
 * @license         GNU GPLv3 <http://www.gnu.org/licenses/gpl.html> or later
*/

defined('_JEXEC') or die('Restricted access');

JFactory::getDocument()->addScriptDeclaration('
    document.addEventListener("DOMContentLoaded", function() {
        var form = document.querySelector(".export_tool form");
        form.addEventListener("submit", function(e) {
            var btn = form.querySelector("button[type=\'submit\']");
            btn.innerText = "' . JText::_('NR_PLEASE_WAIT') . '...";
            document.querySelector(".export_tool").classList.add("working");
        });
    });
');

?>

<div class="export_tool form tmpl-<?php echo $this->tmpl ?>">
    <div class="container">
        <h1><?php echo JText::_('COM_CONVERTFORMS_LEADS_EXPORT') ?></h1>
        <form method="post" action="<?php echo JRoute::_('index.php') ?>" name="adminForm" id="adminForm" >
            <?php echo $this->form->renderFieldset('submission'); ?>
            <button class="btn btn-primary" type="submit">
                <?php echo JText::_('COM_CONVERTFORMS_LEADS_EXPORT') ?>
            </button>
            <input type="hidden" name="option" value="com_convertforms"/>
            <input type="hidden" name="task" value="export.export"/>
            <input type="hidden" name="tmpl" value="<?php echo $this->tmpl ?>"/>
			<?php echo JHtml::_('form.token'); ?>
        </form>
    </div>
</div>