"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/myHub").build();

connection.on("BCNewData",  () => {
    console.log("NewData Triggered!")
    getAppointment()
})

connection.start().then(() => {
    console.log("Init SingalR")
}).catch(function (err) {
    return console.error(err.toString());
});