$(function () {

    var isVerify = false;
    //登录
    $("#login").click(function () {

        var data = {};
        var url = "/Account/Login";

        if (isVerify) {

            url = "/Account/Valid";
            var verifyCode = $("#verifyCode").val();

            if (verifyCode.length === 0) {
                alert("请输入校验码");
                return;
            }

            data = { code: verifyCode };

        } else {
            var email = $("#user").val();
            var password = $("#password").val();

            if (email.length === 0 || password.length === 0) {
                alert("请输入用户名密码");
                return;
            }

            data = { email: email, password: password };
        }
        
        $.ajax({
            //请求方式
            type: "POST",
            //请求地址
            url: url,
            //数据，json字符串
            data: data,
            //请求成功
            success: function (data) {
                if (data.result === 1) {
                    alert("登录成功,跳转");
                    if (data.url) {
                        location.href = data.url;
                    }
                } else if (data.result === 2) {
                    alert("用户名密码校验成功,执行检验2FA逻辑");

                    $("#verifyCode").removeClass("hide");
                    $("#user,#password").css("visibility", "hidden");
                    $("#login").val("校验");

                    isVerify = true;

                } else {
                    alert(data.msg);
                }
            },
            //请求失败，包含具体的错误信息
            error: function (e) {
                console.log(e.status);
                console.log(e.responseText);
            }
        });

    });

    if (this.location.href.toLocaleLowerCase().indexOf('/account/bind') > 0) {

        $.ajax({
            //请求方式
            type: "GET",
            //请求地址
            url: "/account/GetQr",
            //请求成功
            success: function (data) {
                if (data.qrCodeContennt) {
                    jQuery('#qrcode').qrcode({ width: 225, height: 225, text: unescape(data.qrCodeContennt) });
                }
            },
            //请求失败，包含具体的错误信息
            error: function (e) {
                console.log(e.status);
                console.log(e.responseText);
            }
        });
    }

    //绑定
    $("#bind").click(function () {

        var veryCode = $("#verifyCode").val();

        $.ajax({
            //请求方式
            type: "POST",
            //请求地址
            url: "/account/Valid",
            //数据，json字符串
            data: { code: veryCode },
            //请求成功
            success: function (data) {
                if (data.result === 1) {
                    alert(data.msg);//校验成功
                    if (data.url) {
                        location.href = data.url;
                    }
                } else {
                    alert(data.msg);//校验失败逻辑
                }
            },
            //请求失败，包含具体的错误信息
            error: function (e) {
                console.log(e.status);
                console.log(e.responseText);
            }
        });
    });

    //退出
    $("#logout").click(function () {
        location.href = "/account/Logout";
    });
    
});