document.addEventListener('DOMContentLoaded', () => {

    //var socket = io('/')
    var serverUrl = "http://localhost:3000/";

    var socket = io(serverUrl);
    var videoGrid = document.getElementById('video-grid');
    var myPeer = new Peer(undefined,
        {
            host: '/',
            port: '3001'
        });

    var myVideo = document.createElement('video');
    myVideo.muted = true;

    var peers = {};

    navigator.mediaDevices.getUserMedia({
        video: true,
        audio: true
    }).then(stream => {
        addVideoStream(myVideo, stream);

        myPeer.on('call',
            call => {
                call.answer(stream);
                var video = document.createElement('video');
                call.on('stream',
                    userVideoStream => {
                        addVideoStream(video, userVideoStream);
                    });
            });

        socket.on('user-connected',
            userId => {
                connectToNewUser(userId, stream);
            });
    });

    socket.on('user-disconnected',
        userId => {
            if (peers[userId]) peers[userId].close();
        });

    var ROOM_ID = "";

    myPeer.on('open',
        id => {
            socket.emit('join-room', ROOM_ID, id);
        });

    function connectToNewUser(userId, stream) {
        var call = myPeer.call(userId, stream);
        var video = document.createElement('video');
        call.on('stream', userVideoStream => {
            addVideoStream(video, userVideoStream);
        });
        call.on('close', () => {
            video.remove();
        });

        peers[userId] = call;
    }

    function addVideoStream(video, stream) {
        video.srcObject = stream;
        video.addEventListener('loadedmetadata', () => {
            video.play();
        });
        videoGrid.append(video);
    }

});