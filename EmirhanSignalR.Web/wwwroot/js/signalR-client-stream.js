$(document).ready(function () {

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/ExampleTypeSafeHub")
        .configureLogging(signalR.LogLevel.Information).build();
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

    const broadcastStreamDataToAllClient = "BroadcastStreamDataToAllClient";
    const receiveMessageAsStreamForallClient = "ReceiveMessageAsStreamForallClient";


    const broadcastStreamProductToAllClient = "BroadcastStreamProductToAllClient";
    const receiveProductAsStreamForallClient = "ReceiveProductAsStreamForallClient";

    const broadcastFromHubToClient = "BroadcastFromHubToClient";


    connection.on(receiveMessageAsStreamForallClient, (name) => {
        $("#streamBox").append(`<p>${name}</p>`)
    })

    connection.on(receiveProductAsStreamForallClient, (product) => {
        $("#streamBox").append(`<p>${product.id} - ${product.name} - ${product.price}</p>`)
    })


    $("#btn_fromClient_toHub").click(function () {

        const names = $("#txt_stream").val();

        const namesAsChunk = names.split(";");

        const subject = new signalR.Subject();

        connection.send(broadcastStreamDataToAllClient, subject).catch(err => console.error(err))

        namesAsChunk.forEach(name => {
            subject.next(name)
        });

        subject.complete();

    })
    $("#btn_fromClient_toHub2").click(function () {

        const productList = [
            { id: 1, name: "pen 1", price: 100 },
            { id: 2, name: "pen 2", price: 200 },
            { id: 3, name: "pen 3", price: 300 }
        ]


        const subject = new signalR.Subject();
        connection.send(broadcastStreamProductToAllClient, subject).catch(err => console.error(err))

        productList.forEach(product => {
            subject.next(product)
        });

        subject.complete();

    })
    $("#btn_FromHubToClient").click(function () {
        connection.stream(broadcastFromHubToClient, 5).subscribe(
            {
                next: (message) => $("#streamBox").append(`<p>${message}</p>`)
            });
    })

    start();
});