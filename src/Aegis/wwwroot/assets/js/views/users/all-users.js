let usersTable;

$(document).ready(function () {
    initUsersTable();
})

/* Users Table Section */

function initUsersTable() {
    let actionUrl = $('#users-table').data("action");
    let profileUrl = $('#users-table').data("profile");

    usersTable = $('#users-table').DataTable({
        dom: "<'row'<'col-sm-12 col-md-6 mb-2'B><'col-sm-12 col-md-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        responsive: true,
        order: [[2, 'asc']],
        lengthMenu: [[10, 25, 50, 100, -1], ['Show 10 rows', 'Show 25 rows', 'Show 50 rows', 'Show 100 rows', 'Show All rows']],
        language: {
            searchPanes: {
                clearMessage: 'Clear Filters',
                collapse: { 0: 'Filters', _: 'Filters (%d)' }
            }
        },
        buttons: [
            {
                text: 'Refresh',
                className: 'btn-outline-light w-md waves-effect waves-light',
                action: function (e, dt, node, config) {
                    dt.ajax.reload();
                }
            },
            {
                extend: 'searchPanes',
                className: 'btn-outline-light w-md waves-effect waves-light search-panes d-none',
                config: {
                    columns: [1, 2],
                    orderable: false,
                    collapse: false,
                    cascadePanes: true,
                    panes: [
                        {
                            header: 'Active Profile',
                            options: [
                                {
                                    label: 'Yes',
                                    value: function (rowData, rowIdx) {
                                        return rowData.activeProfile;
                                    }
                                },
                                {
                                    label: 'No',
                                    value: function (rowData, rowIdx) {
                                        return !rowData.activeProfile;
                                    }
                                }
                            ],
                            dtOpts: {
                                order: [[1, 'desc']]
                            }
                        },
                        {
                            header: 'Completed Profile',
                            options: [
                                {
                                    label: 'Yes',
                                    value: function (rowData, rowIdx) {
                                        return rowData.completedProfile;
                                    }
                                },
                                {
                                    label: 'No',
                                    value: function (rowData, rowIdx) {
                                        return !rowData.completedProfile;
                                    }
                                }
                            ],
                            dtOpts: {
                                order: [[1, 'desc']]
                            }
                        },
                        {
                            header: '2FA Enabed',
                            options: [
                                {
                                    label: 'Yes',
                                    value: function (rowData, rowIdx) {
                                        return rowData.twoFactorEnabled;
                                    }
                                },
                                {
                                    label: 'No',
                                    value: function (rowData, rowIdx) {
                                        return !rowData.twoFactorEnabled;
                                    }
                                }
                            ],
                            dtOpts: {
                                order: [[1, 'desc']]
                            }
                        },
                        {
                            header: 'System User',
                            options: [
                                {
                                    label: 'Yes',
                                    value: function (rowData, rowIdx) {
                                        return rowData.systemUser;
                                    }
                                },
                                {
                                    label: 'No',
                                    value: function (rowData, rowIdx) {
                                        return !rowData.systemUser;
                                    }
                                }
                            ],
                            dtOpts: {
                                order: [[1, 'desc']]
                            }
                        },
                        {
                            header: 'Protected User',
                            options: [
                                {
                                    label: 'Yes',
                                    value: function (rowData, rowIdx) {
                                        return rowData.protectedUser;
                                    }
                                },
                                {
                                    label: 'No',
                                    value: function (rowData, rowIdx) {
                                        return !rowData.protectedUser;
                                    }
                                }
                            ],
                            dtOpts: {
                                order: [[1, 'desc']]
                            }
                        },
                        {
                            header: 'Locked User',
                            options: [
                                {
                                    label: 'Yes',
                                    value: function (rowData, rowIdx) {
                                        return rowData.lockedUser;
                                    }
                                },
                                {
                                    label: 'No',
                                    value: function (rowData, rowIdx) {
                                        return !rowData.lockedUser;
                                    }
                                }
                            ],
                            dtOpts: {
                                order: [[1, 'desc']]
                            }
                        },
                        {
                            header: 'Soft Delete',
                            options: [
                                {
                                    label: 'Yes',
                                    value: function (rowData, rowIdx) {
                                        return rowData.softDelete;
                                    }
                                },
                                {
                                    label: 'No',
                                    value: function (rowData, rowIdx) {
                                        return !rowData.softDelete;
                                    }
                                }
                            ],
                            dtOpts: {
                                order: [[1, 'desc']]
                            }
                        }
                    ]
                }
            },
            {
                extend: 'pageLength',
                className: 'btn-outline-light w-md waves-effect waves-light',
            }
        ],
        ajax: function (data, callback, settings) {
            $.ajax({
                type: "GET",
                url: actionUrl,
                success: function (response) {
                    if (response.success === true) {
                        let content = JSON.stringify(response.result);
                        let data = { data: JSON.parse(content) };

                        if (data.data.length > 10) {
                            $('.search-panes').removeClass('d-none');
                        }

                        callback(data)
                    }
                    else {
                        toastr["error"](result.message)
                    }

                },
                error: function (response) {
                    toastr["error"]("Something went wrong. Please try again later!");
                }
            });
        },
        columns: [
            {
                data: function (row, type, val, meta) {
                    usersTable.buttons().container()

                    return `
                        <div class="text-center">
                            <img class="rounded-circle header-profile-user border border-3 ${row.activeProfile ? "border-success" : "border-danger"}" src="${row.profilePictureURL}" alt="Profile Picture" />
                        </div>`;
                }
            },
            { data: "fullName" },
            { data: "email" },
            {
                data: function (row, type, val, meta) {
                    return `
                        <div>
                            <span class="badge ${row.activeProfile ? "bg-success" : "bg-danger"} font-size-10">Active Profile</span>
                            <span class="badge ${row.completedProfile ? "bg-success" : "bg-danger"} font-size-10">Completed Profile</span>
                            <span class="badge ${row.twoFactorEnabled ? "bg-success" : "bg-danger"} font-size-10">MFA</span>
                        </div>`;
                }
            },
            {
                data: function (row, type, val, meta) {

                    return `
                        <div>
                            <span class="badge ${row.systemUser ? "bg-success" : "bg-danger"} font-size-11">System User</span>
                            <span class="badge ${row.protectedUser ? "bg-success" : "bg-danger"} font-size-11">Protected User</span>
                            <span class="badge ${row.lockedUser ? "bg-success" : "bg-danger"} font-size-11">Locked User</span>
                            <span class="badge ${row.softDelete ? "bg-success" : "bg-danger"} font-size-11">Soft Delete</span>
                        </div>`;
                }
            },
            {
                data: function (row, type, val, meta) {

                    return `
                        <div class="text-center">
                            <div class="dropdown">
                                <a href="#" class="dropdown-toggle card-drop" data-bs-toggle="dropdown" aria-expanded="false">
                                    <i class="mdi mdi-dots-vertical font-size-18"></i>
                                </a>
                                <ul class="dropdown-menu dropdown-menu-end" style="">
                                    <li><a href="${profileUrl}/${row.id}" id="view-profile-${meta.row}" class="dropdown-item"><i class="mdi mdi-view-split-vertical font-size-16 text-info me-1"></i> View</a></li>
                                    <div class="dropdown-divider"></div>
                                    <li><button onClick="showReset2FAModal('${row.id}','${row.email}')" id="reset-2fa-${meta.row}" class="dropdown-item" ${row.twoFactorEnabled ? "" : "disabled"}><i class="mdi mdi-two-factor-authentication font-size-16 text-warning me-1"></i> Reset 2FA</button></li>
                                    <li><button onClick="ShowResetPassword('${row.id}','${row.email}')" id="reset-password-${meta.row}"  class="dropdown-item"><i class="mdi mdi-lock-reset font-size-16 text-warning me-1"></i> Reset Password</button></li>
                                    <div class="dropdown-divider"></div>
                                    ${row.lockedUser
                            ? `<li><button onClick="ShowUnlock('${row.id}','${row.email}')" id="lock-${meta.row}" class="dropdown-item"><i class="mdi mdi-lock-open font-size-16 text-success me-1"></i> Unlock</button></li>`
                            : `<li><button onClick="ShowLock('${row.id}','${row.email}')" id="lock-${meta.row}" class="dropdown-item"><i class="mdi mdi-lock font-size-16 text-danger me-1"></i> Lock</button></li>`}
                                    <li><button onClick="SoftDelete('${row.id}')" id="soft-delete-${meta.row}"  class="dropdown-item"><i class="mdi mdi-trash-can font-size-16 text-danger me-1"></i> Soft Delete</button></li>
                                </ul>
                            </div>
                        </div>`;
                }
            }
        ],
        fixedColumns: true,
        columnDefs: [
            { targets: [0, 5], responsivePriority: 1 },
            { targets: [0, 3, 4, 5], orderable: false },
            { targets: [0, 5], width: '5%' },
            { targets: [1, 2, 3], width: '20%' },
            { targets: [4], width: '30%' }
        ]
    });

    $('#users-table').width("100%");
}

/* Reset 2FA Section */

$('#reset-2fa-modal').on('hide.bs.modal', function () {
    cancelReset2FaForm()
});

$('#reset-2fa-form-switch').on('change', function () {
    if (this.checked) {
        $('.reset-2fa-form-wrapper').removeClass('d-none');
        $('#reset-2fa-modal-save').removeClass('d-none');
    }
    else {
        $('.reset-2fa-form-wrapper').addClass('d-none');
        $('#reset-2fa-modal-save').addClass('d-none');
    }
});

$('#reset-2fa-form').on('submit', function (e) {
    e.preventDefault();

    if ($(this).parsley().isValid()) {
        const action = $('#reset-2fa-form').data("action");
        let formData = $('#reset-2fa-form').serializeAll();

        ajaxPost(action, formData);

        usersTable.ajax.reload();
        $('#reset-2fa-modal').modal('hide');
    }
});

function submitReset2FaForm() {
    $('#reset-2fa-form').submit();
}

function cancelReset2FaForm() {
    $('#reset-2fa-form-switch').prop("checked", false);
    $('.reset-2fa-form-wrapper').addClass('d-none');
    $('#reset-2fa-modal-save').addClass('d-none');
    $('#reset-2fa-form').parsley().reset();
    $('#reset-2fa-form')[0].reset();
}

function showReset2FAModal(userId, userEmail) {
    cancelReset2FaForm();
    $('#reset-2fa-modal').modal('show');
    $('#reset-2fa-form #id').val(userId);
    $('#reset-2fa-form #email').val(userEmail);
}

/* Reset Password Section */

function ShowResetPassword(userId, userEmail) {
    alert(userId)
}

function ShowLock(userId, userEmail) {
    alert(userId)
}

function ShowUnlock(userId, userEmail) {
    alert(userId)
}

function ShowSoftDelete(userId, userEmail) {
    alert(userId)
}