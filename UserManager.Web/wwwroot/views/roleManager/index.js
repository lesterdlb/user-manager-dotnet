(function ($) {
    const createModal = $('#RoleCreateModal');
    const editModal = $('#RoleEditModal');

    const createForm = createModal.find('form');
    const editForm = editModal.find('form');

    const rolesTable = $('#RolesTable').DataTable({
        "processing": true,
        "serverSide": false,
        "filter": false,
        "paging": false,
        "ordering": false,
        "searching": false,
        "autoWidth": false,
        "responsive": true,
        "ajax": {
            "url": 'RoleManager/Get',
            "type": "GET",
            "datatype": "json",
        },
        columns: [
            {
                "data": 'id',
            },
            {
                "data": 'name',
            },
            {
                "data": null,
                "render": (data) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-role" data-role-id="${data.id}" data-role-name="${data.name}" data-toggle="modal" data-target="#RoleEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> Edit`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-role" data-role-id="${data.id}" data-role-name="${data.name}">`,
                        `       <i class="fas fa-trash"></i> Delete`,
                        '   </button>',
                    ].join('');
                }
            }
        ]
    });

    $(document).on('click', '.edit-role', function (e) {
        e.preventDefault();

        editModal.find('input[name="Id"]').val($(this).attr("data-role-id"));
        editModal.find('#Name').val($(this).attr("data-role-name"));
    });

    createForm.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!createForm.valid()) {
            return;
        }

        const role = createForm.serializeFormToObject();
        createEditRole("/RoleManager/Add", "POST", { name: role.Name }, createModal);
    });

    editForm.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!editForm.valid()) {
            return;
        }

        const role = editForm.serializeFormToObject();
        createEditRole("/RoleManager/Edit", "PUT", { id: role.Id, name: role.Name }, editModal);
    });

    const createEditRole = (url, method, data, modal) => {
        $.ajax({
            url: url,
            data: data,
            type: method,
            success: function (result) {
                modal.modal('hide');
                rolesTable.ajax.reload();
                toastr.success(result);
            },
            error: function (result) {
                modal.modal('hide');
                toastr.error(result.responseText)
            }
        });
    }

    $(document).on('click', '.delete-role', function () {
        const roleId = $(this).attr("data-role-id");
        const roleName = $(this).attr('data-role-name');

        Swal.fire({
            title: 'Are you sure?',
            text: 'This role "' + roleName + '" will be removed from all its associated users.',
            icon: 'warning',
            reverseButtons: 'true',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Delete!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "/RoleManager/Delete",
                    data: { id: roleId },
                    type: "DELETE",
                    success: (result) => {
                        rolesTable.ajax.reload();
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
