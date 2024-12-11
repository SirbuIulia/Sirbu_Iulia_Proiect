"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/ReservationNotification")
    .build();

connection.on("ConnectedToNotifications", (message) => {
    console.log(message);
});

connection.on("AccessDenied", (message) => {
    console.error(message);
    alert("Nu ai acces la notificări.");
});

connection.on("NewReservation", (resData) => {
    console.log("Notificare primită pe client:", resData);

    const notificationsList = document.getElementById("notifications-list");
    if (notificationsList) {
        const li = document.createElement("li");
        li.textContent = `Nouă rezervare: Restaurant ${resData.restaurantName}, Masă ${resData.tableNumber}, Client: ${resData.customerName}, Ora: ${resData.reservationDate}`;
        notificationsList.appendChild(li);
        notificationsList.scrollTop = notificationsList.scrollHeight; 
    }
});


connection.start()
    .then(() => console.log("Conexiunea SignalR este stabilită."))
    .catch(err => console.error("Eroare la conectare:", err));
