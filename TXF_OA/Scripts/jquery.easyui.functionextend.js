//alert扩展
function jqAlert(msg, ico, url, parent) {
    $.messager.defaults = { ok: '确定', cancel: "取消" };
    $.messager.alert('消息框', '<div style="text-align:center;font:14px blod 宋体;margin-left:50px;width:70%;">' + msg + '<div>',
            '' + ico + '',
            function () {
                if (url != null) {
                    if (parent) { window.parent.document.location = url; } else { document.location = url; }
                }
            });
}
//confirm扩展
function uiConfirm(msg, fn) {
    $.messager.defaults = { ok: '确定', cancel: "取消" };
    $.messager.confirm('对话框', '<div style="text-align:center;font:14px blod 宋体;margin-left:60px;width:70%;">' + msg + '<div>',
            function (r) {
                if (r) { fn(); }
            });
}
//日历时间格式扩展
if ($.fn.datebox) {
    $.fn.datebox.defaults.currentText = '今天';
    $.fn.datebox.defaults.closeText = '关闭';
    $.fn.datebox.defaults.okText = '确定';
    $.fn.datebox.defaults.missingMessage = '该输入项为必输项';
    //修改后日期格式-->
    $.fn.datebox.defaults.formatter = function (date) {
        var y = date.getFullYear();
        var m = date.getMonth() + 1;
        var d = date.getDate();
        return y + '-' + (m < 10 ? ('0' + m) : m) + '-' + (d < 10 ? ('0' + d) : d);
    }
}
//改写datagrid条默认值
if ($.fn.datagrid) {
    $.fn.datagrid.defaults.pageSize = 20,
    $.fn.datagrid.defaults.pageList = [20, 30, 50],
    $.fn.datagrid.defaults.pagination = true,
    $.fn.datagrid.defaults.rownumbers = true//显示行号
    $.fn.datagrid.defaults.fitColumns = false,
    $.fn.datagrid.defaults.showFooter = true,
    $.fn.datagrid.defaults.nowrap = true, //是否不换行
    $.fn.datagrid.defaults.striped = true, //显示斑马纹
    $.fn.datagrid.defaults.collapsible = true,
    $.fn.datagrid.defaults.remoteSort = false,
    $.fn.datagrid.defaults.loadMsg = '数据加载中，请稍后……'
}
//改写分页条默认值
if ($.fn.pagination) {
    $.fn.pagination.defaults.beforePageText = '第', //页数文本框前显示的汉字             
    $.fn.pagination.defaults.afterPageText = '页    共 {pages} 页',
    $.fn.pagination.defaults.displayMsg = '当前显示 {from} - {to} 条记录   共 {total} 条记录'
}
////让window居中
//var easyuiPanelOnOpen = function (left, top) {
//    var iframeWidth = $(this).parent().parent().width();
//    var iframeHeight = $(this).parent().parent().height();
//    var windowWidth = $(this).parent().width();
//    var windowHeight = $(this).parent().height();
//    var setWidth = (iframeWidth - windowWidth) / 2;
//    var setHeight = (iframeHeight - windowHeight) / 2;
//    $(this).parent().css({/* 修正面板位置 */
//        left: setWidth,
//        top: setHeight - setHeight * 0.2
//    });
//    if (iframeHeight < windowHeight) {
//        $(this).parent().css({/* 修正面板位置 */
//            left: setWidth,
//            top: 0
//        });
//    }
//}
//$.fn.dialog.defaults.onOpen = easyuiPanelOnOpen;
//textbox添加图标
$.extend($.fn.textbox.methods, {
    addClearBtn: function (jq, iconCls) {
        return jq.each(function () {
            var t = $(this);
            var opts = t.textbox('options');
            opts.icons = opts.icons || [];
            opts.icons.unshift({
                iconCls: iconCls,
                handler: function (e) {
                    $(e.data.target).textbox('clear').textbox('textbox').focus();
                    $(this).css('visibility', 'hidden');
                }
            });
            t.textbox();
            if (!t.textbox('getText')) {
                t.textbox('getIcon', 0).css('visibility', 'hidden');
            }
            t.textbox('textbox').bind('keyup', function () {
                var icon = t.textbox('getIcon', 0);
                if ($(this).val()) {
                    icon.css('visibility', 'visible');
                } else {
                    icon.css('visibility', 'hidden');
                }
            });
        });
    }
});
//阻止panel拖出边界
function easyuiPanelOnMove(left, top) {
    var l = left < 1 ? 1 : left;
    var t = top < 1 ? 1 : top;
    var width = parseInt($(this).parent().css('width')) + 14;
    var height = parseInt($(this).parent().css('height')) + 14;
    var right = l + width;
    var buttom = t + height;
    var browserWidth = $(window).width();
    var browserHeight = $(window).height();
    if (right > browserWidth) {
        l = browserWidth - width;
    }
    if (buttom > browserHeight) {
        t = browserHeight - height;
    }
    $(this).parent().css({
        left: l,
        top: t
    });
}
$.fn.dialog.defaults.onMove = easyuiPanelOnMove;
$.fn.window.defaults.onMove = easyuiPanelOnMove;
$.fn.panel.defaults.onMove = easyuiPanelOnMove;