/*!
 * Bootstrap Simple File Upload
 *
 * Copyright (C) 2017 thirdplay
 *
 * Licensed under the MIT license
 */

(function ($) {
    'use strict';

    /**
     * Constructor.
     *
     * @param {Object} input
     * @param {Object} options
     */
    var SimpleFileUpload = function (input, options) {
        this.init('simplefileupload', input, options);
    };

    /**
     * SimpleFileUpload class.
     */
    SimpleFileUpload.prototype = {
        constructor: SimpleFileUpload,

        /**
         * Initialize.
         *
         * @param {string} type
         * @param {Object} input
         * @param {Object} options
         */
        init: function (type, input, options) {
            this.type = type;

            this.$input = $(input);
            this.$input.on('change', $.proxy(this.changeFileName, this));

            this.options = $.extend({}, $.fn.simplefileupload.defaults, options);
            this.options.onChange = $.proxy(this.options.onChange, this);

            this.buildContainer();
            this.buildFileName();
            this.buildButton();

            this.$input.hide().after(this.$container);
        },

        /**
         * Builds the container of the simplefileupload.
         */
        buildContainer: function () {
            this.$container = $('<div class="input-group"></div>');
        },

        /**
         * Builds the file name of the simplefileupload.
         */
        buildFileName: function () {
            this.$fileName = $('<input type="text" class="' + this.options.fileNameClass + '" placeholder="' + this.options.placeholder + '" readonly />');
            this.$fileName.on('click', $.proxy(this.selectFileName, this));
            this.$container.append(this.$fileName);
        },

        /**
         * Builds the button of the simplefileupload.
         */
        buildButton: function () {
            this.$buttonContainer = $('<span class="input-group-btn"></span>');
            this.$openButton = $('<button type="button" class="' + this.options.openButtonClass + '"><span class="' + this.options.openTextClass + '"> ' + this.options.openButtonText + '</span></button>');
            this.$clearButton = $('<button type="button" class="' + this.options.clearButtonClass + '"><span class="' + this.options.clearTextClass + '"></span></button>');
            this.$openButton.on('click', $.proxy(this.selectFileName, this));
            this.$clearButton.on('click', $.proxy(this.clear, this));

            // Adopt enable state.
            if (!this.options.enableClearButton) {
                this.$clearButton.hide();
            }
            // Adopt active state.
            if (this.$input.prop('disabled')) {
                this.disable();
            } else {
                this.enable();
            }

            this.$buttonContainer.append(this.$openButton, this.$clearButton);
            this.$container.append(this.$buttonContainer);
        },

        /**
         * Call the file selection process.
         */
        selectFileName: function () {
            this.$input.trigger('click');
        },

        /**
         * Clear the selected file name.
         */
        setFileName: function (fileName) {
            this.$input.val(fileName);
            this.$fileName.val(fileName);
        },

        /**
         * Clear the selected file name.
         */
        clear: function () {
            this.$input.val('');
            this.$fileName.val('');
        },

        /**
         * Enable the simplefileupload.
         */
        enable: function () {
            this.$fileName.prop('disabled', false);
            this.$openButton.prop('disabled', false)
                .removeClass('disabled');
            this.$clearButton.prop('disabled', false)
                .removeClass('disabled');
        },

        /**
         * Disable the simplefileupload.
         */
        disable: function () {
            this.$fileName.prop('disabled', true);
            this.$openButton.prop('disabled', true)
                .removeClass('disabled');
            this.$clearButton.prop('disabled', true)
                .removeClass('disabled');
        },

        /**
         * Links values ​​to display text boxes when selecting files.
         */
        changeFileName: function () {
            this.$fileName.val(this.$input.val());
            this.options.onChange(this.$fileName.val());
        }
    };

    /**
     * Plugin definition.
     * How to use: $('#id').simplefileupload()
     *
     * @param {Object} option
     */
    $.fn.simplefileupload = function (option) {
        var args = $.makeArray(arguments);
        args.shift();

        // For HTML element passed to the plugin
        return this.each(function () {
            var $this = $(this),
                data = $this.data('simplefileupload'),
                options = typeof option === 'object' && option;
            if (data === undefined) {
                $this.data('simplefileupload', (data = new SimpleFileUpload(this, options)));
            }
            // Call simplefileupload method.
            if (typeof option === 'string') {
                data[option].apply(data, args);
            }
        });
    };

    /**
     * Default options.
     */
    $.fn.simplefileupload.defaults = {
        // The class of the clear button.
        clearButtonClass: 'btn btn-danger btn-clear',

        // The class of the clear text.
        clearTextClass: 'glyphicon glyphicon-remove',

        // Set to true or false to enable or disable the clear button.
        enableClearButton: true,

        // The class of the file name.
        fileNameClass: 'form-control',

        /**
         * Triggered on change of the file name.
         *
         * @param {String} fileName
         */
        onChange: function (fileName) {
        },

        // The class of the open button.
        openButtonClass: 'btn btn-default active btn-open',

        // The text of the open button.
        openButtonText: '',

        // The class of the open text.
        openTextClass: 'glyphicon glyphicon-folder-open',

        // placeholder text
        placeholder: 'Select file ...'
    };
})(jQuery);