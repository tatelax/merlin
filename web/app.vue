<script setup lang="ts">
const { x, y } = useMouse()
</script>

<template>
  <h1 class="text-3xl font-bold underline">
    Hello world!
  </h1>
  <div>pos: {{x}}, {{y}}</div>
</template>
