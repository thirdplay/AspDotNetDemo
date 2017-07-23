/**
 * 名前空間の定義。
 */
var PROTOTYPE = PROTOTYPE || {
    common: {}
};

/**
 * 共通モジュール。
 */
PROTOTYPE.common = (function () {
    'use strict';

    /** エスケープマップ */
    var escapeMap = {
        '&': '&amp;',
        "'": '&#x27;',
        '`': '&#x60;',
        '"': '&quot;',
        '<': '&lt;',
        '>': '&gt;'
    };
    /** エスケープ用の正規表現 */
    var escapeReg = new RegExp((function () {
        var escapeRegStr = '[';
        for (var p in escapeMap) {
            if (escapeMap.hasOwnProperty(p)) {
                escapeRegStr += p;
            }
        }
        escapeRegStr += ']';
        return escapeRegStr;
    })(), 'g');

    /**
     * 指定要素をフェードインします.
     *
     * @param {jQuery} $target フェードインする要素
     */
    var fadeIn = function ($target) {
        if ($target.is(':hidden')) {
            $target.fadeIn();
        }
    };
    /**
     * 指定要素をフェードアウトします.
     *
     * @param {jQuery} $target フェードアウトする要素
     */
    var fadeOut = function ($target) {
        if ($target.is(':visible')) {
            $target.fadeOut();
        }
    };

    /**
     * ページ内ジャンプの初期化.
     */
    var initializePageInJamp = function () {
        var $pageTop = $('#page-top'),
            $pageBottom = $('#page-bottom');

        $(window).scroll(function () {
            var scrollTop = $(window).scrollTop(),
                scrollRest = $(document).height() - ($(window).scrollTop() + $(window).height());

            if (scrollTop > 300) {
                fadeIn($pageTop);
            } else {
                fadeOut($pageTop);
            }
            if (scrollRest > 300) {
                fadeIn($pageBottom);
            } else {
                fadeOut($pageBottom);
            }
        });

        $pageTop.on('click', function () {
            $('body,html').animate({
                scrollTop: 0
            }, {
                    duration: 600,
                    easing: 'swing'
                });
        });
        $pageBottom.on('click', function () {
            $('body,html').animate({
                scrollTop: $(document).height() - $(window).height()
            }, {
                    duration: 600,
                    easing: 'swing'
                });
        });
    };

    /**
     * 抽出期間FROMの変更イベント。
     *
     * @param {Object} e イベント引数
     */
    var onChangePeriodFrom = function (e) {
        if (e.date) {
            var $periodTo = $('.PeriodToDiv');
            var periodTo = $periodTo.datepicker('getDate');
            if (periodTo === null || e.date.valueOf() > periodTo.valueOf()) {
                var nextDate = new Date(e.date);
                $periodTo.datepicker('setDate', nextDate);
            }
        }
        PROTOTYPE.common.onChangePeriodFromCore(e);
    };

    /**
     * 抽出期間TOの変更イベント。
     *
     * @param {Object} e イベント引数
     */
    var onChangePeriodTo = function (e) {
        if (e.date) {
            var $periodFrom = $('.PeriodFromDiv');
            var periodFrom = $periodFrom.datepicker('getDate');
            if (periodFrom === null || periodFrom.valueOf() > e.date.valueOf()) {
                var previousDay = new Date(e.date);
                $periodFrom.datepicker('setDate', previousDay);
            }
        }
        PROTOTYPE.common.onChangePeriodToCore(e);
    };

    /**
     * 抽出期間のプロパティ変更イベント。
     */
    var changePropertyPeriod = function () {
        if (this.value === "") {
            $(this).parent('div').datepicker('clearDates');
            //$(this).blur();
        }
    };

    /**
     * jQueryの初期化。
     * */
    var initializeJquery = function () {
        // 指定時間ウェイトする関数を追加
        $.wait = function (msec) {
            var d = new $.Deferred();
            setTimeout(function () {
                d.resolve(msec);
            }, msec);

            return d.promise();
        };
    };

    /**
     * プラグインの初期化。
     */
    var initializePlugin = function () {
        // ブロックUI
        $.blockUI.defaults.baseZ = 2000;

        // 日付
        $('.date').datepicker({
            language: 'ja',
            autoclose: true,
            forceParse: true,
            keyboardNavigation: false
        });
        $('.PeriodFromDiv').on('changeDate', onChangePeriodFrom);
        $('.PeriodToDiv').on('changeDate', onChangePeriodTo);
        $('.PeriodFromDiv > input').on('input propertychange', changePropertyPeriod);
        $('.PeriodToDiv > input').on('input propertychange', changePropertyPeriod);

        // プルダウン
        $('.multiSelect').multiselect(PROTOTYPE.common.getMultiSelectOptions());

        // カラーピッカー
        $('.colorPicker').simplecolorpicker({
            picker: true,
            theme: 'glyphicons'
        });

        // ファイル
        PROTOTYPE.common.initializeFileUpload($("input[type='file']"));
    };

    // Public API
    return {
        /**
         * 文字列をHTMLエスケープして返却します。
         *
         * @param {string} str エスケープ対象の文字列
         * @returns {string} エスケープ後の文字列
         */
        escapeHtml: function (str) {
            str = str ? '' + str : '';
            return str.replace(escapeReg, function (match) {
                return escapeMap[match];
            });
        },

        /**
         * ファイルアップロードを初期化します。
         *
         * @param {jQuery} $target ファイル要素
         */
        initializeFileUpload: function ($target) {
            $target.simplefileupload().on('change', function () {
                var itemId = $(this).data('item-id');
                $('#' + $.escapeSelector(itemId)).val('');
            });
        },

        /**
         * プルダウンの設定を取得します。
         *
         * @returns {Object} プルダウンの設定
         */
        getMultiSelectOptions: function () {
            return {
                enableFiltering: true,
                includeSelectAllOption: true,
                selectAllText: '全て選択',
                maxHeight: 300,
                buttonWidth: '100%',
                nonSelectedText: ''
            };
        },

        /**
         * 動的に生成したモーダル要素を初期化します。
         *
         * @param {jQuery} $target モーダル要素のセレクタ
         */
        initializeModal: function ($target) {
            PROTOTYPE.common.initializeFileUpload($target.find("input[type='file']"));
            PROTOTYPE.ajax.initialize();
        },

        /**
         * 初期化処理。
         */
        initialize: function () {
            // 初期化前イベントの呼び出し
            PROTOTYPE.common.onInitializing();

            // jQuery初期化
            initializeJquery();
            // ページ内ジャンプの初期化
            initializePageInJamp();
            // プラグインの初期化
            initializePlugin();
            // AJAXの初期化
            PROTOTYPE.ajax.initialize();

            // 全ての読み込みが完了したらコンテナを表示
            $(window).on('load', function () {
                $('#container').show();
            });

            // 初期化後イベント処理の呼び出し
            $.wait(1).done(function () {
                PROTOTYPE.common.onInitialized();
            });
        },

        /**
         * 初期化の前処理イベント。
         */
        onInitializing: function () {
        },

        /**
         * 初期化の後処理イベント。
         */
        onInitialized: function () {
        },

        /**
         * 指定URLに画面遷移します。
         *
         * @param {string} nextScreenId 次の画面の画面ID
         */
        transitionScreen: function (nextScreenId) {
            $.blockUI({ message: PROTOTYPE.message.getMessage('MC002') });
            $.wait(10).done(function () {
                window.location.href = PROTOTYPE.common.createActionUrl(nextScreenId, 'Index');
            });
        },

        /**
         * 共通のクリアボタンクリック時の処理。
         * defaultDataが定義されている場合、defaultDataの内容に従いデフォルト値を設定します。
         */
        onClickClear: function () {
            // デフォルトデータがない場合は処理しない
            if (!defaultData) {
                return;
            }

            for (var key in defaultData) {
                // ボタンは対象外
                if (key === 'Button') {
                    continue;
                }

                // キーに紐づく要素にデフォルト値を設定する
                if (defaultData[key] instanceof Array) {
                    var defaultArray = defaultData[key];
                    for (var i = 0; i < defaultArray.length; i++) {
                        PROTOTYPE.common.setDefaultValue(key + '[' + i + ']', defaultArray[i]);
                    }
                } else {
                    PROTOTYPE.common.setDefaultValue(key, defaultData[key]);
                }
            }
        },

        /**
         * デフォルト値を対象に設定します。
         *
         * @param {string} key 対象要素のname属性
         * @param {Object} value デフォルト値
         */
        setDefaultValue: function (key, value) {
            // 対象要素がない場合は処理しない
            var $target = $("[name='" + $.escapeSelector(key) + "']");
            if ($target.length === 0) {
                return;
            }

            var tagName = $target.prop('tagName');
            var type = $target.prop('type');

            // タグ名ごとに値を設定する
            if (tagName === 'INPUT' && type === 'checkbox') {
                $target.prop('checked', value);
            } else if (tagName === 'INPUT' && type === 'radio') {
                $target.val([value]);
            } else if (tagName === 'SELECT') {
                if ($target.hasClass('multiSelect')) {
                    $target.multiselect('deselect', $target.val());
                    $target.multiselect('select', value);
                } else if ($target.hasClass('colorPicker')) {
                    $target.simplecolorpicker('selectColor', value);
                } else {
                    $target.val(value);
                }
            } else if ($target.hasClass('datetext')) {
                $('#' + $.escapeSelector($target.prop('id') + '-date')).datepicker('setDate', value);
            } else {
                $target.val(value);
            }

            // ファイルの場合、表示を更新する
            var $file = $("input[data-item-id='" + $target.prop('id') + "']");
            if ($file.length > 0) {
                $file.simplefileupload('setFileName', value);
            }
        },

        /**
         * 指定されたアクション名およびコントローラー名を使用して、アクション メソッドへの完全修飾URLを生成します。
         *
         * @param {string} controllerName コントローラー名
         * @param {string} actionName アクション名
         * @returns {string} 完全装飾URL
         */
        createActionUrl: function (controllerName, actionName) {
            return PROTOTYPE.common.getAppVirtualPath() + '/' + controllerName + '/' + actionName;
        },

        /**
         * 現在のアプリケーションの仮想パスを取得します。
         *
         * @returns {string} アプリケーションの仮想パス
         */
        getAppVirtualPath: function () {
            return $('#CommonParam').data('app-virtual-path');
        },

        /**
         * 共通ディレクトリの仮想パスを取得します。
         *
         * @returns {string} 共通ディレクトリの仮想パス
         */
        getCommonVirtualPath: function () {
            return $('#CommonParam').data('common-virtual-path');
        },

        /**
         * ユーザIDを取得します。
         *
         * @returns {string} ユーザID
         */
        getUserId: function () {
            return $('#CommonParam').data('user-id');
        },

        /**
         * 画面IDを取得します。
         *
         * @returns {string} 画面ID
         */
        getScreenId: function () {
            return $('#CommonParam').data('screen-id');
        },

        /**
         * コントローラー名を取得します。
         *
         * @returns {string} コントローラー名
         */
        getControllerName: function () {
            return $('#CommonParam').data('controller-name');
        },

        /**
         * 最大要求サイズを取得します。
         *
         * @returns {number} 最大要求サイズ(Byte)
         */
        getMaxRequestLength: function () {
            return Number($('#CommonParam').data('max-request-length')) * 1024;
        },

        /**
         * 一時ファイルのURLを取得します。
         *
         * @param {string} fileName ファイル名
         * @returns {string} 一時ファイルのURL
         */
        getTemporaryUrl: function (fileName) {
            return PROTOTYPE.common.getCommonVirtualPath() + '/Temporary/' + PROTOTYPE.common.getUserId() + '/' + fileName;
        },

        /**
         * 解析結果のURLを取得します。
         *
         * @param {string} fileName ファイル名
         * @returns {string} 解析結果のURL
         */
        getResultUrl: function (fileName) {
            return PROTOTYPE.common.getCommonVirtualPath() + '/Result/' + PROTOTYPE.common.getUserId() + '/' + fileName;
        },

        /**
         * 1ページの表示件数の変更処理。
         *
         * @param {Object} option 変更したselect要素
         * @param {boolean} checked 要素の選択状態
         */
        onChangePageSize: function (option, checked) {
            var controllerName = PROTOTYPE.common.getControllerName();
            var params = {
                PageNumber: 1,
                PageSize: $(option).val()
            };
            var url = PROTOTYPE.common.createActionUrl(controllerName, 'SearchUpdate') + '?' + $.param(params);
            $('#PageSizeLink').prop('href', url);
            $('#PageSizeLink').click();
        },

        /**
         * 抽出期間FROMの変更イベント。
         *
         * @param {Object} e イベント引数
         */
        onChangePeriodFromCore: function (e) {
        },

        /**
         * 抽出期間TOの変更イベント。
         *
         * @param {Object} e イベント引数
         */
        onChangePeriodToCore: function (e) {
        }
    };
}());