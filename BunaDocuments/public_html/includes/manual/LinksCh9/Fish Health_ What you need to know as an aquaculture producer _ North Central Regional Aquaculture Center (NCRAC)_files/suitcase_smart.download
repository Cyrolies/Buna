/**
 * @todo
 */

(function($) {
  /**
   * @todo
   */
  Drupal.behaviors.suitcaseSmartMenu = {
    attach: function (context, settings) {

      // do this using .each so that data-sm-options attribute is correctly detected and applied separately for each menu
      $('.sm', context).once('smartmenus-behavior', function() { $(this).smartmenus(); });

      // https://www.smartmenus.org/docs/#menu-toggle-button
      $('.sm-menu-state', context).once('smartmenus-behavior', function() {
        var $menuToggleState = $(this);
        // animate mobile menu
        $menuToggleState.change(function(e) {
          var $menu = $('#' + this.id.slice(0, -6));
          if (this.checked) {
            $menu.hide().slideDown(250, function() { $menu.css('display', ''); });
          } else {
            $menu.show().slideUp(250, function() { $menu.css('display', ''); });
          }
        });
        // hide mobile menu beforeunload
        $(window).bind('beforeunload unload', function() {
          if ($menuToggleState[0].checked) {
            $menuToggleState[0].click();
          }
        });
      });
    }
  };
})(jQuery);