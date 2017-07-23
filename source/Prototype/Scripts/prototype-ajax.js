/**
 * 名前空間の定義。
 */
var PROTOTYPE = PROTOTYPE || {
    ajax: {}
};

/**
 * AJAXモジュール。
 */
PROTOTYPE.ajax = (function () {
    'use strict';

    /**
     * 設定されているファイル要素のIDを取得します。
     *
     * @returns {Array.<string>} ファイル要素IDの配列
     */
    var getSpecifiedFileElementId = function () {
        var result = [];
        var $files = $("input[type='file']");

        if ($files.length > 0) {
            var len = $files.length;
            for (var i = 0; i < len; i++) {
                if ($files[i].files.length > 0) {
                    result.push($($files[i]).prop('id'));
                }
            }
        }

        return result;
    };

    /**
     * ファイルパスからファイル名を取得します。
     *
     * @param {string} filePath ファイルパス
     * @returns {string} ファイル名
     */
    var getFileName = function (filePath) {
        if (!filePath) {
            return '';
        }
        var hierarchies = filePath.split('\\');
        var len = hierarchies.length;
        if (len === 0) {
            return filePath;
        }
        return hierarchies[len - 1];
    };

    /**
     * ファイルパスからファイル拡張を取得します。
     *
     * @param {string} filePath ファイルパス
     * @returns {string} 拡張子
     */
    var getFileExtension = function (filePath) {
        if (!filePath) {
            return '';
        }
        var fileTypes = filePath.split('.');
        var len = fileTypes.length;
        if (len === 0) {
            return '';
        }
        return '.' + fileTypes[len - 1];
    };

    /**
     * ファイル形式をチェックします。
     *
     * @param {string} id ファイル要素のID
     * @returns {Object} エラー情報を格納したオブジェクト
     */
    var checkFileFormat = function (id) {
        var maxRequestLength = PROTOTYPE.common.getMaxRequestLength();
        var $file = $('#' + $.escapeSelector(id));
        var extension = $file.data('file-extension');
        var file = $file[0].files[0];
        var errorMessages = [];

        if (file.size > maxRequestLength) {
            errorMessages.push(PROTOTYPE.message.getMessage('MC005', [maxRequestLength / 1048576]));
        }
        if (getFileExtension(file.name) !== extension) {
            errorMessages.push(PROTOTYPE.message.getMessage('MC006'));
        }

        if (errorMessages.length > 0) {
            return {
                key: $file.data('item-id'),
                errors: errorMessages
            };
        }

        return null;
    };

    /**
     * ファイルを非同期でアップロードします。
     *
     * @param {string} id アップロードするファイル要素のID
     * @returns {Object} プロミス
     */
    var uploadFileAsync = function (id) {
        return function () {
            var screenId = PROTOTYPE.common.getScreenId();
            var $file = $('#' + $.escapeSelector(id));
            var itemId = $file.data('item-id');
            var $item = $('#' + $.escapeSelector(itemId));
            var $original = $('#' + $.escapeSelector(itemId + 'Original'));
            var file = $file[0].files[0];
            var fileName = getFileName(file.name);
            var params = {
                ScreenId: screenId,
                ItemId: itemId,
                FileName: $item.val()
            };

            var formData = new FormData();
            formData.append(file.name, file);

            return $.ajax({
                type: "POST",
                url: PROTOTYPE.common.createActionUrl('File', 'UploadFile') + '?' + $.param(params),
                contentType: false,
                processData: false,
                data: formData,
                success: function (data) {
                    if (data.Result) {
                        $item.val(data.FileName);
                        $original.val(fileName);
                    } else {
                        $item.val('');
                        alert(data.Message);
                    }
                },
                error: function (jqXHR, textStatus) {
                    alert(PROTOTYPE.message.getMessage('MC007'));
                }
            });
        };
    };

    // public API
    return {
        /**
         * 初期化。
         */
        initialize: function () {
            // フォームにAJAX用の追加イベントを設定する
            var $form = $('form[data-ajax=true]');
            if ($form.length > 0) {
                $form.attr('data-ajax-before', 'PROTOTYPE.ajax.onBefore');
                $form.attr('data-ajax-file-upload', 'PROTOTYPE.ajax.onFileUpload');
            }
        },

        /**
         * AJAX要求前に呼び出されるイベント。
         *
         * @param {Array.<Object>} clickInfo クリック情報
         * @returns {boolean} AJAX要求を継続する場合はtrue。中断する場合はfalse。
         */
        onBefore: function (clickInfo) {
            console.log('ajax:onBefore');

            // 確認メッセージ表示
            var button = clickInfo.length > 0 ? clickInfo[0].value : '';
            var message = PROTOTYPE.ajax.getConfirmMessage(button);
            if (message && !window.confirm(message)) {
                return false;
            }

            // エラーメッセージとバリデーションエラーを非表示にする
            PROTOTYPE.message.hideMessage();
            PROTOTYPE.validation.hideValidationErrors();

            return true;
        },

        /**
         * AJAX要求前のファイルアップロード時に呼び出されるイベント。
         *
         * @param {Object} settings AJAX設定
         * @returns {Object} プロミス
         */
        onFileUpload: function (settings) {
            console.log('ajax:onFileUpload');
            var d = new $.Deferred();

            // 指定されているファイル要素を取得
            var fileElemIdList = getSpecifiedFileElementId();
            if (fileElemIdList.length === 0) {
                // 対象がない場合は処理をスキップする
                return d.resolve();
            }
            var len = fileElemIdList.length;

            // ファイルの拡張子とサイズをチェック
            var errors = [];
            var i = 0;
            for (i = 0; i < len; i++) {
                var errorModel = checkFileFormat(fileElemIdList[i]);
                if (errorModel) {
                    errors.push(errorModel);
                }
            }
            if (errors.length > 0) {
                // エラーがある場合は、エラーを表示して中断する
                PROTOTYPE.validation.showValidationErrors(errors);
                return d.reject();
            }

            // 非同期処理の追加
            d = d.resolve();
            for (i = 0; i < len; i++) {
                d = d.then(uploadFileAsync(fileElemIdList[i]));
            }
            $.blockUI({ message: PROTOTYPE.message.getMessage('MC003') });

            return d;
        },

        /**
         * AJAX要求直前に呼び出されるイベント。
         *
         * @param {Object} jqXHR XMLHttpRequestオブジェクト
         * @param {Object} settings AJAX設定
         * @returns {boolean} AJAX要求を継続する場合はtrue。中断する場合はfalse。
         */
        onBegin: function (jqXHR, settings) {
            console.log('ajax:onBegin');
            var params = {
                Button: ''
            };
            if (settings.data) {
                var paramList = settings.data.split('&');
                for (var i = 0; i < paramList.length; i++) {
                    var paramPair = paramList[i].split('=');
                    params[paramPair[0]] = paramPair[1];
                }
            }
            $.blockUI({ message: PROTOTYPE.ajax.getExecutingMessage(params.Button) });
            return true;
        },

        /**
         * AJAX要求前の確認メッセージを取得します。
         *
         * @param {string} button 押下されたボタンの値
         * @returns {string} 確認メッセージ
         */
        getConfirmMessage: function (button) {
            return PROTOTYPE.ajax.getConfirmMessageCore(button);
        },

        /**
         * AJAX要求前の確認メッセージを取得します。（コア処理）
         *
         * @param {string} button 押下されたボタンの値
         * @returns {string} 確認メッセージ
         */
        getConfirmMessageCore: function (button) {
            return '';
        },

        /**
         * 実行中のメッセージを取得します。
         *
         * @param {string} button 押下されたボタンの値
         * @returns {string} 実行メッセージ
         */
        getExecutingMessage: function (button) {
            return PROTOTYPE.ajax.getExecutingMessageCore(button);
        },

        /**
         * 実行中のメッセージを取得します。（コア処理）
         *
         * @param {string} button 押下されたボタンの値
         * @returns {string} 実行メッセージ
         */
        getExecutingMessageCore: function (button) {
            return PROTOTYPE.message.getMessage('MC004');
        },

        /**
         * AJAX要求完了後に呼び出されるイベント。
         *
         * @param {Object} jqXHR XMLHttpRequestオブジェクト
         * @param {string} textStatus ステータス
         */
        onComplete: function (jqXHR, textStatus) {
            console.log('ajax:onComplete');
        },

        /**
         * AJAX要求が失敗した場合に呼び出されるイベント。
         *
         * @param {Object} jqXHR XMLHttpRequestオブジェクト
         * @param {string} textStatus ステータス
         * @param {string} errorThrown HTTPステータスのテキスト
         */
        onFailure: function (jqXHR, textStatus, errorThrown) {
            console.log('ajax:onFailure');
            $.unblockUI();
            if (jqXHR.status === 404 || jqXHR.status === 500) {
                document.head.innerHTML = "";
                document.body.innerHTML = jqXHR.responseText;
                $('html,body').scrollTop(0);
            }
            //var bodyStart = jqXHR.responseText.match(/<body.*>/);
            //var bodyEnd = jqXHR.responseText.match(/<\/body>/);
            //var startIndex = bodyStart.index + bodyStart[0].length;
            //var endIndex = bodyEnd.index - 1;
            //var body = $(jqXHR.responseText.substring(startIndex, endIndex)).text();

            //$.unblockUI();
            //PROTOTYPE.message.showMessage(body);
        },

        /**
         * AJAX要求成功時に呼び出されるイベント。
         *
         * @param {Object} data 応答結果
         * @param {string} textStatus ステータス
         * @param {Object} jqXHR XMLHttpRequestオブジェクト
         */
        onSuccess: function (data, textStatus, jqXHR) {
            // AJAX要求成功時イベント呼び出し
            console.log('ajax:onSuccessing');
            PROTOTYPE.ajax.onSuccessing(data, textStatus, jqXHR);

            // 応答結果がエラーの場合、エラー内容を表示する
            $.unblockUI();
            if (data.Result === false) {
                PROTOTYPE.message.showMessage(data.Message);
                PROTOTYPE.validation.showValidationErrors(data.Errors);
                return;
            }

            // AJAX要求成功後イベント呼び出し
            console.log('ajax:onSuccessed');
            PROTOTYPE.ajax.onSuccessed(data, textStatus, jqXHR);
        },

        /**
         * AJAX要求成功時に呼び出されるイベント。
         *
         * @param {Object} data 応答結果
         * @param {string} textStatus ステータス
         * @param {Object} jqXHR XMLHttpRequestオブジェクト
         */
        onSuccessing: function (data, textStatus, jqXHR) {
        },

        /**
         * AJAX要求成功後に呼び出されるイベント。
         *
         * @param {Object} data 応答結果
         * @param {string} textStatus ステータス
         * @param {Object} jqXHR XMLHttpRequestオブジェクト
         */
        onSuccessed: function (data, textStatus, jqXHR) {
        },

        /**
         * データ解析成功時に呼び出されるイベント。
         *
         * @param {Object} data 応答結果
         * @param {string} textStatus ステータス
         * @param {Object} jqXHR XMLHttpRequestオブジェクト
         */
        onSuccessAnalyze: function (data, textStatus, jqXHR) {
            // AJAX要求成功後のイベントを設定し、共通処理を呼び出す
            PROTOTYPE.ajax.onSuccessed = function (data, textStatus, jqXHR) {
                window.open(PROTOTYPE.common.getResultUrl(data.FileName));
            };
            PROTOTYPE.ajax.onSuccess(data, textStatus, jqXHR);
        }
    };
}());