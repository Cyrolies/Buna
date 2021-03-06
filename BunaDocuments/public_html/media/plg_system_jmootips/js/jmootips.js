/**
 * ToolTips - show tooltips on hover (requires MooTools Javascript libraries)
 * 
 * @author Joachim Schmidt - joachim.schmidt@jschmidt-systemberatung.de
 * @copyright Copyright (C) 2013 Joachim Schmidt. All rights reserved.
 * @license GNU/GPL
 * 
 * change activity: June 3, 2013 first release
 * 
 * 15.08.2013: add support for area-tag (image map) 30.08.2013: change tip close
 * behaviour with parm/option "stickyHover" 16.01.2014: add support for mobile
 * and tablet devices 01.02.2014: recode position calculations for image-element
 * and area-element change layout of tip (45% from left - also see css) removed
 * fromLeft option 16.12.2014; force position to top (-1) if screensize is less
 * than 600px;
 */
function createTip() {
	
	new jmootips({
		container : null,            // hovered elements
		focus: false,                // instead mouseenter event use true
		hovered : '.jmootipper',     // the element that when hovered shows
									 // the tip
		ToolTipClass : 'jmootips',   // tooltip display class
		toolTipPosition : -1,        // -1 above; 1: below; -2: right; 2:
									 // 
		autoPosition: 1,             // allow auto-positioning 
		showDelay : 250,
		openOnClick: false,          // open tooltip only if target clicked
		sticky : false,              // remove tooltip if closed
		stickyHover: false,          // remove tooltip with hover in/out
		fromTop : 10,                // distance from mouse or object
		fromLeftRight: 5,
		arrowFromTop: 0.40,          // arrow position 40% from top of tip
		arrowFromLeft: 0.45,         // arrow position 45% from left of tip
		closeMsg: 'Close',
		loadingMsg: 'Loading... please wait.',
		failMsg: '<b>Error:</b> Could not load content',
		duration : 200,             // fade effect transition duration
		fadeDistance : 50	        // the distance the tooltip starts fading
									// in/out
	});
}

var jmootips = new Class({

	Implements : [Options],

	options : {
		container : null,          // hovered elements
		hovered : '.jmootipper',   // the element that when hovered shows the
									// tip
		ToolTipClass : 'jmootips', // tooltip display class
		focus: false,              // instead mouseenter event use true
		toolTipPosition : -1,      // -1 top; 1: bottom; -2: right; 2: left; 3:
		autoPosition: 1,           // allow auto-positioning 
		showDelay : 250,
		openOnClick: false,        // open tooltip only if target clicked
		sticky : false,            // remove tooltip if closed
		stickyHover: true,         // remove tooltip with hover in/out
		fromTop : 12,              // distance from mouse or object
		fromLeftRight: 12,
		arrowFromTop: 0.40,        // arrow position 40% from top of tip
		arrowFromLeft: 0.45,       // arrow position 45% from left of tip
		closeMsg: 'Close',
		loadingMsg: 'Loading... please wait.',
		failMsg: 'Error: Could not load content',
		duration : 200,            // fade effect transition duration
		fadeDistance : 50	       // the distance the tooltip starts fading
								   // in/out
	},

	initialize : function(options) {
		this.setOptions(options || null);
		if (!this.options.hovered)
			return;

		if (this.options.hovered)
			this.elements = $(this.options.container || document.body).getElements(this.options.hovered);

		if (this.elements == undefined)
			this.elements = new Array();

		this.currentElement = null;
		this.attach();
	},

	attach : function() {
		this.elements.each(function(elem, key) {
			
			var parms = new Hash(JSON.decode(elem.getProperty('data-jmootips')));
			parms.include('visible', 0);

			if (parms.sticky != undefined)
			   var sticky_parm = parms.sticky;
			else
			   var sticky_parm = this.options.sticky;
			
			if (parms.width != undefined)
				   var width_parm = parms.width;
				else
				   var width_parm = null;

			if ( this.isMobile() )
			{	
				sticky_parm = true;
				this.options.sticky = true;
				parms.sticky = true;
				this.options.openOnClick = true;
				parms.openOnClick = true;
			}

			if (parms.position == undefined)
			 parms.position = this.options.toolTipPosition;	
			
			if (parms.autoposition == undefined)
			 parms.autoposition = this.options.autoPosition;	
				
			if (parms.autoposition == 1)
			{				
				var element_position = elem.getPosition();
				var element_size = elem.getSize();
				var window_size = window.getScrollSize();

				if (parms.width != undefined)
				{	
				  var width = parseInt(parms.width) + parseInt(50);
				  var width2 =  (parseInt(parms.width) + parseInt(50)) / 2;
	 			} 
								
				switch(parms.position) {
					
				    case 2:		    	
				    	var min_x = width || 500;
				    	var max_x = window_size.x; 
				        break;
				        
				    case -2:
				    	var min_x = 0;
				    	var max_x = window_size.x - (width || 500);
				        break;
				        
				    case  1:
				    	var min_x = width2 || 250;
				    	var max_x = window_size.x - (width2 || 250);
				        var min_y = 0;
				        var max_y = window_size.y - 250;
				     break;	
				     
				    case -1:
				    	var min_x = width2 || 250;
				    	var max_x = window_size.x - (width2 || 250);
					    var min_y = 250;
					    var max_y = window_size.y;
				     break;	
				     
				    default:
				    	//alert ("default");
				        var min_x = 250;
				        var min_y = 250;
				        var_max_x = parseInt(window_size.x - 250);
				        var max_y = window_size.y - 250;
				} 		
			
                if (parms.position == -2 || parms.position == 2)
                 {                   	
				   if ( element_position.x < min_x)
					 parms.position = -2;
				   if ( element_position.x > max_x )
					 parms.position = 2;	
                 }                
                else 
                {	    	 		  
				  if ( element_position.y < min_y)
					parms.position = 1;
				  
				  if ( element_position.y > max_y)
						parms.position = -1;
				  
	   			  if ( element_position.x < min_x)
	    	  			parms.position = -2;
	    	  				  
	    	 	  if ( element_position.x > max_x)
	    	 	  		parms.position = 2;	
                } 				
			}
	
			var tooltip = this.createContainer(sticky_parm, parms.title, parms.position);

			/*
			 * set the tooltip content. depending on where the content is, set
			 * the according parameters.
			 * 
			 * Parameters for every element are: - title: just input some text
			 * directly into the parameter and there you have it - content:
			 * element id to get the tip content from a HTML element (a div
			 * within the page for example ) - ajax: get the content from a
			 * remote page
			 */
			if (parms.title)
				tooltip.title.set({
					'html' : parms.title
				});
			if (parms.text)
				tooltip.message.set({
					'text' : parms.text
				});
			if (parms.content)
			{
				if (parms.width != undefined)
				{	
				 // alert (parms.width);
				 tooltip.message.setStyle('width', parms.width + "px");
				} 
				tooltip.message.set({
					'html' : $(parms.content).get('html')
				});				
			}
			if (parms.ajax) {
				tooltip.message.setStyle('width', parms.width + "px");
				tooltip.message.set({
				  'html' : parms.ajax_message || this.options.loadingMsg
				});
				new Element('div', {
					'class' : 'loading'
				}).inject(tooltip.message);
				/* the actual ajax call is made when element is hovered */
			}

			tooltip.container.store('properties', parms);
			elem.store('tip', tooltip.container);
			$(document.body).adopt(tooltip.container);
			elem.removeProperties('title', 'data-jmootips');
			
			if ( this.isMobile() )
			{	
				parms.openOnClick = true;
				this.options.openOnClick = true;
			}

			if (parms.openOnClick || this.options.openOnClick)
			 {	
			   if (parms.openOnClick != false)	
			    var startEvent = parms.focus ? 'focus' : 'click';
			   else
				var startEvent = parms.focus ? 'focus' : 'mouseenter';
			 }  
			else
			   var startEvent = parms.focus ? 'focus' : 'mouseenter';	
			var endEvent = parms.focus ? 'blur' : 'mouseleave';
		
			elem.addEvent(startEvent, function(e) {
				this.enter(e, elem);
			}.bind(this));
	
				tooltip.container.addEvent(endEvent, function(e) {
					this.leave(e, tooltip.container);
				}.bind(this));

				elem.addEvent(endEvent, function(e) {
					this.leave(e, tooltip.container);
				}.bind(this));	

			if ((parms.stickyHover || this.options.stickyHover) && !(parms.sticky || this.options.sticky)) 
		    {
					tooltip.container.addEvent('mouseleave', this.hide.pass(tooltip.container).bind(this));
		 	}	
			
			if (parms.sticky || this.options.sticky) {
			 if (parms.sticky != false)	
				tooltip.close.addEvent('click', this.hide.pass(tooltip.container).bind(this));
			 else
				tooltip.container.addEvent('mouseleave', this.hide.pass(tooltip.container).bind(this)); 
		 	}

		}, this);
	},

	enter : function (event, element) {

		var tip = element.retrieve('tip');
		
		/* all the tip properties are stored on the element */
		var elProperties = tip.retrieve('properties');
		if (elProperties.visible == 1 && elProperties.loaded == 1)
			return;
		var tip_position = elProperties.position;
		
		if (elProperties.ajax && !elProperties.loaded) {
			
			var failmsg = this.options.failMsg;
			var open_option = elProperties.openOnClick || this.options.openOnClick;
			new Request.HTML({
				url : elProperties.ajax,
				onSuccess: function (a, b, responsetext) {					  		 
					  tip.getElement('.message').set('html', responsetext);
					  elProperties.set('loaded', 1);
					  elProperties.set('visible', 0);
					  element.focus();
					  if (open_option) 
				        element.fireEvent('click');
					  else 
					    element.fireEvent('mouseenter');
					  },
				onFailure: function (xhr) {
					  failmsg = failmsg + " - "  +"Error " +xhr.status +" (" +xhr.statusText +"):<br /> " +elProperties.ajax;
					  tip.getElement('.message').set('html', failmsg);
					  elProperties.set('loaded', 1);
					  elProperties.set('visible', 0);
					  if (open_option) 
					    element.fireEvent('click');
					  else 
						element.fireEvent('mouseenter');
					  }, 
			}).get();
		}
		else
		  elProperties.set('loaded', 1);		 	
		
		var showAfter = elProperties.target ? $(elProperties.target) : element;
		var elSize = showAfter.getCoordinates();
		var tipSize = tip.getCoordinates();
		var adjustment = 10;
		var img_adjustment = 5;
		
		// calculate height of element if element is style
		if (showAfter.get('tag') == 'a')
		{			
			if (showAfter.getStyle('height').toInt() )
			{	
			  var height = showAfter.getStyle('height').toInt();
			  if (tip_position == -2 || tip_position == 2)
				 elSize.top = elSize.top + parseInt(height / 2) - this.options.fromTop;	
			}  
		}
		
		// calculate new element properties if element is image
		if (showAfter.get('tag') == 'img')
		{
			var src = showAfter.getProperty('src');
			var chkImage = new Image();
			chkImage.src = src;
			
			elSize.width = showAfter.getProperty('width') || chkImage.width;
			var height = showAfter.getProperty('height') || chkImage.height;
		
			// check if image will be rescaled
			if (showAfter.getProperty('width') != null && showAfter.getProperty('height') == null)
					height = parseInt(chkImage.height * showAfter.getProperty('width') / chkImage.width);
				
			if (showAfter.getProperty('height') != null && showAfter.getProperty('width') == null)
				elSize.width = parseInt(chkImage.width * showAfter.getProperty('height') / chkImage.height);
			
			if (tip_position == -2 || tip_position == 2)
				elSize.top = elSize.top + parseInt(height / 2) - img_adjustment;	
				
		}

		// calculate new element properties if tag is area
		if (showAfter.get('tag') == 'area')
		{
		  var shape = showAfter.getProperty('shape');
		  var coords = showAfter.getProperty('coords');
		  coords = coords.split(",");

		  elSize.width = parseInt(parseInt(coords[2]) - parseInt(coords[0])) / 2;
		  elSize.right = elSize.left + parseInt(coords[2]);
		  elSize.left = elSize.left + parseInt(coords[0]);
		  elSize.height = parseInt(coords[3]) - parseInt(coords[1]);
		  if ((tip_position == -2 || tip_position == 2))
		    elSize.top  = elSize.top + parseInt(coords[1]) + elSize.height / 2 - this.options.fromTop;
		   else
		    elSize.top  = elSize.top + parseInt(coords[1]);	  
		}  

		this.fromTop = 0;

		if (tip_position == -1 || tip_position == 1)
		{	
		  if (tip_position == -1)
			 this.fromTop = elSize.top - this.options.fromTop - tipSize.height	  
		  else
			 this.fromTop = elSize.top + this.options.fromTop + elSize.height;	 

		  var top_dist = this.fromTop + (elProperties.position)
		  * this.options.fadeDistance;
		  var left_dist = elSize.left - adjustment + parseInt(elSize.width / 2) - parseInt(tipSize.width * this.options.arrowFromLeft);
		}
		else
		{	
		   this.fromTop = elSize.top - 4 - parseInt(tipSize.height * this.options.arrowFromTop);  	

		   var top_dist = this.fromTop + (elProperties.position)
			  * (this.options.fadeDistance / 2);

		   if (tip_position == -2)			   
			 var left_dist = elSize.right + this.options.fromLeftRight;  
		   else
			 var left_dist = elSize.left - (tipSize.width + this.options.fromLeftRight);			   
		}	

		tip.setStyles({
			'top' : top_dist,
			'left' : left_dist,
			'visibility' : 'visible'
		});

		elProperties.set('leave', top_dist);
		this.currentElement = tip;
		this.timer = clearTimeout(this.timer);
		this.timer = this.show.delay(this.options.showDelay, this);
	},
	
	leave : function(event, element) {

		var elProperties = element.retrieve('properties');
		if (elProperties == null)
			return;
		/*
		 * if tooltip is visible and sticky, it closes when close button is
		 * clicked
		 */       
		if ((elProperties.sticky || this.options.sticky) && elProperties.visible) {
		 if (elProperties.sticky !== false)
			return;
		}
		
		if ((elProperties.stickyHover || this.options.stickyHover) && elProperties.visible)
		 if (elProperties.stickyHover !== false)	
		 {
		   if (this.isHovered(event, element))
			return;	   
		   else
		    this.hide(element);
		}		
		
		this.hide(element);
	},

	isHovered: function(event, element) {
	 
			var tipSize = element.getCoordinates();
            // adjust x- and y-coordinates by 20 pixel
			if (( event.page.x+20 > tipSize.left && event.page.x-20 < tipSize.right) && (event.page.y+20 > tipSize.top && event.page.y-20 < tipSize.bottom))
			 return true;
			else
			 return false;		
	},
	
	hide : function(element) {
		this.timer = clearTimeout(this.timer);
		var elProperties = element.retrieve('properties');
		if (elProperties == null)
			return;
		element.morph({
			'opacity' : 0,
			'top' : elProperties.leave
		});
		
		element.setStyles({
			'visibility' : 'hidden',
			'z-index' : '-10000',
			'opacity' : 0,
		});
		
		elProperties.visible = 0;
	},

	show : function() {
		this.currentElement.setStyles({
			'display' : 'block',
			'opacity' : 0,
			'visibility' : 'visible',
			'z-index' : '10000'
		});
	
		this.currentElement.morph({
			'opacity' : 1,
			'top' : this.fromTop
		});
	
		this.setVisible.delay(this.options.duration, this);
		this.setVisible();
	},

	setVisible : function() {
		var elProperties = this.currentElement.retrieve('properties');
		elProperties.visible = 1;
	},

	isMobile : function () {
	
		var up = navigator.userAgent.toLowerCase();
		var isMobilePlatform = /(ipad|iphone|ipod|android|webos|cros|cromium)/i.test(up);
		if (isMobilePlatform)
		 return true;
		
		var ua = navigator.userAgent;
		var isKindle = /(Kindle|Silk|KFTT|KFOT|KFJWA|KFJWI|KFSOWI|KFTHWA|KFTHWI|KFAPWA|KFAPWI)/i.test(ua);
		if (isKindle)
		 return true;

		if( navigator.userAgent.match(/Opera Mini/i) || navigator.userAgent.match(/IEMobile/i) || navigator.userAgent.match(/BlackBerry/i) )
	       return true;
		if( navigator.userAgent.match(/Android/i) || navigator.userAgent.match(/Mobile/i) )
		   return true;		
			
	    return false;	
	},
	
	createContainer : function(sticky, title_content, position) {

		var direction = document.body.getStyle('direction');
		var left_offset = '0px';
		if (direction != undefined)
		{	
		 if (direction == "rtl")
		  {	 
			 left_offset = (window.getSize().x / 2) + "px";
		  }		 
		}

		var container = new Element('div').set({
			'class' : this.options.ToolTipClass,
			'styles' : {
				'position' : 'absolute',
				'top' : '0',
				'left' : left_offset,
				'opacity' : 0,
				'z-index' : '-10000',
				'visibility' : 'hidden'
			},
			'morph' : {
				duration : this.options.duration,
				link : 'cancel',
				transition : Fx.Transitions.Sine.easeOut
			}
		});

		if (title_content)
			var title = new Element('div', {
				'class' : 'title'
			}).inject(container);
		if (sticky) {
			var closeBtn = new Element('div', {
				'class' : 'sticky_close',
				'title' : this.options.closeMsg
			}).inject(container);
		}
		var message = new Element('div', {
			'class' : 'message'
		});
		container.adopt(message);

		switch (position) 
		{
		case -1:
			var arrow_class = "arrow_below";
			break;
		case 1:
			var arrow_class = "arrow_above";
			break;		
		case -2:
			var arrow_class = "arrow_left";
			break;
		case 2:
			var arrow_class = "arrow_right";			
			break;
		default:
			var arrow_class = "arrow_above";			
		}
		
		var arrow = new Element('div', {
				'class' : arrow_class
			}).inject(container, 'top');

		return {
			'container' : container,
			'title' : title || null,
			'message' : message,
			'close' : closeBtn || null
		};
	}
});