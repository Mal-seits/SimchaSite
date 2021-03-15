$(() => {
    $(".deposit-button").on('click', function () {
        let name = $(this).data("contributername");
        
        $("#deposit-name").append(name);
        let id = $(this).data("contributerid");
        $("#hiddenId").val(id);
        $("#depositModal").modal();
    })
   
})