$(() => {
    $(".edit-contributer").on('click', function () {
        let firstName = $(this).data("first-name");
        let lastName = $(this).data("last-name");
        let cell = $(this).data("cell");
        let alwaysInclude = $(this).data("always-include");
        let date = $(this).data("date");
        let id = $(this).data("contributerid");
        $("#contributor_first_name").val(firstName);
        $("#contributor_last_name").val(lastName);
        $("#contributor_cell_number").val(cell);
        $("#contributor_always_include").prop('checked', alwaysInclude === "True");
        $("#contributor_created_at").val(date);
        $("#initialDepositDiv").remove();
        console.log(id);
        $("#addModal").modal();
        let form = $(".add-contributor");
        form.attr('action', '/contributers/edit');
        $("#contributor-id").val(id);
      
       
    })
})