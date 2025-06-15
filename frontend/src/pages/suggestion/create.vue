<template>
  <v-container class="pa-2" max-width="1200">
    <v-card class="pa-6" elevation="3">
      <v-card-title class="text-h6">Nova Sugestão</v-card-title>
      <v-card-text>
        <!-- Título e Categoria -->
        <v-row class="align-center mb-4" no-gutters>
          <v-col>
            <v-text-field
              v-model="form.title"
              label="Título"
              density="compact"
              required
            />
          </v-col>
          <v-col cols="auto" class="ml-2">
            <v-select
              :items="categories"
              item-value="id"
              item-title="name"
              :model-value="form.categoryId"
              @update:modelValue="val => form.categoryId = val as number | null"
              label="Categoria"
              density="compact"
              style="width: 10rem"
              required
            />
          </v-col>
        </v-row>

        <!-- Descrição -->
        <label class="text-subtitle-2 mt-4 mb-1">Descrição</label>

        <!-- Toolbar -->
        <div class="toolbar mb-2 d-flex align-center justify-center flex-wrap">
          <!-- Tamanho da fonte -->
          <v-select
            :items="['12px', '14px', '16px', '18px', '24px', '32px']"
            v-model="selectedFontSize"
            @update:modelValue="applyFontSize"
            label="Tamanho"
            density="compact"
            hide-details
            style="max-width: 100px;"
          />

          <!-- Negrito / Itálico -->
          <v-btn
            @click="editor.chain().focus().toggleBold().run()"
            :color="editor.isActive('bold') ? 'primary' : undefined"
            size="small"
          >
            <v-icon>mdi-format-bold</v-icon>
          </v-btn>
          <v-btn
            @click="editor.chain().focus().toggleItalic().run()"
            :color="editor.isActive('italic') ? 'primary' : undefined"
            size="small"
          >
            <v-icon>mdi-format-italic</v-icon>
          </v-btn>

          <!-- Cor -->
          <v-menu
            v-model="colorMenu"
            offset-y
            :close-on-content-click="false"
            max-width="300"
          >
            <template #activator="{ props }">
              <v-btn
                v-bind="props"
                size="small"
                :style="{
                  backgroundColor: selectedColor,
                  color: isLightColor(selectedColor) ? '#000' : '#fff',
                }"
              >
                <v-icon>mdi-format-color-fill</v-icon>
              </v-btn>
            </template>
            <v-card>
              <v-color-picker
                v-model="tempColor"
                hide-canvas
                hide-inputs
                show-swatches
                mode="hexa"
                swatches-max-height="150"
              />
              <v-card-actions class="justify-end">
                <v-btn color="primary" @click="applyColor(editor)">Aplicar</v-btn>
              </v-card-actions>
            </v-card>
          </v-menu>

          <!-- Listas -->
          <v-btn
            @click="applyBulletList"
            :color="editor.isActive('bulletList') ? 'primary' : undefined"
            size="small"
          >
            <v-icon>mdi-format-list-bulleted</v-icon>
          </v-btn>
          <v-btn
            @click="applyOrderedList"
            :color="editor.isActive('orderedList') ? 'primary' : undefined"
            size="small"
          >
            <v-icon>mdi-format-list-numbered</v-icon>
          </v-btn>

          <!-- Alinhamento -->
          <v-btn
            @click="editor.chain().focus().setTextAlign('left').run()"
            :color="editor.isActive({ textAlign: 'left' }) ? 'primary' : undefined"
            size="small"
          >
            <v-icon>mdi-format-align-left</v-icon>
          </v-btn>
          <v-btn
            @click="editor.chain().focus().setTextAlign('center').run()"
            :color="editor.isActive({ textAlign: 'center' }) ? 'primary' : undefined"
            size="small"
          >
            <v-icon>mdi-format-align-center</v-icon>
          </v-btn>
          <v-btn
            @click="editor.chain().focus().setTextAlign('right').run()"
            :color="editor.isActive({ textAlign: 'right' }) ? 'primary' : undefined"
            size="small"
          >
            <v-icon>mdi-format-align-right</v-icon>
          </v-btn>

          <!-- Inserir Imagem -->
          <v-btn color="success" size="small" @click="openImageDialog = true">
            <v-icon>mdi-image</v-icon>
          </v-btn>

          <!-- Tela cheia -->
          <v-btn
            color="secondary"
            class="ml-auto"
            size="small"
            @click="openFullScreenEditor = true"
          >
            <v-icon left>mdi-fullscreen</v-icon> Tela Cheia
          </v-btn>
        </div>

        <!-- Editor -->
        <div class="wysiwyg" @click="editor.commands.focus()">
          <EditorContent :editor="editor" />
        </div>
      </v-card-text>

      <v-card-actions>
        <v-spacer />
        <v-hover v-slot:default="{ isHovering, props }">
          <v-btn v-bind="props" :variant="isHovering ? 'outlined' : 'elevated'" color="primary" @click="submitSuggestion">
            Salvar
          </v-btn>
        </v-hover>
      </v-card-actions>
    </v-card>

    <!-- Modal Tela Cheia -->
    <v-dialog v-model="openFullScreenEditor" fullscreen hide-overlay persistent>
      <v-card>
        <v-toolbar dense flat>
          <v-btn icon @click="closeFullScreen">
            <v-icon>mdi-close</v-icon>
          </v-btn>
          <v-toolbar-title>Edição em Tela Cheia</v-toolbar-title>
          <div class="toolbar mb-2 d-flex align-center justify-center flex-wrap">
            <!-- Tamanho da fonte -->
            <v-select
              :items="['12px', '14px', '16px', '18px', '24px', '32px']"
              v-model="selectedFontSize"
              @update:modelValue="applyFontSize"
              label="Tamanho"
              density="compact"
              hide-details
              style="max-width: 100px;"
            />

            <!-- Negrito / Itálico -->
            <v-btn
              @click="editorFullScreen.chain().focus().toggleBold().run()"
              :color="editorFullScreen.isActive('bold') ? 'primary' : undefined"
              size="small"
            >
              <v-icon>mdi-format-bold</v-icon>
            </v-btn>
            <v-btn
              @click="editorFullScreen.chain().focus().toggleItalic().run()"
              :color="editorFullScreen.isActive('italic') ? 'primary' : undefined"
              size="small"
            >
              <v-icon>mdi-format-italic</v-icon>
            </v-btn>

            <!-- Cor -->
            <v-menu
              v-model="colorMenuFullScreen"
              offset-y
              :close-on-content-click="false"
              max-width="300"
            >
              <template #activator="{ props }">
                <v-btn
                  v-bind="props"
                  size="small"
                  :style="{
                    backgroundColor: selectedColor,
                    color: isLightColor(selectedColor) ? '#000' : '#fff',
                  }"
                >
                  <v-icon>mdi-format-color-fill</v-icon>
                </v-btn>
              </template>
              <v-card>
                <v-color-picker
                  v-model="tempColor"
                  hide-canvas
                  hide-inputs
                  show-swatches
                  mode="hexa"
                  swatches-max-height="150"
                />
                <v-card-actions class="justify-end">
                  <v-btn color="primary" @click="applyColor(editorFullScreen)">Aplicar</v-btn>
                </v-card-actions>
              </v-card>
            </v-menu>

            <!-- Listas -->
            <v-btn
              @click="applyBulletList"
              :color="editorFullScreen.isActive('bulletList') ? 'primary' : undefined"
              size="small"
            >
              <v-icon>mdi-format-list-bulleted</v-icon>
            </v-btn>
            <v-btn
              @click="applyOrderedList"
              :color="editorFullScreen.isActive('orderedList') ? 'primary' : undefined"
              size="small"
            >
              <v-icon>mdi-format-list-numbered</v-icon>
            </v-btn>

            <!-- Alinhamento -->
            <v-btn
              @click="editorFullScreen.chain().focus().setTextAlign('left').run()"
              :color="editorFullScreen.isActive({ textAlign: 'left' }) ? 'primary' : undefined"
              size="small"
            >
              <v-icon>mdi-format-align-left</v-icon>
            </v-btn>
            <v-btn
              @click="editorFullScreen.chain().focus().setTextAlign('center').run()"
              :color="editorFullScreen.isActive({ textAlign: 'center' }) ? 'primary' : undefined"
              size="small"
            >
              <v-icon>mdi-format-align-center</v-icon>
            </v-btn>
            <v-btn
              @click="editorFullScreen.chain().focus().setTextAlign('right').run()"
              :color="editorFullScreen.isActive({ textAlign: 'right' }) ? 'primary' : undefined"
              size="small"
            >
              <v-icon>mdi-format-align-right</v-icon>
            </v-btn>

            <!-- Inserir Imagem -->
            <v-btn color="success" size="small" @click="openImageDialog = true">
              <v-icon>mdi-image</v-icon>
            </v-btn>
          </div>
          <v-spacer />
          <v-btn color="primary" @click="saveFullScreenContent">Salvar</v-btn>
        </v-toolbar>
        <v-card-text style="height: 80vh; overflow-y: auto; padding: 16px;">
          <EditorContent :editor="editorFullScreen" />
        </v-card-text>
      </v-card>
    </v-dialog>

    <!-- Modal de Inserção de Imagem -->
    <v-dialog v-model="openImageDialog" max-width="500">
      <v-card>
        <v-card-title>Inserir Imagem</v-card-title>
        <v-card-text>
          <v-text-field v-model="imageUrl" label="URL da Imagem" />
          <small>Ex: https://exemplo.com/imagem.jpg</small>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click="openImageDialog = false">Cancelar</v-btn>
          <v-btn color="primary" @click="insertImage">Inserir</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { createSuggestion } from '@/services/suggestion'
import { fetchCategories } from '@/services/category'

import { Editor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import TextAlign from '@tiptap/extension-text-align'
import Color from '@tiptap/extension-color'
import TextStyle from '@tiptap/extension-text-style'
import { Mark, Node, mergeAttributes } from '@tiptap/core'
import type { CategoryDTO } from '@/types/category/categoryDTO'

const router = useRouter()
const auth = useAuthStore()
const categories = ref<CategoryDTO[]>([])

const form = ref({
  title: '',
  description: '',
  categoryId: null as number | null,
})

const selectedFontSize = ref('16px')
const selectedColor = ref('#000000')
const tempColor = ref('#000000')
const colorMenu = ref(false)
const colorMenuFullScreen = ref(false)
const openFullScreenEditor = ref(false)
const openImageDialog = ref(false)
const imageUrl = ref('')

// Custom mark para tamanho da fonte
const FontSize = Mark.create({
  name: 'fontSize',
  addOptions() {
    return { HTMLAttributes: {} }
  },
  parseHTML() {
    return [{ style: 'font-size' }]
  },
  renderHTML({ HTMLAttributes }) {
    return ['span', mergeAttributes(HTMLAttributes), 0]
  },
  addAttributes() {
    return {
      style: {
        default: null,
        parseHTML: (element: HTMLElement) => element.style.fontSize || null,
        renderHTML: (attributes: any) =>
          attributes.style ? { style: `font-size: ${attributes.style}` } : {},
      },
    }
  },
})

// Custom image node
const CustomImageNode = Node.create({
  name: 'customImage',
  inline: true,
  group: 'inline',
  draggable: true,
  selectable: true,
  addAttributes() {
    return {
      src: { default: null },
      alt: { default: null },
      title: { default: null },
      style: {
        default: 'width: 300px; height: auto;',
        parseHTML: (element: HTMLElement) => element.getAttribute('style'),
        renderHTML: (attributes: any) =>
          attributes.style ? { style: attributes.style } : {},
      },
    }
  },
  parseHTML() {
    return [{ tag: 'img[src]' }]
  },
  renderHTML({ HTMLAttributes }: { HTMLAttributes: any }) {
    return ['img', mergeAttributes(HTMLAttributes)]
  },
})

// Criando o editor
const editor = new Editor({
  content: '',
  extensions: [
    StarterKit,
    CustomImageNode,
    TextStyle,
    Color,
    TextAlign.configure({ types: ['heading', 'paragraph'] }),
    FontSize,
  ],
  onUpdate: ({ editor }) => {
    form.value.description = editor.getHTML()
  },
})

const editorFullScreen = new Editor({
  content: '',
  extensions: [
    StarterKit.configure({}),
    CustomImageNode,
    TextStyle,
    Color,
    TextAlign.configure({ types: ['heading', 'paragraph'] }),
    FontSize,
  ],
})

watch(openFullScreenEditor, (newVal) => {
  if (newVal) {
    // Ao abrir fullscreen, copia conteúdo do editor normal para o fullscreen
    editorFullScreen.commands.setContent(editor.getHTML())
  }
})



// Inserir imagem diretamente com insertContent
function insertImage() {
  if (!imageUrl.value) return

  editor
    .chain()
    .focus()
    .insertContent({
      type: 'customImage',
      attrs: {
        src: imageUrl.value,
        alt: 'Imagem',
        style: 'width: 300px; height: auto; border-radius: 8px; margin: 0.5rem 0;',
      },
    })
    .run()

  openImageDialog.value = false
  imageUrl.value = ''
}

// Aplicar cor de texto
function applyColor(targetEditor: Editor) {
  selectedColor.value = tempColor.value
  targetEditor.chain().focus().setColor(selectedColor.value).run()
  colorMenu.value = false
  colorMenuFullScreen.value = false
}


// Aplicar tamanho de fonte
function applyFontSize(size: string) {
  editor.chain().focus().setMark('fontSize', { style: size }).run()
}

// Aplicar listas
function applyBulletList() {
  editor.chain().focus().toggleBulletList().run()
}

function applyOrderedList() {
  editor.chain().focus().toggleOrderedList().run()
}

// Verificar se cor é clara (para contraste do ícone)
function isLightColor(hex: string): boolean {
  if (!hex) return true
  const c = hex.substring(1)
  const rgb = parseInt(c, 16)
  const r = (rgb >> 16) & 0xff
  const g = (rgb >> 8) & 0xff
  const b = rgb & 0xff
  const luminance = 0.299 * r + 0.587 * g + 0.114 * b
  return luminance > 186
}

// Salvar sugestão
async function submitSuggestion() {
  if (!form.value.title || !form.value.description || !form.value.categoryId) {
    alert('Preencha todos os campos.')
    return
  }

  if (!auth.userId) {
    alert('Usuário não autenticado')
    return
  }

  try {
    await createSuggestion({
      ...form.value,
      categoryId: form.value.categoryId!,
      userId: auth.userId,
    })

    router.push('/suggestion')
  } catch (error) {
    alert('Erro ao criar sugestão: ' + (error instanceof Error ? error.message : ''))
  }
}

// Salvar conteúdo da edição em tela cheia
function saveFullScreenContent() {
  const content = editorFullScreen.getHTML()
  editor.commands.setContent(content)
  form.value.description = content
  openFullScreenEditor.value = false
}

function closeFullScreen(){
  openFullScreenEditor.value = false;
  saveFullScreenContent()
}

onMounted(async () => {
  categories.value = await fetchCategories()
})
</script>

<style scoped>
.wysiwyg {
  border: 1px solid #ccc;
  border-radius: 6px;
  padding: 0 1.4rem;
  min-height: 200px;
  max-height: 400px;
  overflow-y: auto;
  overflow-x: hidden;
  cursor: text;
}

.wysiwyg img {
  max-width: 8500px !important;
  height: auto;
  border-radius: 8px;
  margin: 0.5rem 0;
  display: block;
}

.toolbar {
  display: flex;
  gap: 4px;
  flex-wrap: wrap;
}
</style>
