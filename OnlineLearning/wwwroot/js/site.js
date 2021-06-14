// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function notify(msg, type) {
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
			delay: 10000,
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
			console.log('error:',err);
        }
    })
}
function bindLanguage(cnt) {
	getdata('/account/GetLanguageList').done(res => {
		$(cnt).append($('<option/>').text('--Select Language--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.name).val(el.id));
		})
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
		return $(cnt);
	})
}
function bindTest(cnt,testid) {
	getdata('/tutor/GetTest').done(res => {
		$(cnt).append($('<option/>').text('--Select Test--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.title).val(el.id));
		})
		testid !== undefined ? $(cnt).val(testid) : null;
		return $(cnt);
	})
}
function bindTestSection(cnt, testid) {
	if (testid !== undefined) {
		
		getdata('/tutor/GetTestSections', testid).done(res => {
			console.log(res)
			$(cnt).empty().append($('<option/>').text('--Select Section--').val(''));
			res.forEach(el => {
				$(cnt).append($('<option/>').text(el.sectionName).val(el.id));
			})

		})
	}
}
function bindQuestionType(cnt) {
	getdata('/tutor/GetQuestionType').done(res => {
		$(cnt).append($('<option/>').text('--Select Qus Type--').val(''));
			res.forEach(el => {
			$(cnt).append($('<option/>').text(el.qustionTypeName).val(el.id));
		})
		return $(cnt);
	})
}

function initDataTable(tbl) {
	var tb = $(tbl).DataTable();
	console.log(tb)
}