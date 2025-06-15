import { Node, mergeAttributes } from '@tiptap/core'
import type { CommandProps, RawCommands } from '@tiptap/core'
import { VueNodeViewRenderer } from '@tiptap/vue-3'
import ResizableDraggableImage from '@/components/editor/ResizableDraggableImage.vue'

export const ResizableDraggableImageExtension = Node.create({
  name: 'image',
  group: 'inline',
  inline: true,
  draggable: true,
  selectable: true,

  addAttributes() {
    return {
      src: { default: null },
      alt: { default: null },
      title: { default: null },
      style: {
        default: 'width: 300px; height: auto;',
        parseHTML: element => element.getAttribute('style'),
        renderHTML: attributes => (attributes.style ? { style: attributes.style } : {}),
      },
    }
  },

  parseHTML() {
    return [{ tag: 'img[src]' }]
  },

  renderHTML({ HTMLAttributes }) {
    return ['img', mergeAttributes(HTMLAttributes)]
  },

  addCommands() {
    return {
      setImage:
        (options: { src: string; alt?: string; title?: string; style?: string }) =>
        ({ commands }: CommandProps) => {
          return commands.insertContent({
            type: this.name,
            attrs: options,
          })
        },
    } as any // <- força o TS a aceitar, já que setImage é comando customizado
  },

  addNodeView() {
    return VueNodeViewRenderer(ResizableDraggableImage)
  },
})
