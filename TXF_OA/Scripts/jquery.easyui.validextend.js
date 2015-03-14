//扩大easyui表单的验证
$.extend($.fn.validatebox.defaults.rules, {
    isChinese: {// 验证中文 
        validator: function (value) {
            return /^[\Α-\￥]+$/i.test(value);
        },
        message: '请输入中文.'
    },
    isEnglish: {// 验证英语 
        validator: function (value) {
            return /^[A-Za-z]+$/i.test(value);
        },
        message: '请输入英文.'
    },
    isInt: {  //数字验证
        validator: function (value, param) {
            return /^[+]?[1-9]+\d*$/i.test(value);
        },
        message: '请输入整数.'
    },
    isIntOrFloat: {// 验证整数或小数 
        validator: function (value) {
            return /^\d+(\.\d+)?$/i.test(value);
        },
        message: '请输入数字，并确保格式正确.'
    },
    isCurrency: {// 验证货币 
        validator: function (value) {
            return /^\d+(\.\d+)?$/i.test(value);
        },
        message: '货币格式不正确.'
    },
    isAge: {// 验证年龄
        validator: function (value) {
            return /^(?:[1-9][0-9]?|1[01][0-9]|120)$/i.test(value);
        },
        message: '年龄必须是0到120之间的整数.'
    },
    isTel: {// 验证电话号码 
        validator: function (value) {
            return /^(1[3|4|5|8|9]\d{9}|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)?$/i.test(value);
        },
        message: '电话号码格式不正确.'
    },
    isFax: {// 验证传真 
        validator: function (value) {
            return /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/i.test(value);
        },
        message: '传真号码格式不正确.'
    },
    isZIP: { //国内邮编验证
        validator: function (value) {
            return /^[1-9]\d{5}$/i.test(value);
        },
        message: "邮编格式不正确."
    },
    isQQ: {//qq验证      
        validator: function (value, param) {
            return /^[1-9]\d{4,10}$/i.test(value);
        },
        message: 'QQ号码不正确.'
    },
    isIP: {// 验证IP地址 
        validator: function (value) {
            return /^((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))$/i.test(value);
        },
        message: 'IP地址格式不正确'
    },
    isName: {// 验证姓名，可以是中文或英文 
        validator: function (value) {
            return /^[\Α-\￥]+$/i.test(value) | /^\w+[\w\s]+\w+$/i.test(value);
        },
        message: '请输入正确的姓名'
    },
    isMSN: { //msn验证
        validator: function (value) {
            return /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/i.test(value);
        },
        message: '请输入有效的msn账号(例：abc@hotnail(msn/live).com)'
    },
    isPassword: {//密码验证         
        validator: function (value, param) {
            return /^[\@A-Za-z0-9\!\#\$\%\^\&\*\.\~]{6,22}$/i.test(value);
        },
        message: '密码由字母和数字组成至少6位.'
    },
    //身份证验证    
    isIdCard: {// 验证身份证 
        validator: function (value) {
            return /^\d{15}(\d{2}[A-Za-z0-9])?$/i.test(value);
        },
        message: '身份证号码格式不正确.'
    },
    //时间验证    
    isDateTime: {
        validator: function (value) {
            //格式yyyy-MM-dd或yyyy-M-d
            return /^(?:(?!0000)[0-9]{4}([-]?)(?:(?:0?[1-9]|1[0-2])\1(?:0?[1-9]|1[0-9]|2[0-8])|(?:0?[13-9]|1[0-2])\1(?:29|30)|(?:0?[13578]|1[02])\1(?:31))|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)([-]?)0?2\2(?:29))$/i.test(value);
        },
        message: '清输入合适的日期格式.'
    },
    //时间比较
    isStart: {
        validator: function (value, param) {
            var dateA = $.fn.datebox.defaults.parser(value);
            var dateB = $.fn.datebox.defaults.parser($(param[0]).datebox('getValue'));
            return dateA < dateB;
        },
        message: '开始时间不能大于结束时间.'
    },
    isEnd: {
        validator: function (value, param) {
            var dateB = $.fn.datebox.defaults.parser(value);
            var dateA = $.fn.datebox.defaults.parser($(param[0]).datebox('getValue'));
            return dateB > dateA;
        },
        message: '结束时间不能小于开始时间.'
    },
    isEmail: {
        validator: function (value, param) {
            return /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/i.test(value);
        },
        message: '邮箱格式不正确.'
    },
    noEqual: { //不相等验证     
        validator: function (value, param) {
            return value != param[0];
        },
        message: '请选择内容！'
    },
    equalTo: { //是否一致          
        validator: function (value, param) {
            return value == $(param[0]).val();
        },
        message: '两次输入的字符不一至.'
    },
    isRange: {//字符范围
        validator: function (value, param) {
            return value.length >= param[0] && value.length <= param[1]; ;
        },
        message: '字符长度不在指定范围内.'
    },
    minLength: {//最小字符
        validator: function (value, param) {
            return value.length >= param[0];
        },
        message: '请输入至少{0}个字符.'
    },
    maxLength: {//最大字符
        validator: function (value, param) {
            return value.length <= param[0];
        },
        message: '请输入最多{0}个字符.'
    },
    unnormal: {// 验证是否包含空格和非法字符 
        validator: function (value) {
            return /.+/i.test(value);
        },
        message: '输入值不能为空和包含其他非法字符.'
    },
    username: {// 验证用户名 
        validator: function (value) {
            return /^[a-zA-Z][a-zA-Z0-9_]{5,15}$/i.test(value);
        },
        message: '用户名不合法（字母开头，允许6-16字节，允许字母数字下划线）.'
    }
});