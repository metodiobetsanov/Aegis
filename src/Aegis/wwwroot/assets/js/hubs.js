"use strict";

window.connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub")
    .configureLogging(signalR.LogLevel.Error)
    .build();

async function start() {
    try {
        await connection.start();
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});

connection.on('info', (message) => {
    toastr['info'](message)
});

connection.on('success', (message) => {
    toastr['success'](message)
});

connection.on('warning', (message) => {
    toastr['warning'](message)
});

connection.on('error', (message) => {
    toastr['error'](message)
});

connection.on('notification', (message) => {
    console.log(message)
});

// Start the connection.
start();