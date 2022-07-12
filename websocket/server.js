import uWS from "uWebSockets.js";
import {TextDecoder} from "util";
const port = 9001;

const session = new Map([
    ["sampleUserId",    {
                            worlds: [
                                        {
                                            id: 1234,
                                            name: "Hello World!"
                                        }
                                    ]
                        }
    ]
]);

const app = uWS./*SSL*/App({
    key_file_name: 'misc/key.pem',
    cert_file_name: 'misc/cert.pem',
    passphrase: '1234'
}).ws('/*', {
    /* Options */
    compression: uWS.SHARED_COMPRESSOR,
    maxPayloadLength: 16 * 1024 * 1024,
    maxBackpressure: 1024,
    idleTimeout: 16,
    /* Handlers */
    open: (ws) => {
        console.log('A WebSocket connected!');
        ws.send("Hello!");
    },
    message: (ws, message, isBinary) => {
        switch (new TextDecoder().decode(message)) {
            case "getSessions":

        }
        console.log()
    },
    drain: (ws) => {
        console.log('WebSocket backpressure: ' + ws.getBufferedAmount());
    },
    close: (ws, code, message) => {
        console.log('WebSocket closed');
    }
}).any('/*', (res, req) => {
    res.end('Nothing to see here!');
}).listen(port, (token) => {
    if (token) {
        console.log('Listening to port ' + port);
    } else {
        console.log('Failed to listen to port ' + port);
    }
});