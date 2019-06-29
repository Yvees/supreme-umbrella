
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

    if (queries["w"] == undefined || queries["scores"] == undefined) {
        $.hideLoading();
        $.toast("查看失败", 'cancel');
        return;
    }

    let win = queries["w"];
    let scores = queries["scores"];

    if (win == 1) {
        $('#score-title').html("恭喜获胜");
        $('#score-desc').html("旗开得胜 效果拔群");
    }

    //设置scores
    $.each(scores.split('|'), function (c, q) {
        if (q == '') {
            return;
        }
        let score = q.split(',');
        let name = decodeURI(score[0]);
        let color = score[1];
        let state = score[2];

        let $itemName = $('<div class="weui-flex__item"><div class="score-cell">' + name + '</div></div>');

        let icon = databus.icons[color];
        let $itemColor = $('<div><div class="score-cell"><img class="color-icon" src="' + icon + '" /></div></div>');

        let statePlayer = "战败";
        if (state == 1) {
            statePlayer = "获胜";
        }
        let $itemState = $('<div><div class="score-cell">' + statePlayer + '</div></div>');

        let $score = $('<div class="weui-flex"></div>');
        $score.append($itemName);
        $score.append($itemColor);
        $score.append($itemState);

        $('#scores').append($score);
    });
    
    $.hideLoading();

});

var OnError = function(rr) {
    console.log('Error');

    $.hideLoading();

    $.toast("查看失败", 'cancel');
}
