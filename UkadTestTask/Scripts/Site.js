var isLoadAnimCompleted = false;
var intervalId;
var pattern = /^(http|https)?:\/\/[a-zA-Z0-9-\.]+\.[a-z]{2,4}/;
function LoadComplete() {
    //$("body").animate({ backgroundColor: "#0072c6" }, 600);
    clearInterval(intervalId);
    $("#load-bar-1").animate({ left: "-200%" }, 700, function() { $("#load-bar-1").hide(); });
}

$(document).ready(function () {
    $("#main-desc").show();
    $("#start-handle-button").click(function (e) {

        if (!pattern.test($("#input-url").val())) {
            $.notify({                
                title: 'Invalid Url',
                message: 'Please check the address is correct'                
            }, {
                type: "danger",
                allow_dismiss: true,
                newest_on_top: true,
                showProgressbar: false,
                placement: {
                    from: "top",
                    align: "center"
                },animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                },               
                template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                    '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                    '<span data-notify="icon"></span> ' +
                    '<span data-notify="title">{1}</span> ' +
                    '<span data-notify="message">{2}</span>' +
                    '<div class="progress" data-notify="progressbar">' +
                    '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                    '</div>' +
                    '<a href="{3}" target="{4}" data-notify="url"></a>' +
                    '</div>'
            });
            e.preventDefault();
            return;
        }

        isLoadAnimCompleted = false;
        $("#desc-1").animate({"margin-left": "250%" }, 600, "swing" );
        $("#desc-2").animate({ "margin-left": "-250%" }, 600, "swing");
        $("body").animate({ backgroundColor: "#304249" }, 1000);

        $("#load-bar-1").fadeOut();        
        
        $(".navbar").removeClass("navbar-default");
        $(".navbar").addClass("navbar-inverse");

        $("#input-group-url").fadeOut();
        
        
        var base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64 } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9+/=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u != 64) { t = t + String.fromCharCode(r) } if (a != 64) { t = t + String.fromCharCode(i) } } t = base64._utf8_decode(t); return t }, _utf8_encode: function (e) { e = e.replace(/rn/g, "n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128) } } return t }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++ } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3 } } return t } }
        var url = $("#input-group-url").find("input").val();
        var encodedString = base64.encode(url);

        

        intervalId = setInterval(function () {            
            $.get('api/StateProvider/GetState/' + encodedString, function (data) {

                if ($("#load-bar-1").css("display") == "none") {
                    setTimeout(function () { $("#load-bar-1").show().animate({ left: "0" }, 1000); $(".progress").fadeIn(); }, 0);
                }

                if (data.totalAddresses <= 0) {
                    $("#status").html();
                } else {
                    $(".progress-bar").css("width", data.scannedAddresses/data.totalAddresses*100 + "%");
                    $(".progress-bar").html(data.scannedAddresses + "/" + data.totalAddresses);
                    $("#status").html(data.textState);
                    $("#eta").html(data.etaText);
                    if (data.scannedAddresses == data.totalAddresses) {
                        $("#status").html("Completed!");
                        stopColorAnim();
                    }
                }
            });            
        }, 1000);

        setTimeout(function() {
                startColorAnim();
            },
            1000);
    });

    $('html').on('click',
        '.input-group button',
        function () {            
            //$(this).prop("disabled", "true");
            //$(this).parent().parent(".input-group").find("input").prop("disabled", "true");
        });

    $("#completedLink").on('click',       
        function () {
            
            $(".navbar").removeClass("navbar-default");
            $(".navbar").addClass("navbar-inverse");
        });
});