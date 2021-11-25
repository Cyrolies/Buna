/**
 * Function controlling how collapsible facet blocks work.
 */

(function($) {
 
  Drupal.behaviors.suitcaseFacet = {
    attach: function (context) {
      $('body', context).once('suitcaseFacet', function() {

        // Add a javascript class to blocks
        $('.zone-content section.block').addClass('js-suitcase-facet');

        // Add the caret to facet titles
        $('.js-suitcase-facet .block-title').each(function() {
          $(this).append('<span class="caret"></span>');
        });

        // If the window is less than than 980px wide...
        if($(window).width() < 980) {
          // Close all blocks not open by default
          $('.js-suitcase-facet:not(.block-default-open) .block-title').addClass('block-title-closed');
          $('.js-suitcase-facet:not(.block-default-open) .content').addClass('block-closed');
        };

        // If a block is closed by defatul, close everything
        $('.block-default-closed .block-title').addClass('block-title-closed');
        $('.block-default-closed .content').addClass('block-closed');

        // When the title is clicked...
        $('.block-title').click(function() {
          // Toggle the title class
          $(this).toggleClass('block-title-closed');
          // Toggle the closed class to open/closed
          $(this).parent().find('.content').toggleClass('block-closed');
        });

      });
    }
  };
})(jQuery);

/**
 * NOTES:
 * 1. If javascript is disabled, all blocks are always open.
 * 2. If javascript is enabled, and the screen is narrow, all blocks are closed by default.
 * 3. If javascript is enabled, blocks closed by default begin closed
 * 4. If javascript is enabled, blocks can be open/closed on click/tap

 * To set a block closed by default (on screens wider than 980px), give the .js-suitcase-facet 
 * block the .block-default-closed class.

 * To force a block to begin opened on mobile, give the .js-suitcase-facet block the
 * .block-default-open class.
*/