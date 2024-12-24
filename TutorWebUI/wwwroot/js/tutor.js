document.addEventListener('readystatechange', doc => {

    if (doc.target.readyState === 'complete') {
        require('/js/models/questionsModel.js');
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
           
            if (currenttestid > 0)
                getdata('/tutor/GetTopicsByTestId', { testid: currenttestid }).done(res => {
                    let option = document.getElementById('selTopic');
                    option.innerHTML = '';
                    option.appendChild(new Option("Select Topic", '0', true));
                    res.forEach(op => {
                        option.appendChild(new Option(op.Topic, op.id, false));
                    })
                    option.value = currenttopicId;
                    option.onchange.apply();
                })
            getdata('/tutor/GetTestSectionByTestId?testid=' + $('.selTest').val()).done(res => {
                formatSection(res);
               
                if (currentquestiontypeid != undefined && currentquestiontypeid != 0)
                    $('.selQuestionType').val(currentquestiontypeid).change();
                if (currenctsectionid != undefined && currenctsectionid != 0)
                    $('#selSection').val(currenctsectionid).change();
            });
        });

        document.getElementById('chkComp').addEventListener('click', function (et) {
        })

        tinymce.init({
            selector: 'div#editor3',
            height: 250,
            contextmenu: "paste | copy | link image inserttable | cell row column deletetable ",
            insert_toolbar: 'quickimage quicktable',
            //language: "ta_IN",
            language_url: '/tinymce/langs/ta_IN.js',
            //directionality: "ltl",
            menubar: true,
            //images_upload_url: '/tutor/questionimageupload',
            automatic_uploads: false,
            plugins: [
                'advlist autolink lists link image charmap print preview anchor',
                'searchreplace visualblocks code fullscreen',
                'insertdatetime imagetools media table paste code help wordcount'
            ],
            entity_encoding: "raw",
            toolbar: 'insertfile undo redo superscript subscript',

            //tinymce change event
            
        })

        tinymce.init({
            selector: 'div#editor2',
            height: 250,
            contextmenu: "paste | copy | link image inserttable | cell row column deletetable ",
            insert_toolbar: 'quickimage quicktable',
            //language: "ta_IN",
            language_url: '/tinymce/langs/ta_IN.js',
            //directionality: "ltl",
            menubar: true,
            //images_upload_url: '/tutor/questionimageupload',
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

                    let ans = [];
                    let textStr = tinyMCE.get('editor2').getContent({ format: 'html' }).split('{');
                    $('#mcqoptions .labels').empty();

                    
                    if (textStr.length > 0) {
                        textStr.forEach(el => {
                            if (el.indexOf('}') > 0) {
                                ans.push(el.match("(.*)}")[1]);
                                //$('#mcqoptions .options').append('</label>').html(el.match("(.*)}")[1]);
                                $('#mcqoptions .labels').append('<label class="alert alert-info">' +
                                    el.match("(.*)}")[1] + '<sup class="sup-checkbox"><input class="checkbox" '
                                    + (qusOptions.filter(c => c.Option.trim() == el.match("(.*)}")[1].trim())[0]?.IsCorrect ? 'checked' : null) + ' type="checkbox"/></sup></label>');
                            }
                        })
                        options.empty().select2({ data: ans });

                    }
                });
            }
        })

        //CKEDITOR.instances.editor1.create({
        //    ckfinder: {
        //        uploadUrl: '/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Files&responseType=json',
        //    },
        //    toolbar: ['ckfinder', 'imageUpload', '|', 'heading', '|', 'bold', 'italic', '|', 'undo', 'redo']
        //})

        tinymce.init({
            selector: 'textarea#editor1',
            height: 200,
            contextmenu: "paste | copy | link image inserttable | cell row column deletetable ",
            insert_toolbar: 'quickimage quicktable',
            //language: "ta_IN",
            language_url: '/tinymce/langs/ta_IN.js',
            //directionality: "ltl",
            menubar: true,
            //images_upload_url: '/tutor/questionimageupload',
            automatic_uploads: false,
            autoresize_on_init: true,
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
                            //let ans = [];
                            //let textStr = tinymce.editors.editor2.getContent({ format: 'html' }).split('{');
                            //$('#mcqoptions .labels').empty();
                            //if (textStr.length > 0) {
                            //    textStr.forEach(el => {
                            //        if (el.indexOf('}') > 0) {
                            //            ans.push(el.match("(.*)}")[1]);
                            //            //$('#mcqoptions .options').append('</label>').html(el.match("(.*)}")[1]);
                            //            //$('#mcqoptions .labels').append('<label class="alert alert-info">' + el.match("(.*)}")[1] + '</label>');
                            //            if (el.match("(.*)}")[1] === qusOptions.filter(f => f.isCorrect)[0]?.option)
                            //                $('#mcqoptions .labels').append('<label class="alert alert-info">' +
                            //                    el.match("(.*)}")[1] + '<sup class="sup-checkbox"><input class="checkbox" checked type="checkbox"/></sup></label>');
                            //            else
                            //                $('#mcqoptions .labels').append('<label class="alert alert-info">' +
                            //                    el.match("(.*)}")[1] + '<sup class="sup-checkbox"><input class="checkbox" type="checkbox"/></sup></label>');
                            //        }
                            //    })
                            //    //options.empty().select2({ data: ans });
                            //}
                            break;
                        }
                        case QuestionType["Match the following"]: {
                            if (tinyMCE.editors.editor1.getContent() == '') {
                                tinyMCE.editors.editor1.setContent('Match the following.')
                            }
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
                                var textStr = tinyMCE.get('editor1').getContent({ format: 'html' }).split('{');
                                let ans = [];
                                $('#mcqoptions .labels').empty()
                                //$('#mcqoptions .options').empty()

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
                        //case QuestionType["True or False"]: {
                        //    //var textStr = tinyMCE.activeEditor.getContent({ format: 'text' }).split('{');
                        //    //let ans = [];
                        //    //if (textStr.length > 0) {
                        //    //    textStr.forEach(el => {
                        //    //        if (el.indexOf('}') > 0) {
                        //    //            ans.push(el.match("(.*)}")[1]);
                        //    //        }
                        //    //    });
                        //    //}
                        //    //$('#mcqoptions .options select').select2({ data: ans }).val(ans).change();
                        //    //$('#mcqoptions .ans select').select2({ data: ans });
                        //    break;
                        //}
                        case QuestionType["Re-Arrange"]: {
                            var textStr = tinyMCE.editors.editor1.getContent({ format: 'html' }).split('{');
                            let ans = [];
                            $('#mcqoptions .labels').empty()
                            //$('#mcqoptions .options').empty()
                            if (tinyMCE.editors.editor1.getContent() == '')
                                tinyMCE.editors.editor1.setContent('Re-arrange the following question.');
                            break;
                        }
                        case QuestionType.Dropdown:
                            {
                                currentquestiontypeid = questiontypeid;
                                var textStr = tinyMCE.editors.editor1.getContent({ format: 'text' }).split('{');
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
                                    $('#dropdown').append(ddl);
                                    $('#selDropdown' + i).select2({ data: el.split(','), minimumResultsForSearch: -1 }).val(el.split(','))
                                });
                                correctopts.forEach(function (el, i) {
                                    $('#dropdown select').eq(i).val(el).change();
                                })
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
            $('#matching, .mcqoption-options .labels, #dropdown, #editor2, .ans, .comper-question').hide();
            tinymce.editors.editor2.hide()
            var s2correct = correctoption.select2({
                tags: true,
                placeholder: "Enter correct option/s here...",
                allowClear: false,
                minimumResultsForSearch: -1,
                data: options.val()
            });
            var questiontypeID = parseInt(fun.target.value);
            //CKEDITOR.instances.editor1.on('change');

            $('#mcqoptions').show('slow');
            $('#mcqoptions .ans').fadeIn('slow');
            i = 0;

            //switch different type of questions
            switch (questiontypeID) {
                case QuestionType.MCQ: {
                    $('#mcqoptions .labels').empty();
                    $('.options select, .ans, .options, #dropdown').hide();
                    tinymce.editors.editor2.show('slow')
                    $('.mcqoption-options,.labels').fadeIn('slow');
                    tinymce.get('editor2').setContent('{ ' + opts.join(' } { ') + ' } ');
                    break;
                }
                case QuestionType["One word answer"]:
                    {
                        $('#mcqoptions .ans,.options').hide();
                        $('.labels').empty();
                        $('.mcqoption-options,.labels').fadeIn('slow');
                        qusOptions.forEach(el => {
                            $('#mcqoptions .labels').append('<label class="alert alert-info">' +
                                el.option + '<sup class="sup-checkbox"><input class="checkbox" ' + (el.IsCorrect ? 'checked' : null) + ' type="checkbox"/></sup></label>');
                        })
                       
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
                case QuestionType["Re-Arrange"]:
                    {

                        $('#mcqoptions .ans,.mcqoption-options').show('slow');
                        $('.labels, .mcqoption-options').hide();
                        options.select2({
                            tags: true,
                            placeholder: "Enter your options here...",
                            allowClear: true,
                            disabled: false,
                            maximumSelectionLength: 300
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
                case QuestionType['Gap Filling']:
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
                        //setTimeout(() => { options.change() }, 1000);
                        //options.select2({
                        //    tags: true,
                        //    placeholder: 'Type the sentence here...',
                        //    allowClear: true,
                        //    disabled: false
                        //}).on('select2:select change', opt => {
                        //    let tr = '';
                            if (questiontypeID == QuestionType["Match the following"]) {
                                $('#matching .table tbody').empty();
                                //format unformated options
                                if (correctopts != null && correctopts != undefined && correctopts.length === 0) {
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

                                    correctopts.forEach(el => {
                                        tr = $(
                                            `<tr>
                                            <td><input class="qus" value="${Object.keys(el)}"/></td >
                                            <td><input class="ans" value='${Object.values(el)}'/></td>
                <td><button type="button" class="border-0 btnMatchingAdd" onclick="addRow(this)" ><i class="fa fa-plus-circle text-success"></i></button> 
                <button type="button" class="border-0 btnMatchingDelete" onclick="deleteRow(this,'#matching')"><i class="fa fa-times-circle text-danger"></i></button>
                </td>
                                        </tr>`
                                        );
                                        $('#matching .table tbody').append(tr);

                                    });

                                }
                            }

                        //});
                        break;
                    }
                case QuestionType["True or False"]:
                    {
                        opts = opts.length > 0 ? opts : [{ 'id': 'True', 'text': 'True' }, { 'id': 'False', 'text': 'False' }, { 'id': 'None', 'text': 'None' }];

                        $('.mcqoption-options, .options').fadeIn('slow');
                        options.select2({
                            tags: true,
                            //data: opts,
                            placeholder: "Type your correct options here...",
                            allowClear: false,
                            maximumSelectionLength: 3,
                            disabled: false
                        })
                            .on('select2:select change', fun => {
                                opts = options.select2('val');

                                $('.ans select').select2({ data: opts });
                                //$(fun.target).select2('data', opts)
                            })
                        //    .on('select2:unselect', fun => {
                        ////    opts = options.select2('val');
                        //   $('.ans select').empty().select2({ data: opts }).val(correctoption);

                        //});
                        //options.val(opts).change();
                        correctoption.val(correctopts);
                        break;
                    }
                case QuestionType.Dropdown: {
                    $('#dropdown').fadeIn('slow');
                    $('#mcqoptions').hide();
                   
                    break;
                }
                case QuestionType.ComphresionAnswer: {
                    $('.comper-question').fadeIn('slow');
                }
                default: {
                    $('#mcqoptions').hide();
                    break;
                }

            }

            tinymce.activeEditor.delegates.keyup();
            tinymce.editors.editor2.delegates.keyup();

            //}
        });
        $('#selSection').change(func => {
            currenctsectionid = func.target.value;
        })
        $('#selSubTopic').val(currentsubtopicid).change();

    }
});

var opts = [];
var correctopts = [];
var qusOptions = [];
var comprehensionQuestion = null;
let currenttestid = 0;
let currentquestiontypeid = 0;
let currenctsectionid = 0;
let currentquestionId = 0;
let currenttopicId = 0;
let currentsubtopicid = 0;

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
    currentquestionId = $('[name=QusID]').val();
    currentquestionId = currentquestionId == '' ? 0 : currentquestionId;
    //var isvalid = formvalidator();
    //if (!isvalid)
    //    return undefined;

    var answers = [];
    var correctoption = [];
    var model = {};
    var ComprehensionModels = null;
    switch (parseInt(questiontype)) {
        case QuestionType.MCQ: {

            $('#mcqoptions div .labels label').toArray().forEach(function (fun, i) {
                answers.push({
                    Option: $(fun).clone().find('.sup-checkbox').remove().end().html().trim(),
                    Position: i,
                    IsCorrect: $(fun).find('input[type=checkbox]').is(':checked')
                });
            });
            if (answers.filter(fi => { return fi.IsCorrect }).length == 0 && $(cnt).data('save') === 2) {
                notify('MCQ question must contain atleast one option to be correct.', 'warning', 4000);
                return false;
            }
            correctoption = JSON.stringify(answers.map(el => el.Option));
            break
        }
        case QuestionType["Match the following"]:
            {
                //for matching type question option params
                $('#matching table tr').toArray().forEach(function (fun, i) {
                    
                        var o = $(fun).find('td input').toArray().map(m => m.value);
                        var o2 = {};
                        o2[o[0]] = o[1];
                        correctoption.push(o2)
                    
                    return answers.push({
                        Option: $(fun).find('.ans').val(),
                        IsCorrect: true,
                        Position: i,
                        //CorrectAnswer: $(fun).find('td input').val()
                    });
                });
                break;
            }

        case QuestionType.Dropdown:
            {
                var textStr = tinyMCE.activeEditor.getContent({ format: 'text' }).split('{');
                if ($('#dropdown select').toArray().filter(el => el.value == 'select').length > 0 && $(cnt).data('save') === 3) {
                    notify('Please check you have selected a correct answers from the all dropdown.', 'info', 2000);
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
                correctoption = $('#dropdown select').toArray().map(el => el.value);
                break;
            }

        case QuestionType["Re-Arrange"]: 
            {
                correctoption = $('#mcqoptions .ans select').val();
                correctoption.forEach(function (fun, i) {
                    answers.push({
                        Option: fun,
                        Position: i,
                        //IsCorrect: $('#mcqoptions .ans select').val()
                          //  == fun
                    });
                });
                break;
            }
        case QuestionType['True or False']: {
            correctoption = $('#mcqoptions .ans select').val();
            $('.mcqoption-options select').val().forEach(function (fun, i) {
                answers.push({
                    Option: fun,
                    Position: i,
                    IsCorrect: $('#mcqoptions .ans select').val()== fun
                });
            });
            break;
        }
        case QuestionType.ComphresionAnswer: case QuestionType.ComphresionQuestion: {
            var qusId = $('.comper-question .selCompQus').val() ?? 0;
            ComprehensionModels = new ComperhesionQustion(currenttestid, currentquestionId, qusId, currenctsectionid);
            break;
        }
       
        default:
            correctoption = $('#mcqoptions div .labels label').toArray().map(t => t.innerHTML);
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
            break;
    }
    let topicid = $('#selTopic').val();
    let subtopicid = $('#selSubTopic').val();
    subtopicid = subtopicid == 0 || subtopicid == '' ? null : subtopicid;
    topicid = topicid == 0 || topicid == '' ? null : topicid;
    let QusModel = new QuestionModel(currentquestionId, tinyMCE.editors.editor1.getContent(), $(cnt).data('save'), topicid, subtopicid, questiontype, currenttestid, currenctsectionid == 0 || currenctsectionid == '' ? null : currenctsectionid, correctoption, $('#txtMarks').val(), answers, ComprehensionModels, tinymce.editors.editor3.getContent()
    );
    initLoader();
    insertdata('/Question/SaveQuestion', { model: QusModel }).done(res => {
        removeLoader();

        if (res == 'ok') {
            clearformvalues('#frmCreateExam', false);
            var isupdate = (model.QusID !== 0 && model.QusID !== undefined && model.QusID !== null);
            notify(`Question ${isupdate ? 'updated' : 'added'} successfully.`, 'success', 1000);
            if (isupdate)
                $('#myModal2').modal('hide');
            if ($('#pnlQus').length > 0)
                loaddata();
        }
        else
            notify(res, 'warning', 1000)
    }).fail(status => {
        removeLoader();
    });
}

var QuestionType =
    { 'MCQ': 1, 'Gap Filling': 2, 'Match the following': 3, 'True or False': 4, 'One word answer': 5, 'Re-Arrange': 6, 'Dropdown': 7,'ComphresionAnswer':8,'ComphresionQuestion':9 }

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
            currenctsectionid = 0;
            currentquestiontypeid = '';
        }
    })
    if (tinyMCE.editors.editor1 !== null) {
        tinyMCE.editors.editor1.setContent('');
    }
    if (tinyMCE.editors.editor2 !== null)
        tinymce.editors.editor2.setContent('');

    if (tinyMCE.editors.editor3 !== null)
        tinyMCE.editors.editor3.setContent('');

    $('div#editor2').html('');
    //$('#mcqoptions .options div').empty()
    if ($('select').length > 0)
        $('.options select').empty();
    $('.ans select').empty();
    $('#matching table tbody tr').remove();
    $('[name=QusID]').val('');
    $('.validator-1').remove();
    correctopts = [];
    qusOptions = [];
    opts = [];
    $(form).find('.selTest').change();
}

function question_popup(cnt) {

    currentquestionId = parseInt($(cnt).data('questionid')) || 0;
    $('[name=QusID]').val(currentquestionId);
    $('#myModal2').modal('show');
    let testid = 0;
    let questiontypeid = 0;

    if (currentquestionId > 0) {
        initLoader();
        insertdata('/Question/GetQuestionDetails', { QuestionId: currentquestionId }).done(res => {
            questiontypeid = parseInt(res.QuestionTypeId);
            qusOptions = res.Options;
            $('#txtMarks').val(res.Mark);
           
            comprehensionQuestion = res.comprehensionModels ??  new ComperhesionQustion();
          
            tinyMCE.editors.editor1.setContent(res.QuestionName);
            opts = []; correctopts = res.CorrectOption ?? '[]';

            correctopts = JSON.parse(correctopts);
                res.Options.forEach(op => {
                    opts.push(op.Option);
                });

            currenttestid = res.TestId;
            testid = currenttestid;
            currentquestiontypeid = res.QuestionTypeId;
            currenctsectionid = res.SectionId;
            $('#mcqoptions .options select').select2({ data: opts }).val(opts).change();
            $('#mcqoptions .ans select').select2({ data: opts }).val(correctopts).change();
            $('.selTest').val(testid.toString()).change();
            if (res.AnswerExplanation!=null)
            tinyMCE.editors.editor3.setContent(res.AnswerExplanation);

            currenttopicId = res.Topic;
            currentsubtopicid = res.SubTopic;
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
            width: '20%',
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
                        <span class="col-md-5 small">${el.SectionName}</span>
                        <span class="col-md-4 small">${el.Topic ?? 'N/A'}</span>
                        <em class="col-md-3 small text-white-50">${el.SubTopic ?? 'N/A'}</em>
                    </div>`,
            id: el.Id,
            text: el.SectionName,
            title: el.SectionName
        });


        $('#selSection').select2({
            'data': dt,
            height: '100%',
            width: '20%',
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
        $('#modalConfirm #btnDelete').text('Processing...').attr('disabled', true)
        window.location.href = url;
    })
}

function getSubTopicByTopicId(cnt) {
    getdata('/Tutor/GetSubTopic', { Id: $('#selTopic').val() }).done(res => {
        let option = document.getElementById('selSubTopic');
        option.innerHTML = '';
        option.appendChild(new Option("Select Topic", '0', true));
        res.forEach(op => {
            option.appendChild(new Option(op.SubTopic, op.Id, false));
        })
        $('#selSubTopic').val(currentsubtopicid);
    });
}