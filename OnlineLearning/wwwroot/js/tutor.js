document.addEventListener('readystatechange', doc => {
    if (doc.target.readyState === 'complete') {
        CKEDITOR.instances.editor1.on('change',fun => {
            var textStr = (CKEDITOR.instances.editor1.document.getBody().getText().toString());
            let ans = [];
            for (var i = 0; i < textStr.Split('['); i++) {
                ans.push(textStr.match("*(.)*"));
            }
            ans
        })
        
        $('#mcqoptions').hide();
        $('#mcqoptions .options select').select2({
            tags: true,
            placeholder: "Enter your options here...",
            allowClear: true
        }).on('select2:select change', fun => {
            opts = []; i = 0;
            $(fun.target).find('option').toArray().forEach(fun => {
                return opts.push({ id: fun.value, text: fun.value });
            })
            $('#mcqoptions .ans select').select2({ data: opts });
        });
        $('#mcqoptions .ans select').select2({
            tags: false,
            placeholder: "Enter correct option/s here...",
            allowClear: true
        });
        $('.selQuestionType').change(fun => {
            
            if (fun.target.value === '1') {
                $('#mcqoptions').show('slow');
            } else {
                $('#mcqoptions').hide('slow');
            }

        })
    }
})

var opts = [];
var correctopts = [];
function savequestion(cnt) {
        var isvalid = true;
    $("#frmCreateExam").find('select,input,textarea').toArray().forEach(function (el, i) {
        console.log($(el.value))
        if ((el.value == '' || el.value == '0' || el.value.length == 0) && $(el).attr('required')==='required') {
            isvalid = false;
            var h = document.createElement("span");
            h.className = 'text-danger small p-3 bg-warning';
            var t = document.createTextNode("*");
            h.appendChild(t);
            $('.validator').eq(i).text('*').addClass('text-danger');
        }
    });
    if (!isvalid)
        return isvalid;
    var answers = [];
    console.log(opts);
        opts.forEach(fun => {
            return answers.push({
                Option: fun.text,
                IsCorrect: $('#mcqoptions .ans select').toArray()
                    .filter(el => { return el.value === fun.text }).length > 0
            });
        })
        var model = {
            QuestionName: CKEDITOR.instances['editor1'].getData(),
            QuestionTypeId: $('.selQuestionType').val(),
            TestId: $('.selTest').val(),
            SectionId: $('#selSection').val(),
            Options: answers
        }
        console.log(model, answers)
        insertdata('SaveQuestion', { model: model }).done(res => {
            if (res=='ok')
                notify('Question added successfully.', 'success');
            else
                notify(res, 'warning')
            console.log(res);
        })
   
}

/*gap filling drag and drop*/
function allowDrop(ev) {
    ev.preventDefault();
}

function drag(ev) {
    ev.dataTransfer.setData($(this), ev.target.id);
}
function drop(ev) {
    ev.preventDefault();
    var data = ev.dataTransfer.getData($(this));
    $('#draginput').appendTo($(this));
    ev.target.appendChild(CKEDITOR.inline(data.get(0)));
    
}

