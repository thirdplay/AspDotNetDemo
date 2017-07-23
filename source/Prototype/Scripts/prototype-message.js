/**
 * 名前空間の定義。
 */
var PROTOTYPE = PROTOTYPE || {
    message: {}
};

/**
 * メッセージモジュール。
 */
PROTOTYPE.message = (function () {
    'use strict';

    // public API
    return {
        /**
         * メッセージを取得します。
         *
         * @param {string} id メッセージID
         * @param {Array.<string>} params メッセージのパラメータ配列
         * @returns {string} メッセージ文字列
         */
        getMessage: function (id, params) {
            var message = '';

            if (id in PROTOTYPE.data.message) {
                message = PROTOTYPE.data.message[id];
            }
            if (Array.isArray(params)) {
                for (var i = 0; i < params.length; i++) {
                    message = message.replace(new RegExp('\\{' + i + '\\}', 'g'), params[i]);
                }
            }

            return message;
        },

        /**
         * メッセージ領域にメッセージを表示します.
         *
         * @param {string} message 表示するメッセージ
         */
        showMessage: function (message) {
            var id = 'messageArea';

            // モーダル判定
            if ($('.modal').is(':visible')) {
                id = 'modalMessageArea';
            }

            if ($('#' + id).length > 0) {
                var msg = PROTOTYPE.common.escapeHtml(message).replace('\\n', '<br />');
                $('#' + id).html(msg)
                    .wrapInner("<strong />")
                    .show();

                // 画面上部にスクロールする
                $('body,html').animate({
                    scrollTop: 0
                }, 'fast');
            }
        },

        /**
         * メッセージ領域のメッセージを非表示にします.
         *
         * @param {string} id メッセージ領域のID
         */
        hideMessage: function (id) {
            if (!id) {
                id = 'messageArea';

                // モーダル判定
                if ($('.modal').is(':visible')) {
                    id = 'modalMessageArea';
                }
            }

            if ($('#' + id).length > 0) {
                $('#' + id).empty().hide();
            }
        },

        /**
         * モーダルのメッセージ領域のメッセージを非表示にします.
         */
        hideMessageModal: function () {
            PROTOTYPE.message.hideMessage('modalMessageArea');
        }
    };
}());