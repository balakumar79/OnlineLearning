// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.
document.addEventListener('readystatechange', doc => {
	if (doc.target.readyState === 'complete') {
    }
})
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
			console.log('error:', err.responseText);
        }
    })
}

function bindLanguage(cnt, languageselected = null) {
	getdata('/manageexams/GetLanguage').done(res => {
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
	getdata('/manageexams/GetGradeLevels').done(res => {
		$(cnt).append($('<option/>').text('--Select Grade--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.grade).val(el.id));
		})
		gradeid !== undefined ? $(cnt).val(gradeid) : null;
		return $(cnt);
	})
}

function bindSubject(cnt, subjectid) {
	getdata('/manageexams/GetSubject').done(res => {
		$(cnt).append($('<option/>').text('--Select Subject--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.subjectName).val(el.id));
		})
		subjectid !== undefined ? $(cnt).val(subjectid) : null;
		return $(cnt);
	})
}

function bindTest(cnt,testid) {
	getdata('/manageexams/GetTest').done(res => {
		$(cnt).empty();
		$(cnt).append($('<option/>').text('--Select Test--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.title).val(el.id));
		})
		if (testid !== undefined && testid !== null && testid !== '') {
			//$(cnt).val(testid)
		}
		console.log(cnt, testid);
		return $(cnt);
	});
}

function bindTestSection(cnt, testid) {
	if (testid !== undefined) {
		alert('section changed');
		getdata('/manageexams/GetTestSectionByTestId', testid).done(res => {
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
	getdata('/manageexams/GetQuestionType').done(res => {
		$(cnt).empty();
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

function initLoader() {
	$('.loading').css('display', 'block');
	$('.loader').css('display', 'block');
}

function removeLoader() {
	$('.loading').css('display', 'none');
	$('.loader').css('display', 'none');

}

function initializeDataTable(id, url, method, columns) {
	$('#' + id).DataTable({
		"serverSide": true,
		traditional:true,
		ajax: {
			url: url,
			type: method,
			traditional:true,
			contentType: 'application/json;charset=utf-8',
			data: function (d) {
				console.log(d, {
					PageNumber: d.start,
					PageSize: d.length,
					Draw: d.draw,
					SearchString: d.search.value,
					Order: {column:1,dir:'asc'}
				})
				return {
					PageNumber: d.start / d.length+1,
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

function createDeleteButton(id, onclickFunction) {
	return '<button type="button" class="btn btn-sm btn-primary" onclick="' + onclickFunction + '(' + id + ')">' +
		'<i class="fa fa-times-circle text-danger"></i></button>';
}

function truncateText(data, maxLength) {
	return '<label title="' + data + '">' + (data.length > maxLength ? data.substring(0, maxLength) + '...' : data) + '</label>';
}

function createCheckbox(name, id, isChecked, isDisabled=false, onChangeFunction) {
	var checkboxHtml = '<input type="checkbox" name="' + name + '" id="' + id + '"';
	checkboxHtml += isChecked ? ' checked="checked"' : '';
	checkboxHtml += isDisabled ? "disabled" : '';
	checkboxHtml += ' onchange="' + onChangeFunction + '(this)" />';
	return checkboxHtml;
}




