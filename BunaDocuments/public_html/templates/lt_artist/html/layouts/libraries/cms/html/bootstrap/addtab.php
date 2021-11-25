<?php
/**
 * @package Joomla template Framework
 * @author Ltheme https://www.ltheme.com
 * @copyright Copyright (c) Ltheme
 * @license http://www.gnu.org/licenses/gpl-2.0.html GNU/GPLv2 or Later
*/

defined('JPATH_BASE') or die;

$id       = empty($displayData['id']) ? '' : $displayData['id'];
$active   = empty($displayData['active']) ? '' : $displayData['active'];
$selector = empty($displayData['selector']) ? '' : $displayData['selector'];
$title    = empty($displayData['title']) ? '' : $displayData['title'];
?>
<div id="<?php echo $id; ?>" class="tab-pane<?php echo $active; ?>" data-node="<?php echo htmlspecialchars($active, ENT_COMPAT, 'UTF-8') .'['. htmlspecialchars($id, ENT_COMPAT, 'UTF-8') .'['. htmlspecialchars($title, ENT_COMPAT, 'UTF-8'); ?>">
