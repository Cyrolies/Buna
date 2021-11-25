/**
 * @version $Id$
 * @package DJ-MegaMenu
 * @copyright Copyright (C) 2020 DJ-Extensions.com, All rights reserved.
 * @license DJ-Extensions.com Proprietary Use License
 * @author url: http://dj-extensions.com
 * @author email contact@dj-extensions.com
 * @developer Szymon Woronowski - szymon.woronowski@design-joomla.eu
 */
(function ($) {

	var DJMegaMenu = function (menu, options) {

		this.options = {
			openDelay: 250, // delay before open sub-menu
			closeDelay: 500, // delay before close sub-menu
			animIn: 'fadeIn',
			animOut: 'fadeOut',
			animSpeed: 'normal',
			duration: 450, // depends on speed: normal - 450, fast - 250, slow - 650
			wrap: null,
			direction: 'ltr',
			event: 'mouseenter',
			eventClose: 'mouseleave',
			touch: (('ontouchstart' in window) || (navigator.MaxTouchPoints > 0) || (navigator.msMaxTouchPoints > 0)), // touch screens detection
			offset: 0,
			wcag: 1,
			overlay: 0
		};

		this.init(menu, options);
	};

	DJMegaMenu.prototype.init = function (menu, options) {

		var self = this;

		jQuery.extend(self.options, options);

		//console.log(self.options);

		if ( !menu.length ) return;

		// remove hidden menu items from the DOM
		menu.find('.dj-hideitem').remove();

		// add active class
		menu.find('.modules-wrap ul.nav li.current').each(function () {
			var $this = $(this);
			$this.parents('ul.dj-submenu > li, li.dj-up').each(function () {
				$this.addClass('active').find('> a').addClass('active');
			});
		});

		self.options.menu = menu;
		self.options.blurTimer = null;

		switch (self.options.animSpeed) {
			case 'fast':
				self.options.duration = 250;
				break;
			case 'slow':
				self.options.duration = 650;
				break;
		}

		if( self.options.animIn == '0' && self.options.animOut == '0' ) {
			self.options.duration = 0;
		}

		menu.addClass(self.options.animSpeed);

		var kids = menu.find('li.dj-up');
		self.kids = [];

		self.options.wrap = $('#' + self.options.wrap);
		if (!self.options.wrap.length) self.options.wrap = menu.parents('div').last();

		if (self.options.touch) menu.on('touchstart', function (e) {
			e.stopPropagation();
		}); // stop propagation

		kids.each(function (index) {
			var kid = $(this);
			self.kids[index] = new DJMMenuItem(kid, 0, self, self.options);
		});

		if (self.options.fixed == 1) {
			$(window).one('load', self.makeSticky.bind(self, menu));
		}

		// keyboard arrows navigation
		if (self.options.wcag == 1) {
			self.focusable = menu.find('a[href], [tabindex]');
			menu.on('keydown', function (e) {
				self.focusNearest(e);
			});
		}

		//overlay
		var body = $('body');
		$(document).on('djmegamenu:showsubmenu', function(event, _self) {
			//console.log(_self);
			if(typeof _self.options !== 'undefined' && _self.options.overlay == 1) {
				//console.log('djmegamenu:showsubmenu');
				if( ! body.hasClass('dj-megamenu-overlay') ) {
					body.addClass('dj-megamenu-overlay').append('<div class="dj-megamenu-overlay-box" />');
				}
			}
		});

		$(document).on('djmegamenu:hidesubmenu', function(event, _self) {
			if(typeof _self.options !== 'undefined' && _self.options.overlay == 1) {
				setTimeout(function() {
					//console.log('djmegamenu:hidesubmenu');
					if( _self.options.eventClose == 'mouseleave' && _self.options.menu.find('.parent.hover').length ) return; //make sure that any dropdowns are not opened
					body.removeClass('dj-megamenu-overlay').find('.dj-megamenu-overlay-box').remove();
				}, 50);
			}
		});
		

	};

	DJMegaMenu.prototype.focusNearest = function (event) {
		//console.log('focusNearest');
		var self = this;

		var key = event.which,
			cur = self.options.menu.find(':focus'),
			pos = cur.offset(),
			min = {
				x: 1024,
				y: 1024
			},
			nearest = null;

		if (!pos) return;

		var compare = function (item) {
			if (item.is(':hidden') || item == cur) return;
			var k = item.offset();
			var tmp = {
				x: Math.abs(k.left - pos.left),
				y: Math.abs(k.top - pos.top)
			};
			if ((key == 37 && k.left < pos.left) || (key == 39 && k.left > pos.left)) { // left or right key
				if (tmp.y < min.y || (tmp.y == min.y && tmp.x < min.x)) {
					min = tmp;
					nearest = item;
				}
			} else if ((key == 38 && k.top < pos.top) || (key == 40 && k.top > pos.top) && tmp.x < item.width()) { // up or down key
				if (tmp.x + tmp.y < min.x + min.y) {
					min = tmp;
					nearest = item;
				}
			}
		};

		self.focusable.each(function () {
			compare($(this));
		});

		if (nearest) {
			//console.log(nearest);
			event.preventDefault();
			event.stopPropagation();
			nearest.focus();
		}
	};

	DJMegaMenu.prototype.makeSticky = function (menu) {

		var self = this;

		self.sticky = false;
		var wrapper = $('#' + menu.attr('id') + 'sticky');
		var placeholder = $('<div />');
		placeholder.css({
			display: 'none',
			opacity: 0,
			height: menu.height()
		});
		placeholder.attr('id', menu.attr('id') + 'placeholder');
		placeholder.insertBefore(wrapper);
		$(window).scroll(self.scroll.bind(self, wrapper, menu, placeholder, false));
		$(window).resize(self.scroll.bind(self, wrapper, menu, placeholder, true));
		self.scroll(wrapper, menu, placeholder, false);
		$(window).on('orientationchange', function () {
			setTimeout(function () {
				$(window).trigger('resize');
			}, 500);
		});
	};

	DJMegaMenu.prototype.scroll = function (wrapper, menu, placeholder, resize) {

		var self = this;

		if (menu.is(':hidden')) return;
		var scroll = $(window).scrollTop();
		var step = (self.sticky ? placeholder.offset().top : menu.offset().top) - parseInt(self.options.offset);

		// we need to clean the sticky styles and classes on scroll above the step or window resize
		if (self.sticky && (scroll < step || resize)) {

			menu.css({
				position: '',
				top: '',
				background: '',
				width: '',
				height: ''
			});

			menu.removeClass('dj-megamenu-fixed');
			wrapper.find('.dj-stickylogo').css('display', 'none');

			wrapper.css({
				position: '',
				top: '',
				height: '',
				left: '',
				width: '',
				display: 'none'
			});

			placeholder.css({
				display: 'none',
				'min-width': ''
			});
			// remove the max height for the submenu in sticky megamenu
			menu.find('.dj-up > .dj-subwrap').css({
				'max-height': '',
				'overflow-y': ''
			});
			self.sticky = false;
		}

		// if menu is not sticky (also on resize) we add styles and classes to make it sticky
		if (!self.sticky && scroll >= step) {

			wrapper.css({
				position: 'fixed',
				top: parseInt(self.options.offset),
				left: 0,
				width: '100%',
				display: 'block'
			});

			placeholder.css({
				'min-width': menu.outerWidth(true),
				display: ''
			});

			var lh = 0;
			var logo = wrapper.find('.dj-stickylogo');
			if (logo.length) {
				logo.css('display', '');
				if (logo.hasClass('dj-align-center')) {
					lh = logo.outerHeight(true);
					//console.log(lh);
				}
			}
			menu.css({
				position: 'fixed',
				top: parseInt(self.options.offset) + lh,
				background: 'transparent',
				height: 'auto'
			});

			menu.addClass('dj-megamenu-fixed');
			menu.css('width', placeholder.width() ? placeholder.width() + 1 : 'auto');
			placeholder.css('height', menu.outerHeight());
			// add place for sticky logo
			wrapper.css('height', lh + menu.outerHeight());
			// set the max height for the submenu in sticky megamenu
			var mh = $(window).height() - parseInt(self.options.offset) - wrapper.height();
			menu.find('.dj-up > .dj-subwrap').each(function () {
				if (!$(this).find('.dj-subwrap').length) {
					$(this).css({
						'max-height': mh,
						'overflow-y': 'auto'
					});
				}
			});
			self.sticky = true;
		}
	};

	/* DJMenuItem private constructor class */
	var DJMMenuItem = function (menu, level, parent, options) {
		this.options = {};
		this.init(menu, level, parent, options);
	};

	DJMMenuItem.prototype.init = function (menu, level, parent, options) {

		var self = this;

		jQuery.extend(self.options, options);

		self.menu = menu;
		self.level = level;
		self.parent = parent;

		self.timer = null;
		self.blurTimer = null;

		self.sub = self.menu.find('> .dj-subwrap').first();

		var subitems = self.menu.find('.dj-submenu > li, .dj-subtree > li');

		//menu.mouseenter(function(){console.log(subitems)});

		if ( ! subitems.length ) {
			// no subitems, clean and change to non-parent item
			self.sub.remove();
			self.menu.removeClass('parent');
			self.menu.find('span.dj-drop').removeClass('dj-drop');
			self.menu.find('i.arrow').remove();
		}

		var anchor = self.menu.find('> a').first();
		var href = anchor.attr('href');
		if ( ( ! href ) && ! self.menu.hasClass('separator')) self.menu.addClass('separator');
		if (self.menu.hasClass('separator')) anchor.css('cursor', 'pointer');

		if( self.options.touch || self.options.event == 'click_all' || (self.options.event == 'click' && self.menu.hasClass('separator')) ) {

			anchor.on('touchend click', function (e) {
				if( self.sub.length && ! self.menu.hasClass('hover') ) {
					e.preventDefault();
					if (e.type == 'touchend') self.menu.trigger('click');
				}
			});
		}

		//show submenu
		self.menu.on('click', function(e) {
			var link = $(e.target).closest('.dj-up_a');
			if( ! self.menu.hasClass('hover') ) {
				self.showSub();
			} else if( link.length && (self.menu.hasClass('separator') || link.attr('href') === '#') ) {
				self.hideSub();
			}
		}.bind(self));

		if( self.options.event == 'mouseenter' || (self.options.event == 'click' && ! self.menu.hasClass('separator')) ) {
			self.menu.on('mouseenter', function (e) {
				//console.log('mouseenter');
				self.showSub();
			}.bind(self));
		}

		//close submenu
		if( self.options.touch || self.options.eventClose == 'click' ) {
			$(document).on('mouseup', function (e) {
				if( ! $(e.target).closest('.dj-megamenu').length ) {
					self.hideSub();
				}
			});
		}

		if( self.options.eventClose == 'mouseleave' ) {
			self.menu.on('mouseleave', function (e) {
				//console.log('mouseleave');
				if ( $(e.target).is('input') ) return;
				self.hideSub();
			});
		}

		//self.menu.on("click mouseenter mouseleave touchstart touchend", function(e){ console.log(e.type + ' => ' + e.currentTarget.localName); });

		if (self.options.wcag == 1) {

			self.mouse = false;
			anchor.on('mousedown', function (e) {
				self.mouse = true;
			});

			// keyboard tab navigation compatibility
			anchor.on('focus', function(e) {
				if( self.mouse || $(e.target).is(':hover') ) return;
				//console.log('focus');
				self.showSub();
			}.bind(self));

			anchor.on('blur', function (e) {
				
				if( self.mouse || $(e.target).is(':hover') ) return;
				//console.log('blur');
				self.mouse = false;

				self.blurTimer = setTimeout(function () {
					//console.log('blur2');
					if (!self.options.menu.find(':focus').length) {
						//console.log('blur3');
						var firstLvl = self;
						while (firstLvl.level > 0) {
							firstLvl.hideSub();
							firstLvl = firstLvl.parent;
						}
						firstLvl.hideSub();
					}
				}, 1000);
			});

			self.options.menu.on('click', function () {
				clearTimeout(self.blurTimer);
			});
		}

		if (self.sub.length) {
			self.kids = [];
			self.initKids();
		}
	};



	DJMMenuItem.prototype.showSub = function () {

		//console.log('showSub');

		var self = this;

		clearTimeout(self.timer);

		if (self.menu.hasClass('hover') && !self.sub.hasClass(self.options.animOut)) {
			return; // do nothing if menu is open
		}

		if (self.sub.length) {
			self.sub.css('display', 'none');
		}

		self.timer = setTimeout(function () {

			clearTimeout(self.animTimer);

			self.menu.addClass('hover');
			self.hideOther(); // hide other submenus at the same level

			if (self.sub.length) {
				self.sub.css('display', '');
				self.sub.removeClass(self.options.animOut);
				self.checkDir();
				self.sub.addClass(self.options.animIn);
				self.menu.find('> a').attr('aria-expanded', 'true');
				self.menu.trigger('djmegamenu:showsubmenu', [self]);
				if (self.sub.find('.modules-wrap').length) {
					// it's required to refresh the modules inside the submenu
					$(window).trigger('resize');
				}
			}
		}, self.options.openDelay);
	};

	DJMMenuItem.prototype.hideSub = function () {

		var self = this;

		if (! self.menu.hasClass('hover') ) {
			return; // do nothing if menu is closed
		}

		//console.log('hideSub');
		
		clearTimeout(self.timer);

		if (self.sub.length/* && self.options.eventClose != 'click'*/) {
			self.timer = setTimeout(function () {
				self.sub.removeClass(self.options.animIn);
				self.sub.addClass(self.options.animOut);
				self.animTimer = setTimeout(function () {
					self.menu.removeClass('hover');
					self.menu.find('> a').attr('aria-expanded', 'false');
					self.menu.trigger('djmegamenu:hidesubmenu', [self]);
				}, self.options.duration);
			}, self.options.closeDelay);
		} else {
			self.menu.removeClass('hover');
			self.menu.find('> a').attr('aria-expanded', 'false');
			self.menu.trigger('djmegamenu:hidesubmenu');
		}

	};

	DJMMenuItem.prototype.focusNearest = function (event) {

		var self = this;

		var key = event.which;
		var pos = self.menu.offset(),
			min = {
				x: 1024,
				y: 1024
			},
			nearest = null;

		var compare = function (item) {
			if (!item.menu || !item.menu.find('> a').length) return;
			var k = item.menu.offset();
			var tmp = {
				x: Math.abs(k.top - pos.top),
				y: Math.abs(k.left - pos.left)
			};
			if ((key == 37 && k.left < pos.left) || (key == 39 && k.left > pos.left)) { // left or right key
				if (tmp.x < min.x || (tmp.x == min.x && tmp.y < min.y)) {
					min = tmp;
					nearest = item;
				}
			} else if ((key == 38 && k.top < pos.top) || (key == 40 && k.top > pos.top)) { // up or down key
				if (tmp.y < min.y || (tmp.y == min.y && tmp.x < min.x)) {
					min = tmp;
					nearest = item;
				}
			}
		};

		$.each(self.parent.kids, function (index, kid) {
			compare(kid);
		});

		if (!nearest) {
			compare(self.parent);
			if (self.sub.length) {
				$.each(self.kids, function (index, kid) {
					compare(kid);
				});
			}
		}

		if (nearest) {
			event.preventDefault();
			event.stopPropagation();
			nearest.menu.find('> a').first().focus();
		}
	};

	DJMMenuItem.prototype.checkDir = function () {

		//console.log('checkDir');

		var self = this;

		if (self.menu.hasClass('fullsub')) return;

		self.sub.css('left', '');
		self.sub.css('right', '');
		self.sub.css('margin-left', '');
		self.sub.css('margin-right', '');

		var sub = self.sub.offset();
		var wrap = self.options.wrap.offset();

		//console.log(sub);
		//if(self.options.wrap.hasClass('dj-megamenu')) { // fix wrapper position for sticky menu
		//var placeholder = $('#'+self.options.wrap.attr('id')+'placeholder');
		//if(placeholder.length) wrap = placeholder.offset();
		//}

		if (self.options.direction == 'ltr') {
			var offset = sub.left + self.sub.outerWidth() - self.options.wrap.outerWidth() - wrap.left;
			//console.log(offset+' = '+sub.left+' + '+self.sub.outerWidth()+' - '+self.options.wrap.outerWidth()+' - '+wrap.left);
			if (offset > 0 || self.sub.hasClass('open-left')) {
				if (self.level) {
					self.sub.css('right', self.menu.outerWidth());
					self.sub.css('left', 'auto');
				} else {
					if (self.sub.hasClass('open-left')) {
						self.sub.css('right', self.sub.css('left'));
						self.sub.css('left', 'auto');
					} else {
						self.sub.css('margin-left', -offset);
					}
				}
			}
		} else if (self.options.direction == 'rtl') {
			var offset = sub.left - wrap.left;
			//console.log(offset+' = '+sub.left+' - '+wrap.left);
			if (offset < 0 || self.sub.hasClass('open-right')) {
				if (self.level) {
					self.sub.css('left', self.menu.outerWidth());
					self.sub.css('right', 'auto');
				} else {
					if (self.sub.hasClass('open-right')) {
						self.sub.css('left', self.sub.css('right'));
						self.sub.css('right', 'auto');
					} else {
						self.sub.css('margin-right', offset);
					}
				}
			}
		}
	};

	DJMMenuItem.prototype.initKids = function () {

		var self = this;

		var kids = self.sub.find('> .dj-subwrap-in > .dj-subcol > ul.dj-submenu > li');
		//var sub_options = {h: self.options.hs, w: self.options.ws, o: self.options.os};
		//var cloneOptions = Object.clone(self.options);
		//sub_options = Object.merge(cloneOptions, sub_options);
		kids.each(function (index) {
			var kid = $(this);
			self.kids[index] = new DJMMenuItem(kid, self.level + 1, self, self.options);
		});
	};

	DJMMenuItem.prototype.hideOther = function () {

		//console.log('hideOther');

		var self = this;

		$.each(self.parent.kids, function (index, kid) {

			if (kid.menu.hasClass('hover') && kid != self) {

				if (kid.sub.length) {
					kid.hideOtherSub(); // hide next levels immediately

					kid.sub.removeClass(kid.options.animIn);
					kid.sub.addClass(kid.options.animOut);
					kid.animTimer = setTimeout(function () {
						kid.menu.removeClass('hover');
						kid.menu.find('> a').attr('aria-expanded', 'false');
					}, self.options.duration);
				} else {
					kid.menu.removeClass('hover');
					kid.menu.find('> a').attr('aria-expanded', 'false');
				}
			}
		});
	};

	DJMMenuItem.prototype.hideOtherSub = function () {

		var self = this;

		$.each(self.kids, function (index, kid) {
			if (kid.sub.length) {
				kid.hideOtherSub();
				kid.sub.removeClass(kid.options.animIn);
				kid.sub.removeClass(kid.options.animOut);
			}
			kid.menu.removeClass('hover');
			kid.menu.find('> a').attr('aria-expanded', 'false');
		});
	};

	$(document).ready(function () {

		// init mega menu
		$('.dj-megamenu[data-options]').each(function () {
			var menu = $(this);

			menu.data();
			var options = menu.data('options');
			menu.removeAttr('data-options');

			new DJMegaMenu(menu, options);
		});

	});

})(jQuery);