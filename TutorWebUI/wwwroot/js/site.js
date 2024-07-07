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
		$(cnt).empty().append($('<option/>').text('--Select Language--'));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.Name).val(el.Id));
		})
		if (languageselected !== '' && languageselected !== undefined && languageselected !== null && languageselected!=0)
			$(cnt).val(languageselected);

		return $(cnt);
    })
}

function bindGradeLevels(cnt, gradeid = undefined) {
	getdata('/tutor/GetGradeLevels').done(res => {
		$(cnt).append($('<option/>').text('--Select Grade--'));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.Grade).val(el.Id));
		});
			(gradeid !== undefined && gradeid!=0) ? $(cnt).val(gradeid) : null;
		return $(cnt);
	})
}

function bindSubject(cnt, subjectid) {
	getdata('/Tutor/GetSubject').done(res => {
		$(cnt).append($('<option/>').text('--Select Subject--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.SubjectName).val(el.Id));
		});
			(subjectid !== undefined && subjectid!=0) ? $(cnt).val(subjectid).trigger('change') : null;
	})
	return $(cnt);
}

function bindTest(cnt, testid) {
	getdata('/Exam/GetTest').done(res => {
		$(cnt).empty();
		$(cnt).append($('<option/>').text('--Select Test--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.Title).val(el.Id));
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
			$(cnt).empty().append($('<option/>').text('--Select Section--').val('0'));
			res.forEach(el => {
				if (el.addedQuestions >= el.totalQuestions && $('#frmCreateExam').length > 0)
					$(cnt).append($('<option/>').text(el.SectionName).val(el.Id).attr('disabled', true));
				else
					$(cnt).append($('<option/>').text(el.SectionName).val(el.Id));

			});

		})
	}
}

function bindQuestionType(cnt) {
	getdata('/Question/GetQuestionType').done(res => {
		$(cnt).empty();
		$(cnt).append($('<option/>').text('--Select Qus Type--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.QustionTypeName).val(el.Id));
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

function initializeDataTable(id, url, method, columns) {
	$('#' + id).DataTable({
		"serverSide": true,
		traditional: true,
		ajax: {
			url: url,
			type: method,
			traditional: true,
			contentType: 'application/json;charset=utf-8',
			data: function (d) {
				
				return {
					PageNumber: d.start / d.length + 1,
					PageSize: d.length,
					Draw: d.draw,
					SearchString: d.search.value,
					columns: JSON.stringify(d.columns),
					order: JSON.stringify(d.order)
				};
			},
			dataSrc: function (data) {
				console.log(data)
				return data.data;
			},
			error: function (xhr, textStatus, errorThrown) {
				console.log(xhr.responseText, textStatus, errorThrown);
			}
		},
		"columns": columns
	});
}

function createDropdown(name, id, selectedValue, items, onchangeFunction) {
	var selectHtml = '<select name="' + name + '" id="' + name + '-' + id + '" data-id="' + id + '" onchange="' + onchangeFunction + '(this)">';
	items.forEach(function (item) {
		selectHtml += '<option value="' + item.Id + '"' + (item.Id == selectedValue ? 'selected' : '') + '>' + item.Status + '</option>';
	});
	selectHtml += '</select>';
	return selectHtml;
}

function createViewLink(action) {
	return '<a href="' + action + '"><i class="fa fa-eye text-info"> </i></a>';
}

function createEditLink(action) {
	return '<a href="' + action + '"><i class="fa fa-pencil text-info"> </i></a>';
}

function createDeleteButton(id, onclickFunction) {
	return '<button type="button" class="btn btn-sm btn-primary" onclick="' + onclickFunction + '(' + id + ')">' +
		'<i class="fa fa-times-circle text-danger"></i></button>';
}

function truncateText(data, maxLength) {
	return '<label title="' + data + '">' + (data.length > maxLength ? data.substring(0, maxLength) + '...' : data) + '</label>';
}

function createCheckbox(name, id, isChecked, onChangeFunction) {
	var checkboxHtml = '<input type="checkbox" name="' + name + '" id="' + id + '"';
	checkboxHtml += isChecked ? ' checked="checked"' : '';
	checkboxHtml += ' onchange="' + onChangeFunction + '(this)" />';
	return checkboxHtml;
}

function createHyperlink(url, text) {
	return '<a href="' + url + '">' + text + '</a>';
}



