!function(){"use strict";document.addEventListener("ConvertFormsBeforeSubmit",function(e){var t=e.detail.instance.selector.querySelectorAll('.cf-control-group[data-type="editor"] textarea');0!=t.length&&t.forEach(function(e){var t=Joomla.editors.instances[e.id];t&&(e.value=t.getValue())})})}();

