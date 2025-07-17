import * as signalR from "@microsoft/signalr";

// Backend Hub'ımızın adresini belirtiyoruz. Port numaranızı kontrol edin.
const HUB_URL = "https://localhost:7007/dashboardHub"; 

// Bağlantıyı oluşturuyoruz.
const connection = new signalR.HubConnectionBuilder()
    .withUrl(HUB_URL, {
        // Login olduğumuzda token'ı da gönderebilmek için hazırlık
        // Bu satır, localStorage'dan token'ı alıp her bağlantı denemesinde kullanır.
        accessTokenFactory: () => localStorage.getItem('token')
    })
    .withAutomaticReconnect() // Bağlantı koparsa otomatik olarak tekrar denemesini sağlar.
    .build();

// Bağlantıyı başlatan fonksiyon.
const startConnection = async () => {
    // Eğer bağlantı zaten kurulu veya kuruluyor değilse başlat.
    if (connection.state === signalR.HubConnectionState.Disconnected) {
        try {
            await connection.start();
            console.log("SignalR Bağlantısı Başarılı.");
        } catch (err) {
            console.error("SignalR Bağlantı Hatası: ", err);
            // Bağlantı başarısız olursa 5 saniye sonra tekrar dene.
            setTimeout(startConnection, 5000);
        }
    }
};

// Bağlantıyı durduran fonksiyon
const stopConnection = () => {
    if (connection.state === signalR.HubConnectionState.Connected) {
        connection.stop();
    }
};

// Backend'den gelen belirli bir mesajı dinlemek için bir fonksiyon.
const on = (eventName, callback) => {
    connection.on(eventName, callback);
};

// Bir dinleyiciyi kaldırmak için fonksiyon
const off = (eventName, callback) => {
    connection.off(eventName, callback);
};


export default {
    startConnection,
    stopConnection,
    on,
    off
};