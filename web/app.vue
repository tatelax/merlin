<script setup lang="ts">
const { x, y } = useMouse()

const appId = '6J2FnJq8hQ8mEJ5Q1vdT'
const userId = 79
const sessions = ref<any[]>([])
const sessionData = ref<any[]>([])

const loadSession = ({ sessionID }: any) => {
  const url = `ws://localhost:2414/?userID=79&appID=${appId}&sessionID=${sessionID}`
  const { status, data, send } = useWebSocket(url, { protocols: ['SessionObserver'] })
  watch(status, status => status === 'OPEN' && send('gimme state update'))
  watch(data, async data => sessionData.value.push(JSON.parse(await data.text())))
}

const getSessions = () => {
  const url = `ws://localhost:2414/ws?userID=${userId}&appID=${appId}`
  const { status, data, send } = useWebSocket(url, { protocols: ['GetSessionsList'] })
  watch(status, status => status === 'OPEN' && send('getSessions'))
  watch(data, data => sessions.value = JSON.parse(data))
}

onMounted(() => getSessions())
</script>

<template>
  <button class="underline text-sky-500" v-for="s in sessions" @click="loadSession(s)">Session {{ s.sessionID }}</button>
  <pre class="text-sm">{{ sessionData }}</pre>
</template>
