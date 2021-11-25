/**
 * @version $Id$
 * @package DJ-MegaMenu
 * @copyright Copyright (C) 2017 DJ-Extensions.com, All rights reserved.
 * @license DJ-Extensions.com Proprietary Use License
 * @author url: http://dj-extensions.com
 * @author email contact@dj-extensions.com
 * @developer Szymon Woronowski - szymon.woronowski@design-joomla.eu
 */

(function($) {

	var createSelectMenu = function(menu, mobile) {
		
		// Create the dropdown base
		var id = menu.attr('id');
		var name = mobile.attr('data-label') || 'menu';

		var select = $('<select id="'+ id +'select" class="inputbox dj-select" />')
		.on('change', function(){
			if($(this).val) window.location = $(this).val();
		});

		var label = $('<label class="hidden" aria-hidden="true" for="'+ id +'select">' + name + '</label>')

		var list = menu.find('li.dj-up');
		addOptionsFromDJMegaMenu(list, select, 0);
		
		label.appendTo(mobile);
		select.appendTo(mobile);
		
		mobile.find('.dj-mobile-open-btn').on('click', function(e){
			e.stopPropagation();
			e.preventDefault();
			
			var element = select[0];
			if (document.createEvent) { // all browsers
				var ev = document.createEvent("MouseEvents");
				ev.initMouseEvent("mousedown", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
				element.dispatchEvent(ev);
			} else if (element.fireEvent) { // ie
				element.fireEvent("onmousedown");
			}
		});
	};
	
	var addOptionsFromDJMegaMenu = function(items, select, level) {
		
		var sep = '',
		active = false;
		for(var i=0;i<level;i++) {
			sep += '- ';
		}
		//console.log(items);
		items.each(function(){
			
			var item = $(this);

			var link = item.find('> a').first();
			var kids = item.find('> .dj-subwrap > .dj-subwrap-in > .dj-subcol > .dj-submenu > li, > .dj-subtree > li');
			
			if(link.length) {
				var text = '';
				var img = link.find('img').first();
				if(img.length) {
					text = sep + img.attr('alt');
				} else {
					text = link.html().replace(/(<small[^<]+<\/small>)/ig,"");
					text = sep + text.replace(/(<([^>]+)>)/ig,"");
				}
				//console.log(text);
				var option = $('<option value="'+link.prop('href')+'">'+ text +'</option>').appendTo(select);
				if(!link.prop('href')) { option.prop('disabled',true); }
				if(item.hasClass('active')) {
					select.val(option.val());
					active = true;
				}
			}
			if(kids) addOptionsFromDJMegaMenu(kids, select, level+1);
		});
		
		if(!level && !active) {
			//$('<option value="">Menu</option>').prependTo(select);
			select.val('');
		}
	};
	
	var initAccordion = function(mobile) {
		//console.log('initAccordion');
		var focusTimer = null;
		
		mobile.find('ul.dj-mobile-nav > li, ul.dj-mobile-nav-child > li').each(function(){
			
			var menu = $(this);
			
			var anchor = menu.find('> a').first();

			if(anchor.length) {
				var subs = menu.find('> ul.dj-mobile-nav-child > li:not(:empty)');

				if(!subs.length) {
					menu.removeClass('parent');
					menu.find('ul.dj-mobile-nav-child').remove();
				}

				if(menu.hasClass('parent')) {
					
					if(menu.hasClass('active')) {
						anchor.attr('aria-expanded', true);
					} else {
						anchor.attr('aria-expanded', false);
					}
					
					anchor.append('<span class="toggler"></span>');

					anchor.on('click',function(e){
						if(!menu.hasClass('active')) e.preventDefault();
						else if($(e.target).hasClass('toggler')) {
							e.preventDefault();
							e.stopPropagation();
							clearTimeout(focusTimer);
							menu.removeClass('active');
							anchor.attr('aria-expanded', false);
						}
					});
				}
				
				anchor.on('focus',function(){
					focusTimer = setTimeout(function(){
						menu.click();
					}, 250);
				});
			}
			
			menu.on('click', function(){
				menu.siblings().removeClass('active');
				menu.siblings().find('> a').attr('aria-expanded', false);
				menu.addClass('active');

				if(anchor.length) {
					anchor.attr('aria-expanded', true);

					if ( anchor.is('[href^="#"]') ) {
						var offcanvas_btn = mobile.find('.dj-offcanvas-close-btn');
						if( offcanvas_btn.length ) {
							$(document.body).addClass('dj-offcanvas-no-effects');
							offcanvas_btn.click();
						}
					}
				}

			});
		});
	};
	
	var createOffcanvas = function(mobile) {
		
		var content 	= null;
		var wrapper 	= jQuery('.dj-offcanvas-wrapper').first();
		var pusher 		= jQuery('.dj-offcanvas-pusher').first();
		var pusherIn 	= jQuery('.dj-offcanvas-pusher-in').first();
		
		if(!wrapper.length) {
			content		= $(document.body).children();
			wrapper		= $('<div class="dj-offcanvas-wrapper" />');
			pusher		= $('<div class="dj-offcanvas-pusher" />');
			pusherIn	= $('<div class="dj-offcanvas-pusher-in" />');
		}
		
		var offcanvas = mobile.find('.dj-offcanvas').first();
		var fx = offcanvas.data('effect');
		
		$(document.body).addClass('dj-offcanvas-effect-' + fx);
		
		var timer = null; 
		
		mobile.find('.dj-mobile-open-btn').on('click', function(e){
			e.stopPropagation();
			e.preventDefault();
			clearTimeout(timer);
			offcanvas.data('scroll', $(window).scrollTop());
			$(document.body).addClass('dj-offcanvas-anim').removeClass('dj-offcanvas-no-effects');
			setTimeout(function(){
				$(document.body).addClass('dj-offcanvas-open');
			}, 50 );
			pusherIn.css('top', -offcanvas.data('scroll'));
			offcanvas.find('.dj-offcanvas-close-btn').focus();
		});
		
		if(content) $(document.body).prepend(wrapper);
		if(fx == 3 || fx == 6 || fx == 7 || fx == 8 || fx == 14) {
			pusher.append(offcanvas);
		} else {
			wrapper.append(offcanvas);
		}
		if(content) {
			wrapper.append(pusher);
			pusher.append(pusherIn);
			pusherIn.append(content);
		}
		
		offcanvas.find('.dj-offcanvas-close-btn').on('click', function(e){
			e.stopPropagation();
			e.preventDefault();

			if($(document.body).hasClass('dj-offcanvas-open')) {
				$(document.body).removeClass('dj-offcanvas-open');
				
				if( $(document.body).hasClass('dj-offcanvas-no-effects') ) {
					pusherIn.css('top', 0);
					$(document.body).removeClass('dj-offcanvas-anim');
					timer = setTimeout(function(){
						$(document.body).removeClass('dj-offcanvas-no-effects');
					}, 500 );
				} else {
					timer = setTimeout(function(){
						pusherIn.css('top', 0);
						$(document.body).removeClass('dj-offcanvas-anim');
						$(window).scrollTop($(window).scrollTop() + offcanvas.data('scroll'));
						mobile.find('.dj-mobile-open-btn').focus();
					}, 500 );
				}
				
			}
		});
		
		$('.dj-offcanvas-pusher').on('click', function(e){
			if(!$(e.target).hasClass('dj-offcanvas-pusher')) return;
			offcanvas.find('.dj-offcanvas-close-btn').click();
		});
		
		offcanvas.find('.dj-offcanvas-close-btn').on('keydown', function(event){
			if(event.which==9) {
				setTimeout(function(){
					if(!offcanvas.find(':focus').length){
						offcanvas.find('.dj-offcanvas-close-btn').click();
					}
				}, 50);
			}
		});
		
		offcanvas.find('.dj-offcanvas-end').on('focus', function(){
			offcanvas.find('.dj-offcanvas-close-btn').click();
		});
		
		initAccordion(offcanvas);
	};
	
var createSimpleOffcanvas = function(mobile) {
		
		var offcanvas = mobile.find('.dj-offcanvas').first();
		
		$(document.body).addClass('dj-offcanvas-effect-1');
		
		var timer = null; 
		
		mobile.find('.dj-mobile-open-btn').on('click', function(e){
			e.stopPropagation();
			e.preventDefault();
			clearTimeout(timer);
			offcanvas.data('scroll', $(window).scrollTop());
			$(document.body).addClass('dj-offcanvas-anim');
			setTimeout(function(){
				$(document.body).addClass('dj-offcanvas-open');
			}, 50 );
			offcanvas.find('.dj-offcanvas-close-btn').focus();
		});
		
		offcanvas.find('.dj-offcanvas-close-btn').on('click', function(e){
			e.stopPropagation();
			e.preventDefault();
			if($(document.body).hasClass('dj-offcanvas-open')) {
				$(document.body).removeClass('dj-offcanvas-open');
				
				timer = setTimeout(function(){
					$(document.body).removeClass('dj-offcanvas-anim');
					mobile.find('.dj-mobile-open-btn').focus();
				}, 500 );
			}
		});
		
		offcanvas.find('.dj-offcanvas-close-btn').on('keydown', function(event){
			if(event.which==9) {
				setTimeout(function(){
					if(!offcanvas.find(':focus').length){
						offcanvas.find('.dj-offcanvas-close-btn').click();
					}
				}, 50);
			}
		});
		
		offcanvas.find('.dj-offcanvas-end').on('focus', function(){
			offcanvas.find('.dj-offcanvas-close-btn').click();
		});
		
		initAccordion(offcanvas);
	};
	
	var createAccordion = function(mobile) {
		
		mobile.find('.dj-mobile-open-btn').on('click', function(e){
			e.stopPropagation();
			e.preventDefault();
			mobile.find('.dj-accordion-in').slideToggle('fast');
		});
		
		$(document).on('click', function(e){
			if(!$(e.target).closest('.dj-accordion-in').length) {
				if(mobile.find('.dj-accordion-in').is(':visible')) mobile.find('.dj-accordion-in').slideUp('fast');
			}
		});
		initAccordion(mobile);
		
	};
	
	var mega = [];
	
	var timer = null;
	var switchMenu = function() {
		
		window.clearTimeout(timer);
		timer = window.setTimeout(function(){
			
			for(var idx = 0; idx < mega.length; idx++) {
				
				if(mega[idx].mobile) {
					
					if(window.matchMedia("(max-width: "+mega[idx].trigger+"px)").matches) {
						
							$(document.body).addClass('dj-megamenu-mobile');
							$(document.body).addClass(mega[idx].id+'-mobile');
							// we need only one menu in DOM
							if($.contains(document, mega[idx].menu[0])) {
								mega[idx].menu.after(mega[idx].menuHandler);
								mega[idx].menu.detach();
							}
							if($.contains(document, mega[idx].mobileHandler[0])) {
								mega[idx].mobileHandler.replaceWith(mega[idx].mobile);
							}
							if($.contains(document, mega[idx].offcanvasHandler[0])) {
								mega[idx].offcanvasHandler.replaceWith(mega[idx].offcanvas);
							}
							
					} else {
						
							$(document.body).removeClass('dj-megamenu-mobile');
							$(document.body).removeClass(mega[idx].id+'-mobile');
							// we need only one menu in DOM
							if($.contains(document, mega[idx].mobile[0])) {
								mega[idx].mobile.after(mega[idx].mobileHandler);
								mega[idx].mobile.detach();
							}
							if(mega[idx].offcanvas && $.contains(document, mega[idx].offcanvas[0])) {
								mega[idx].offcanvas.after(mega[idx].offcanvasHandler);
								mega[idx].offcanvas.detach();
							}
							if($.contains(document, mega[idx].menuHandler[0])) {
								mega[idx].menuHandler.replaceWith(mega[idx].menu);
							}
					}
				}
			}
			
		}, 100);
	};
	
	$(document).ready(function(){
		
		// init mobile menu
		$('.dj-megamenu:not(.dj-megamenu-sticky)').each(function(){
			
			var menu = $(this);
			var mobile = $('#'+menu.prop('id')+'mobile');
			var offcanvas = $('#'+menu.prop('id')+'offcanvas');
			
			var idx = mega.length;
			mega[idx] = {};
			mega[idx].id = menu.prop('id');
			mega[idx].trigger = menu.data('trigger');
			mega[idx].menu = menu;
			mega[idx].menuHandler = $('<div />');
			mega[idx].mobile = mobile.length ? mobile : null;
			mega[idx].mobileHandler = $('<div />');
			mega[idx].offcanvas = offcanvas.length ? offcanvas : null;
			mega[idx].offcanvasHandler = $('<div />');
			
			var wrapper = $('#'+menu.prop('id')+'mobileWrap');
			if(wrapper.length) {
				wrapper.empty().append(mega[idx].mobile);
			}
			
			if(mega[idx].mobile) {
				
				// remove hidden menu items from the DOM
				mega[idx].mobile.find('.dj-hideitem').remove();
				
				if(mega[idx].mobile.hasClass('dj-megamenu-offcanvas')) {
					
					var isJoomla4 = mobile.parents('[data-joomla4]').length ? true : false;
					
					if(isJoomla4) {
						createSimpleOffcanvas(mega[idx].mobile);
						//createOffcanvas(mega[idx].mobile);
					} else {
						createOffcanvas(mega[idx].mobile);
					}
					
				} else if(mega[idx].mobile.hasClass('dj-megamenu-accordion')) {
					
					createAccordion(mega[idx].mobile);
				}
				
				// fix double collapse Joomla!3 Protostar 
				mega[idx].mobile.parents('.nav-collapse').addClass('collapse in').css('height', 'auto').prev('.navbar').remove();
				
				// fix double collapse Joomla!4 Cassiopeia
				mega[idx].mobile.parents('.navbar-collapse').addClass('show').prev('.navbar-toggler').remove();
			}
			
			
		});
		
		$(window).resize(switchMenu);
		switchMenu();
	});	
	
	$(window).one('load', function(){
		
		for(var idx = 0; idx < mega.length; idx++) {
			
			if(mega[idx].mobile) {
				
				if(mega[idx].mobile.hasClass('dj-megamenu-select')) {
					createSelectMenu(mega[idx].menu, mega[idx].mobile);
				}
			}
		}
		
		$('.dj-offcanvas-close-btn').click();
	});
	
})(jQuery);
