import { WebSocket } from 'ws';

const ws = new WebSocket('ws://localhost:9001', {
    perMessageDeflate: false
});

ws.addEventListener('message', event => {
    console.log('Message received from server');
    console.log(event.data);

    const testData = {
        something: "Woo lad",
        somethingElse: 1,
        andAnother: true
    }

    ws.send(JSON.stringify(testData));
});