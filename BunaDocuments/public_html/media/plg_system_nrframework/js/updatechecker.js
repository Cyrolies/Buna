!function(){"use strict";document.addEventListener("DOMContentLoaded",function(){setTimeout(function(){!function(){var e=document.querySelector(".nr_updatechecker"),t=e.dataset,n="?option=com_ajax&format=raw&plugin=nrframework&task=updatenotification&element="+t.element+"&"+t.token+"=1",o=new XMLHttpRequest;o.open("POST",t.base+n),o.onload=function(){200<=o.status&&o.status<300&&-1<o.response.indexOf("nr-updatechecker")&&(e.innerHTML=o.response)},o.send()}()},1e3)})}();

