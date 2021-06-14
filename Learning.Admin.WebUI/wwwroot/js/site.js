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
			delay: 1000,
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
			console.log(err);
        }
    })
}
function bindLanguage(cnt) {
	getdata('/account/GetLanguageList').done(res => {
		$(cnt).append($('<option/>').text('--Select--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.name).val(el.id));
		})
		return $(cnt);
    })
}

function bindGradeLevels(cnt) {
	getdata('/account/GetGradeLevels').done(res => {
		$(cnt).append($('<option/>').text('--Select--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.grade).val(el.id));
		})
		return $(cnt);
	})
}

function bindSubject(cnt) {
	getdata('/account/GetSubject').done(res => {
		$(cnt).append($('<option/>').text('--Select--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.grade).val(el.id));
		})
		return $(cnt);
	})
}
function bindTestSection(cnt, testid) {
	getdata('/account/GetTestSections', { testid: testid }).done(res => {
		$(cnt).append($('<option/>').text('--Select--').val(''));
		res.forEach(el => {
			$(cnt).append($('<option/>').text(el.grade).val(el.id));
		})
		return $(cnt);
	})
}