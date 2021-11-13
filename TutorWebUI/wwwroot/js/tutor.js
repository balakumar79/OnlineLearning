document.addEventListener('readystatechange', doc => {

    if (doc.target.readyState === 'complete') {
        var questiontypeid = parseInt($('.selQuestionType').val());

        $('#mcqoptions').hide();
        $('#trueorfalse').hide();



        //bind to section dropdown upon test select change
        $('.selTest').on('change', testF => {
            getdata('/tutor/GetTestSectionByTestId?testid=' + $('.selTest').val()).done(res => {
                formatSection(res);
            });
            //$('#selSection').select2({
            //        ajax: {
            //            url: '/tutor/GetTestSectionByTestId?testid=' + $('.selTest').val(),
            //            dataType: 'json',
            //            processResults: function (res) {
            //                dt = [];
            //                res.forEach(el => {
            //                    dt.push({
            //                        html: '<div class="text-center"><span class="col-md-5">' + el.sectionName + '</span><span class="col-md-5 small">' +
            //                            el.topic + '</span><em class="col-md-4 small text-white-50">' + el.subTopic + '</em></div>',
            //                        id: el.id,
            //                        text: el.sectionName,
            //                        title: el.sectionName
            //                    });
            //                });
            //                var te = {
            //                    results: dt
            //                };
            //                console.log(te);
            //                return te;
            //            },
            //        },
            //        templateResult: function (el) {
            //            return $(el.html);
            //    },

            //    dropdownAutoWidth: true
            //});
            //});

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
            contextmenu: "paste | copy | link image inserttable | cell row column deletetable",
            insert_toolbar: 'quickimage quicktable',
            //language: "ta_IN",
            //language_url: '/tinymce/langs/ta_IN.js',
            //directionality: "ltl",
            menubar: true,
            images_upload_url: '/tutor/questionimageupload',
            automatic_uploads: false,
            plugins: [
                'advlist autolink lists link image charmap print preview anchor',
                'searchreplace visualblocks code fullscreen',
                'insertdatetime imagetools media table paste code help wordcount'
            ],
            toolbar: 'insertfile undo redo',

            //tinymce change event
            setup: function (ed) {
                ed.on('keyup keydown', fun => {
                    questiontypeid = parseInt($('.selQuestionType').val());
                    //bind options to gap filling select2
                    if (questiontypeid === QuestionType["Gap Filling"] || questiontypeid === 5) {

                        var textStr = tinyMCE.activeEditor.getContent({ format: 'text' }).split('{');
                        let ans = [];
                        if (textStr.length > 0) {
                            textStr.forEach(el => {
                                if (el.indexOf('}') > 0) {
                                    ans.push(el.match("(.*)}")[1]);
                                }
                            });
                        }
                        $('#mcqoptions .options select').select2({ data: ans }).val(ans).change();
                        $('#mcqoptions .ans select').select2({ data: ans }).val(ans).change();
                    }
                });
            },
            images_upload_handler: function (blobInfo, success, failure) {
                alert('gone')
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
            $(this).trigger("change");
        })

        //option select change function
        $('.selQuestionType').change(fun => {
            $('#matching').hide('fast');

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
            $('#mcqoptions .ans').show();

            //check if the question is new or edit
            if (opts.length > 0) {

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
                case 1: case 5: case 6:
                    {
                        if (questiontypeID == 5)
                            $('#mcqoptions .ans').hide('slow');
                        if (questiontypeID == 6) {
                            tinymce.activeEditor.getBody().setAttribute('contenteditable', false);
                            correctoption.on("select2:select", function (evt) {
                                tinyMCE.activeEditor.setContent($('#mcqoptions .ans select').val().join(' '));
                            });
                        } else {
                            tinymce.activeEditor.getBody().setAttribute('contenteditable', true);
                        }
                        options.select2({
                            tags: true,
                            placeholder: "Enter your options here...",
                            allowClear: true,
                            disabled: false
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
                        $('#mcqoptions .ans').hide('slow');
                        options.select2({
                            tags: false,
                            placeholder: null,
                            allowClear: false,
                            disabled: true
                        }).on('select2:select change', fun => {

                            correctoption.select2({ data: opts });
                        });
                        break;
                    }
                case 3:
                    {
                        $('#mcqoptions').show('slow');
                        $('#mcqoptions .ans').hide('slow');
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
                                    $('#mcqoptions .options select').val().forEach(el => {
                                        el = el;
                                        tr = $(
                                            `<tr>
                                            <td>${el}</td>
                                            <td><input class="ans" value=''/></td>
                                        </tr>`
                                        );
                                        $('#matching .table tbody').append(tr);

                                    });
                                } else {
                                    console.log(correctopts)
                                    correctopts.forEach(el => {
                                        el = el.option;
                                        tr = $(
                                            `<tr>
                                            <td>${el.substring(0, el.lastIndexOf('{'))}</td>
                                            <td><input class="ans" value='${el.substring(el.lastIndexOf('{') + 1, el.lastIndexOf('}'))}'/></td>
                                        </tr>`
                                        );
                                        $('#matching .table tbody').append(tr);

                                    });
                                }
                            }
                        });
                        break;
                    }

                case 4:
                    {
                        options.select2({
                            tags: true,
                            placeholder: "Type your options here...",
                            allowClear: true,
                            maximumSelectionLength: 3,
                            disabled: false
                        }).on('select2:select change', fun => {

                            $('#mcqoptions .ans select').select2({ data: options.val() });
                        });
                        break;
                    }

                default: {
                    $('#mcqoptions').hide('slow');
                }

            }
            correctoption.on('select2:select', el => {

                el.target
            })

            //}
        });

        //form validator
        $('select').change(fun => {
            formvalidator();

        })
    }
});

var opts = [];
var correctopts = [];
function savequestion(cnt) {
    var questiontype = $('.selQuestionType').val();

    var isvalid = formvalidator();
    if (!isvalid)
        return undefined;

    var answers = [];
    var model = {};

        console.log(formvalidator());
    if (questiontype !== '3') {
        //for mcq, gap filling, true or false type question option params
        $('.options select').val().forEach(function (fun, i) {
            return answers.push({
                Option: fun,
                Position: i,
                IsCorrect: $('#mcqoptions .ans select').val()
                    .filter(el => { return el === fun }).length > 0
            });
        });
        model = {
            QusID: parseInt($('[name=QusID]').val())||0,
            QuestionName: tinyMCE.activeEditor.getContent(),
            QuestionTypeId: questiontype,
            TestId: $('.selTest').val(),
            SectionId: parseInt($('#selSection').val()) || null,
            Mark: $('#txtMarks').val(),
            Options: answers,
            IsActive: $(cnt).data('save')
        }
    } else {
        //for matching type question option params
        $('#matching table tr').toArray().forEach(function (fun, i) {
            return answers.push({
                Option: $(fun).find('td').eq(0).text() + ' {' + $(fun).find('td input').val()+'}',
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
            Mark: $('#txtMarks').val(),
            Options: answers,
            IsActive: $(cnt).data('save')
        }
    }
    console.log(model, answers)
    initLoader();
    insertdata('/Tutor/SaveQuestion', { model: model }).done(res => {
        removeLoader();
        if (res == 'ok') {
                clearformvalues('#frmCreateExam',false);
                var isupdate = (model.QusID !== 0 && model.QusID !== undefined && model.QusID !== null);
                notify(`Question ${isupdate ? 'updated' : 'added'} successfully.`, 'success');
                if (isupdate)
                    $('#myModal2').modal('hide');
            }
            else
                notify(res, 'warning')
            console.log(res);
        })
   
}

var QuestionType =
    { 'MCQ': 1, 'Gap Filling': 2, 'Match the following': 3, 'True or False': 4, 'One word answer': 5, 'Re-Arrange':6 }


function formvalidator() {
    var formcnt = $("#frmCreateExam").find('select,input,textarea')
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
        return isvalid;
}

function clearformvalues(form,clearDefaultSelection=true) {

    $(form).find('input,select,textarea').toArray().forEach(fun => {
        if (clearDefaultSelection) {
            if (fun.tagName.toLowerCase() === 'select') {
                $(fun).val($(fun).find('option:first').val());
            } else {
                fun.value == "";
            }
        }
    })
    if (tinyMCE.activeEditor !== null) {

        tinyMCE.activeEditor.setContent('');

    }
    if ($('select').length > 0)
        $('select').change();
    $('.options select').empty();
    $('.ans select').empty();
    $('#mcqoptions').hide();
    $('#matching table tbody tr').remove();
    $('[name=QusID]').val('');
    $('.validator-1').remove();
}



function question_popup(cnt) {
    clearformvalues('#frmCreateExam');
    bindTest('.selTest');
    bindQuestionType('.selQuestionType');
    var questionid = parseInt($(cnt).data('questionid'))||0;
    $('[name=QusID]').val(questionid);  
    $('#myModal2').modal('show');

    if (questionid > 0) {
        initLoader();
        insertdata('/tutor/GetQuestionDetails', { QuestionId: questionid }).done(res => {
          
            var testid = parseInt(res.testId)

            var questiontypeid = parseInt(res.questionTypeId);
            console.log(res);
            $('.selTest').val(testid).change();
            $('#txtMarks').val(res.mark);
            tinyMCE.activeEditor.setContent(res.questionName);
            opts = []; correctopts = [];
            res.options.forEach(function (el, i) {

                //format the matching type question options
                if (questiontypeid == 3) {
                    opts.push(el.option.slice(0, el.option.lastIndexOf('{')));
                    correctopts.push(el);
                }
                else {
                    opts.push(el.option);
                }
                if (el.isCorrect)
                    correctopts.push(el.option);
            });
            $('#mcqoptions .options select').select2({ data: opts }).val(opts).change();
            $('#mcqoptions .ans select').select2({ data: opts }).val(correctopts).change();
            $('.selQuestionType').val(questiontypeid).change();
    
        })
        removeLoader();
    }
   
}
function formatSection(res) {
    console.log(res)
    dt = [];
    res.forEach(el => {
        dt.push({
            html: `<div class="text-center">
                        <span class="col-md-5">${el.sectionName}</span>
                        <span class="col-md-5 small">${el.topic}</span>
                        <em class="col-md-4 small text-white-50">${el.subTopic}</em>
                    </div>`,
            id: el.id,
            text: el.sectionName,
            title: el.sectionName
        });
        console.log(dt);
        $('#selSection').empty();
        $('#selSection').select2({
            'data': dt,
            height:'100%',
            templateResult: function (data) {
                return $(data.html);
            },
            templateSelection: function (data) {
                return data.text;
            },
            dropdownAutoWidth: true
        });
        return dt;
    })
};

function deleteConfirmation(url) {
    $('#modalConfirm').modal('show');
    $('#modalConfirm #btnDelete').click(fun => {
        window.location.href = url;
    })
}



