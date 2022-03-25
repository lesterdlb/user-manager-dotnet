(function ($) {
    const createModal = $('#UserCreateModal');
    const editModal = $('#UserEditModal');

    const createForm = createModal.find('form');
    const editForm = editModal.find('form');

    const usersTable = $('#UsersTable').DataTable({
        "processing": true,
        "serverSide": false,
        "filter": false,
        "paging": false,
        "ordering": false,
        "searching": false,
        "autoWidth": false,
        "responsive": true,
        "ajax": {
            "url": 'UserManager/GetAll',
            "type": "GET",
            "datatype": "json",
        },
        columns: [
            {
                "data": 'userName',
            },
            {
                "data": 'email',
            },
            {
                "data": 'firstName',
            },
            {
                "data": 'lastName',
            },

            {
                "data": null,
                "render": (data) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-user" data-user-id="${data.id}" data-user-name="${data.name}" data-toggle="modal" data-target="#UserEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> Edit`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-user" data-user-id="${data.id}" data-user-username="${data.userName}">`,
                        `       <i class="fas fa-trash"></i> Delete`,
                        '   </button>',
                    ].join('');
                }
            }
        ]
    });

    $(document).on('click', '.edit-user', function (e) {
        e.preventDefault();

        const userId = $(this).attr("data-user-id");

        $.ajax({
            url: '/UserManager/Get?userId=' + userId,
            type: 'POST',
            success: function (result) {
                editModal.find('input[name="Id"]').val(result.id);
                editModal.find('#Email').val(result.email);
                editModal.find('#FirstName').val(result.firstName);
                editModal.find('#LastName').val(result.lastName);
            },
            error: function (e) {
            }
        });
    });

    createForm.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!createForm.valid()) {
            return;
        }

        const user = createForm.serializeFormToObject();
        createEditUser("/UserManager/Create", "POST", user, createModal);
    });

    editForm.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!editForm.valid()) {
            return;
        }

        const user = editForm.serializeFormToObject();
        createEditUser("/UserManager/Edit", "PUT", user, editModal);
    });

    const createEditUser = (url, method, data, modal) => {
        $.ajax({
            url: url,
            data: data,
            type: method,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            success: function (result) {
                modal.modal('hide');
                usersTable.ajax.reload();
                toastr.success(result);
            },
            error: function (result) {
                modal.modal('hide');
                toastr.error(result.responseText)
            }
        });
    }

    $(document).on('click', '.delete-user', function () {
        const userId = $(this).attr("data-user-id");
        const userName = $(this).attr('data-user-username');

        Swal.fire({
            title: 'Are you sure?',
            text: 'This User "' + userName + '" will be deleted.',
            icon: 'warning',
            reverseButtons: 'true',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Delete!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "/UserManager/Delete",
                    data: { userId: userId },
                    type: "DELETE",
                    success: (result) => {
                        usersTable.ajax.reload();
                        toastr.success(result);
                    },
                    error: (result) => {
                        toastr.error(result.responseText)
                    }
                });
            }
        })
    });

    createModal.add(editModal).on('shown.bs.modal', function () {
        $(this).find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', function () {
        $(this).find('form').clearForm();
    });
})(jQuery);
