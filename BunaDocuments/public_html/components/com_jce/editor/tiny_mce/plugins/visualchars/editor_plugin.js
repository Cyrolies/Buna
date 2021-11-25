/* jce - 2.7.18 | 2019-09-26 | https://www.joomlacontenteditor.net | Copyright (C) 2006 - 2019 Ryan Demmer. All rights reserved | GNU/GPL Version 2 or later - http://www.gnu.org/licenses/gpl-2.0.html */
!function(){var Cookie=tinymce.util.Cookie;tinymce.create("tinymce.plugins.VisualChars",{init:function(ed,url){var state,self=this;self.editor=ed,ed.getParam("use_state_cookies",!0)&&(state=Cookie.get("wf_visualchars_state")),state=tinymce.is(state,"string")?parseFloat(state):ed.getParam("visualchars_default_state",0),ed.onInit.add(function(){ed.controlManager.setActive("visualchars",state),self._toggleVisualChars(state)}),ed.addButton("visualchars",{title:"visualchars.desc",cmd:"mceVisualChars"}),ed.onExecCommand.add(function(ed,cmd,ui,v,o){"mceNonBreaking"===cmd&&self._toggleVisualChars(state)}),ed.addCommand("mceVisualChars",function(){state=!state,ed.controlManager.setActive("visualchars",state),self._toggleVisualChars(state),ed.getParam("use_state_cookies",!0)&&Cookie.set("wf_visualchars_state",state?1:0)},self),ed.onKeyUp.add(function(ed,e){state&&13==e.keyCode&&self._toggleVisualChars(state)}),ed.onPreProcess.add(function(ed,o){o.get&&self._toggleVisualChars(!1,o.node)}),ed.onSetContent.add(function(ed,o){self._toggleVisualChars(state)})},_toggleVisualChars:function(state,o){function wrapCharWithSpan(value){return'<span data-mce-bogus="1" class="mce-item-'+charMap[value]+'">'+value+"</span>"}function compileCharMapToRegExp(){var key,regExp="";for(key in charMap)regExp+=key;return new RegExp("["+regExp+"]","g")}function compileCharMapToCssSelector(){var key,selector="";for(key in charMap)selector&&(selector+=","),selector+="span.mce-item-"+charMap[key];return selector}function isNode(n){return 3===n.nodeType&&!ed.dom.getParent(n,".mce-item-nbsp, .mce-item-shy")}var node,nodeList,i,nodeValue,div,charMap,visualCharsRegExp,ed=this.editor,body=o||ed.getBody();ed.selection;if(charMap={" ":"nbsp","­":"shy"},visualCharsRegExp=compileCharMapToRegExp(),state)for(nodeList=[],tinymce.walk(body,function(n){isNode(n)&&n.nodeValue&&visualCharsRegExp.test(n.nodeValue)&&nodeList.push(n)},"childNodes"),i=0;i<nodeList.length;i++){for(nodeValue=nodeList[i].nodeValue,nodeValue=nodeValue.replace(visualCharsRegExp,wrapCharWithSpan),div=ed.dom.create("div",null,nodeValue);node=div.lastChild;)ed.dom.insertAfter(node,nodeList[i]);ed.dom.remove(nodeList[i])}else for(nodeList=ed.dom.select(compileCharMapToCssSelector(),body),i=nodeList.length-1;i>=0;i--)ed.dom.remove(nodeList[i],1)}}),tinymce.PluginManager.add("visualchars",tinymce.plugins.VisualChars)}();