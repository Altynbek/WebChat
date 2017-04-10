$("#inputSearchField").on('change keyup paste', function () {
    var searchPattern = $(this).val();
    if ($('#contacts-btn').hasClass('active')) {
        SearchForContacts(searchPattern);
    }
    else {
        SearchForGroups(searchPattern);
    }
});


function SearchForContacts(searchPattern) {
    if (!searchPattern) {
        $('.not-added').remove();
        $('#btnCommonSearch').addClass('hidden');
        $(".user-contact").removeClass('hidden');
    }
    else {
        $('#btnCommonSearch').removeClass('hidden');
        FilterContactsBy(searchPattern);
    }
}


function SearchForGroups(searchPattern) {
    $('#btnCommonSearch').addClass('hidden');
    if (!searchPattern)
        $(".user-contact").removeClass('hidden');
    else
        FilterContactsBy(searchPattern);
}


$("#inputSearchField").keypress(function (e) {
    if (e.keyCode == 13) {
        FindUser()
    }
});

$("#btnCommonSearch").click(function () {
    FindUser();
});

function FilterContactsBy(pattern) {
    $(".user-contact:not([data-name*='" + pattern + "' i])").addClass('hidden');
    $(".user-contact[data-name*='" + pattern + "' i]").removeClass('hidden');
}

function FindUser() {
    if ($('#btnCommonSearch').hasClass('hidden') == false) {
        var searchedString = $("#inputSearchField").val();
        if (searchedString != undefined && searchedString.length > 0) {
            $.ajax({
                url: "/Account/FindUsers",
                data: { "searchedString": searchedString },
                success: function (result) {
                    $('#search-result-container').html(result);
                }
            });
        }
    }
}

function AddContact(contactRow) {
    if (!$(contactRow).hasClass("contact-selected")) {
        $(contactRow).addClass("contact-selected");
    }

    var contactId = $(contactRow).data("contact-id");
    $.ajax({
        url: "/Contact/AddNewContact",
        data: { "contactId": contactId },
        success: function (result) {
            $('#dialogue-container').html(result);
        }
    });
}

function StartDialogue(contactRow) {
    $('.contact-selected').removeClass('contact-selected');
    if (!$(contactRow).hasClass("contact-selected")) {
        $(contactRow).addClass("contact-selected");
    }

    var contactId = $(contactRow).data("contact-id");

    $.ajax({
        url: "/Im/Index",
        data: { "contactId": contactId },
        success: function (result) {
            $('#dialogue-container').html(result);
            if ($(contactRow).data("has-new-message") == "True") {
                var dialogueId = $('#dialogueId').val();
                MarkDialogueAsReaded(dialogueId, contactRow);
            }
            scrollToBottom(document.getElementById('message-list'));
        }
    });
}

function StartGroupDialogue(groupRow) {
    $('.contact-selected').removeClass('contact-selected');
    if (!$(groupRow).hasClass("contact-selected")) {
        $(groupRow).addClass("contact-selected");
    }

    var dialogueId = $(groupRow).data("dialogue-id");

    $.ajax({
        url: "/Im/StartGroupDialogue",
        data: { "dialogueId": dialogueId },
        success: function (result) {
            $('#dialogue-container').html(result);
            if ($(groupRow).data("has-new-message") == "True") {
                var dialogueId = $('#dialogueId').val();
                MarkDialogueAsReaded(dialogueId, groupRow);
            }
            scrollToBottom(document.getElementById('message-list'));
        }
    });
}

function MarkDialogueAsReaded(dialogueId, selectedContact) {
    $.ajax({
        url: "/Im/MarkDialogueAsReaded",
        data: { "dialogueId": dialogueId },
        type: "post",
        success: function (result) {
            if (result.success == true) {
                $(selectedContact).find('.new-message').addClass('hidden');
            }
        }
    });
}

function CheckResult(result) {
    if (result != undefined && result.success) {
        $('#dialogue-container').empty();
        ShowAlert("success", "This user added to your contact book. Please wait until this user will confirm your invitation", 'dialogue-container');
        setTimeout(function () {
            window.location.href = window.location.href;
        }, 1000)
    }
    else {
        ShowAlert("Error", "Unfortunately we could not proceed your request. Please, try again later! ", 'dialogue-container');
    }
}

function ShowAlert(alertType, message, containerId) {
    var panelType = "";
    if (alertType == "success") {
        panelType = "panel-success";
    }
    else if (alertType == "error") {
        panelType = "panel-danger";
    }
    else if (alertType == "warning") {
        panelType = "panel-warning";
    }
    else if (alertType == "info") {
        panelType = "panel-info";
    }
    var htmlMarkup = '<div class="panel ' + panelType + '" id="notification"><div class="panel-heading">' + message + '</div></div>';
    $('#' + containerId).html(htmlMarkup);
    setTimeout(function () {
        $('#notification').remove();
    }, 5000)
}

function DeclineContact(companionId) {
    $.ajax({
        url: "/Contact/DeleteContact",
        data: { "companionId": companionId },
        type: "post",
        success: function (result) {
            $('.contact-selected').remove();
            $('#dialogue-container').empty();
            $('.user-contact').removeClass('contact-selected');
        }
    });
}

function AcceptContact(companionId) {
    $.ajax({
        url: "/Contact/AcceptContact",
        data: { "companionId": companionId },
        type: "post",
        success: function (result) {
            CheckResult(result);
        }
    });
}


function GetContacts() {
    $.ajax({
        url: "/Contact/GetContactsForGroupDialogues",
        success: function (result) {
            $('#modal-body').html(result);
            $('#contac-list-modal').modal('show');
        }
    });
};

function ClearMessageBox() {
    $('#message-box').val("");
    autosize.update(document.querySelector('textarea'));
}

function ProcessMessage() {
    var dialogueId = $('#dialogueId').val();
    var message = $('#message-box').val();
    var companionId = $('#companionId').val();

    if (!message)
        return;

    $.ajax({
        url: "/Im/SendMessage",
        data: { "dialogueId": dialogueId, "companionId": companionId, "message": message },
        type: "post",
        success: function (result) {
            PushMessage(result);
            ClearMessageBox();
        }
    });
}

function PushMessage(message) {
    const messageList = document.getElementById('message-list');
    var shouldScroll = messageList.scrollTop + messageList.clientHeight === messageList.scrollHeight;

    $('#message-list').append(message)
    if (shouldScroll) {
        scrollToBottom(messageList);
    }
}

function scrollToBottom(messageContainer) {
    if (messageContainer == null)
        return;

    messageContainer.scrollTop = messageContainer.scrollHeight;
}

$('#groups-btn').click(function () {
    $('#search-result-container').empty();
    if ($('#btnCommonSearch').hasClass("hidden") == false) {
        $('#btnCommonSearch').addClass("hidden")
    }

    $.ajax({
        url: "/Im/GetGroups",
        success: function (result) {
            $('#group-list').empty();
            $('#group-list').html(result);
        }
    });
});


$('#create-group-btn').on('click', function () {
    if ($('#group-contact-form').valid()) {
        $.ajax({
            url: "/Im/CreateGroup",
            type: 'POST',
            dataType: 'json',
            data: $("#group-contact-form").serialize(),
            async: false,
            success: function (result) {
                if (result.success == true) {
                    $('#group-list').prepend(result.htmlMarkup)
                    $('#dialogue-' + result.createdDialogueId).click();
                    $('#contac-list-modal').modal('hide');
                }
            },
        });
    }
    else {
        event.preventDefault();
    }
});


$('#contacts-btn').on('click', function () {
    $.ajax({
        url: "/Contact/GetContacts",
        success: function (result) {
            $('#contact-list').empty();
            $('#contact-list').html(result);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Status: " + textStatus); alert("Error: " + errorThrown);
        }
    });
});
