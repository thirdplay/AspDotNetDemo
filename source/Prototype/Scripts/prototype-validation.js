/**
 * 名前空間の定義。
 */
var PROTOTYPE = PROTOTYPE || {
    validation: {}
};

/**
 * バリデーションモジュール。
 */
PROTOTYPE.validation = (function () {
    'use strict';

    // public API
    return {
        /**
         * バリデーションエラーを表示します。
         *
         * @param {Array.<Object>} errors バリデーションエラー情報を格納する配列
         */
        showValidationErrors: function (errors) {
            if (!errors) {
                return;
            }
            for (var i = 0; i < errors.length; i++) {
                var errorModel = errors[i],
                    errorMsg = '';

                for (var j = 0; j < errorModel.errors.length; j++) {
                    errorMsg += PROTOTYPE.common.escapeHtml(errorModel.errors[j]) + '<br />';
                }
                PROTOTYPE.validation.showItemError(errorModel.key, errorMsg);
            }
        },

        /**
         * バリデーションエラーを非表示にします。
         *
         * @param {jQuery} $target 対象要素
         */
        hideValidationErrors: function ($target) {
            if ($('.modal').is(':visible')) {
                $target = $('.modal');
            }
            if ($target) {
                $target.find('span.field-validation-valid').empty();
            } else {
                $('span.field-validation-valid').empty();
            }
        },

        /**
         * 項目のバリデーションエラーを表示します。
         *
         * @param {string} id 項目ID
         * @param {string} message エラーメッセージ
         * @param {boolean} isAppend エラーメッセージを追加するか否か
         */
        showItemError: function (id, message, isAppend) {
            if (isAppend) {
                message = $("span[data-valmsg-for='" + id + "']").html() + '<br />' + message;
            }
            $("span[data-valmsg-for='" + id + "']").html(message);
        }
    };
}());