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
    }
});
//添加时间戳
function addTimestamp(url) {
    return url += "timestamp=" + (new Date()).getTime;
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
        onResize: function (width, height) {
            $("[name=datagrid]").datagrid("resize", width);
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
            handler: function () {
                fn();
            }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                jq.dialog('close');
            }
        }]
    });
}
function showMyDialog1(jq, title, icon, href, name, width, height, fn, modal, minimizable, maximizable) {
    var dlg = jq.dialog({
        title: title,
        width: width === undefined ? 750 : width,
        height: height === undefined ? 750 : height,
        iconCls: "" + icon + "",
        content: '<iframe name="' + name + '" scrolling="yes" frameborder="0"  src="' + href + '" style="width:100%;height:100%;"></iframe>',
        top: 10,
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
            handler: function () {
                fn();
            }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                jq.dialog('close');
            }
        }]
    });
}
//window窗体
function showMyWindow(jq, title, icon, href, width, height, modal, minimizable, maximizable) {
    var win = jq.window({
        title: title,
        width: width === undefined ? 750 : width,
        height: height === undefined ? 480 : height,
        iconCls: "" + icon + "",
        href: href,
        modal: modal === undefined ? true : modal,
        minimizable: minimizable === undefined ? false : minimizable,
        maximizable: maximizable === undefined ? false : maximizable,
        shadow: false,
        closed: true,
        cache: false,
        collapsible: true,
        draggable: true,
        resizable: false,
        loadingMessage: '数据正在加载中，请稍等......'
    });
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