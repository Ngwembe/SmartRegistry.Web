"use strict";

let connection = new signalR.HubConnectionBuilder()
    .withUrl("/messages")
    .build();

connection.on("ReceiveMessage", (message) => {
    let msg = message.replace(/b/g, "&amp;").replace(/</g, "&alt;")
        .replace(/>/g, "&gt;");

    let div = document.createElement("div");
    div.innerHTML = msg + "<hr/>";

    document.getElementById("messages").appendChild(div);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", (event) => {
    let message = document.getElementById("message").nodeValue;

    connection.invoke("SendMessageToAll", message).catch(function (err) {
        return console.error(err.toString());
    });

    event.preventDefault();
});
