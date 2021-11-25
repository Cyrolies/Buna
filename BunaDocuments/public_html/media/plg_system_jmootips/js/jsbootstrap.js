/*!
 * Bootstrap v3.3.1 (http://getbootstrap.com)
 * Copyright 2011-2014 Twitter, Inc.
 * Licensed under MIT (https://github.com/twbs/bootstrap/blob/master/LICENSE)
 */

/*!
 * Generated using the Bootstrap Customizer (http://getbootstrap.com/customize/?id=62c7717d9027d1c2843c)
 * Config saved to config.json and https://gist.github.com/62c7717d9027d1c2843c
 */
if (typeof jQuery === 'undefined') {
  throw new Error('Bootstrap\'s JavaScript requires jQuery');
}
+function ($) {
  var version = $.fn.jquery.split(' ')[0].split('.');
  if ((version[0] < 2 && version[1] < 9) || (version[0] == 1 && version[1] == 9 && version[2] < 1)) {
    throw new Error('Bootstrap\'s JavaScript requires jQuery version 1.9.1 or higher');
  }
}(jQuery);
/* ========================================================================
 * Bootstrap: transition.js v3.3.5
 * http://getbootstrap.com/javascript/#transitions
 * ========================================================================
 * Copyright 2011-2015 Twitter, Inc.
 * Licensed under MIT (https://github.com/twbs/bootstrap/blob/master/LICENSE)
 * ======================================================================== */


+function ($) {
  'use strict';

  // CSS TRANSITION SUPPORT (Shoutout: http://www.modernizr.com/)
  // ============================================================

  function transitionEnd() {
    var el = document.createElement('bootstrap')

    var transEndEventNames = {
      WebkitTransition : 'webkitTransitionEnd',
      MozTransition    : 'transitionend',
      OTransition      : 'oTransitionEnd otransitionend',
      transition       : 'transitionend'
    }

    for (var name in transEndEventNames) {
      if (el.style[name] !== undefined) {
        return { end: transEndEventNames[name] }
      }
    }

    return false // explicit for ie8 (  ._.)
  }

  // http://blog.alexmaccaw.com/css-transitions
  $.fn.emulateTransitionEnd = function (duration) {
    var called = false
    var $el = this
    $(this).one('bsTransitionEnd', function () { called = true })
    var callback = function () { if (!called) $($el).trigger($.support.transition.end) }
    setTimeout(callback, duration)
    return this
  }

  $(function () {
    $.support.transition = transitionEnd()

    if (!$.support.transition) return

    $.event.special.bsTransitionEnd = {
      bindType: $.support.transition.end,
      delegateType: $.support.transition.end,
      handle: function (e) {
        if ($(e.target).is(this)) return e.handleObj.handler.apply(this, arguments)
      }
    }
  })

}(jQuery);

/* ========================================================================
 * Bootstrap: tooltip.js v3.3.1
 * http://getbootstrap.com/javascript/#tooltip
 * Inspired by the original jQuery.tipsy by Jason Frame
 * ========================================================================
 * Copyright 2011-2014 Twitter, Inc.
 * Licensed under MIT (https://github.com/twbs/bootstrap/blob/master/LICENSE)
 * ======================================================================== */


+function ($) {
  'use strict';

  // TOOLTIP PUBLIC CLASS DEFINITION
  // ===============================

  var Tooltip = function (element, options) {
    this.type       =
    this.options    =
    this.enabled    =
    this.timeout    =
    this.hoverState =
    this.$element   = null;

    this.init('tooltip', element, options);
  };

  Tooltip.VERSION  = '3.3.1';

  Tooltip.TRANSITION_DURATION = 150;

  Tooltip.DEFAULTS = {
    animation: true,
    placement: 'top',
    selector: false,
    template: '<div class="tooltip" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
    trigger: 'hover focus',
    title: '',
    delay: 0,
    html: true,
    container: false,
    viewport: {
      selector: 'body',
      padding: 0
    }
  };

  Tooltip.prototype.init = function (type, element, options) {
    this.enabled   = true;
    this.type      = type;
    this.$element  = $(element);
    this.options   = this.getOptions(options);
    this.$viewport = this.options.viewport && $(this.options.viewport.selector || this.options.viewport);

    var triggers = this.options.trigger.split(' ');

    for (var i = triggers.length; i--;) {
      var trigger = triggers[i];

      if (trigger == 'click') {
        this.$element.on('click.' + this.type, this.options.selector, $.proxy(this.toggle, this));
      } else if (trigger != 'manual') {
        var eventIn  = trigger == 'hover' ? 'mouseenter' : 'focusin';
        var eventOut = trigger == 'hover' ? 'mouseleave' : 'focusout';

        this.$element.on(eventIn  + '.' + this.type, this.options.selector, $.proxy(this.enter, this));
        this.$element.on(eventOut + '.' + this.type, this.options.selector, $.proxy(this.leave, this));
      }
    }

    this.options.selector ?
      (this._options = $.extend({}, this.options, { trigger: 'manual', selector: '' })) :
      this.fixTitle();
  };

  Tooltip.prototype.getDefaults = function () {
    return Tooltip.DEFAULTS;
  };

  Tooltip.prototype.getOptions = function (options) {
    options = $.extend({}, this.getDefaults(), this.$element.data(), options);

    if (options.delay && typeof options.delay == 'number') {
      options.delay = {
        show: options.delay,
        hide: options.delay
      };
    }

    return options;
  };

  Tooltip.prototype.getDelegateOptions = function () {
    var options  = {};
    var defaults = this.getDefaults();

    this._options && $.each(this._options, function (key, value) {
      if (defaults[key] != value) options[key] = value;
    });

    return options;
  };

  Tooltip.prototype.enter = function (obj) {
    var self = obj instanceof this.constructor ?
      obj : $(obj.currentTarget).data('bs.' + this.type);

    if (self && self.$tip && self.$tip.is(':visible')) {
      self.hoverState = 'in';
      return
    }

    if (!self) {
      self = new this.constructor(obj.currentTarget, this.getDelegateOptions());
      $(obj.currentTarget).data('bs.' + this.type, self);
    }

    clearTimeout(self.timeout);

    self.hoverState = 'in';

    if (!self.options.delay || !self.options.delay.show) return self.show();

    self.timeout = setTimeout(function () {
      if (self.hoverState == 'in') self.show();
    }, self.options.delay.show);
  };

  Tooltip.prototype.leave = function (obj) {
    var self = obj instanceof this.constructor ?
      obj : $(obj.currentTarget).data('bs.' + this.type);

    if (!self) {
      self = new this.constructor(obj.currentTarget, this.getDelegateOptions());
      $(obj.currentTarget).data('bs.' + this.type, self);
    }

    clearTimeout(self.timeout);

    self.hoverState = 'out';

    if (!self.options.delay || !self.options.delay.hide) return self.hide();

    self.timeout = setTimeout(function () {
      if (self.hoverState == 'out') self.hide();
    }, self.options.delay.hide);
  };

  Tooltip.prototype.show = function () {
    var e = $.Event('show.bs.' + this.type);

    if (this.hasContent() && this.enabled) {
      this.$element.trigger(e);

      var inDom = $.contains(this.$element[0].ownerDocument.documentElement, this.$element[0]);
      if (e.isDefaultPrevented() || !inDom) return
      var that = this;

      var $tip = this.tip();

      var tipId = this.getUID(this.type);

      this.setContent();
      $tip.attr('id', tipId);
      this.$element.attr('aria-describedby', tipId);

      if (this.options.animation) $tip.addClass('fade');

      var placement = typeof this.options.placement == 'function' ?
        this.options.placement.call(this, $tip[0], this.$element[0]) :
        this.options.placement;

      var autoToken = /\s?auto?\s?/i;
      var autoPlace = autoToken.test(placement);
      if (autoPlace) placement = placement.replace(autoToken, '') || 'top';

      $tip
        .detach()
        .css({ top: 0, left: 0, display: 'block' })
        .addClass(placement)
        .data('bs.' + this.type, this);

      this.options.container ? $tip.appendTo(this.options.container) : $tip.insertAfter(this.$element);

      var pos          = this.getPosition();
      var actualWidth  = $tip[0].offsetWidth;
      var actualHeight = $tip[0].offsetHeight;

      if (autoPlace) {
        var orgPlacement = placement;
        var $container   = this.options.container ? $(this.options.container) : this.$element.parent();
        var containerDim = this.getPosition($container);

        placement = placement == 'bottom' && pos.bottom + actualHeight > containerDim.bottom ? 'top'    :
                    placement == 'top'    && pos.top    - actualHeight < containerDim.top    ? 'bottom' :
                    placement == 'right'  && pos.right  + actualWidth  > containerDim.width  ? 'left'   :
                    placement == 'left'   && pos.left   - actualWidth  < containerDim.left   ? 'right'  :
                    placement;

        $tip
          .removeClass(orgPlacement)
          .addClass(placement);
      }

      var calculatedOffset = this.getCalculatedOffset(placement, pos, actualWidth, actualHeight);

      this.applyPlacement(calculatedOffset, placement);

      var complete = function () {
        var prevHoverState = that.hoverState;
        that.$element.trigger('shown.bs.' + that.type);
        that.hoverState = null;

        if (prevHoverState == 'out') that.leave(that);
      };

      $.support.transition && this.$tip.hasClass('fade') ?
        $tip
          .one('bsTransitionEnd', complete)
          .emulateTransitionEnd(Tooltip.TRANSITION_DURATION) :
        complete();
    }
  };

  Tooltip.prototype.applyPlacement = function (offset, placement) {
    var $tip   = this.tip();
    var width  = $tip[0].offsetWidth;
    var height = $tip[0].offsetHeight;

    // manually read margins because getBoundingClientRect includes difference
    var marginTop = parseInt($tip.css('margin-top'), 10);
    var marginLeft = parseInt($tip.css('margin-left'), 10);

    // we must check for NaN for ie 8/9
    if (isNaN(marginTop))  marginTop  = 0;
    if (isNaN(marginLeft)) marginLeft = 0;

    offset.top  = offset.top  + marginTop;
    offset.left = offset.left + marginLeft;

    // $.fn.offset doesn't round pixel values
    // so we use setOffset directly with our own function B-0
    $.offset.setOffset($tip[0], $.extend({
      using: function (props) {
        $tip.css({
          top: Math.round(props.top),
          left: Math.round(props.left)
        });
      }
    }, offset), 0);

    $tip.addClass('in');

    // check to see if placing tip in new offset caused the tip to resize itself
    var actualWidth  = $tip[0].offsetWidth;
    var actualHeight = $tip[0].offsetHeight;

    if (placement == 'top' && actualHeight != height) {
      offset.top = offset.top + height - actualHeight;
    }

    var delta = this.getViewportAdjustedDelta(placement, offset, actualWidth, actualHeight);

    if (delta.left) offset.left += delta.left;
    else offset.top += delta.top;

    var isVertical          = /top|bottom/.test(placement);
    var arrowDelta          = isVertical ? delta.left * 2 - width + actualWidth : delta.top * 2 - height + actualHeight;
    var arrowOffsetPosition = isVertical ? 'offsetWidth' : 'offsetHeight';

    $tip.offset(offset);
    this.replaceArrow(arrowDelta, $tip[0][arrowOffsetPosition], isVertical);
  };

  Tooltip.prototype.replaceArrow = function (delta, dimension, isHorizontal) {
    this.arrow()
      .css(isHorizontal ? 'left' : 'top', 50 * (1 - delta / dimension) + '%')
      .css(isHorizontal ? 'top' : 'left', '');
  };

  Tooltip.prototype.setContent = function () {
    var $tip  = this.tip();
    var title = this.getTitle();

    $tip.find('.tooltip-inner')[this.options.html ? 'html' : 'text'](title);
    $tip.removeClass('fade in top bottom left right');
  };

  Tooltip.prototype.hide = function (callback) {
    var that = this;
    var $tip = this.tip();
    var e    = $.Event('hideme' + this.type);

    function complete() {
      if (that.hoverState != 'in') $tip.detach();
      that.$element
        .removeAttr('aria-describedby')
        .trigger('hidden.bs.' + that.type);
      callback && callback();
    }

    this.$element.trigger(e);

    if (e.isDefaultPrevented()) return

    $tip.removeClass('in');

    $.support.transition && this.$tip.hasClass('fade') ?
      $tip
        .one('bsTransitionEnd', complete)
        .emulateTransitionEnd(Tooltip.TRANSITION_DURATION) :
      complete();

    this.hoverState = null;

    return this;
  };

  Tooltip.prototype.fixTitle = function () {
  	  
    var $e = this.$element;
    if ($e.attr('title') || typeof ($e.attr('data-original-title')) != 'string') {
      $e.attr('data-original-title', $e.attr('title') || '').attr('title', '');
    }
   
  };

  Tooltip.prototype.hasContent = function () {
    return this.getTitle();
  };

  Tooltip.prototype.getPosition = function ($element) {
    $element   = $element || this.$element;

    var el     = $element[0];
    var isBody = el.tagName == 'BODY';

    var elRect    = el.getBoundingClientRect();
    if (elRect.width == null) {
      // width and height are missing in IE8, so compute them manually; see https://github.com/twbs/bootstrap/issues/14093
      elRect = $.extend({}, elRect, { width: elRect.right - elRect.left, height: elRect.bottom - elRect.top });
    }
    var elOffset  = isBody ? { top: 0, left: 0 } : $element.offset();
    var scroll    = { scroll: isBody ? document.documentElement.scrollTop || document.body.scrollTop : $element.scrollTop() };
    var outerDims = isBody ? { width: $(window).width(), height: $(window).height() } : null;

    return $.extend({}, elRect, scroll, outerDims, elOffset);
  };

  Tooltip.prototype.getCalculatedOffset = function (placement, pos, actualWidth, actualHeight) {
    return placement == 'bottom' ? { top: pos.top + pos.height,   left: pos.left + pos.width / 2 - actualWidth / 2  } :
           placement == 'top'    ? { top: pos.top - actualHeight, left: pos.left + pos.width / 2 - actualWidth / 2  } :
           placement == 'left'   ? { top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left - actualWidth } :
        /* placement == 'right' */ { top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left + pos.width   };

  };

  Tooltip.prototype.getViewportAdjustedDelta = function (placement, pos, actualWidth, actualHeight) {
    var delta = { top: 0, left: 0 };
    if (!this.$viewport) return delta;

    var viewportPadding = this.options.viewport && this.options.viewport.padding || 0;
    var viewportDimensions = this.getPosition(this.$viewport);

    if (/right|left/.test(placement)) {
      var topEdgeOffset    = pos.top - viewportPadding - viewportDimensions.scroll;
      var bottomEdgeOffset = pos.top + viewportPadding - viewportDimensions.scroll + actualHeight;
      if (topEdgeOffset < viewportDimensions.top) { // top overflow
        delta.top = viewportDimensions.top - topEdgeOffset;
      } else if (bottomEdgeOffset > viewportDimensions.top + viewportDimensions.height) { // bottom overflow
        delta.top = viewportDimensions.top + viewportDimensions.height - bottomEdgeOffset;
      }
    } else {
      var leftEdgeOffset  = pos.left - viewportPadding;
      var rightEdgeOffset = pos.left + viewportPadding + actualWidth;
      if (leftEdgeOffset < viewportDimensions.left) { // left overflow
        delta.left = viewportDimensions.left - leftEdgeOffset;
      } else if (rightEdgeOffset > viewportDimensions.width) { // right overflow
        delta.left = viewportDimensions.left + viewportDimensions.width - rightEdgeOffset;
      }
    }

    return delta;
  };

  Tooltip.prototype.getTitle = function () {
    var title;
    var $e = this.$element;
    var o  = this.options;

    title = (typeof o.title == 'function' ? o.title.call($e[0]) :  o.title) || $e.attr('data-original-title');

    return title;
  };

  Tooltip.prototype.getUID = function (prefix) {
    do prefix += ~~(Math.random() * 1000000);
    while (document.getElementById(prefix));
    return prefix;
  };

  Tooltip.prototype.tip = function () {
    return (this.$tip = this.$tip || $(this.options.template));
  };

  Tooltip.prototype.arrow = function () {
    return (this.$arrow = this.$arrow || this.tip().find('.tooltip-arrow'));
  };

  Tooltip.prototype.enable = function () {
    this.enabled = true;
  };

  Tooltip.prototype.disable = function () {
    this.enabled = false;
  };

  Tooltip.prototype.toggleEnabled = function () {
    this.enabled = !this.enabled;
  };

  Tooltip.prototype.toggle = function (e) {
    var self = this;
    if (e) {
      self = $(e.currentTarget).data('bs.' + this.type);
      if (!self) {
        self = new this.constructor(e.currentTarget, this.getDelegateOptions());
        $(e.currentTarget).data('bs.' + this.type, self);
      }
    }

    self.tip().hasClass('in') ? self.leave(self) : self.enter(self);
  };

  Tooltip.prototype.destroy = function () {
    var that = this;
    clearTimeout(this.timeout);
    this.hide(function () {
      that.$element.off('.' + that.type).removeData('bs.' + that.type);
    });
  };


  // TOOLTIP PLUGIN DEFINITION
  // =========================

  function Plugin(option) {
    return this.each(function () {
      var $this    = $(this);
      var data     = $this.data('bs.tooltip');
      var options  = typeof option == 'object' && option;
      var selector = options && options.selector;

      if (!data && option == 'destroy') return
      if (selector) {
        if (!data) $this.data('bs.tooltip', (data = {}));
        if (!data[selector]) data[selector] = new Tooltip(this, options);
      } else {
        if (!data) $this.data('bs.tooltip', (data = new Tooltip(this, options)));
      }
      if (typeof option == 'string') data[option]();
    });
  }

  var old = $.fn.tooltip;

  $.fn.tooltip             = Plugin;
  $.fn.tooltip.Constructor = Tooltip;


  // TOOLTIP NO CONFLICT
  // ===================

  $.fn.tooltip.noConflict = function () {
    $.fn.tooltip = old;
    return this;
  };

}(jQuery);

/* ========================================================================
 * Bootstrap: popover.js v3.3.1
 * http://getbootstrap.com/javascript/#popovers
 * ========================================================================
 * Copyright 2011-2014 Twitter, Inc.
 * Licensed under MIT (https://github.com/twbs/bootstrap/blob/master/LICENSE)
 * ======================================================================== */


+function ($) {
  'use strict';

  // POPOVER PUBLIC CLASS DEFINITION
  // ===============================

  var Popover = function (element, options) {
    this.init('popover', element, options);
  };

  if (!$.fn.tooltip) throw new Error('Popover requires tooltip.js');

  Popover.VERSION  = '3.3.1';

  Popover.DEFAULTS = $.extend({}, $.fn.tooltip.Constructor.DEFAULTS, {
    placement: 'right',
    trigger: 'hover',
    content: '',
    template: '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'
  });


  // NOTE: POPOVER EXTENDS tooltip.js
  // ================================

  Popover.prototype = $.extend({}, $.fn.tooltip.Constructor.prototype);

  Popover.prototype.constructor = Popover;

  Popover.prototype.getDefaults = function () {
    return Popover.DEFAULTS;
  };

  Popover.prototype.setContent = function () {
    var $tip    = this.tip();
    var title   = this.getTitle();
    var content = this.getContent();

    $tip.find('.jspopover-title')[this.options.html ? 'html' : 'text'](title);
    $tip.find('.jspopover-content').children().detach().end()[ // we use append for html objects to maintain js events
      this.options.html ? (typeof content == 'string' ? 'html' : 'append') : 'text'
    ](content);

    $tip.removeClass('fade top bottom left right in');

    // IE8 doesn't accept hiding via the `:empty` pseudo selector, we have to do
    // this manually by checking the contents.
    if (!$tip.find('.jspopover-title').html()) $tip.find('.jpopover-title').hide();
  };

  Popover.prototype.hasContent = function () {
    return this.getTitle() || this.getContent();
  };

  Popover.prototype.getContent = function () {
    var $e = this.$element;
    var o  = this.options;

    return $e.attr('data-content')
      || (typeof o.content == 'function' ?
            o.content.call($e[0]) :
            o.content);
  };

  Popover.prototype.arrow = function () {
    return (this.$arrow = this.$arrow || this.tip().find('.arrow'));
  };

  Popover.prototype.tip = function () {
    if (!this.$tip) this.$tip = $(this.options.template);
    return this.$tip;
  };


  // POPOVER PLUGIN DEFINITION
  // =========================

  function Plugin(option) {
    return this.each(function () {
      var $this    = $(this);
      var data     = $this.data('bs.popover');
      var options  = typeof option == 'object' && option;
      var selector = options && options.selector;

      if (!data && option == 'destroy') return
      if (selector) {
        if (!data) $this.data('bs.popover', (data = {}));
        if (!data[selector]) data[selector] = new Popover(this, options);
      } else {
        if (!data) $this.data('bs.popover', (data = new Popover(this, options)));
      }
      if (typeof option == 'string') data[option]();
    });
  }

  var old = $.fn.popover;

  $.fn.popover             = Plugin;
  $.fn.popover.Constructor = Popover;


  // POPOVER NO CONFLICT
  // ===================

  $.fn.popover.noConflict = function () {
    $.fn.popover = old;
    return this;
  };

}(jQuery);

!function($) {

	"use strict"; 
	/*
	 * ToolTips - show tooltips 
	 * 
	 * @author Joachim Schmidt - joachim.schmidt@jschmidt-systemberatung.de
	 * @copyright Copyright (C) 2013 Joachim Schmidt. All rights reserved.
	 * @license GNU/GPL
	 * 
	 * change activity: June 3, 2013 first release
	 *                  changed to support  bootstrap 3.3.1 
	 * 
	*/
	var StickyPopover = function(element, options) {
		this.init('sticky_popover', element, options);
	};

	/*
	 * NOTE: STICKY_POPOVER EXTENDS BOOTSTRAP-POPOVER.js and
	 * BOOTSTRAP-TOOLTIP.js ==========================================
	 */

	StickyPopover.prototype = jQuery.extend({}, jQuery.fn.popover.Constructor.prototype, {

		constructor : StickyPopover

		,
		init : function(type, element, options) {
			if (options.trigger == 'sticky-hover') {
				options.sticky_hover = true;
				options.trigger = 'sticky-hover';
			}
			if (options.trigger == 'click') {
				options.trigger_click = true;
				options.trigger = 'manual';
			}
			if (options.trigger == 'hover') {
				options.sticky_hover = false;
			}
			
			jQuery.fn.popover.Constructor.prototype.init.apply(this, arguments);
			this.displayState = this.displayState || 'hide';

			if (this.options.show)
				this.show();
			if (this.options.trigger_click) {
				this.$element.on('click', jQuery.proxy(this.click_toggle, this));
			}	
			else
			{
				this.$element.on('mouseenter', jQuery.proxy(this.enter_target, this));
				this.$element.on('mouseleave', jQuery.proxy(this.leave_target, this));
			}				
		},
	
		enter_target : function(e) {
			e.preventDefault();
 			if (this.displayState == "show")				
			  this.show(true);
			else
			  this.show();	
		},
		leave_target : function(e) {
			e.preventDefault();			
			if (this.options.trigger == "sticky-hover")
			{  
				var tip = this;
				this.tip().hover(function () {  return; } );
				this.tip().on('mouseleave', function () { tip.hide();});
			}		
			else
			  this.hide();
		},		
		click_toggle : function(e) {
			e.preventDefault();
			this.toggle();
		},
		show : function(force) {				
			if (force || this.displayState !== 'show') {
				if (this.displayState !== 'show')
					this.$element.trigger(jQuery.Event('show'));

				this.displayState = 'show';
				this.trigger_load();
				jQuery.fn.popover.Constructor.prototype.show.apply(this);
			 }
		},
		hide : function() {
			if (this.displayState === 'show') {
				this.displayState = 'hide';
				jQuery.fn.popover.Constructor.prototype.hide.apply(this);
			}
		},
		trigger_load : function() {
			var self = this, href = this.$element.data('popover-href'), loaded = this.$element.data('popover-loaded');

			if (!href)
				return;

			if (!loaded) {
				jQuery.get(href, function(data) {
					var $data = jQuery(data);
					self.$element.attr('data-original-title', $data.filter('.jspopover-title').html());
					self.$element.attr('data-content', $data.filter('.jspopover-content').html());
					self.$element.data('popover-loaded', true);
					if (self.displayState === 'show')
						self.show(true);
				});
			}
		}		
	});

	/*
	 * STICKY_POPOVER PLUGIN DEFINITION =======================
	 */

	jQuery.fn.sticky_popover = function(option) {
		return this.each(function() {
			var $this = jQuery(this), data = $this.data('sticky_popover'), options = typeof option == 'object'
					&& option;
			if (!data) {
				$this.data('sticky_popover', (data = new StickyPopover(this, options)));
				$this.attr('data-sticky_popover', true);
			}
			if (typeof option == 'string')
				data[option]();
		});
	};

	jQuery.fn.sticky_popover.Constructor = StickyPopover;
	jQuery.fn.sticky_popover.defaults = jQuery.extend({}, jQuery.fn.popover.defaults, {});

}(window.jQuery);

function createjsTips(position, trigger) {
	jQuery.noConflict();

	jQuery(".jmootipper")
			.each(
					function() {
						var $pElem = jQuery(this);
						var width = getPopOption($pElem.attr("id"), 'width');
						if (width != "auto")
						 width += "px";	
						// alert(getPopOption($pElem.attr("id"), 'position'));
						$pElem.sticky_popover({
									html : true,
									trigger : getPopOption($pElem.attr("id"), 'trigger') || trigger,
									placement : getPopOption($pElem.attr("id"), 'position') || position,
									delay : {
										show : 300,
										hide : 300
									},
									title : getPopOption($pElem.attr("id"), 'title') || '',
									//width : width || '500',
									content : getPopContent($pElem.attr("id")),
									template : '<div class="jspopover" style="width: auto;"><div class="arrow"></div><div class="jspopover-inner" style="width: auto;"><h3 class="jspopover-title"></h3><div class="jspopover-content" style="width:' + width +';"><p></p></div></div></div>'
								});
					});
};

function isMobile() {
		
	var up = navigator.userAgent.toLowerCase();
	var isMobilePlatform = /(iPad|iPhone|iPod|android|webos|cros|cromium)/i.test(up);
	if (isMobilePlatform)
	 return true;
	
	var ua = navigator.userAgent;
	var isKindle = /(Kindle|Silk|KFTT|KFOT|KFJWA|KFJWI|KFSOWI|KFTHWA|KFTHWI|KFAPWA|KFAPWI)/i.test(ua);
	if (isKindle)
	 return true;
	
	return false;	
}
 
function isJSON(data) {
	var isJson = false;
	try {
		var json = jQuery.parseJSON(data);
		isJson = typeof json === 'object';
	} catch (ex) {
		return false;
	}
	return isJson;
};

function getPopPlacement (elem, parms) {

	var element_position = jQuery("#" +elem).offset();  // returns top and left
	var position = parms.position;

	var window_sizex = Math.max(jQuery(document).width(), jQuery(window).width());
	var window_sizey = Math.max ( jQuery(document).height(), jQuery(window).height() );
	var min_x = 250;
	var max_x = 250;
	
	if (parms.width != undefined)
	{	
	  var width = parseInt(parms.width) + parseInt(50);
	  var width2 =  (parseInt(parms.width) + parseInt(50)) / 2;
	} 

	switch(parms.position) {
		
	    case "left":
	    	var min_x = width || 500;
	    	var max_x = window_sizex; 
	        break;
	    case "right":
	    	var min_x = 0;
	    	var max_x = window_sizex - (width || 500);
	        break;
	    case  "bottom":
	    	var min_x = width2 || 250;
	    	var max_x = window_sizex - (width2 || 250);
	        var min_y = 0;
	        var max_y = window_sizey - 250;
	     break;	
	    case "top":
	    	var min_x = width2 || 250;
	    	var max_x = window_sizex - (width2 || 250);
		    var min_y = 250;
		    var max_y = window_sizey;
	     break;	
	    default:
	        var min_x = 250;
	        var_max_x = parseInt(window_sizex - 250);				        
	 } 
	
    if (parms.position == "right" || parms.position == "left")
     {	
	   if ( parseInt(element_position.left) < min_x)
		 position = "right";
	   if ( parseInt(element_position.left) > max_x )
		 position = "left";	
     }
    else
    {	
	  if ( parseInt(element_position.top) < min_y)
		position = "bottom";	
	  
	  if ( parseInt(element_position.top) > max_y)
		position = "top";
	  
	  if ( parseInt(element_position.left) < min_x)
		position = "right";
	  			  
	  if ( parseInt(element_position.left) > max_x)
 		position = "left";	
    }

    return position;	
}	

function getPopOption(target, option) {
	
	if (option == "trigger" && isMobile())
	 return "click";
	
    var options = jQuery("#" + target + "_content > div.tooltipOptions").text();

	try {
		var parms = JSON.parse(options);
		
		if (option == "trigger")
		  return (parms.trigger);

		if (option == "position")
		{	
		  if (parms.autoposition == "1")
		    return getPopPlacement(target, parms);
		  else
		    return parms.position;
		}
		
		if (option == "title")
		  return (parms.title);
		if (option == "width")
		  return (parms.width || 'auto');
	} catch (err) {
		//alert ('Invalid Tooltip Options: ' + err.message);
		return null;
	}
	return null;
};

function getPopContent(target) {
	
	var content = jQuery("#" + target + "_content > div.tooltipContent").html();
	
	if (content.indexOf('"ajax"') == -1)
		return content;	
	if (isJSON(content) == false)
		return content;
	else {
		var ajaxurl = jQuery.parseJSON(content);
		content = jQuery.ajax({
			url : ajaxurl.ajax,
			type : "GET",
			data : jQuery(this).serialize(),
			dataType : "html",
			async : false
		});
        if (content.statusText == 'OK') 
		  return content.responseText;
        else
          return "Error " +content.status +": " +content.statusText + "<br />" + ajaxurl.ajax;	
	}
};
