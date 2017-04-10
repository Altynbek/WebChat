$(function () {
    var chat = $.connection.webChatHub;
    chat.client.updateGroupDialogue = function (dialogueId, message, senderId) {
        var currentUserId = $('#currentUserId').val();
        if (senderId == currentUserId)
            return;

        $('#dialogue-' + dialogueId).find('.new-message').removeClass('hidden');
        $('#dialogue-' + dialogueId).data('has-new-message', "True");
    };

    chat.client.updatePersonalDialogue = function (companionId, message, senderId) {
        var currentUserId = $('#currentUserId').val();
        if (senderId == currentUserId)
            return;

        $('#companion-' + senderId).find('.new-message').removeClass('hidden');
        $('#companion-' + senderId).data('has-new-message', "True");
    };

    chat.client.onConnected = function (id, userId, allUsers) {
        console.log("current user id = " + userId);
    }

    chat.client.onUserDisconnected = function (connectionId, userId) {
        console.log('disconnected');
    }

    $.connection.hub.start().done(function () {
        chat.server.connect(name);
    });
});