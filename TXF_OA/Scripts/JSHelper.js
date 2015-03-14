//自定义公共js
//jquery类扩展
$.extend({
    ltrim: function (str) {
        if (str == null || value == "") return str;
        return str.replace(/^\s+/, "");
    },
    rtrim: function (str) {
        if (str == null || value == "") return str;
        return str.replace(/\s+$/, "");
    },
    trimend: function (str, char) {
        if (str == null || str == "") return str;
        var regExp = new RegExp(char + "$");
        return str.replace(regExp, "");
    },
    strFilter: function (str) {
        str = $.trim(str).replace(/[~'"!<>@#$%^&*()-+_=:]/g, "");
        return str;
    },
    //ajaxGet方法
    ajaxGet: function (url, data, fn) {
        $.ajax({
            type: "GET",
            url: url,
            data: data,
            cache: false,
            error: function () { alert('执行失败.', 'warning'); },
            success: function (obj) { fn(obj) }
        });
    },
    //ajaxPost方法
    ajaxPost: function (url, data, fn) {
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            cache: false,
            async: false,
            error: function () { alert('执行失败.', 'warning'); },
            success: function (obj) { fn(obj) }
        });
    }
});
$.fn.extend({
     //固定表头
    fixTableHead: function (options) {
        return this.each(function () {
            var $this = $(this);
            var $uniqueID = $(this).attr("ID") + ("wrapper");
            $(this).css('width', options.width).addClass("_scrolling");
            $(this).wrap('<div class="scrolling_outer"><div id="' + $uniqueID + '" class="scrolling_inner"></div></div>');
            $(".scrolling_outer").css({ 'position': 'relative' });
            $("#" + $uniqueID).css({
                'border': '1px solid #CCCCCC',
                'overflow-x': 'hidden',
                'overflow-y': 'auto',
                'padding-right': '17px'
            });
            $("#" + $uniqueID).css('height', options.height);
            $("#" + $uniqueID).css('width', options.width);
            $(this).before($(this).clone().attr("id", "").addClass("_thead").css({
                'width': 'auto',
                'display': 'block',
                'position': 'absolute',
                'border': 'none',
                'border-bottom': '0px solid #CCC',
                'top': '0px'
            }));
            $('._thead').children('tbody').remove();
            $(this).each(function ($this) {
                if (options.width == "100%" || options.width == "auto") {
                    $("#" + $uniqueID).css({ 'padding-right': '0px' });
                }
                if (options.scrolling == "no") {
                    $("#" + $uniqueID).before('<a href="#" class="expander" style="width:100%;">Expand table</a>');
                    $("#" + $uniqueID).css({ 'padding-right': '0px' });
                    $(".expander").each(
            function (int) {
                $(this).attr("ID", int);
                $(this).bind("click", function () {
                    $("#" + $uniqueID).css({ 'height': 'auto' });
                    $("#" + $uniqueID + " ._thead").remove();
                    $(this).remove();
                });
            });
                    $("#" + $uniqueID).resizable({ handles: 's' }).css("overflow-y", "hidden");
                }
            });
            $curr = $this.prev();
            $("thead:eq(0)>tr td", this).each(function (i) {
                $("thead:eq(0)>tr td:eq(" + i + ")", $curr).width($(this).width());
            });
            if (options.width == "100%" || "auto") {
                $(window).resize(function () {
                    resizer($this);
                });
            }
        });
    }
});
function resizer($this) {
    $curr = $this.prev();
    $("thead:eq(0)>tr td", $this).each(function (i) {
        $("thead:eq(0)>tr td:eq(" + i + ")", $curr).width($(this).width());
    });
}
//格式化日期格式
function dateFormatter(value) {
    if (value == null || value == "") return value;
    var date;
    if (value.toString().indexOf("Date") > -1)
        date = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
    else
        date = new Date(value);
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    return date.getFullYear() + "-" + month + "-" + currentDate;
}
//格式化时间格式
function timeFormatter(value) {
    if (value == null || value == "") return value;
    var date;
    if (value.toString().indexOf("Date") > -1) {
        date = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
    }
    else
        date = new Date(value);
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    var t = y + '-' + (m < 10 ? ('0' + m) : m) + '-' + (d < 10 ? ('0' + d) : d) + ' ';

    var hh = date.getHours();
    var mm = date.getMinutes();
    var ss = date.getSeconds();
    t += (hh < 10 ? ('0' + hh) : hh) + ':' + (mm < 10 ? ('0' + mm) : mm) + ':' + (ss < 10 ? ('0' + ss) : ss);
    return t;
}
//子窗体加载树形
function loadTree(url, fn) {
    $('#uiRTree').tree({
        url: url,
        animate: true,
        expand: true,
        cache: false,
        onDblClick: function (node) {
            if (node.attributes.TableName != "") {
                $("#uiBody").layout('collapse', 'east');
                fn(node);
            }
        },
        onLoadSuccess: function (node, data) {
            $('#uiRTree').tree('expandAll');
        }
    });
    $("#uiBody").layout('expand', 'east');
}
//js获取url参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return (unescape(r[2]));
    return ""; //返回参数值
}
function getUrlParam1(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return (decodeURI(r[2]));
    return ""; //返回参数值
}
//调整datagrid大小
function datagridResize(jq) {
    jq.layout('panel', 'center').panel({
        onResize: function (w, h) {
            setTimeout(function () {
                $("[name='datagrid']").datagrid("resize", { width: w });
            }, 0);
        }
    });
}
//对话框
function showMyDialog(jq, title, icon, href, width, modal, fn, minimizable, maximizable) {
    var dlg = jq.dialog({
        title: title,
        width: width === undefined ? 750 : width,
        iconCls: "" + icon + "",
        href: href,
        top: 100,
        shadow: false,
        cache: false,
        closed: true,
        collapsible: false,
        resizable: false,
        modal: modal === undefined ? true : modal,
        minimizable: minimizable === undefined ? false : minimizable,
        maximizable: maximizable === undefined ? false : maximizable,
        loadingMessage: '数据正在加载中，请稍等......',
        buttons: [{//底部按钮
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () { fn(); }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                jq.dialog('close');
            }
        }]
    });
}
function showMyDialog1(jq, title, icon, href, name, modal, minimizable, maximizable) {
    var dlg = jq.dialog({
        title: title,
        iconCls: "" + icon + "",
        content: '<iframe id="' + name + '" name="' + name + '" scrolling="yes" frameborder="0"  src="" style="width:100%;height:100%;"></iframe>',
        shadow: false,
        cache: false,
        closed: true,
        collapsible: false,
        draggable: true,
        resizable: false,
        modal: modal === undefined ? true : modal,
        minimizable: minimizable === undefined ? false : minimizable,
        maximizable: maximizable === undefined ? false : maximizable,
        loadingMessage: '数据正在加载中，请稍等......'
    });
    return dlg;
}
//window窗体
function showMyWindow(jq, title, icon, href, name, modal, minimizable, maximizable) {
    var win = jq.window({
        title: title,
        iconCls: "" + icon + "",
        content: '<iframe id="' + name + '" name=' + name + ' src="" scrolling="auto" frameborder="0" style="width:100%;height:100%"></iframe>',
        shadow: false,
        cache: false,
        closed: true,
        collapsible: true,
        draggable: true,
        resizable: false,
        modal: modal === undefined ? true : modal,
        minimizable: minimizable === undefined ? false : minimizable,
        maximizable: maximizable === undefined ? false : maximizable,
        loadingMessage: '数据正在加载中，请稍等......'
    });
    return win;
}
//获取datagrid查询列
function getWhereColumns(jq, fn) {
    var fields = jq.datagrid("getColumnFields");
    var whereColumns = "";
    for (var i = 0; i < fields.length; i++) {
        var option = jq.datagrid("getColumnOption", fields[i]);
        if (option.where) {
            whereColumns += '{ColumnField:"' + fields[i] + '",ColumnTitle:"' + encodeURI(option.title) + '"},';
        }
    }
    if (whereColumns != "") {
        whereColumns = "[" + $.trimend(whereColumns, ',') + "]";
        fn(whereColumns);
    }
}
//获取datagrid导出列
function getExportColumns(jq, fn) {
    var fields = jq.datagrid("getColumnFields");
    var exportColumns = "";
    for (var i = 0; i < fields.length; i++) {
        var option = jq.datagrid("getColumnOption", fields[i]);
        exportColumns += '{ColumnField:"' + fields[i] + '",ColumnTitle:"' + encodeURI(option.title) + '"},';
    }
    if (exportColumns != "") {
        exportColumns = "[" + $.trimend(exportColumns, ',') + "]";
        fn(exportColumns);
    }
}
