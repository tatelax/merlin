import asyncio
import websockets


async def receive(websocket):
    async for message in websocket:
        await websocket.send(message)

async def start():
        async with websockets.serve(receive, "localhost", 8765, subprotocols=["test", "hello"]):
            await asyncio.Future()  # run forever