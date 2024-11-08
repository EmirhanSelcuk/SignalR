﻿$(document).ready(function () {

    const connection = new signalR.HubConnectionBuilder().withUrl("/ExampleTypeSafeHub")
        .configureLogging(signalR.LogLevel.Information).build();

    const broadcastMessageToAllClientMethodCall = "BroadcastMessageToAllClient";
    const receiveMessageForallClientClientMethodCall = "ReceiveMessageForallClient"


    const broadcastMessageToCallerClient = "BroadcastMessageToCallerClient"
    const receiveMessageForCallerClient = "ReceiveMessageForCallerClient"

    const broadcastMessageToOthersClient = "BroadcastMessageToOthersClient"
    const receiveMessageForOthersClient = "ReceiveMessageForOthersClient"


    const broadcastMessageToIndividualClient = "BroadcastMessageToIndividualClient"
    const receiveMessageForIndividualClient = "ReceiveMessageForIndividualClient"


    const receiveConnectedClientCountAllClient = "ReceiveConnectedClientCountAllClient"


    const receiveTypedMessageForallClient = "ReceiveTypedMessageForallClient";
    const broadcastTypedMessageToAllClient = "BroadcastTypedMessageToAllClient";



    const groupA = "Group A";
    const groupB = "Group B";
    let currentGroupList = [];


    async function start() {

        try {

            await connection.start().then(() => {
                console.log("Hub ile Bağlantı kuruldu");
                $("#connectionId").html(`Connection Id : ${connection.connectionId}`);

            });
        }
        catch (err) {
            console.error("Hub ile bağlantı kurulamadı", err);
            setTimeout(() => start(), 5000)
        }
    }
    connection.onclose(async () => {
        await start();
    })
    start();



    function refreshGroupList() {
        $("#groupList").empty();
        currentGroupList.forEach(x => {
            $("#groupList").append(`<p>${x}</p>`)
        })
    }
    $("#btn-groupA-add").click(function () {

        if (currentGroupList.includes(groupA)) return;

        connection.invoke("AddGroup", groupA).then(() => {

            currentGroupList.push(groupA);
            refreshGroupList();

        })

    })

    $("#btn-groupA-remove").click(function () {
        if (!currentGroupList.includes(groupA)) return;

        connection.invoke("RemoveGroup", groupA).then(() => {

            currentGroupList = currentGroupList.filter(x => x != groupA);
            refreshGroupList();

        })

    })

    $("#btn-groupB-add").click(function () {

        if (currentGroupList.includes(groupB)) return;

        connection.invoke("AddGroup", groupB).then(() => {

            currentGroupList.push(groupB);
            refreshGroupList();

        })

    })

    $("#btn-groupB-remove").click(function () {
        if (!currentGroupList.includes(groupB)) return;

        connection.invoke("RemoveGroup", groupB).then(() => {

            currentGroupList = currentGroupList.filter(x => x != groupB);
            refreshGroupList();

        })

    })

    $("#btn-groupA-send-message").click(function () {
        const message = "Group A Mesaj";

        connection.invoke("BroadcastMessageToGroupClient",groupA, message).catch(err => console.error
            ("hata", err));
        console.log("Mesaj gönderildi");

    })

    $("#btn-groupB-send-message").click(function () {
        const message = "Group B Mesaj";

        connection.invoke("BroadcastMessageToGroupClient", groupB,message).catch(err => console.error
            ("hata", err));
        console.log("Mesaj gönderildi");

    })

    connection.on("ReceiveMessageForGroupClients", (message) => {
        console.log("Gelen Mesaj", message);
    })



    function start() {
        connection.start().then(() => {
            console.log("Hub ile Bağlantı kuruldu");
        $("#connectionId").html(`Connection Id : ${connection.connectionId}`);

        })
          
    }

    try {
        start();
    }
    catch
    {
        setTimeout(() => start(), 5000)
    }







    const span_client_count = $("#span-connected-client-count");
    connection.on(receiveConnectedClientCountAllClient, (count) => {
        span_client_count.text(count);
        console.log("connected client count ", count);
    })
   








    connection.on(receiveMessageForallClientClientMethodCall, (message) => {
        console.log("Gelen Mesaj", message);
    })


    connection.on(receiveTypedMessageForallClient, (product) => {
        console.log("Gelen Ürün", product);
    })

    connection.on(receiveMessageForOthersClient, (message) => {
        console.log("(Others) Gelen Mesaj", message);
    })

    connection.on(receiveMessageForCallerClient, (message) => {
        console.log("(Caller) Gelen Mesaj", message);
    })

    connection.on(receiveMessageForIndividualClient, (message) => {
        console.log("(Individual) Gelen Mesaj", message);
    })






    $("#btn-send-message-all-client").click(function () {
        const message = "Merhaba";

        connection.invoke(broadcastMessageToAllClientMethodCall, message).catch(err => console.error
            ("hata", err));
        console.log("Mesaj gönderildi");

    })
    $("#btn-send-message-caller-client").click(function () {
        const message = "Merhaba";

        connection.invoke(broadcastMessageToCallerClient, message).catch(err => console.error
            ("hata", err));
        console.log("Mesaj gönderildi");


    })

    $("#btn-send-message-others-client").click(function () {
        const message = "Merhaba";

        connection.invoke(broadcastMessageToOthersClient, message).catch(err => console.error("hata", err));
        console.log("Mesaj gönderildi");


    })
    $("#btn-send-message-individual-client").click(function () {

        const message = "Merhaba";
        const connectionId = $("#text-connectionId").val();
        connection.invoke(broadcastMessageToIndividualClient, connectionId, message).catch(err => console.error
            ("hata", err));
        console.log("Mesaj gönderildi");

    })


    $("#btn-send-typed-message-all-client").click(function () {

        const product = { id: 1, name:"pen 1", price: 200 };

        connection.invoke(broadcastTypedMessageToAllClient, product).catch(err => console.error
            ("hata", err));
        console.log("Ürün gönderildi");

    });


})