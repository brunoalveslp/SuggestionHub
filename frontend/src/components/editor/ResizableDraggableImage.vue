<template>
  <div
    class="image-wrapper"
    ref="wrapper"
    @mousedown.prevent="startDrag"
    :style="wrapperStyle"
  >
    <img
      :src="node.attrs.src"
      :alt="node.attrs.alt || ''"
      :title="node.attrs.title || ''"
      class="resizable-image"
      ref="image"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, computed } from 'vue'
import type { NodeViewProps } from '@tiptap/vue-3'
import type { CSSProperties } from 'vue'

const props = defineProps<NodeViewProps>()

const wrapper = ref<HTMLElement>()
const image = ref<HTMLImageElement>()
const isDragging = ref(false)
let offsetX = 0
let offsetY = 0

const wrapperStyle = computed<CSSProperties>(() => ({
  position: 'absolute',
  display: 'inline-block',
  cursor: 'move',
}))

function startDrag(e: MouseEvent) {
  if (!wrapper.value) return
  isDragging.value = true
  const rect = wrapper.value.getBoundingClientRect()
  offsetX = e.clientX - rect.left
  offsetY = e.clientY - rect.top
  document.addEventListener('mousemove', onDrag)
  document.addEventListener('mouseup', stopDrag)
}

function onDrag(e: MouseEvent) {
  if (!isDragging.value || !wrapper.value) return
  wrapper.value.style.left = `${e.clientX - offsetX}px`
  wrapper.value.style.top = `${e.clientY - offsetY}px`
}

function stopDrag() {
  isDragging.value = false
  document.removeEventListener('mousemove', onDrag)
  document.removeEventListener('mouseup', stopDrag)
}

let observer: ResizeObserver | null = null

onMounted(() => {
  if (!image.value) return
  observer = new ResizeObserver(() => {
    const width = image.value?.clientWidth
    const height = image.value?.clientHeight
    const style = `width: ${width}px; height: ${height}px;`
    props.updateAttributes({ style })
  })
  observer.observe(image.value)
})

onBeforeUnmount(() => {
  observer?.disconnect()
})
</script>

<style scoped>
.image-wrapper {
  border: 1px dashed transparent;
  resize: both;
  overflow: auto;
  min-width: 50px;
  min-height: 50px;
}

.resizable-image {
  display: block;
  max-width: 100%;
  height: auto;
  pointer-events: none;
}
</style>
