

var ServerLog = "https://stats.bizweb.vn/";
window.logging = {
    setCookie: function (name, value, expires, path, domain, secure) {
        var today = new Date();
        today.setTime(today.getTime());
        if (expires) {
            expires = expires * 1000 * 60 * 60 * 24;
        }
        var expires_date = new Date(today.getTime() + (expires));

        document.cookie = name + "=" + escape(value) + ((expires) ? ";expires=" + expires_date.toGMTString() : "") + ((path) ? ";path=" + path : "") + ((domain) ? ";domain=" + domain : "") + ((secure) ? ";secure" : "");
    },
    getCookie: function (name) {
        var start = document.cookie.indexOf(name + "=");
        var len = start + name.length + 1;
        if ((!start) && (name != document.cookie.substring(0, name.length))) {
            return null;
        }
        if (start == -1) return null;
        var end = document.cookie.indexOf(";", len);
        if (end == -1) end = document.cookie.length;
        return unescape(document.cookie.substring(len, end));
    }
};

function SiteStats(SiteID) {
    this.siteId = SiteID;
}

SiteStats.prototype.toHtml = function () {
    var html = this.getLink();
    return html;
}

SiteStats.prototype.getLink = function () {

    if (document.referrer == null || document.referrer == "") {

        var link = '<div style="display:none"><img src="' + ServerLog + "Delivery/Logging?SiteId=" + this.siteId + "&Url=" + document.URL + '&ReferenceUrl=Null"/></div>';
    }
    else {
        var link = '<div style="display:none"><img src="' + ServerLog + "Delivery/Logging?SiteId=" + this.siteId + "&Url=" + document.URL + "&ReferenceUrl=" + document.referrer + '"/></div>';
    }
    return link;
}

SiteStats.prototype.draw = function () {
    //this.bindCSS(this.css);
    var newStats = document.createElement("div");
    newStats.innerHTML = this.toHtml();
    document.body.appendChild(newStats);
}

function Statistic(b) {
    if (b) {
        var temp = {
            SiteId: parseInt(b.SiteId),
            Current: parseInt(b.Current),
            Today: parseInt(b.Today),
            Yesterday: parseInt(b.Yesterday),
            Total: parseInt(b.Total)
        };
        var property;
        for (property in b) this[property] = b[property];
    }
};

Statistic.prototype.toHtml = function () {

    var html = '<table class="UserOnlineTable"><tbody><tr id="showOnlineCurrent"><td class="showIconToday"></td>';
    html += '<td><span class="UserOnlineLabel">Đang trực tuyến: </span></td>';
    html += '<td><span style="font-weight:bold;" class="UserOnlineValue">' + this.Current + '</span></td></tr>';
    html += '<tr id="showOnlineToday"><td class="showIconYestoday"></td><td><span class="UserOnlineLabel">Hôm nay: </span></td>';
    html += '<td><span style="font-weight:bold;" class="UserOnlineValue">' + this.Today + '</span></td></tr>';
    html += '<tr id="showOnlineYesterday"><td class="showIconYestoday"></td><td><span class="UserOnlineLabel">Hôm qua: </span></td>';
    html += '<td><span style="font-weight:bold;" class="UserOnlineValue">' + this.Yesterday + '</span></td></tr>';
    html += '<tr id="showOnlineTotal"><td class="showIconVisitor"></td><td><span class="UserOnlineLabel">Tất cả: </span></td>'; 

    html += '<td><span style="font-weight:bold;" class="UserOnlineValue">' + this.Total + '</span></td></tr></tbody></table>';
    return html;  
}

function Preview(statistic) {
    this.statistic = new Statistic(statistic);
}

Preview.prototype.toHtml = function () {
    //alert(JSON.stringify(this.statistic.Style));
    var html = this.statistic.toHtml();
    return html;
}

Preview.prototype.print = function () {
    //    <div id="ViewStats"></div>
    var viewStat = document.getElementById('ViewStats');
    if (viewStat != null) {
        viewStat.innerHTML = this.toHtml();
    }
    //document.write(this.toHtml());
}

function getTracking() {
    var siteStats = new SiteStats(263497); 
    siteStats.draw();
    
}
var _admTrackingTime = 0;
function checkgetTracking() {

    if ((window.document.readyState == 'complete') || (typeof (window.document.readyState) == 'undefined')) {
        if (!_trackingSend) {
            _trackingSend = true;
            getTracking();
        }
    }
    else {
        _admTrackingTime++;
        if (_admTrackingTime == 5) {
            if (!_trackingSend) {
                _trackingSend = true;
                getTracking();
            }
            return false;
        }
        setTimeout("checkgetTracking()", 1000);
    }
}
if (typeof (_trackingSend) == 'undefined') {
    var _trackingSend = false;
}
checkgetTracking();
