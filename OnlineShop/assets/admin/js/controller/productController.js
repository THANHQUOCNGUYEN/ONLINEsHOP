var product = {
    init: function () {
        product.registerEvents();
    },
    registerEvents: function () {
        $('#btnSelectedImage').off('click').on('click', function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (url) {
                $("#Image").val(url);
            };
            finder.popup();
        });

        var editor = CKEDITOR.replace('txtDetail', {
            customConfig: '/assets/admin/js/plugins/ckeditor/config.js',
        });

        $('#Name').off('keyup').on('keyup', function () {
            var alias = $(this).val();
            product.getAlias(alias);
        });

      
    },

    getAlias: function (alias) {
        $.ajax({
            url: '/Admin/Product/getAlias',
            dataType: 'json',
            type: 'post',
            data: {
                alias: alias
            },
            success: function (result) {
                if (result.status) {
                    $('#MetaTitle').val(result.data);
                }
            }
        });
    }
};
product.init();
