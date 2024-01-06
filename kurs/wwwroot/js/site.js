createStars = function (container, input) {
    var max = input.attr("max")
    container.empty()
    for (let i = 1; i <= max; i++) {
        var star = $("<i>").css({ padding: "0 2px" }).attr("data-value", i)
        if (!input.attr("disabled")) {
            star.attr("role", "button")
        }
        container.append(star)
    }
}

updateStars = function (container, value) {
    container.find("i").each(function () {
        var starValue = $(this).attr("data-value")
        var starClass = starValue <= Math.round(value) ? "fas fa-star" : "far fa-star"
        if (starValue == Math.ceil(value) && value % 1 >= 0.25 && value % 1 <= 0.75) {
            starClass = "fas fa-star-half-alt";
        }
        $(this).attr("class", starClass)
    })
}

initMultiSelect = function () {
    $('.multi-select').multiselect({
        maxHeight: 160,
        buttonClass: 'custom-select d-flex justify-content-start',
        buttonText: function (options, select) {
            var selected = options.length === 0 ? $('.multi-select').data("placeholder") + "  " : '';
            options.each(function () {
                selected += $(this).text() + ', ';
            });
            return selected.substr(0, selected.length - 2);
        }
    });
    $('.multiselect-container').attr("class", "multiselect-container dropdown-menu w-100");
}
initRatingStars = function () {
    $(".rating-stars").each(function () {
        if (!$(this).attr("data-init")) {
            $(this).attr("data-init", true)
            var input = $(this).find("input").attr("type", "number").attr("min", 0).attr("hidden", true);
            var stars = $("<div>").attr("class", "text-primary")
            createStars(stars, input)
            updateStars(stars, input.val())
            $(this).append(stars)
            stars.find("i").on("click", function () {
                if (!input.attr("disabled")) {
                    input.val($(this).data("value"))
                    updateStars(stars, input.val())
                }
            })
        }
    })
}
initQuantityPlusMinus = function () {
    $('.quantity button').on('click', function () {
        var button = $(this);
        var input = button.parent().parent().find('input');
        var min = parseFloat(input.attr("min") ?? Number.MIN_VALUE)
        var max = parseFloat(input.attr("max") ?? Number.MAX_VALUE)
        var oldValue = parseFloat(input.val());
        if (button.hasClass('btn-plus')) {
            input.val(oldValue < max ? oldValue + 1 : max);
            input.trigger("change")
        }
        if (button.hasClass('btn-minus')) {
            input.val(oldValue > min ? oldValue - 1 : min);
            input.trigger("change")
        }
    });
}

showInPopup = function (url, title) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal("show");
            initMultiSelect();
            initRatingStars();
        }
    })
}
AjaxPost = function (form) {
    $.ajax({
        type: "POST",
        url: form.action,
        data: new FormData(form),
        contentType: false,
        processData: false,
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            if (!$("#form-modal .validation-summary-errors").data()) {
                $("#form-modal").modal("hide");
                $("#form-modal").trigger("success");
            }
            initMultiSelect();
            initRatingStars();
        }
    })
    return false;
}
AjaxDelete = function (url) {
    $.ajax({
        type: "GET",
        url: url
    }).done(function () {
        $("#form-modal").trigger("success")
    })
}
AddToCart = function (productId, amount) {
    $.ajax({
        type: "GET",
        url: "/cart/add",
        data: {
            productId: productId,
            amount: amount
        },
        success: function () {
            $.notify("Product added to cart!", {
                globalPosition: 'top center',
                showDuration: 700,
                className: "success"
            });
            $(document).trigger("cartUpdate")
        },
        error: function (xhr) {
            $.notify(xhr.responseText, {
                globalPosition: 'top center',
                showDuration: 700,
                className: "error"
            });
        }
    })
}
var loadTable = function (url) {
    var field = $("[name=field]").val();
    var search = $("[name=search]").val();
    $.ajax({
        type: "GET",
        url: url,
        data: {
            field: field,
            search: search
        },
        success: function (res) {
            $("#container").html(res);
        },
        error: function (xhr) {
            $.notify(xhr.responseText ?? "Error!", {
                globalPosition: 'top center',
                showDuration: 700,
                className: "error"
            });
        }
    })
}

$(document).ready(function () {
    initMultiSelect()
    initRatingStars()
    initQuantityPlusMinus()

    $(this).on("cartUpdate", function () {
        $.ajax({
            type: "GET",
            url: "/cart/itemscount",
            success: function (res) {
                $(".cart-items-count").text(res)
            }
        })
    })
    $(this).trigger("cartUpdate")

    $("#products-search").on("change", function () {
        location.href = "/shop?s=" + $(this).val()
    })
});
