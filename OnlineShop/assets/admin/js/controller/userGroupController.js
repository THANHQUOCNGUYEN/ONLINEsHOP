var config = {
    init: function () {
        config.loadData();
        config.registeEvents();
    },
    registeEvents: function () {
        $('#btnLuu').off('click').on('click', function () {
            config.updateRoles();
        });
    },

    loadData: function () {
        $.ajax({
            url: '/Admin/Group/getList',
            dataType: 'json',
            type: 'post',
            data: {
                id: $('#txtID').data('id')
            },
            success: function (result) {
                if (result.status) {
                    var data = result.data;
                    $.each(data, function (i, item) {
                        var txt = '#' + item;
                        $(txt).prop('checked', true);
                    });
                }
            }
        });
    },
    updateRoles: function () {
        var lst = [];
        $.each($('.txtRoles'), function (i, item) {
            if ($(item).prop('checked')) {
                lst.push({
                    RoleID: $(item).data('id'),
                    UserGroupID: $('#txtID').data('id')
                });
            }
        });
        $.ajax({
            url: '/Admin/Group/updateRoles',
            dataType: 'json',
            type: 'post',
            data: {
                Credentials: JSON.stringify(lst)
            },
            success: function (result) {
                if (result.status) {
                    alert('Phân Quyền Thành Công !');
                    config.loadData();
                }
            }
        })
    }
}
config.init();