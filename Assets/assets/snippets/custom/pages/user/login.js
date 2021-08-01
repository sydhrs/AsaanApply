//== Class Definition
var SnippetLogin = function () {

    var login = $('#m_login');

    // $("#genderType").selectpicker();


    var showErrorMsg = function (form, type, msg) {
        var alert = $('<div class="m-alert m-alert--outline alert alert-' + type + ' alert-dismissible" role="alert">\
			<button type="button" class="close" data-dismiss="alert" aria-label="Close"></button>\
			<span></span>\
		</div>');

        form.find('.alert').remove();
        alert.prependTo(form);
        //alert.animateClass('fadeIn animated');
        mUtil.animateClass(alert[0], 'fadeIn animated');
        alert.find('span').html(msg);
    }

    //== Private Functions

    var displaySignUpForm = function () {
        login.removeClass('m-login--forget-password');
        login.removeClass('m-login--signin');

        login.addClass('m-login--signup');
        mUtil.animateClass(login.find('.m-login__signup')[0], 'flipInX animated');
    }

    var displaySignInForm = function () {
        login.removeClass('m-login--forget-password');
        login.removeClass('m-login--signup');

        login.addClass('m-login--signin');
        mUtil.animateClass(login.find('.m-login__signin')[0], 'flipInX animated');
        //login.find('.m-login__signin').animateClass('flipInX animated');
    }

    var displayForgetPasswordForm = function () {
        login.removeClass('m-login--signin');
        login.removeClass('m-login--signup');

        login.addClass('m-login--forget-password');
        //login.find('.m-login__forget-password').animateClass('flipInX animated');
        mUtil.animateClass(login.find('.m-login__forget-password')[0], 'flipInX animated');

    }

    var handleFormSwitch = function () {
        $('#m_login_forget_password').click(function (e) {
            e.preventDefault();
            displayForgetPasswordForm();
        });

        $('#m_login_forget_password_cancel').click(function (e) {
            e.preventDefault();
            displaySignInForm();
        });

        $('#m_login_signup').click(function (e) {
            e.preventDefault();
            displaySignUpForm();
        });

        $('#m_login_signup_cancel').click(function (e) {
            e.preventDefault();
            displaySignInForm();
        });
    }

    var handleSignInFormSubmit = function () {
        $('#m_login_signin_submit').click(function (e) {
            e.preventDefault();
            var btn = $(this);
            var form = $(this).closest('form');

            form.validate({
                rules: {
                    email: {
                        required: true,
                        email: true
                    },
                    password: {
                        required: true
                    }
                }
            });

            if (!form.valid()) {
                return;
            }

            let data = {};
            form.serializeArray().map(function (x) {
                data[x.name] = x.value;
            });


            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            let fd = new FormData();
            fd.append("Username", data.username);
            fd.append('Password', data.password);

            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "/Student/Login",
                data: fd,
                processData: false,
                contentType: false,
                cache: false,
                timeout: 600000,
                success: function (response) {
                    // Do something with the return value from.Net method
                    btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    console.log(response);
                    if (response.StatusCode === 200) {
                        showErrorMsg(form, 'success', response?.Message || 'Logged In Successfully!');
                        window.location.href = response.Data.Role === "Student" ? '/Student' : '/Admin';
                    } else {
                        showErrorMsg(form, 'danger', response?.Message || "Error Occured While Signing in. Please Try Again!");
                    }
                },
                error: function (err) {
                    btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    // if (showErrors)
                    console.error("Error: ", err);
                    console.log("JQ", $.parseHTML(err.responseText))
                    showErrorMsg(form, 'danger', $.parseHTML(err.responseText)[1]?.innerText || "Error Occured While Signing in. Please Try Again!");
                }
            });
        });
    }

    var handleSignUpFormSubmit = function () {
        $('#m_login_signup_submit').click(function (e) {
            e.preventDefault();

            var btn = $(this);
            var form = $(this).closest('form');

           var validator =  form.validate({
                rules: {
                    FirstName: {
                        required: true
                    },
                    LastName: {
                        required: true
                    },
                    Username: {
                        required: true
                    },
                    Email: {
                        required: true,
                        email: true
                    },
                    Password: {
                        required: true
                    },
                    Cpassword: {
                        required: true
                    },
                    CNIC: {
                        required: true,
                        digits: true,
                        minlength: 13,
                        maxlength: 13
                    },
                    agree: {
                        required: true
                    }
                }
            });

            if (!form.valid()) {
                return;
            }

            let txtPassword = document.getElementById("txtPassword");
            let txtConfirmPassword = document.getElementById("txtConfirmPassword");
            txtConfirmPassword.setCustomValidity("");
            if (txtPassword.value !== txtConfirmPassword.value) {
                validator.showErrors({
                    "Cpassword": "Passwords do not Match!"
                });
                return;
            }

            // Confirm Password Check.

            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

            let data = {};
            let tagHistory = {};
            let isApDone = false;

            form.serializeArray().map(function (x) {
                let e = $("[name='" + x.name + "']");
                let tag = e.attr("tag");
                if (tag !== undefined && tag.toString().indexOf('Person') > -1) {
                    data[x.name] = x.value;
                }
                if (tag !== undefined && tag.toString().indexOf('ApplicationUser') > -1) {
                    if (!isApDone) {
                        data['ApplicationUser'] = {};
                        isApDone = true;
                    }
                    data.ApplicationUser[x.name] = x.value;
                }
            });

            data.DateOfBirth = moment(data.DateOfBirth, 'YYYY-MM-DD').format("L");

            let fdata = JSON.stringify(data);
            let fd = new FormData();
            fd.append("personJson", fdata);
            $.each($("input:file"), ((index, element) => {
                console.log(element.files[0]);
                if (element.name === 'ProfilePic')
                    fd.append(element.name, element.files[0])
            }));

            console.log("DATA: ", data);
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "/Student/Register",
                data: fd,
                processData: false,
                contentType: false,
                cache: false,
                timeout: 600000,
                success: function (response) {
                    // Do something with the return value from.Net method
                    btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    console.log(response);

                    if (response.StatusCode === 200) {

                        // display signup form
                        form.clearForm();
                        form.validate().resetForm();

                        displaySignInForm();
                        var signInForm = login.find('.m-login__signin form');
                        signInForm.clearForm();
                        signInForm.validate().resetForm();

                        showErrorMsg(signInForm, 'success', 'Thank you. To complete your registration please check your email.');

                    } else {
                        // swal({
                        //     "title": "Error Occured!",
                        //     "text": response.Message,
                        //     "type": "error",
                        //     "confirmButtonClass": "btn btn-danger m-btn m-btn--wide"
                        // });
                        showErrorMsg(form, 'danger', response?.Message || "Error Occured While Registering please Try Again!");
                    }

                },
                error: function (err) {
                    // if (showErrors)
                    btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    console.error("Error: ", err);
                    console.log("JQ", $.parseHTML(err.responseText))
                    showErrorMsg(form, 'danger', $.parseHTML(err.responseText)[1]?.innerText || "Error Occured While Registering please Try Again!");

                    // swal({
                    //     "title": "Error Occured!",
                    //     "text": $.parseHTML(err.responseText)[1].innerText,
                    //     "type": "error",
                    //     "confirmButtonClass": "btn btn-error m-btn m-btn--wide"
                    // });
                }
            });
        });
    }

    var handleForgetPasswordFormSubmit = function () {
        $('#m_login_forget_password_submit').click(function (e) {
            e.preventDefault();

            var btn = $(this);
            var form = $(this).closest('form');

            form.validate({
                rules: {
                    email: {
                        required: true,
                        email: true
                    }
                }
            });

            if (!form.valid()) {
                return;
            }

            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

            form.ajaxSubmit({
                url: '',
                success: function (response, status, xhr, $form) {
                    // similate 2s delay
                    setTimeout(function () {
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false); // remove 
                        form.clearForm(); // clear form
                        form.validate().resetForm(); // reset validation states

                        // display signup form
                        displaySignInForm();
                        var signInForm = login.find('.m-login__signin form');
                        signInForm.clearForm();
                        signInForm.validate().resetForm();

                        showErrorMsg(signInForm, 'success', 'Cool! Password recovery instruction has been sent to your email.');
                    }, 2000);
                }
            });
        });
    }

    var handleGenderTypesLoading = function () {
        let genderTypes = [];
        let genderTypeIdEle = $("#genderType");
        $.ajax({
            type: 'GET',
            url: '/GenderTypes/GetList',
            dataType: 'json',
            success: (response) => {
                console.log(response);
                if (response !== null) {
                    response.forEach(el => {
                        genderTypes.push(el);
                        genderTypeIdEle.append(`<option value="${el.Id}">${el.Name}</option>`)
                    });
                } else {

                }
            },
            error: (err) => {
                // if (showErrors)
                console.error(err);

            }
        });
    }

    //== Public Functions
    return {
        // public functions
        init: function () {
            handleFormSwitch();
            handleSignInFormSubmit();
            handleSignUpFormSubmit();
            handleForgetPasswordFormSubmit();
            // Dropzone.options.mDropzoneProPic = {
            //     paramName: "file",
            //     maxFiles: 1,
            //     maxFilesize: 5,
            //     addRemoveLinks: !0
            // }
            handleGenderTypesLoading();
        }
    };
}();

//== Class Initialization
jQuery(document).ready(function () {
    SnippetLogin.init();

    $(".dropify").dropify({
        messages: {
            'default': 'Drag and drop a file here or click',
            'replace': 'Drag and drop or click to replace',
            'remove': 'Remove',
            'error': 'Ooops, something wrong happended.'
        }
    });

});