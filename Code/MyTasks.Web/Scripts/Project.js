$(document).ready(function () {
    //Add button click event
    $('#add')
        .click(function() {
            var isAllValid = true;

            if ($('#areaName').val().trim() == '' ){ 
                isAllValid = false;
                $('#areaName').siblings('span.error').css('visibility', 'visible');
            } else {
                $('#areaName').siblings('span.error').css('visibility', 'hidden');
            }

            if (isAllValid) {
                var $newRow = $('#mainrow').clone().removeAttr('id');

                //Replace add button with remove button
                $('#add', $newRow).addClass('remove').val('Remove').removeClass('btn-success').addClass('btn-danger');

                //remove id attribute from new clone row
                $('#areaName,#add', $newRow).removeAttr('id');
                $('span.error', $newRow).remove();
                //append clone row
                $('#areadetailsItems').append($newRow);

                //clear select data
                $('#areaName').val('');
                $('#areaItemError').empty();
            }

        });

    //remove button click event
    $('#areadetailsItems').on('click', '.remove', function () {
        $(this).parents('tr').remove();
    });

    $('#submit').click(function () {
        var isAllValid = true;
        
        $('#areaItemError').text('');
        var list = [];
        var errorItemCount = 0;
        $('#areaItems tr')
            .each(function(index, ele) {
                if ($('.areaName', this).val() == "") {
                    errorItemCount++;
                    $(this).addClass('error');
                } else {
                    var newArea = {
                        Name: $('.areaName', this).val()
                    }
                    list.push(newArea);
                }
            });
        if (errorItemCount > 0) {
            $('#areaItemError').text(errorItemCount + Resources.InvalidAreaInList);
            isAllValid = false;
        }

        if (list.length == 0) {
            $('#areaItemError').text(Resources.OnAreaIsRequired);
            isAllValid = false;
        }
        
        if ($("#ProjectManagerId").val() === "") {
            $('#areaItemError').text(Resources.ProjectManagerIsRequired);
            isAllValid = false;
        }

        if ($("#CustomerId").val() === "") {
            $('#areaItemError').text(Resources.CustomerIsRequired);
            isAllValid = false;
        }
        

        if ($("#ProjectName").val() === "") {
            $('#areaItemError').text(Resources.ProjectNameIsRequired);
            isAllValid = false;
        }

        if (isAllValid) {
            var data = {
                ProjectName: $("#ProjectName").val(),
                ProjectManagerId: $("#ProjectManagerId").val(),
                CustomerId: $("#CustomerId").val(),
                Description: $("#Description").val(),
                Areas: list
            }

            $.ajax({
                type: 'POST',
                url: window.applicationBaseUrl + 'project/create',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status.IsValid===true) {
                        window.location.href = window.applicationBaseUrl + "project/index";}
                    else {
                        
                        $('#areaItemError').text(data.status.ErrorMessage);
                    }
                    $('#submit').text('Save');
                },
                error: function (error) {
                    console.log(error);
                    $('#areaItemError').text(Resources.UnableToSaveChanges);
                }
            });
        }

    });

});


