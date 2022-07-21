document.addEventListener('readystatechange', doc => {

    if (doc.target.readyState === 'complete') {
        var questiontypeid = parseInt($('.selQuestionType').val());
       bindTest('.selTest');
        bindQuestionType('.selQuestionType');
        $('#mcqoptions').hide();
        $('#trueorfalse').hide();
        $('#dropdown').hide();
       
        formatSection([]);

        //bind to section dropdown upon test select change
        $('.selTest').on('change', testF => {
            currenttestid = testF.target.value;
            getdata('/tutor/GetTestSectionByTestId?testid=' + $('.selTest').val()).done(res => {
                formatSection(res);
                console.log('SelTest: ' + testF.target.value);
            });
            $('.selQuestionType').val(currentquestiontypeid).change()
        });
        //CKEDITOR.instances.editor1.create({
        //    ckfinder: {
        //        uploadUrl: '/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Files&responseType=json',
        //    },
        //    toolbar: ['ckfinder', 'imageUpload', '|', 'heading', '|', 'bold', 'italic', '|', 'undo', 'redo']
        //})
        tinymce.init({
            selector: 'textarea#editor1',
            height: 350,
            contextmenu: "paste | copy | link image inserttable | cell row column deletetable ",
            insert_toolbar: 'quickimage quicktable',
            //language: "ta_IN",
            language_url: '/tinymce/langs/ta_IN.js',
            //directionality: "ltl",
            menubar: true,
            images_upload_url: '/tutor/questionimageupload',
            automatic_uploads: false,
            plugins: [
                'advlist autolink lists link image charmap print preview anchor',
                'searchreplace visualblocks code fullscreen',
                'insertdatetime imagetools media table paste code help wordcount'
            ],
            entity_encoding: "raw",
            toolbar: 'insertfile undo redo superscript subscript',

            //tinymce change event
            setup: function (ed) {
                ed.on('keyup', fun => {
                    questiontypeid = parseInt($('.selQuestionType').val());
                    switch (questiontypeid) {
                        case QuestionType.MCQ: {
                            let ans = [];
                            let textStr = tinyMCE.activeEditor.getContent({ format: 'html' }).split('{');
                            $('#mcqoptions .labels').empty();
                            if (textStr.length > 0) {
                                textStr.forEach(el => {
                                    if (el.indexOf('}') > 0) {
                                        ans.push(el.match("(.*)}")[1]);
                                        //$('#mcqoptions .options').append('</label>').html(el.match("(.*)}")[1]);
                                        //$('#mcqoptions .labels').append('<label class="alert alert-info">' + el.match("(.*)}")[1] + '</label>');
                                        if (el.match("(.*)}")[1] === qusOptions.filter(f => f.isCorrect)[0].option)
                                        $('#mcqoptions .labels').append('<label class="alert alert-info">' +
                                            el.match("(.*)}")[1] + '<sup class="sup-checkbox"><input class="checkbox" checked type="checkbox"/></sup></label>');
                                        else
                                            $('#mcqoptions .labels').append('<label class="alert alert-info">' +
                                                el.match("(.*)}")[1] + '<sup class="sup-checkbox"><input class="checkbox" type="checkbox"/></sup></label>');
                                    }
                                });
                                //options.empty().select2({ data: ans });
                            }
                            break;
                        }
                        case QuestionType["Match the following"]: {
                            //correctopts.forEach(el => {
                            //    el = el.option ?? '';
                            //    tr = $(
                            //        `<tr>
                            //           <td>${el.substring(0, el.lastIndexOf('{'))}</td>
                            //           <td><input class="ans" value='${el.substring(el.lastIndexOf('{') + 1, el.lastIndexOf('}'))}'/></td>
                            //         </tr>`
                            //    );
                            //    $('#matching .table tbody').append(tr);

                            //});
                            break;
                        }
                        case QuestionType["Gap Filling"]:
                        case QuestionType["One word answer"]:
                            {
                                var textStr = tinyMCE.activeEditor.getContent({ format: 'html' }).split('{');
                                let ans = [];
                                $('#mcqoptions .labels').empty()
                                $('#mcqoptions .options').empty()

                                if (textStr.length > 0) {
                                    textStr.forEach(el => {
                                        if (el.indexOf('}') > 0) {
                                            ans.push(el.match("(.*)}")[1]);
                                            //$('#mcqoptions .options').append('</label>').html(el.match("(.*)}")[1]);
                                            $('#mcqoptions .labels').append('<label class="alert alert-info">' + el.match("(.*)}")[1] + '</label>');
                                        }
                                    });
                                }
                              
                               // $('#mcqoptions .options select').select2({ data: ans }).val(ans).change();
                                //$('#mcqoptions .ans select').select2({ data: ans }).val(ans).change();
                                break;
                            }
                        case QuestionType["True or False"]: {
                            //var textStr = tinyMCE.activeEditor.getContent({ format: 'text' }).split('{');
                            //let ans = [];
                            //if (textStr.length > 0) {
                            //    textStr.forEach(el => {
                            //        if (el.indexOf('}') > 0) {
                            //            ans.push(el.match("(.*)}")[1]);
                            //        }
                            //    });
                            //}
                            //$('#mcqoptions .options select').select2({ data: ans }).val(ans).change();
                            //$('#mcqoptions .ans select').select2({ data: ans });
                            break;
                        }
                        case QuestionType["Re-Arrange"]: {
                            var textStr = tinyMCE.activeEditor.getContent({ format: 'html' }).split('{');
                            let ans = [];
                            $('#mcqoptions .labels').empty()
                            $('#mcqoptions .options').empty() 
                            

                            if (textStr.length > 0) {
                                textStr.forEach(el => {
                                    if (el.indexOf('}') > 0) {
                                        ans.push(el.match("(.*)}")[1]);
                                    }
                                });
                                //correctoption.empty().select2({'data': ans });
                                //$('#mcqoptions .labels').append('<label class="alert alert-info">' + el.match("(.*)}")[1] + '</label>');
                                console.log('re-arrange value:',ans)
                            }
                            break;
                        }
                        case QuestionType.Dropdown:
                            {
                                currentquestiontypeid = questiontypeid;
                                var textStr = tinyMCE.activeEditor.getContent({ format: 'text' }).split('{');
                                let ans = [];
                                if (textStr.length > 0) {
                                    textStr.forEach(el => {
                                        if (el.indexOf('}') > 0) {
                                            ans.push(el.match("(.*)}")[1]);
                                        }
                                    });
                                }
                                $('#dropdown').empty();
                                ans.forEach(function (el, i) {
                                    var ddl = $('<select id="selDropdown' + i + '"></select>');
                                    console.log('drop: ', ddl, el.split(','));
                                    $('#dropdown').append(ddl);
                                    $('#selDropdown' + i).select2({ data: el.split(','), minimumResultsForSearch: -1 }).val(el.split(','))
                                });
                                break;
                            }
                    }
                })
            },
            images_upload_handler: function (blobInfo, success, failure) {
                var image_size = blobInfo.blob().size / 1000;  // image size in kbytes
                var max_size = max_size_value                // max size in kbytes
                if (image_size > max_size) {
                    failure('Image is too large( ' + image_size + ') ,Maximum image size is:' + max_size + ' kB');
                    return;
                } else {
                    // Your code
                }
            }
        });

        var options = $('#mcqoptions .options select');

        var correctoption = $('#mcqoptions .ans select');

        //select2 order item issue fix
        correctoption.on("select2:select", function (evt) {
            var element = evt.params.data.element;
            var $element = $(element);

            $element.detach();
            $(this).append($element);
            //$(this).trigger("change");
        })

        //option select change function
        $('.selQuestionType').change(fun => {
            currentquestiontypeid = fun.target.value;
            $('#matching, .mcqoption-options .labels, #dropdown').hide();

            var s2correct = correctoption.select2({
                tags: true,
                placeholder: "Enter correct option/s here...",
                allowClear: false,
                minimumResultsForSearch: -1,
                data: options.val()
            });
            var questiontypeID = parseInt(fun.target.value);
            //CKEDITOR.instances.editor1.on('change');

            //bind correct answers to select2
            /*if (questiontypeID === QuestionType.MCQ || questiontypeID === QuestionType["Gap Filling"] || questiontypeID === QuestionType. || fun.target.value === '3') {*/
            $('#mcqoptions').show('slow');
            $('#mcqoptions .ans').fadeIn('slow');
            $
            //$('#mcqoptions .mcqoption-options').show();

            //check if the question is new or edit
            if (opts.length > 0) {
                //opts = [];
            } else {
                opts = options.val();
                //clear the options on select change event
                options.empty().select2().val(null).change();
                correctoption.empty();
                s2correct.select2('destroy').select2();
            }
            i = 0;
            console.log('selectedQuestionTypeID', questiontypeID);
            //switch different type of questions
            switch (questiontypeID) {
                case QuestionType.MCQ: {
                    $('#mcqoptions .labels').empty();
                    $('.options select, .ans, .options, #dropdown').hide();
                    $('.mcqoption-options,.labels').fadeIn('slow');
                    qusOptions.forEach(el => {
                        $('#mcqoptions .labels').append('<label class="alert alert-info">' +
                            el.option + '<sup class="sup-checkbox"><input class="checkbox" '
                            + (el.isCorrect ?  'checked': null) + ' type="checkbox"/></sup></label>');
                    })
                    break;
                }
                case QuestionType["One word answer"]:
                    {
                        $('#mcqoptions .ans,.options').hide();
                        $('.labels').empty();
                        $('.mcqoption-options,.labels').fadeIn('slow');
                        qusOptions.forEach(el => {
                            $('#mcqoptions .labels').append('<label class="alert alert-info">' +
                                el.option + '<sup class="sup-checkbox"><input class="checkbox" ' + (el.isCorrect ? 'checked' : null) + ' type="checkbox"/></sup></label>');
                        })
                        //if (questiontypeID == 5)
                        //    $('#mcqoptions .ans').hide('slow');
                        if (questiontypeID == 6) {
                            correctoption.on("select2:select", function (evt) {
                                tinyMCE.activeEditor.setContent('Ans: <br/><br/>Instruction: ');
                            });
                        }
                        $('#mcqoptions div .labels label').toArray().forEach(function (fun, i) {
                            $(fun).find('.sup-checkbox').remove().end().html();
                        });
                        break;
                    }
                case QuestionType["Re-Arrange"] :
                    {

                        $('#mcqoptions .ans,.mcqoption-options').show('slow');
                        $('.labels, .mcqoption-options').hide();
                        options.select2({
                            tags: true,
                            placeholder: "Enter your options here...",
                            allowClear: true,
                            disabled: false,
                        }).on('select2:select change', fun => {
                            correctoption.select2({ minimumResultsForSearch: -1, allowClear: false, data: options.val() });
                        }).on('select2:clear', sclear => {
                            s2correct.empty().change();
                        }).on('select2:unselect', ucf => {
                            correctoption.empty();
                            correctoption.select2({ minimumResultsForSearch: -1, allowClear: false, data: options.val() });
                        });
                        break;
                    }
                case 2:
                    {
                        $(' #mcqoptions .ans, #mcqoptions .options').hide();
                        $('#mcqoptions, #mcqoptions .mcqoption-options, #mcqoptions .labels').show('slow');
                        options.select2({
                            tags: false,
                            placeholder: null,
                            allowClear: false,
                            disabled: true
                        }).on('select2:select change', fun => {

                            correctoption.select2({ data: opts });
                        })

                        $('#mcqoptions .labels').empty()
                       
                        opts.forEach(op => {
                            $('#mcqoptions .labels').append('<label class="alert alert-info">' + op + '</label>');
                        })
                        break;
                    }
                case QuestionType["Match the following"]:
                    {
                        $('#mcqoptions').fadeIn('slow');
                        $('#mcqoptions .mcqoption-options, .labels, .ans').hide();
                        $('#matching').show('slow');

                        setTimeout(() => { options.change() }, 1000);
                        options.select2({
                            tags: true,
                            placeholder: 'Type the sentence here...',
                            allowClear: true,
                            disabled: false
                        }).on('select2:select change', opt => {
                            let tr = '';
                            if (questiontypeID == QuestionType["Match the following"]) {
                                $('#matching .table tbody').empty();
                                //format unformated options
                                if (correctopts.length === 0) {
                                    //$('#mcqoptions .options select').val().forEach(el => {
                                    //    el = el;
                                    //    tr = $(
                                    //        `<tr>
                                    //        <td><input class="qus" value='${el}' /></td>
                                    //        <td><input class="ans" value=''/></td>
                                    //    </tr>`
                                    //    );
                                    //    $('#matching .table tbody').append(tr);

                                    //});
                                    tr = $(
                                            `<tr>
                                            <td><input class="qus" value='' /></td>
                                            <td><input class="ans" value=''/></td>
                <td><button type="button" class="btn-link btnMatchingAdd" onclick="addRow(this)"><i class="fa fa-plus-circle text-success"></i></button>
                <button type="button" class="btn-link btnMatchingDelete" onclick="deleteRow(this,'#matching')"><i class="fa fa-times-circle text-danger"></i></button>
                </td>
                                        </tr>`
                                        );
                                        $('#matching .table tbody').append(tr);
                                } else {

                                    console.log(correctopts)
                                    Object.keys(correctopts).forEach(el => {
                                        tr = $(
                                            `<tr>
                                            <td><input class="qus" value="${el}"/></td >
                                            <td><input class="ans" value='${correctopts[el]}'/></td>
                <td><button type="button" class="border-0 btnMatchingAdd" onclick="addRow(this)" ><i class="fa fa-plus-circle text-success"></i></button> 
                <button type="button" class="border-0 btnMatchingDelete" onclick="deleteRow(this,'#matching')"><i class="fa fa-times-circle text-danger"></i></button>
                </td>
                                        </tr>`
                                        );
                                        $('#matching .table tbody').append(tr);

                                    });

                                }
                            }

                        });
                        break;
                    }
                case QuestionType["True or False"]:
                    {
                        opts = opts.length > 0 ? opts : [{ 'id': 'True', 'text': 'True' }, { 'id': 'False', 'text': 'False' }, { 'id': 'None', 'text': 'None' }];

                        $('.mcqoption-options').fadeIn('slow');
                        options.select2({
                            tags: true,
                            data: opts,
                            placeholder: "Type your correct options here...",
                            allowClear: false,
                            maximumSelectionLength: 3,
                            disabled: false
                        }).on('select2:select change', fun => {
                            opts = options.select2('val');

                            $('.ans select').select2({ data: opts }).val(correctoption);
                            //$(fun.target).select2('data', opts)
                        }).on('select2:unselect', fun => {
                            opts = options.select2('val');
                            $('.ans select').empty().select2({ data: opts }).val(correctoption);

                        });
                        options.val(opts).change();
                        correctoption.val(correctopts);
                        break;
                    }
                case QuestionType.Dropdown: {
                    $('#dropdown').fadeIn('slow');
                    $('#mcqoptions').hide();
                    break;
                }
                default: {
                    $('#mcqoptions').hide();
                    break;
                }

            }
            correctoption.on('select2:select', el => {

                el.target
            })
            tinymce.activeEditor.delegates.keyup();
            //}
        });

        //form validator
        //$('select').on('change', fun => {
        //    setTimeout(t => {
        //        formvalidator();

        //    }, 2000)
        //});
    }
});

var opts = [];
var correctopts = [];
var qusOptions = [];

let currenttestid = 0;
let currentquestiontypeid = 0;
let currenctsectionid = 0;
function deleteRow(cnt, selector) {
    if ($(selector).find('tr').length > 1) {
        $(cnt).closest('tr').remove();
    }
}
function addRow(cnt) {
    var $tr = $(cnt).closest('tr');
    var $clone = $tr.clone();
    $clone.find(':text').val('');
    $tr.after($clone);
}
function savequestion(cnt) {
    var questiontype = $('.selQuestionType').val();

    var isvalid = formvalidator();
    if (!isvalid)
        return undefined;

    var answers = [];
    var model = {};
    switch (parseInt(questiontype)) {
        case QuestionType.MCQ: {
            $('#mcqoptions div .labels label').toArray().forEach(function (fun, i) {
                answers.push({
                    Option: $(fun).clone().find('.sup-checkbox').remove().end().html(),
                    Position: i,
                    IsCorrect: $(fun).find('input[type=checkbox]').is(':checked')
                });
            });
            if (answers.filter(fi => { return fi.IsCorrect }).length == 0 && $(cnt).data('save') === 3) {
                notify('MCQ question must contain atleast one option to be correct.','warning',4000);
                return false;
            }
            require('../js/models/questionsModel')
            var m = new Model()
            model = {
                QusID: parseInt($('[name=QusID]').val()) || 0,
                QuestionName: tinyMCE.activeEditor.getContent(),
                QuestionTypeId: questiontype,
                TestId: $('.selTest').val(),
                SectionId: parseInt($('#selSection').val()) || 0,
                Mark: $('#txtMarks').val(),
                Options: answers,
                CorrectOption: JSON.stringify(answers.map(el => el.Option)),
                StatusId: $(cnt).data('save'),
                Topic: $('#txtTopic').val(),
                SubTopic: $('#txtSubTopic').val()
            }
            break
        }


        case QuestionType["Match the following"]:
            {
                var correctoption = [];
                //for matching type question option params
                $('#matching table tr').toArray().forEach(function (fun, i) {
                    correctoption.push($(fun).find('.qus').eq(0).val() + '":"' + $(fun).find('.ans').val())
                    return answers.push({
                        Option: $(fun).find('.ans').val(),
                        IsCorrect: true,
                        Position: i,
                        //CorrectAnswer: $(fun).find('td input').val()
                    });
                });
                model = {
                    QusID: parseInt($('[name=QusID]').val()) || 0,
                    QuestionName: tinyMCE.activeEditor.getContent() ?? 'Matching',
                    QuestionTypeId: questiontype,
                    TestId: $('.selTest').val(),
                    SectionId: $('#selSection').val(),
                    Topic: $('#txtTopic').val(),
                    SubTopic: $('#txtSubTopic').val(),
                    Mark: $('#txtMarks').val(),
                    Options: answers,
                    CorrectOption: '[{"' + correctoption.join('","') + '"}]',
                    StatusId: $(cnt).data('save')
                }
                break;
            }

        case QuestionType.Dropdown:
            {
                var textStr = tinyMCE.activeEditor.getContent({ format: 'text' }).split('{');
                if ($('#dropdown select').toArray().filter(el => el.value == 'select').length > 0 && $(cnt).data('save') === 3) {
                    notify('Please check you have selected correct answers for all options.', 'info', 2000);
                    return false;
                }
                if (textStr.length > 0) {
                    textStr.forEach(function (el, i) {
                        if (el.indexOf('}') > 0) {
                            answers.push({
                                Option: el.match("(.*)}")[1],
                                Position: i
                            });
                        }
                    });
                }
                model = {
                    QusID: parseInt($('[name=QusID]').val()) || 0,
                    QuestionName: tinyMCE.activeEditor.getContent(),
                    QuestionTypeId: questiontype,
                    TestId: $('.selTest').val(),
                    SectionId: $('#selSection').val(),
                    Mark: $('#txtMarks').val(),
                    Options: answers,
                    CorrectOption: '["' + $('#dropdown select').toArray().map(el => el.value).join('","') + '"]',
                    StatusId: $(cnt).data('save'),
                    Topic: $('#txtTopic').val(),
                    SubTopic: $('#txtSubTopic').val()
                }
                break;
            }

        case QuestionType["Re-Arrange"]: case QuestionType["True or False"]:
            {
                $('#mcqoptions .ans select').select2().val().forEach(function (fun, i) {
                    answers.push({
                        Option: fun,
                        Position: i,
                        IsCorrect: $('#mcqoptions .ans select').val()
                            == fun
                    });
                });
                model = {
                    QusID: parseInt($('[name=QusID]').val()) || 0,
                    QuestionName: tinyMCE.activeEditor.getContent(),
                    QuestionTypeId: questiontype,
                    TestId: $('.selTest').val(),
                    SectionId: parseInt($('#selSection').val()) || 0,
                    Mark: $('#txtMarks').val(),
                    Options: answers,
                    CorrectOption: '["' + $('#mcqoptions .ans select').val().join('","') + '"]',
                    StatusId: $(cnt).data('save'),
                    Topic: $('#txtTopic').val(),
                    SubTopic: $('#txtSubTopic').val()
                }
                break;
            }

        default:
            //for gap filling type question option params
            $('#mcqoptions div .labels label').toArray().forEach(function (fun, i) {
                answers.push({
                    Option: fun.innerHTML,
                    Position: i,
                    IsCorrect: $('#mcqoptions .ans select').val()
                        .filter(el => {
                            return el === fun.textContent
                        }).length > 0
                });
            });
            model = {
                QusID: parseInt($('[name=QusID]').val()) || 0,
                QuestionName: tinyMCE.activeEditor.getContent(),
                QuestionTypeId: questiontype,
                TestId: $('.selTest').val(),
                SectionId: parseInt($('#selSection').val()) || 0,
                Mark: $('#txtMarks').val(),
                Options: answers,
                CorrectOption: '["' + $('#mcqoptions .ans select').val().join('","') + '"]',
                StatusId: $(cnt).data('save'),
                Topic: $('#txtTopic').val(),
                SubTopic: $('#txtSubTopic').val()
            }
    }

    console.log(model)
    initLoader();
    insertdata('/Tutor/SaveQuestion', { model: model }).done(res => {
        removeLoader();

        if (res == 'ok') {
            clearformvalues('#frmCreateExam', false);
            var isupdate = (model.QusID !== 0 && model.QusID !== undefined && model.QusID !== null);
            notify(`Question ${isupdate ? 'updated' : 'added'} successfully.`, 'success', 4000);
            if (isupdate)
                $('#myModal2').modal('hide');
        }
        else
            notify(res, 'warning', 2000)
        console.log(res);
    }).fail(status => {
        removeLoader();
    });
}

var QuestionType =
    { 'MCQ': 1, 'Gap Filling': 2, 'Match the following': 3, 'True or False': 4, 'One word answer': 5, 'Re-Arrange':6,'Dropdown':7 }


function formvalidator() {
    var formcnt = $("#frmCreateExam").find('select,input,textarea').not('.select2-hidden-accessible,.select2-search__field');
    if ($('.selQuestionType').val() !== '1' || $('.selQuestionType').val() !== '2') {
        formcnt = formcnt.not('.ans select,.options select')
    }
        var isvalid = true;
    formcnt.toArray().forEach(function (el, i) {
        $(el).next('.validator-1').remove().end();
        var h = document.createElement('span');
        h.textContent = '*';
        h.className = 'validator-1 text-warning';
        if ((el.value == undefined || el.value == "" || el.value.length == 0) && $(el).attr('required') === 'required'
            && el.tagName.toLowerCase() !== 'textarea') {
            isvalid = false;
            $(el).after(h).end();
        }
        if (el.tagName === 'textarea') {
            if ($(el).text() === null || $(el).text() === undefined || $(el).text()==="") {
                isvalid = false;
                $(el).after(h).end();
            }
        }
    });
    $('.selTest').val(currenttestid);
    $('.selQuestionType').val(currentquestiontypeid);
    $('#selSection').val(currenctsectionid);
        return isvalid;
}

function clearformvalues(form, clearDefaultSelection = true) {
    
    $('#mcqoptions .labels').empty();
    $('.mcqoption-options').hide();
    $('#dropdown').empty().hide();
    $('#pnlShowMore').hide();
    $(form).find('input,select,textarea').not('.select2-hidden-accessible,.select2-search__field').toArray().forEach(fun => {
        if (clearDefaultSelection) {
            if (fun.tagName.toLowerCase() === 'select') {
                $(fun).val($(fun).find('option:first').val());
            } else {
                fun.value == "";
            }
            if (fun.id == 'selSection') {
                formatSection([]);
            }
            currenttestid = '';
            currenctsectionid = '';
            currentquestiontypeid = '';
        } 
    })
    if (tinyMCE.activeEditor !== null) {
        tinyMCE.activeEditor.setContent('');
    }
    $('#mcqoptions .options div').empty()
    if ($('select').length > 0)
        $('.options select').empty();
    $('.ans select').empty();
    $('#mcqoptions').hide();
    $('#matching table tbody tr').remove();
    $('[name=QusID]').val('');
    $('.validator-1').remove();
    correctopts = [];
    qusOptions = [];
    //$(form).find('input,select,textarea').change();
}

function question_popup(cnt) {
    //clearformvalues('#frmCreateExam');
   
    var questionid = parseInt($(cnt).data('questionid'))||0;
    $('[name=QusID]').val(questionid);  
    $('#myModal2').modal('show');
    let testid = 0;
    let questiontypeid = 0;
    if (questionid > 0) {
        initLoader();
        insertdata('/tutor/GetQuestionDetails', { QuestionId: questionid }).done(res => {
             testid = parseInt(res.testId)
            questiontypeid = parseInt(res.questionTypeId);
            qusOptions = res.options;
            $('#txtMarks').val(res.mark);
            $('#txtTopic').val(res.topic);
            $('#txtSubTopic').val(res.subTopic);
            tinyMCE.activeEditor.setContent(res.questionName);
            opts = []; correctopts = [];
            if (questiontypeid == QuestionType["Match the following"]) {
                correctopts = JSON.parse(res.correctOption)[0];
            } else  {
                correctopts = JSON.parse(res.correctOption);
                res.options.forEach(op => {
                    opts.push(op.option);
                });
            } 
            //res.options.forEach(function (el, i) {

            //    //format the matching type question options
            //    if (questiontypeid == QuestionType["Match the following"]) {
            //        opts.push(el.option);
            //        correctopts.push(el);
            //    }
            //    else {
            //        opts.push(el.option);
            //    }
            //    if (el.isCorrect)
            //        correctopts.push(el.option);
            //});
            $('#mcqoptions .options select').select2({ data: opts }).val(opts).change();
            $('#mcqoptions .ans select').select2({ data: opts }).val(correctopts).change();

            currenttestid = res.testId;
            currentquestiontypeid = res.questionTypeId;
            currenctsectionid = res.sectionId;
            console.log(res, correctopts, questiontypeid, testid);
            $('.selTest').val(testid.toString()).change()
            
        });
        removeLoader();
    }
   
}
function formatSection(res) {
    $('#selSection').empty();
    dt = [];
    
        dt.push({
            html: `<div class="row">
                       <span class="col-md-5 small">No section</span>
                        <span class="col-md-4 small">N/A</span>
                        <em class="col-md-3 small text-white-50">N/A</em>
                    </div>`,
            id: 0,
            text: 'No section',
            title: 'No section'
        });
    if (res.length == 0) {
        $('#selSection').select2({
            'data': dt,
            height: '100%',
            width:'100%',
            templateResult: function (data) {
                return $(data.html);
            },
            templateSelection: function (data) {
                return data.text;
            },
            dropdownAutoWidth: true
        });
        return dt;
    }
       
    res.forEach(el => {
        dt.push({
            html: `<div class="row">
                        <span class="col-md-5 small">${el.sectionName}</span>
                        <span class="col-md-4 small">${el.topic??'N/A'}</span>
                        <em class="col-md-3 small text-white-50">${el.subTopic??'N/A'}</em>
                    </div>`,
            id: el.id,
            text: el.sectionName,
            title: el.sectionName
        });
       
       
        $('#selSection').select2({
            'data': dt,
            height: '100%',
            width:'20%',
            templateResult: function (data) {
                return $(data.html);
            },
            templateSelection: function (data) {
                return data.text;
            },
            dropdownAutoWidth: true
        });
        //return dt;
    })
    //$('#selSection').val(currenctsectionid).change();
};

function deleteConfirmation(url) {
    $('#modalConfirm').modal('show');
    $('#modalConfirm #btnDelete').click(fun => {
        window.location.href = url;
    })
}

class 



