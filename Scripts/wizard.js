//== Class definition
var WizardDemo = function () {
    //== Base elements
    var wizardEl = $('#m_wizard');
    var formEl = $('#m_form');
    var validator;
    var wizard;
    var studentAcademics = [];

    //== Private functions
    var initWizard = function () {
        //== Initialize form wizard
        wizard = new mWizard('m_wizard',
            {
                startStep: 1
            });

        //== Validation before going to next page
        wizard.on('beforeNext',
            function (wizardObj) {
                if (validator.form() !== true) {
                    wizardObj.stop(); // don't go to the next step
                }
            })

        //== Change event
        wizard.on('change',
            function (wizard) {
                mUtil.scrollTop();
            });

        //== Change event
        wizard.on('change',
            function (wizard) {
                if (wizard.getStep() === 4) {


                    //Personal Information
                    $("#StudentName").html($("[name='FirstName']").val() + $("[name='MiddleName']").val() + $("[name='LastName']").val());
                    $("#phoneNoVal").html($("[name='MobilePhone']").val());
                    $("#CNICNoVal").html($("[name='CnicNo']").val());
                    $("#DoBVal").html($("[name='DateOfBirth']").val());
                    $("#EmailVal").html($("[name='Email']").val());
                    $("#MaritalStatusVal").html($("[name='MaritalStatusTypeId']").val());  //id & name property
                    $("#AddressVal").html($("[name='Address1']").val());
                    $("#DistrictVal").html($("[name='District']").val());
                    $("#CityVal").html($("[name='City']").val());
                    $("#SoRVal").html($("[name='StateOfRegion']").val());
                    $("#ZipCodeVal").html($("[name='ZipCode']").val());

                    //Gaurdian Information
                    $("#GaurdianNameVal").html($("[name='GuardianName']").val());
                    $("#GaurdianPhoneNOVal").html($("[name='GuardianContact']").val());
                    $("#GaurdianAddressVal").html($("[name='GuardianAddress']").val());
                    $("#EmergencyNameVal").html($("[name='EmergencyName']").val());
                    $("#EmergencyPhoneNOVal").html($("[name='EmergencyContact']").val());
                 

                    //Academic Information
                    $("#IsMorningShiftVal").html($("[name='IsMorningShift']").val());
                    $("#StudentTypeIdVal").html($("[name='StudentTypeId']").val());
                    $("#DegreePursuingTypeIdVal").html($("[name='DegreePursuingTypeId']").val());
                    console.log("Students Academics",studentAcademics);

                    var finalHtml = '';

                    studentAcademics.forEach((val, index, arr) => {
                        console.log(arr);
                        console.log(studentAcademics);

                        let board = val.Boards.find(f => f.Id === Number($(`[name='AcademicDegreeBoardTypeId[${index}]']`).val()));

                        let boardName = board === null ? 'Select One First!' : (board?.Description === null || board?.RTCSessionDescription === '' ? board?.Description : board?.Name)
                           

                        if (index === 0)
                            finalHtml += `<div class="m-form__section m-form__section--first">
                                                                <div class="m-form__heading">
                                                                    <h4 class="m-form__heading-title">${val.Description}</h4>
                                                                </div>
                                                                <div class="form-group m-form__group row">
                                                                    <label class="col-form-label col-lg-2 col-sm-12">Roll No:</label>
                                                                    <div class="col-lg-4 col-md-9 col-sm-12">
                                                                        <span class="m-form__control-static">${$(`[name='RollNo[${index}]']`).val()}</span>
                                                                    </div>
                                                                    <label class="col-form-label col-lg-2 col-sm-12">Martic Board:</label>
                                                                    <div class="col-lg-4 col-md-9 col-sm-12">
                                                                        <span class="m-form__control-static">${boardName}</span>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group m-form__group row">
                                                                    <label class="col-form-label col-lg-2 col-sm-12">Total Marks:</label>
                                                                    <div class="col-lg-4 col-md-9 col-sm-12">
                                                                        <span class="m-form__control-static">${$(`[name='TotalMarks[${index}]']`).val()}</span>
                                                                    </div>
                                                                    <label class="col-form-label col-lg-2 col-sm-12">Obtained Marks:</label>
                                                                    <div class="col-lg-4 col-md-9 col-sm-12">
                                                                        <span class="m-form__control-static">${$(`[name='MarksObtained[${index}]']`).val()}</span>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group m-form__group m-form__group--sm row">
                                                                    <label class="col-xl-4 col-lg-4 col-form-label">YEAR Martic:</label>
                                                                    <div class="col-xl-8 col-lg-8">
                                                                        <span class="m-form__control-static">${$(`[name='YearOfPassing[${index}]']`).val()}</span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            `;
                        if (index === 0 && arr.length !== 1)
                            finalHtml += `<div class="m-separator m-separator--dashed m-separator--lg"></div>`;
                        if (index !== 0) {
                            finalHtml += `<div class="m-form__section">
                                <div class="m-form__heading">
                                    <h4 class="m-form__heading-title">${val.Description}</h4>
                                </div>
                                <div class="form-group m-form__group row">
                                    <label class="col-form-label col-lg-2 col-sm-12">Roll No:</label>
                                    <div class="col-lg-4 col-md-9 col-sm-12">
                                        <span class="m-form__control-static">${$(`[name='RollNo[${index}]']`).val()}</span>
                                    </div>
                                    <label class="col-form-label col-lg-2 col-sm-12">Martic Board:</label>
                                    <div class="col-lg-4 col-md-9 col-sm-12">
                                        <span class="m-form__control-static">${boardName}</span>
                                    </div>
                                </div>
                                <div class="form-group m-form__group row">
                                    <label class="col-form-label col-lg-2 col-sm-12">Total Marks:</label>
                                    <div class="col-lg-4 col-md-9 col-sm-12">
                                        <span class="m-form__control-static">${$(`[name='TotalMarks[${index}]']`).val()}</span>
                                    </div>
                                    <label class="col-form-label col-lg-2 col-sm-12">Obtained Marks:</label>
                                    <div class="col-lg-4 col-md-9 col-sm-12">
                                        <span class="m-form__control-static">${$(`[name='MarksObtained[${index}]']`).val()}</span>
                                    </div>
                                </div>
                                <div class="form-group m-form__group m-form__group--sm row">
                                    <label class="col-xl-4 col-lg-4 col-form-label">YEAR Martic:</label>
                                    <div class="col-xl-8 col-lg-8">
                                        <span class="m-form__control-static">${$(`[name='YearOfPassing[${index}]']`).val()}</span>
                                    </div>
                                </div>

                            </div>`;
                        }
                    });

                    $("#academicInformationContent").html(finalHtml);
                    console.log(studentAcademics[0]);

                    //     alert(1);
                }
            });
    }

    var initValidation = function () {
        validator = formEl.validate({
            //== Validate only visible fields
            ignore: ":hidden",

            //== Validation rules
            rules: {
                //=== Client Information(step 1)
                //== Client details
                name: {
                    required: true
                },
                email: {
                    required: true,
                    email: true
                },
                MobilePhone: {
                    required: true,
                    digits: true,
                    minlength: 11,
                    maxlength: 11
                },

                //== Mailing address
                address1: {
                    required: true
                },
                state: {
                    required: true
                },
                city: {
                    required: true
                },
                country: {
                    required: true
                },
                //=== Client Information(step 2)
                //== Account Details
                account_url: {
                    required: true,
                    url: true
                },
                account_username: {
                    required: true,
                    minlength: 4
                },
                account_password: {
                    required: true,
                    minlength: 6
                },

                //== Client Settings
                account_group: {
                    required: true
                },
                'account_communication[]': {
                    required: true
                },

                //=== Client Information(step 3)
                //== Billing Information
                billing_card_name: {
                    required: true
                },
                billing_card_number: {
                    required: true,
                    creditcard: true
                },
                billing_card_exp_month: {
                    required: true
                },
                billing_card_exp_year: {
                    required: true
                },
                billing_card_cvv: {
                    required: true,
                    minlength: 2,
                    maxlength: 3
                },

                //== Billing Address
                billing_address_1: {
                    required: true
                },
                billing_address_2: {},
                billing_city: {
                    required: true
                },
                billing_state: {
                    required: true
                },
                billing_zip: {
                    required: true,
                    number: true
                },
                billing_delivery: {
                    required: true
                },

                //=== Confirmation(step 4)
                accept: {
                    required: true
                }
            },

            //== Validation messages
            messages: {
                'account_communication[]': {
                    required: 'You must select at least one communication option'
                },
                accept: {
                    required: "You must accept the Terms and Conditions agreement!"
                }
            },

            //== Display error  
            invalidHandler: function (event, validator) {
                mUtil.scrollTop();

                swal({
                    "title": "",
                    "text": "There are some errors in your submission. Please correct them.",
                    "type": "error",
                    "confirmButtonClass": "btn btn-secondary m-btn m-btn--wide"
                });
            },

            //== Submit valid form
            submitHandler: function (form) {

            }
        });
    }

    function objectifyForm(formArray) {
        //serialize data function
        var returnArray = {};
        for (var i = 0; i < formArray.length; i++) {
            returnArray[formArray[i]['name']] = formArray[i]['value'];
        }
        return returnArray;
    }

    var initSubmit = function () {
        var btn = formEl.find('[data-wizard-action="submit"]');

        btn.on('click',
            function (e) {
                e.preventDefault();
                if (validator.form()) {
                    //== See: src\js\framework\base\app.js
                    mApp.progress(btn);
                    //mApp.block(formEl); 

                    //== See: http://malsup.com/jquery/form/#ajaxSubmit
//                    var data = objectifyForm($(formEl).serializeArray());

                    let data = {};
                    let tagHistory = {};
                    let isSPDone = false;
                    let isADone = false;
                    let isSDone = false;
                    let isSMDone = false;

                    $("#m_form").serializeArray().map(function (x) {
                        // debugger;
                        let e = $("[name='" + x.name + "']");
                        let tag = e.attr("tag");
                        if (tag === 'Admission') {
                            data[x.name] = x.value;
                        } else if (tag === 'PersonContact') {
                            if (!isSPDone) {
                                if (!isSDone) {
                                    data['Student'] = {};
                                    isSDone = true;
                                }
                                data.Student['PersonContact'] = {};
                                isSPDone = true;
                            }
                            data.Student.PersonContact[x.name] = x.value;
                        } else if (tag === 'Address') {
                            if (!isADone) {
                                if (!isSDone) {
                                    data['Student'] = {};
                                    isSDone = true;
                                }
                                data.Student['Address'] = {};
                                isADone = true;
                            }

                            data.Student.Address[x.name] = x.value;
                        } else if (tag === 'StudentAcademics') {
                            if (!isSMDone) {
                                var count = $("[id^='StdAcd_']").length;
                                data.StudentAcademics = [];
                                for (let i = 0; i < count; i++) {
                                    data.StudentAcademics.push({});
                                }
                                isSMDone = true;
                            }
                            let parts = x.name.split('[');
                            let finalName = parts[0];
                            let ind = Number(parts[1].replace(']', ''));

                            data.StudentAcademics[ind][finalName || x.name] = x.value;
                        } else {
                            if (!isSDone) {
                                data['Student'] = {};
                                isSDone = true;
                            }
                            data.Student[x.name] = x.value;
                        }
                    });

//                    $("#m_form").serializeArray().map(function (x) {
//                        // debugger;
//                        let e = $(`[name="${x.name}"]`);
//                        let tag = e.attr("tag");
//                        let type = e[0].type.toLowerCase();
//
//                        if (tag !== null && tag !== undefined) {
//                            if (tagHistory[tag] === undefined || tagHistory[tag] === null || tagHistory[tag] === false) {
//                                data[tag] = {};
//                                tagHistory[tag] = true;
//                            }
//                            data[tag][x.name] = type === 'checkbox' ? e.prop("checked") === 'checked' || e.prop("checked") === true : x.value;
//                        } else {
//                            data[x.name] = type === 'checkbox' ? e.prop("checked") === 'checked' || e.prop("checked") === true : x.value;
//                        }
//                    });
//
//                    $("#m_form input[type='checkbox']").each(function (ind, x) {
//                        let e = $(`[name="${x.name}"]`);
//                        let tag = e.attr("tag");
//                        let type = x.type.toLowerCase();
//
//                        if (tag !== null && tag !== undefined) {
//                            if (tagHistory[tag] === undefined || tagHistory[tag] === null || tagHistory[tag] === false) {
//                                data[tag] = {};
//                                tagHistory[tag] = true;
//                            }
//                            data[tag][x.name] = type === 'checkbox' ? e.prop("checked") === 'checked' || e.prop("checked") === true : x.value;
//                        } else {
//                            data[x.name] = type === 'checkbox' ? e.prop("checked") === 'checked' || e.prop("checked") === true : x.value;
//                        }
//                    });

                   
                    let fd = new FormData();

                    if (data.IsMorningShift === 'both') {
                        fd.append("isMorningShift", 'both');
                        data.IsMorningShift = 'true';
                    } else {
                        fd.append("isMorningShift", 'single');
                    }

                    let fdata = JSON.stringify(data);
                    fd.append("admissionJson", fdata);

                    console.log("DATA: ", data);


                    $.ajax({
                        type: "POST",
                        enctype: 'multipart/form-data',
                        url: "/Student/PostForm",
                        data: fd,
                        processData: false,
                        contentType: false,
                        cache: false,
                        timeout: 600000,
                        success: function (response) {
                            mApp.unprogress(btn);
                            // Do something with the return value from.Net method
                            if (response.StatusCode === 200) {
                                swal({
                                    "title": "Done!",
                                    "text": "The application has been successfully submitted!",
                                    "type": "success",
                                    "confirmButtonClass": "btn btn-success m-btn m-btn--wide"
                                });
                                
                                setTimeout(() => {
                                    window.location.href = '/Student/TrackApplication'
                                }, 3000);
                                
                            } else {
                                swal({
                                    "title": "Error Occured!",
                                    "text": response.Message,
                                    "type": "error",
                                    "confirmButtonClass": "btn btn-danger m-btn m-btn--wide"
                                });
                            }

                        },
                        error: function (err) {
                            mApp.unprogress(btn);

                            // if (showErrors)
                            console.error("Error: ", err);
                            console.log("JQ", $.parseHTML(err.responseText))

                            swal({
                                "title": "Error Occured!",
                                "text": $.parseHTML(err.responseText)[1].innerText,
                                "type": "error",
                                "confirmButtonClass": "btn btn-error m-btn m-btn--wide"
                            });
                        }
                    });

//                formEl.ajaxSubmit({
//                    success: function() {
//                        mApp.unprogress(btn);
//                        //mApp.unblock(formEl);
//
//                    }
//                });

                }
            });
    }

    function getData(onCompletion) {
        let defaultHeaders = {};
        let showLogs = true;
        $.ajax({
            type: 'GET',
            url: '/Student/GetTypes',
            headers: defaultHeaders,
            dataType: 'json',
            success: (response) => {
                if (showLogs)
                    console.log(response);
                if (response !== null) {
                    onCompletion(true, response);
                } else {
                    onCompletion(false, null);
                }
                if (showLogs) {
                    console.log(response);
                }

            },
            error: (err) => {
                // if (showErrors)
                console.error(err);
                onCompletion(false, err);
            }
        });
    }

    let initOtherStuff = function () {
        // Lets Do it;

        let defaultHeaders = {};
        let showLogs = true;

        var degreePursuingTypes = [];
        var studentTypes = [];
        var degreePursuingTypeIdEle = $("#degreePursuingTypeId");
        var studentTypeIdEle = $("#studentTypeId");
        var academicSection = $("#academicsSection");
        degreePursuingTypeIdEle.on("change", function (e) {
            console.log("On Change DegreePursuingTypeId", e)

            let val = e.target.value;

            $.ajax({
                type: 'GET',
                url: '/Student/GetStudentAcademicTypes/' + val,
                headers: defaultHeaders,
                dataType: 'json',
                success: (response) => {
                    if (showLogs)
                        console.log(response);

                    if (response !== null) {
                        // Now...
                        console.log("Academic Degree Types: ", response);
                        academicSection.html('');
                        studentAcademics = response;
                        response.forEach((val, index) => {
                            console.log(index);
                            let boards = '';
                            val.Boards.forEach(v => {
                                boards += `<option value="${v.Id}" ${v.Description !== "" && v.Description !== null
                                    ? `title="${v.Description}"`
                                    : ''}>${v.Name}</option>`;
                            });
                            academicSection.append(`<div class="m-form__heading" id="StdAcd_${index}">
                                                    <h3 class="m-form__heading-title">${val.Description} Required*:</h3>
                                                    <div class="form-group m-form__group row">
                                                        <label class="col-form-label col-lg-2 col-sm-12"> Board:</label>
                                                        <div class="col-lg-4 col-md-9 col-sm-12">
                                                            <select class="form-control m-bootstrap-select m_selectpicker m_selectpicker_sub" title="Select Board..." name="AcademicDegreeBoardTypeId[${index}]" tag="StudentAcademics" data-style="btn-success">
                                                                ${boards}
                                                            </select>
                                                        </div>
                                                        <label class="col-form-label col-lg-2 col-sm-12">Roll No:</label>
                                                        <div class="col-lg-4 col-md-9 col-sm-12">
                                                            <input type='text' class="form-control" name="RollNo[${index}]" maxlength="6" tag="StudentAcademics" placeholder="123456"/>
                                                            <input type='text' class="form-control" hidden="hidden" name="AcademicDegreeTypeId[${index}]" value="${val.Id}" tag="StudentAcademics" placeholder="123456"/>
                                                        </div>
                                                    </div>
                                                    <div class="form-group m-form__group row">
                                                        <label class="col-form-label col-lg-2 col-sm-12">Total Marks:</label>
                                                        <div class="col-lg-4 col-md-9 col-sm-12">
                                                            <input type='text' class="form-control" maxlength="4" name="TotalMarks[${index}]" tag="StudentAcademics" placeholder="1100" />
                                                        </div>
                                                        <label class="col-form-label col-lg-2 col-sm-12">Obtained Marks:</label>
                                                        <div class="col-lg-4 col-md-9 col-sm-12">
                                                            <input type='text' class="form-control" maxlength="4" name="MarksObtained[${index}]" tag="StudentAcademics" placeholder="1000" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group m-form__group row">
                                                        <label class="col-form-label col-lg-2 col-sm-12">Passing Year:</label>
                                                        <div class="col-lg-4 col-md-9 col-sm-12">
                                                            <input type="number" class="form-control" name="YearOfPassing[${index}]" tag="StudentAcademics"/>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="m--space-20">
                                                    <hr />
                                                </div>`)
                        });
                        $(".m_selectpicker_sub").selectpicker();
                    } else {
                    }
                },
                error: (err) => {
                    console.error(err);
                }
            });
        });

        getData((isSuccessful, data) => {
            if (isSuccessful) {
                // degreePursuingTypeIdEle.selectpicker("refresh").empty();
                // studentTypeIdEle.selectpicker("refresh").empty();

                data.DegreePursuingTypes.forEach(el => {
                    degreePursuingTypes.push(el);
                    degreePursuingTypeIdEle.append(`<option value="${el.Id}">${el.Name}</option>`)
                });
                data.StudentTypes.forEach(el => {
                    studentTypeIdEle.append(`<option value="${el.Id}">${el.Name}</option>`)
                    studentTypes.push(el);
                });

                degreePursuingTypeIdEle.selectpicker("refresh");
                studentTypeIdEle.selectpicker("refresh");
                // Populate:

            } else {
                // Not SuccessFul = Show Error
            }

        })


    }

    var handleMaritalStatusTypesLoading = function () {
        let MaritalStatusTypes = [];
        let MaritalStatusTypeIdEle = $("#MaritalStatusType");  //
        $.ajax({
            type: 'GET',
            url: '/MaritalStatusTypes/GetList',
            dataType: 'json',
            success: (response) => {
                console.log(response);
                if (response !== null) {
                    response.forEach(el => {
                        MaritalStatusTypes.push(el);
                        MaritalStatusTypeIdEle.append(`<option value="${el.Id}">${el.Name}</option>`)
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


    return {
        // public functions
        init: function () {
            wizardEl = $('#m_wizard');
            formEl = $('#m_form');

            initOtherStuff();

            initWizard();
            initValidation();
            initSubmit();

            handleMaritalStatusTypesLoading();
        }
    };
}();

jQuery(document).ready(function () {
    WizardDemo.init();
});