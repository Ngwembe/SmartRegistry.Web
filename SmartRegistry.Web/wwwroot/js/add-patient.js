$(function()
{
    // process the form
    $("form.add-patient").on('submit', function (e) {
        e.preventDefault();
        var form = $(this);
        var action = form.attr("action");
        var data = form.serializeArray();

        console.log("Doing the AJAX POST request");
        console.log("Sending data:");

        console.log(JSON.stringify(getFormData(data)));

        // send ajax
        $.ajax({

            headers: {
                'Accept': 'application/json',
                'content-Type': 'application/json'
            },
            url: 'https://admissions-dot-medipark-hospital.appspot.com/v1/Patient/create',
            crossDomain: true,
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            type: 'POST',
            data: JSON.stringify(getFormData(data)),
            success: function (data) {
                // you can see the result from the console
                // tab of the developer tools
                console.log(data);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    });

    //utility function
    function getFormData(data) {
        var unindexed_array = data;
        var indexed_array = {};

        $.map(unindexed_array, function (n, i) {
            indexed_array[n['name']] = n['value'];
        });

        return indexed_array;
    }
});


