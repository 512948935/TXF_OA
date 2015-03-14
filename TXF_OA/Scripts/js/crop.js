function updateCoords(n) {
    jQuery("#x").val(n.x),
    jQuery("#y").val(n.y),
    jQuery("#w").val(n.w),
    jQuery("#h").val(n.h)
}
var croper;
$(function () {
    new Uploader($("#jquery-wrapped-fine-uploader"),
    function (n) {
        $("#preview_title").html("头像预览"),
        $("img.preview-image").each(function () {
            this.src = n
        }),
        $("#imgsrc").val(n),
        croper ? croper.setImage(n) : croper = new Croper($("#crop_image"), new Previewer([[$("#preview_small"), 48], [$("#preview_large"), 180]])),
        //防止重复添加click事件
        $("#crop_operation_submit").unbind("click");
        $("#crop_operation_submit").bind("click",
        function () {
            $("#crop_operation_msg").html("操作中...").show();
            //保存
            $.ajax({
                type: "POST",
                data: $('#form_crop').serialize().replace(/\+/g, " "),
                url: "/Item/SaveImage?ID=" + getUrlParam("ID"),
                cache: false,
                error: function () { alert('执行失败.', 'warning'); },
                success: function (obj) {
                    n = eval(obj);
                    $("#crop_image").attr("src", "");
                    croper.setImage(""),
                    $("#preview_small").removeAttr("style").attr("src", n.FaceSrc);
                    $("#preview_large").removeAttr("style").attr("src", n.AvatarSrc);
                    $("#crop_operation").hide();
                    $("#crop_operation_msg").html("");
                    $("#croped_message").html("裁切并保存成功");
                    $("#preview_title").html("更新后的头像");
                }
            });
        }),
        $("#crop_operation").show(),
        $("#croped_message").html("")
    });
})
var Uploader = function () {
    var n = function (n, t) {
        n.fineUploader({
            validation: {
                allowedExtensions: ["png", "gif", "jpg", "jpeg"],
                sizeLimit: 10485760
            },
            request: {
                endpoint: "/Item/UploadImage"
            },
            text: {
                uploadButton: '<i class="icon-upload icon-white"><\/i> 上传头像图片',
                dropProcessing: "（支持文件拖放上传，只能上传单张10M以下png、jpg、gif图片）"
            },
            template: '<div class="qq-uploader span12"><pre class="qq-upload-drop-area span12"><span>{dragZoneText}<\/span><\/pre><div class="qq-upload-button btn btn-success" style="width: auto;">{uploadButtonText}<\/div><span class="qq-drop-processing"><span>{dropProcessingText}<\/span><span class="qq-drop-processing-spinner"><\/span><\/span><ul class="qq-upload-list" style="margin-top: 10px;overflow:hidden;"><\/ul><\/div>',
            classes: {
                success: "alert alert-success",
                fail: "alert alert-error"
            },
            multiple: !1
        }).on("complete", function (n, i, r, u) {
            if (u.success) {
                var f = u.message;
                f += "?id=" + (new Date).getTime() + Math.floor(Math.random() * 1e3);
                t(f);
            }
            else
                $("#message").html(u.message)
        })
    };
    //裁剪计算方法.
    return n.prototype = { constructor: n }, n
} (),
	Previewer = function () {
	    var n, t = function (t) {
	        n = t
	    };
	    return t.prototype = {
	        constructor: t,
	        showAllPreview: function (t) {
	            var i = this.getWidgetSize(),
					r;
	            width = i[0], height = i[1], r = function (n, t, i) {
	                if (parseInt(n.w) > 0) {
	                    var r = i / n.w,
							u = i / n.h;
	                    t.css({
	                        width: Math.round(r * width) + "px",
	                        height: Math.round(u * height) + "px",
	                        marginLeft: "-" + Math.round(r * n.x) + "px",
	                        marginTop: "-" + Math.round(u * n.y) + "px"
	                    }).show()
	                }
	            }, $.each(n, function () {
	                r(t, this[0], this[1])
	            })
	        },
	        hideAllPreview: function () {
	            $.each(n, function () {
	                this[0].stop().fadeOut("fast")
	            })
	        }
	    }, t
	} (),
	Croper = function () {
	    var n, t, i = function (i, r) {
	        t = this, i.Jcrop({
	            minSize: [48, 48],
	            onChange: r.showAllPreview,
	            onSelect: updateCoords,
	            aspectRatio: 1
	        }, function () {
	            n = this, t.setSelect()
	        });
	    };
	    return i.prototype = {
	        constructor: i,
	        setImage: function (i) {
	            n.setImage(i, function () {
	                t.setSelect()
	            });
	        },
	        setSelect: function () {
	            var t = Math.min.apply(Math, n.getWidgetSize());
	            n.setSelect([0, 0, t, t])
	        }
	    }, i
	} ();