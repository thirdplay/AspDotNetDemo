/**
 * 初期化。
 */
PROTOTYPE.common.onInitialized = function () {
    var $userId = $('#UserId');
    var value = $userId.val();
    $userId.val('');
    $userId.focus();
    $userId.val(value);
};

/**
 * AJAX要求成功後に呼び出されるイベント。
 *
 * @param {Object} data 応答結果
 * @param {string} textStatus ステータス
 * @param {Object} jqXHR XMLHttpRequestオブジェクト
 */
PROTOTYPE.ajax.onSuccessed = function (data, textStatus, jqXHR) {
    // 遷移先のURL取得
    var nextUrl = PROTOTYPE.common.createActionUrl('MainMenu', 'Index');

    // 現在のウィンドウを取得
    var myWin = window.open('about:blank', '_self');

    // 初回ログインの場合
    if (myWin.name !== 'prototype') {
        var width = 1366 - 4,
            height = 768 - 4,
            option = 'toolbar=no,location=no,status=no,menubar=no,resizable=yes,scrollbars=yes';
        option += ',width=' + width + ',height=' + height;

        // 新規ウィンドウを開き、現在のウィンドウを閉じる
        var win = window.open('about:blank', 'prototype', option);
        win.location.href = nextUrl;
        myWin.close();
    } else {
        // 現在のウィンドウで遷移
        myWin.location.href = nextUrl;
    }
}