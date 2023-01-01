<script setup lang="ts">
import YAML from 'yaml'

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
  <button v-for="s in sessions" :key="s.sessionID" class="underline text-sky-500" @click="loadSession(s)">
    Session {{ s.sessionID }}
  </button>
  <pre v-if="sessionData.length" class="text-sm" v-text="YAML.stringify(sessionData)" />
</template>
