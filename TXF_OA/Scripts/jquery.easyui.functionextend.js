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
    $.fn.datebox.defaults.buttons = ClearButton();
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
    //$.fn.datagrid.defaults.singleSelect = true,
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
//=====================================
//datebox：添加清除按钮
//添加人：txf
//添加时间：2015-01-01
//=====================================
function ClearButton() {
    var buttons = $.extend([], $.fn.datebox.defaults.buttons);
    buttons.splice(1, 0, {
        text: '清空',
        handler: function (target) {
            $(target).datebox("setValue", "");
            $(target).combo("hidePanel")
        }
    });
    return buttons;
}
//=====================================
//textbox：添加图标功能
//添加人：txf
//添加时间：2015-01-01
//=====================================
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
//=====================================
//阻止panel,dialog,window拖出边界
//添加人：txf
//添加时间：2015-01-01
//=====================================
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

//=====================================
//Layout：隐藏显示标题功能
//添加人：txf
//添加时间：2015-01-01
//=====================================
$.extend($.fn.layout.paneldefaults, {
    onBeforeCollapse: function () {
        var popts = $(this).panel('options');
        var dir = popts.region;
        var buttonDir = { north: 'down', south: 'up', east: 'left', west: 'right' };
        var btnDir = buttonDir[dir];
        if (!btnDir) return false;
        setTimeout(function () {
            var pDiv = $('.layout-button-' + btnDir).closest('.layout-expand').css({
                "text-align": 'center', "line-height": "18px", "font-family": "黑体", "font-weight": 'bold'
            });
            if (popts.title) {
                var vTitle = popts.title;
                if (dir == "east" || dir == "west") {
                    var vTitle = popts.title.split('').join('<br/>');
                    pDiv.find('.panel-body').html(vTitle);
                } else {
                    $('.layout-button-' + btnDir).closest('.layout-expand').find('.panel-title')
                        .css({ textAlign: 'left' })
                        .html(vTitle)
                }

            }
        }, 100);

    }
});
//=====================================
//Tabs：内部iframe的添加，更新，清除
//添加人：txf
//添加时间：2015-03-16
//=====================================
$.extend($.fn.tabs.methods, {
    //加载iframe内容  
    loadTabFrame: function (jq, params) {
        return jq.each(function () {
            var $tab = $(this).tabs('getTab', params.which);
            if ($tab == null) return;
            var $tabBody = $tab.panel('body');

            //销毁已有的iframe   
            var $frame = $('iframe', $tabBody);
            if ($frame.length > 0) {
                try {//跨域会拒绝访问，这里处理掉该异常   
                    $frame[0].contentWindow.document.write('');
                    $frame[0].contentWindow.close();
                } catch (e) {
                    //Do nothing   
                }
                $frame.remove();
                if ($.browser.msie) {
                    CollectGarbage();
                }
            }
            $tabBody.html('');
            $tabBody.css({ 'overflow': 'hidden', 'position': 'relative' });
            //var $mask = $('<div style="position:absolute;z-index:2;width:100%;height:100%;background:#ccccc;z-index:1000;opacity:0.3;filter:alpha(opacity=30);"><div>').appendTo($tabBody);
            //var $maskMessage = $('<div class="mask-message" style="z-index:3;width:auto;height:16px;line-height:16px;position:absolute;top:50%;left:50%;margin-top:-20px;margin-left:-92px;border:2px solid #95B8E7;padding: 12px 5px 10px 30px;background: #ffffff url(\'../Content/themes/default/images/loading.gif\') no-repeat scroll 5px center;">' + (params.iframe.message || '数据加载中，请稍后……') + '</div>').appendTo($tabBody);
            //var $containterMask = $('<div style="position:absolute;width:100%;height:100%;z-index:1;background:#fff;"></div>').appendTo($tabBody);
            var $containter = $('<div style="position:absolute;width:100%;height:100%;z-index:0;"></div>').appendTo($tabBody);

            var iframe = document.createElement("iframe");
            iframe.id = params.iframe.id;
            iframe.src = params.iframe.src;
            iframe.frameBorder = params.iframe.frameBorder || 0;
            iframe.height = params.iframe.height || '100%';
            iframe.width = params.iframe.width || '100%';
            if (iframe.attachEvent) {
                iframe.attachEvent("onload", function () {
                    params.iframe.onComplete();
                });
            } else {
                iframe.onload = function () {
                    params.iframe.onComplete();
                };
            }
            $containter[0].appendChild(iframe);
        });
    },
    //增加iframe模式的标签页  
    addFrameTab: function (jq, params) {
        return jq.each(function () {
            if (params.tab.href) {
                delete params.tab.href;
            }
            $(this).tabs('add', params.tab);
            $(this).tabs('loadTabFrame', { 'which': params.tab.title, 'iframe': params.iframe });
        });
    },
    //更新tab的iframe内容  
    updateFrameTab: function (jq, params) {
        return jq.each(function () {
            params.iframe = params.iframe || {};
            if (!params.iframe.src) {
                var $tab = $(this).tabs('getTab', params.which);
                if ($tab == null) return;
                var $tabBody = $tab.panel('body');
                var $iframe = $tabBody.find('iframe');
                if ($iframe.length === 0) return;
                $.extend(params.iframe, { 'src': $iframe.attr('src') });
            }
            $(this).tabs('loadTabFrame', params);
        });
    },
    //清除tab的iframe内容
    clearFrameTab: function (jq, params) {
        return jq.each(function () {
            var $tab = $(this).tabs('getTab', params.which);
            if ($tab == null) return;
            var $tabBody = $tab.panel('body');
            var $frame = $tabBody.find('iframe');
            //销毁已有的iframe   
            if ($frame.length > 0) {
                try {//跨域会拒绝访问，这里处理掉该异常   
                    $frame[0].contentWindow.document.write('');
                    $frame[0].contentWindow.close();
                } catch (e) {
                    //Do nothing   
                }
                $frame.remove();
                if ($.browser.msie) {
                    CollectGarbage();
                }
            }
        });
    }
});
//=====================================
//datagrid toolbar：工具栏的按钮添加、删除
//添加人：txf
//添加时间：2015-03-16
//=====================================
$.extend($.fn.datagrid.methods, {
    addToolbarItem: function (jq, items) {
        return jq.each(function () {
            var dpanel = $(this).datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (!toolbar.length) {
                toolbar = $("<div class=\"datagrid-toolbar\"><table cellspacing=\"0\" cellpadding=\"0\"><tr></tr></table></div>").prependTo(dpanel);
                $(this).datagrid('resize');
            }
            var tr = toolbar.find("tr");
            for (var i = 0; i < items.length; i++) {
                var btn = items[i];
                if (btn == "-") {
                    $("<td><div class=\"datagrid-btn-separator\"></div></td>").appendTo(tr);
                } else {
                    var td = $("<td></td>").appendTo(tr);
                    var b = $("<a href=\"javascript:void(0)\"></a>").appendTo(td);
                    b[0].onclick = eval(btn.handler || function () { });
                    b.linkbutton($.extend({}, btn, { plain: true }));
                }
            }
        });
    },
    removeToolbarItem: function (jq, param) {
        return jq.each(function () {
            var dpanel = $(this).datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            var cbtn = null;
            if (typeof param == "number") {
                cbtn = toolbar.find("td").eq(param).find('span.l-btn-text');
            } else if (typeof param == "string") {
                cbtn = toolbar.find("span.l-btn-text:contains('" + param + "')");
            }
            if (cbtn && cbtn.length > 0) {
                cbtn.closest('td').remove();
                cbtn = null;
            }
        });
    }
});
