var queries = {};

$(function() {
    FastClick.attach(document.body);
});

$(function() {
    //获取查询字符串
    queries = {};
    $.each(document.location.search.substr(1).split('&'),function(c,q){
        var i = q.split('=');
        queries[i[0]] = i[1];
    });
    console.log(queries);

    $.showLoading();
    //设置名称
    if (queries["name"] != undefined) {
        $('#name').attr('value', decodeURIComponent(queries["name"]));
    }

    $.hideLoading();
});

var OnError = function(rr) {
    console.log('Error');

    $.hideLoading();

    if (rr.responseText != undefined && rr.responseText != "") {
        console.log(rr.responseText);
    }

    $.toast("创建失败", 'cancel');
}

var OnSubmit = function () {

    if (queries["s"] == undefined || queries["oid"] == undefined) {
        $.hideLoading();
        $.toast("创建失败", 'cancel');
    }

    let s = queries["s"];
    let oid = queries["oid"];
    let color = $('#color').val();
    let map = $('#map').val();

    $.showLoading("创建游戏中");
    //创建
    $.ajax({
        url: "../api/GameHelper/New",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify({ "oid": oid, "s": s, "color":parseInt(color), "map":map}),
        success: function (data) {
            console.log(data);
        },
        error: OnError
    });
    $.hideLoading();
}