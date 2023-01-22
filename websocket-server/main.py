import asyncio
import server


async def main():
    await server.start()


asyncio.run(main())
