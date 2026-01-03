

<template>
  <div v-if="editor" class="bg-gray-50   rounded-lg border border-gray-300  ">
    <div class="p-2 border-b border-gray-300   flex flex-wrap gap-1">
      <button 
        type="button" 
        @click="editor.chain().focus().toggleBold().run()"
        :class="{'is-active bg-gray-200  ': editor.isActive('bold')}"
        class="px-2 py-1 rounded text-sm text-gray-700  hover:bg-gray-200 "
        aria-label="Negrita"
      >
        <span class="font-bold">B</span>
      </button>
      <button 
        type="button" 
        @click="editor.chain().focus().toggleItalic().run()"
        :class="{'is-active bg-gray-200  ': editor.isActive('italic')}"
        class="px-2 py-1 rounded text-sm text-gray-700  hover:bg-gray-200 "
        aria-label="Cursiva"
      >
        <span class="italic">I</span>
      </button>
      <button 
        type="button" 
        @click="editor.chain().focus().toggleUnderline().run()"
        :class="{'is-active bg-gray-200  ': editor.isActive('underline')}"
        class="px-2 py-1 rounded text-sm text-gray-700  hover:bg-gray-200 "
        aria-label="Subrayado"
      >
        <span class="underline">U</span>
      </button>
      <button 
        type="button" 
        @click="editor.chain().focus().toggleStrike().run()"
        :class="{'is-active bg-gray-200  ': editor.isActive('strike')}"
        class="px-2 py-1 rounded text-sm text-gray-700  hover:bg-gray-200 "
        aria-label="Tachado"
      >
        <span class="line-through">S</span>
      </button>
      <button 
        type="button" 
        @click="editor.chain().focus().toggleCodeBlock().run()"
        :class="{'is-active bg-gray-200  ': editor.isActive('codeBlock')}"
        class="px-2 py-1 rounded text-sm text-gray-700  hover:bg-gray-200 "
        aria-label="Bloque de código"
      >
        <span class="font-mono">{}</span>
      </button>

      <button
        type="button"
        @click="editor.chain().focus().toggleBulletList().run()"
        :class="{'is-active bg-gray-200  ': editor.isActive('bulletList')}"
        class="px-2 py-1 rounded text-sm text-gray-700  hover:bg-gray-200 "
        aria-label="Lista con viñetas"
      >• Lista
        <svg class="w-4 h-4 inline-block" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16m-7 6h7"></path></svg>
      </button>
      
      <button
        type="button"
        @click="editor.chain().focus().toggleOrderedList().run()"
        :class="{'is-active bg-gray-200  ': editor.isActive('orderedList')}"
        class="px-2 py-1 rounded text-sm text-gray-700  hover:bg-gray-200 "
        aria-label="Lista numerada"
      >
      1. Lista <svg class="w-4 h-4 inline-block" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 10h12m-9 4h9m-9 4h9"></path></svg>
      </button>

      <button 
        type="button" 
        @click="editor.chain().focus().toggleHeading({ level: 2 }).run()"
        :class="{'is-active bg-gray-200  ': editor.isActive('heading', { level: 2 })}"
        class="px-2 py-1 rounded text-sm text-gray-700  hover:bg-gray-200 "
        aria-label="Encabezado 2"
      >
        <span class="font-bold">H2</span>
      </button>
    </div>
    <EditorContent :editor="editor" class="p-4" />
  </div>
</template>

<script setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import CodeBlock from '@tiptap/extension-code-block'
import Highlight from '@tiptap/extension-highlight'
import Underline from '@tiptap/extension-underline'
import HardBreak from '@tiptap/extension-hard-break'

import { watch, onBeforeUnmount } from 'vue'

const props = defineProps({
  modelValue: {
    type: String,
    default: '',
  },
})

const emit = defineEmits(['update:modelValue'])

const editor = useEditor({
  content: props.modelValue,
  extensions: [
    StarterKit.configure({
      heading: {
        levels: [1, 2, 3],
      },
      codeBlock: false, // Deshabilitamos para usar nuestra versión
      hardBreak: false, // Deshabilitamos para usar nuestra versión
    }),
    CodeBlock,
    Highlight.configure({
      multicolor: true,
    }),
    Underline,
    HardBreak,
  ],
  editorProps: {
    attributes: {
      class: 'prose prose-sm sm:prose lg:prose-lg xl:prose-xl mx-auto focus:outline-none p-4 w-full h-[300px] overflow-y-auto text-gray-900  ',
    },
  },
  onUpdate: ({ editor }) => {
    emit('update:modelValue', editor.getHTML())
  },
})

watch(() => props.modelValue, (newValue) => {
  // if (editor.value && editor.value.getHTML() !== newValue) {
  //   editor.value.commands.setContent(newValue, false)
  // }
    if (editor.value && editor.value.getHTML() !== (newValue ?? '')) {
    editor.value.commands.setContent(newValue ?? '', false, {
      preserveWhitespace: 'full',
    })
  }
})

onBeforeUnmount(() => {
  if (editor.value) {
    editor.value.destroy()
  }
})
</script>

<style>
/* Estilos para el editor */
.ProseMirror {
  outline: none;
}
.ProseMirror > * + * {
  margin-top: 0.75em;
}

/* Estilos para las listas */
.ProseMirror ul,
.ProseMirror ol {
  padding-left: 1.5rem;
}

.ProseMirror ul {
  list-style-type: disc;
}

.ProseMirror ol {
  list-style-type: decimal;
}

.ProseMirror li p {
  margin-top: 0;
  margin-bottom: 0;
}

.ProseMirror { line-height: 1.4; }
.ProseMirror p { margin: 0 0 0.5rem; }    
.ProseMirror li p { margin: 0; }   

</style>