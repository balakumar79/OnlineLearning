// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.
document.addEventListener('readystatechange', doc => {
	if (doc.target.readyState === 'complete') {
    }
})
function notify(msg, type, timeout) {
	timeout = timeout ?? 10000;
	$.notify({
		icon: '/images/new-notification-icon.jpg',
		title: 'Online Learning',
		message: msg,
	},
		{
			animate: {
				enter: 'animate__animated animate__slideInDown',
				exit: 'animate__animated animate__slideOutUp'
			},
			icon_type: 'image',
			type: 'pastel-' + type,
			delay: timeout,
			template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
				'<span data-notify="title">{1}</span>' +
				'<span data-notify="message">{2}</span>' +
				'</div>',
            }
		);
}

function insertdata(url, data, datatype = 'json') {
	return $.ajax({
		url: url,
		data: data,
		dataType: datatype,
		type: 'post',
		success: function () { },
		error: function (err) {
			notify(err.responseText, 'danger');
			console.log(err.responseText);
        }
    })
}

function getdata(url, data, datatype = 'json') {
	return $.ajax({
		url: url,
		data: data,
		type:'get',
		dataType: datatype,
		error: function (err) {
			notify(err.responseText, 'danger');
			console.log('error:', err.responseText);
        }
    })
}
function bindLanguage(cnt, languageselected = null) {
	getdata('/tutor/GetLanguage').done(res => {
		$(cnt).empty().append($('<option/>').text('--Select Language--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.name).val(el.id));
		})
		if (languageselected !== '' && languageselected !== undefined && languageselected !== null)
			$(cnt).val(languageselected);
		return $(cnt);
    })
}

function bindGradeLevels(cnt, gradeid = undefined) {
	getdata('/tutor/GetGradeLevels').done(res => {
		$(cnt).append($('<option/>').text('--Select Grade--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.grade).val(el.id));
		})
		gradeid !== undefined ? $(cnt).val(gradeid) : null;
		return $(cnt);
	})
}

function bindSubject(cnt, subjectid) {
	getdata('/Tutor/GetSubject').done(res => {
		$(cnt).append($('<option/>').text('--Select Subject--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.subjectName).val(el.id));
		})
		subjectid !== undefined ? $(cnt).val(subjectid) : null;
	})
	return $(cnt);
}
function bindTest(cnt, testid) {
	getdata('/tutor/GetTest').done(res => {
		$(cnt).empty();
		$(cnt).append($('<option/>').text('--Select Test--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.title).val(el.id));
		})
		if (testid !== undefined && testid !== null && testid !== '') {
			//$(cnt).val(testid)
		}
	});
	return $(cnt);
}
function bindTestSection(cnt, testid) {
	if (testid !== undefined) {
		alert('section changed');
		getdata('/tutor/GetTestSectionByTestId', testid).done(res => {
			console.log(res)
			$(cnt).empty().append($('<option/>').text('--Select Section--').val('0'));
			res.forEach(el => {
				if (el.addedQuestions >= el.totalQuestions && $('#frmCreateExam').length > 0)
					$(cnt).append($('<option/>').text(el.sectionName).val(el.id).attr('disabled', true));
				else
					$(cnt).append($('<option/>').text(el.sectionName).val(el.id));

			});

		})
	}
}
function bindQuestionType(cnt) {
	getdata('/tutor/GetQuestionType').done(res => {
		$(cnt).empty();
		$(cnt).append($('<option/>').text('--Select Qus Type--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.qustionTypeName).val(el.id));
		})
	});
	return $(cnt);
}

function deleteTest(id, dt = null) {
	$('#modalConfirm').modal('show');
	$('#modalConfirm #btnDelete').on('click', del => {
		initLoader();
		insertdata('/Tutor/DeleteTest', { id: id }).done(res => {
			notify('Test has been deleted successfully !!!', 'success');
			removeLoader();
			if (dt != null) {
				if ($(dt).length>0)
				$(dt).DataTable().ajax.reload();
            }
		})
	});
}

function initDataTable(tbl) {
	return $(tbl).DataTable();
	
}
function initLoader() {
	$('.loading').css('display', 'block');
	$('.loader').css('display', 'block');
}
function removeLoader() {
	$('.loading').css('display', 'none');
	$('.loader').css('display', 'none');

}
function require(script) {
	$.ajax({
		url: script,
		dataType: "script",
		async: false,           // <-- This is the key
		success: function () {
			// all good...
		},
		error: function () {
			throw new Error("Could not load script " + script);
		}
	});
}



