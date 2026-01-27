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
      <!-- Botón para enlaces -->
      <button
        type="button"
        @click="setLink"
        :class="{'is-active bg-gray-200': editor.isActive('link')}"
        class="px-2 py-1 rounded text-sm text-gray-700 hover:bg-gray-200"
        aria-label="Enlace"
        title="Insertar/editar enlace"
      >
        <svg class="w-4 h-4 inline-block" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.828 10.172a4 4 0 010 5.656m-3.656-3.656a4 4 0 015.656 0m-7.778 7.778a4 4 0 010-5.656m3.656 3.656a4 4 0 01-5.656 0" />
        </svg>
        Enlace
      </button>

      <!-- Botón para quitar enlace -->
      <button
        v-if="editor && editor.isActive('link')"
        type="button"
        @click="editor.chain().focus().unsetLink().run()"
        class="px-2 py-1 rounded text-sm text-red-600 hover:bg-red-100"
        aria-label="Quitar enlace"
        title="Quitar enlace"
      >
        <svg class="w-4 h-4 inline-block" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 8l4 4m0 0l-4 4m4-4H7a4 4 0 01-4-4V7" />
        </svg>
        Quitar enlace
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

      <!-- Separador -->
      <span class="border-l border-gray-300 mx-1"></span>

      <!-- Botones de alineación de texto -->
      <button 
        type="button" 
        @click="editor.chain().focus().setTextAlign('left').run()"
        :class="{'is-active bg-gray-200': editor.isActive({ textAlign: 'left' })}"
        class="px-2 py-1 rounded text-sm text-gray-700 hover:bg-gray-200"
        aria-label="Alinear izquierda"
        title="Alinear a la izquierda"
      >
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h10M4 18h16"></path>
        </svg>
      </button>
      <button 
        type="button" 
        @click="editor.chain().focus().setTextAlign('center').run()"
        :class="{'is-active bg-gray-200': editor.isActive({ textAlign: 'center' })}"
        class="px-2 py-1 rounded text-sm text-gray-700 hover:bg-gray-200"
        aria-label="Centrar"
        title="Centrar texto"
      >
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M7 12h10M4 18h16"></path>
        </svg>
      </button>
      <button 
        type="button" 
        @click="editor.chain().focus().setTextAlign('right').run()"
        :class="{'is-active bg-gray-200': editor.isActive({ textAlign: 'right' })}"
        class="px-2 py-1 rounded text-sm text-gray-700 hover:bg-gray-200"
        aria-label="Alinear derecha"
        title="Alinear a la derecha"
      >
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M10 12h10M4 18h16"></path>
        </svg>
      </button>
      <button 
        type="button" 
        @click="editor.chain().focus().setTextAlign('justify').run()"
        :class="{'is-active bg-gray-200': editor.isActive({ textAlign: 'justify' })}"
        class="px-2 py-1 rounded text-sm text-gray-700 hover:bg-gray-200"
        aria-label="Justificar"
        title="Justificar texto"
      >
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"></path>
        </svg>
      </button>

      <!-- Separador -->
      <span class="border-l border-gray-300 mx-1"></span>

      <!-- Botón para insertar imagen por URL -->
      <button 
        type="button" 
        @click="showImageUrlModal = true"
        class="px-2 py-1 rounded text-sm text-gray-700  hover:bg-gray-200 "
        aria-label="Insertar imagen por URL"
        title="Insertar imagen por URL"
      >
        <svg class="w-4 h-4 inline-block" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"></path>
        </svg>
        URL
      </button>

      <!-- Botón para subir imagen -->
      <button 
        type="button" 
        @click="triggerFileInput"
        class="px-2 py-1 rounded text-sm text-gray-700  hover:bg-gray-200 "
        aria-label="Subir imagen"
        title="Subir imagen desde tu dispositivo"
      >
        <svg class="w-4 h-4 inline-block" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-8l-4-4m0 0L8 8m4-4v12"></path>
        </svg>
        Subir
      </button>
      <input 
        ref="fileInput" 
        type="file" 
        accept="image/*" 
        class="hidden" 
        @change="handleFileUpload"
      />

      <!-- Botón para insertar video de YouTube -->
      <button 
        type="button" 
        @click="showYoutubeModal = true"
        class="px-2 py-1 rounded text-sm text-gray-700 hover:bg-gray-200"
        aria-label="Insertar video de YouTube"
        title="Insertar video de YouTube"
      >
        <svg class="w-4 h-4 inline-block" viewBox="0 0 24 24" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
          <path d="M23.498 6.186a3.016 3.016 0 0 0-2.122-2.136C19.505 3.545 12 3.545 12 3.545s-7.505 0-9.377.505A3.017 3.017 0 0 0 .502 6.186C0 8.07 0 12 0 12s0 3.93.502 5.814a3.016 3.016 0 0 0 2.122 2.136c1.871.505 9.376.505 9.376.505s7.505 0 9.377-.505a3.015 3.015 0 0 0 2.122-2.136C24 15.93 24 12 24 12s0-3.93-.502-5.814zM9.545 15.568V8.432L15.818 12l-6.273 3.568z"/>
        </svg>
        YouTube
      </button>

      <!-- Controles de tamaño de imagen (aparecen cuando hay una imagen seleccionada) -->
      <template v-if="isImageSelected">
        <span class="border-l border-gray-300 mx-1"></span>
        <span class="text-xs text-gray-500 self-center mr-1">Tamaño:</span>
        <button 
          type="button" 
          @click="setImageSize('25%')"
          class="px-2 py-1 rounded text-xs text-gray-700 hover:bg-gray-200"
          title="25% del ancho"
        >
          25%
        </button>
        <button 
          type="button" 
          @click="setImageSize('50%')"
          class="px-2 py-1 rounded text-xs text-gray-700 hover:bg-gray-200"
          title="50% del ancho"
        >
          50%
        </button>
        <button 
          type="button" 
          @click="setImageSize('75%')"
          class="px-2 py-1 rounded text-xs text-gray-700 hover:bg-gray-200"
          title="75% del ancho"
        >
          75%
        </button>
        <button 
          type="button" 
          @click="setImageSize('100%')"
          class="px-2 py-1 rounded text-xs text-gray-700 hover:bg-gray-200"
          title="Ancho completo"
        >
          100%
        </button>
        <span class="border-l border-gray-300 mx-1"></span>
        <input 
          type="number" 
          v-model="customWidth"
          placeholder="Ancho px"
          class="w-16 px-1 py-1 text-xs border border-gray-300 rounded"
          @keyup.enter="setImageSize(customWidth + 'px')"
        />
        <button 
          type="button" 
          @click="setImageSize(customWidth + 'px')"
          :disabled="!customWidth"
          class="px-2 py-1 rounded text-xs text-gray-700 hover:bg-gray-200 disabled:opacity-50"
          title="Aplicar ancho personalizado"
        >
          Aplicar
        </button>

        <!-- Separador -->
        <span class="border-l border-gray-300 mx-1"></span>
        
        <!-- Botones de alineación -->
        <span class="text-xs text-gray-500 self-center mr-1">Alinear:</span>
        <button 
          type="button" 
          @click="setImageAlignment('left')"
          :class="{'bg-gray-200': currentImageAlignment === 'left'}"
          class="px-2 py-1 rounded text-xs text-gray-700 hover:bg-gray-200"
          title="Alinear a la izquierda"
        >
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h10M4 18h16"></path>
          </svg>
        </button>
        <button 
          type="button" 
          @click="setImageAlignment('center')"
          :class="{'bg-gray-200': currentImageAlignment === 'center'}"
          class="px-2 py-1 rounded text-xs text-gray-700 hover:bg-gray-200"
          title="Centrar"
        >
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M7 12h10M4 18h16"></path>
          </svg>
        </button>
        <button 
          type="button" 
          @click="setImageAlignment('right')"
          :class="{'bg-gray-200': currentImageAlignment === 'right'}"
          class="px-2 py-1 rounded text-xs text-gray-700 hover:bg-gray-200"
          title="Alinear a la derecha"
        >
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M10 12h10M4 18h16"></path>
          </svg>
        </button>
      </template>

      <!-- Controles de alineación de video (aparecen cuando hay un video seleccionado) -->
      <template v-if="isYoutubeSelected">
        <span class="border-l border-gray-300 mx-1"></span>
        <span class="text-xs text-gray-500 self-center mr-1">Alinear video:</span>
        <button 
          type="button" 
          @click="setYoutubeAlignment('left')"
          :class="{'bg-gray-200': currentYoutubeAlignment === 'left'}"
          class="px-2 py-1 rounded text-xs text-gray-700 hover:bg-gray-200"
          title="Alinear a la izquierda"
        >
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h10M4 18h16"></path>
          </svg>
        </button>
        <button 
          type="button" 
          @click="setYoutubeAlignment('center')"
          :class="{'bg-gray-200': currentYoutubeAlignment === 'center'}"
          class="px-2 py-1 rounded text-xs text-gray-700 hover:bg-gray-200"
          title="Centrar"
        >
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M7 12h10M4 18h16"></path>
          </svg>
        </button>
        <button 
          type="button" 
          @click="setYoutubeAlignment('right')"
          :class="{'bg-gray-200': currentYoutubeAlignment === 'right'}"
          class="px-2 py-1 rounded text-xs text-gray-700 hover:bg-gray-200"
          title="Alinear a la derecha"
        >
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M10 12h10M4 18h16"></path>
          </svg>
        </button>
      </template>
    </div>
    <EditorContent :editor="editor" class="p-4" />

    <!-- Modal para insertar imagen por URL -->
    <div v-if="showImageUrlModal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div class="bg-white rounded-lg p-6 w-full max-w-md mx-4 shadow-xl">
        <h3 class="text-lg font-semibold text-gray-900 mb-4">Insertar imagen por URL</h3>
        <div class="space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">URL de la imagen</label>
            <input 
              v-model="imageUrl" 
              type="url" 
              placeholder="https://ejemplo.com/imagen.jpg"
              class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              @keyup.enter="insertImageFromUrl"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Texto alternativo (opcional)</label>
            <input 
              v-model="imageAlt" 
              type="text" 
              placeholder="Descripción de la imagen"
              class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
        </div>
        <div class="flex justify-end gap-2 mt-6">
          <button 
            type="button"
            @click="closeImageModal"
            class="px-4 py-2 text-sm text-gray-600 hover:text-gray-800"
          >
            Cancelar
          </button>
          <button 
            type="button"
            @click="insertImageFromUrl"
            :disabled="!imageUrl"
            class="px-4 py-2 text-sm bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            Insertar
          </button>
        </div>
      </div>
    </div>

    <!-- Modal para insertar video de YouTube -->
    <div v-if="showYoutubeModal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div class="bg-white rounded-lg p-6 w-full max-w-md mx-4 shadow-xl">
        <h3 class="text-lg font-semibold text-gray-900 mb-4 flex items-center gap-2">
          <svg class="w-6 h-6 text-red-600" viewBox="0 0 24 24" fill="currentColor">
            <path d="M23.498 6.186a3.016 3.016 0 0 0-2.122-2.136C19.505 3.545 12 3.545 12 3.545s-7.505 0-9.377.505A3.017 3.017 0 0 0 .502 6.186C0 8.07 0 12 0 12s0 3.93.502 5.814a3.016 3.016 0 0 0 2.122 2.136c1.871.505 9.376.505 9.376.505s7.505 0 9.377-.505a3.015 3.015 0 0 0 2.122-2.136C24 15.93 24 12 24 12s0-3.93-.502-5.814zM9.545 15.568V8.432L15.818 12l-6.273 3.568z"/>
          </svg>
          Insertar video de YouTube
        </h3>
        <div class="space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">URL del video</label>
            <input 
              v-model="youtubeUrl" 
              type="url" 
              placeholder="https://www.youtube.com/watch?v=..."
              class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-red-500"
              @keyup.enter="insertYoutubeVideo"
            />
            <p class="text-xs text-gray-500 mt-1">Formatos aceptados: youtube.com/watch?v=..., youtu.be/..., youtube.com/embed/...</p>
          </div>
          <div class="grid grid-cols-2 gap-4">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Ancho (px)</label>
              <input 
                v-model="youtubeWidth" 
                type="number" 
                placeholder="640"
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-red-500"
              />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Alto (px)</label>
              <input 
                v-model="youtubeHeight" 
                type="number" 
                placeholder="360"
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-red-500"
              />
            </div>
          </div>
        </div>
        <div class="flex justify-end gap-2 mt-6">
          <button 
            type="button"
            @click="closeYoutubeModal"
            class="px-4 py-2 text-sm text-gray-600 hover:text-gray-800"
          >
            Cancelar
          </button>
          <button 
            type="button"
            @click="insertYoutubeVideo"
            :disabled="!youtubeUrl"
            class="px-4 py-2 text-sm bg-red-600 text-white rounded-md hover:bg-red-700 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            Insertar
          </button>
        </div>
      </div>
    </div>

    <!-- Modal para insertar/editar enlace -->
    <div v-if="showLinkModal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div class="bg-white rounded-lg p-6 w-full max-w-md mx-4 shadow-xl">
        <h3 class="text-lg font-semibold text-gray-900 mb-4">Insertar/Editar enlace</h3>
        <div class="space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">URL del enlace</label>
            <input
              v-model="linkUrl"
              type="url"
              placeholder="https://ejemplo.com"
              class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              @keyup.enter="applyLink"
            />
          </div>
        </div>
        <div class="flex justify-end gap-2 mt-6">
          <button
            type="button"
            @click="closeLinkModal"
            class="px-4 py-2 text-sm text-gray-600 hover:text-gray-800"
          >
            Cancelar
          </button>
          <button
            type="button"
            @click="applyLink"
            class="px-4 py-2 text-sm bg-blue-600 text-white rounded-md hover:bg-blue-700"
          >
            {{ linkUrl ? 'Insertar' : 'Quitar enlace' }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import CodeBlock from '@tiptap/extension-code-block'
import Highlight from '@tiptap/extension-highlight'
import Underline from '@tiptap/extension-underline'
import HardBreak from '@tiptap/extension-hard-break'
import Image from '@tiptap/extension-image'
import TextAlign from '@tiptap/extension-text-align'
import Youtube from '@tiptap/extension-youtube'
import Link from '@tiptap/extension-link'

import { ref, computed, watch, onBeforeUnmount } from 'vue'

// Extensión personalizada de Image con soporte para redimensionamiento y alineación
const ResizableImage = Image.extend({
  addAttributes() {
    return {
      ...this.parent?.(),
      width: {
        default: null,
        parseHTML: element => element.getAttribute('width') || element.style.width || null,
        renderHTML: attributes => {
          if (!attributes.width) return {}
          return { style: `width: ${attributes.width}` }
        },
      },
      height: {
        default: null,
        parseHTML: element => element.getAttribute('height') || element.style.height || null,
        renderHTML: attributes => {
          if (!attributes.height) return {}
          return { style: `height: ${attributes.height}` }
        },
      },
      alignment: {
        default: 'center',
        parseHTML: element => element.getAttribute('data-alignment') || 'center',
        renderHTML: attributes => {
          return { 'data-alignment': attributes.alignment || 'center' }
        },
      },
    }
  },
})

// Extensión personalizada de YouTube con soporte para alineación
const AlignableYoutube = Youtube.extend({
  addAttributes() {
    return {
      ...this.parent?.(),
      alignment: {
        default: 'center',
      },
    }
  },
  addNodeView() {
    return ({ node, HTMLAttributes, editor }) => {
      const alignment = node.attrs.alignment || 'center'
      
      // Contenedor principal
      const wrapper = document.createElement('div')
      wrapper.setAttribute('data-youtube-video', '')
      wrapper.setAttribute('data-alignment', alignment)
      wrapper.style.margin = '1rem 0'
      
      // Aplicar estilos de alineación
      wrapper.style.display = 'flex'
      if (alignment === 'left') {
        wrapper.style.justifyContent = 'flex-start'
      } else if (alignment === 'right') {
        wrapper.style.justifyContent = 'flex-end'
      } else {
        wrapper.style.justifyContent = 'center'
      }
      
      // Crear iframe
      const iframe = document.createElement('iframe')
      iframe.setAttribute('src', node.attrs.src)
      iframe.setAttribute('width', node.attrs.width || 640)
      iframe.setAttribute('height', node.attrs.height || 360)
      iframe.setAttribute('allowfullscreen', 'true')
      iframe.setAttribute('allow', 'accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture')
      iframe.setAttribute('frameborder', '0')
      iframe.style.borderRadius = '8px'
      iframe.style.maxWidth = '100%'
      
      wrapper.appendChild(iframe)
      
      return {
        dom: wrapper,
        update: (updatedNode) => {
          if (updatedNode.type.name !== 'youtube') {
            return false
          }
          
          const newAlignment = updatedNode.attrs.alignment || 'center'
          wrapper.setAttribute('data-alignment', newAlignment)
          
          if (newAlignment === 'left') {
            wrapper.style.justifyContent = 'flex-start'
          } else if (newAlignment === 'right') {
            wrapper.style.justifyContent = 'flex-end'
          } else {
            wrapper.style.justifyContent = 'center'
          }
          
          iframe.setAttribute('src', updatedNode.attrs.src)
          iframe.setAttribute('width', updatedNode.attrs.width || 640)
          iframe.setAttribute('height', updatedNode.attrs.height || 360)
          
          return true
        },
      }
    }
  },
})

const props = defineProps({
  modelValue: {
    type: String,
    default: '',
  },
})

const emit = defineEmits(['update:modelValue'])

// Estados para el modal de imagen
const showImageUrlModal = ref(false)
const imageUrl = ref('')
const imageAlt = ref('')
const fileInput = ref(null)
const customWidth = ref('')

// Estados para el modal de YouTube
const showYoutubeModal = ref(false)
const youtubeUrl = ref('')
const youtubeWidth = ref(640)
const youtubeHeight = ref(360)

// Computed para detectar si hay una imagen seleccionada
const isImageSelected = computed(() => {
  return editor.value?.isActive('image') || false
})

// Computed para detectar si hay un video de YouTube seleccionado
const isYoutubeSelected = computed(() => {
  return editor.value?.isActive('youtube') || false
})

// Computed para obtener la alineación actual de la imagen seleccionada
const currentImageAlignment = computed(() => {
  if (!editor.value || !isImageSelected.value) return 'center'
  const attrs = editor.value.getAttributes('image')
  return attrs.alignment || 'center'
})

// Computed para obtener la alineación actual del video seleccionado
const currentYoutubeAlignment = computed(() => {
  if (!editor.value || !isYoutubeSelected.value) return 'center'
  const attrs = editor.value.getAttributes('youtube')
  return attrs.alignment || 'center'
})

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
    TextAlign.configure({
      types: ['heading', 'paragraph'],
    }),
    ResizableImage.configure({
      inline: false,
      allowBase64: true,
      HTMLAttributes: {
        class: 'editor-image',
      },
    }),
    AlignableYoutube.configure({
      controls: true,
      nocookie: true,
      allowFullscreen: true,
      modestBranding: true,
    }),
    Link.configure({
      openOnClick: true,
      autolink: true,
      linkOnPaste: true,
      HTMLAttributes: {
        rel: 'noopener noreferrer',
        target: '_blank',
        class: 'text-blue-600 underline hover:text-blue-800',
      },
    }),
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

// Función para insertar imagen desde URL
const insertImageFromUrl = () => {
  if (imageUrl.value && editor.value) {
    editor.value.chain().focus().setImage({ 
      src: imageUrl.value,
      alt: imageAlt.value || 'Imagen'
    }).run()
    closeImageModal()
  }
}

// Función para cerrar el modal
const closeImageModal = () => {
  showImageUrlModal.value = false
  imageUrl.value = ''
  imageAlt.value = ''
}

// Función para insertar video de YouTube
const insertYoutubeVideo = () => {
  if (youtubeUrl.value && editor.value) {
    editor.value.commands.setYoutubeVideo({
      src: youtubeUrl.value,
      width: Math.max(320, parseInt(youtubeWidth.value) || 640),
      height: Math.max(180, parseInt(youtubeHeight.value) || 360),
    })
    closeYoutubeModal()
  }
}

// Función para cerrar el modal de YouTube
const closeYoutubeModal = () => {
  showYoutubeModal.value = false
  youtubeUrl.value = ''
  youtubeWidth.value = 640
  youtubeHeight.value = 360
}

// Función para activar el input de archivo
const triggerFileInput = () => {
  fileInput.value?.click()
}

// Función para manejar la subida de archivo
const handleFileUpload = (event) => {
  const file = event.target.files?.[0]
  if (file && editor.value) {
    const reader = new FileReader()
    reader.onload = (e) => {
      const base64 = e.target?.result
      if (base64) {
        editor.value.chain().focus().setImage({ 
          src: base64,
          alt: file.name
        }).run()
      }
    }
    reader.readAsDataURL(file)
  }
  // Limpiar el input para permitir subir el mismo archivo de nuevo
  event.target.value = ''
}

// Función para cambiar el tamaño de la imagen seleccionada
const setImageSize = (width) => {
  if (!editor.value) return
  
  const { state } = editor.value
  const { from, to } = state.selection
  
  // Buscar el nodo de imagen en la selección actual
  state.doc.nodesBetween(from, to, (node, pos) => {
    if (node.type.name === 'image') {
      editor.value.chain()
        .focus()
        .setNodeSelection(pos)
        .updateAttributes('image', { width, height: 'auto' })
        .run()
    }
  })
}

// Función para cambiar la alineación de la imagen seleccionada
const setImageAlignment = (alignment) => {
  if (!editor.value) return
  
  const { state } = editor.value
  const { from, to } = state.selection
  
  // Buscar el nodo de imagen en la selección actual
  state.doc.nodesBetween(from, to, (node, pos) => {
    if (node.type.name === 'image') {
      editor.value.chain()
        .focus()
        .setNodeSelection(pos)
        .updateAttributes('image', { alignment })
        .run()
    }
  })
}

// Función para cambiar la alineación del video de YouTube seleccionado
const setYoutubeAlignment = (alignment) => {
  if (!editor.value) return
  
  const { state } = editor.value
  const { from, to } = state.selection
  
  // Buscar el nodo de youtube en la selección actual
  state.doc.nodesBetween(from, to, (node, pos) => {
    if (node.type.name === 'youtube') {
      editor.value.chain()
        .focus()
        .setNodeSelection(pos)
        .updateAttributes('youtube', { alignment })
        .run()
    }
  })
}

// Modal para enlaces
import { ref as vueRef } from 'vue'
const showLinkModal = vueRef(false)
const linkUrl = vueRef('')

const setLink = () => {
  if (!editor.value) return
  const previousUrl = editor.value.getAttributes('link').href || ''
  linkUrl.value = previousUrl
  showLinkModal.value = true
}

const applyLink = () => {
  if (!editor.value) return
  if (linkUrl.value) {
    editor.value.chain().focus().setLink({ href: linkUrl.value }).run()
  } else {
    editor.value.chain().focus().unsetLink().run()
  }
  showLinkModal.value = false
  linkUrl.value = ''
}

const closeLinkModal = () => {
  showLinkModal.value = false
  linkUrl.value = ''
}

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

/* Estilos para imágenes */
.ProseMirror img,
.ProseMirror .editor-image {
  max-width: 100%;
  height: auto;
  border-radius: 8px;
  margin: 1rem 0;
  cursor: pointer;
  transition: box-shadow 0.2s ease;
}

.ProseMirror img:hover,
.ProseMirror .editor-image:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.ProseMirror img.ProseMirror-selectednode,
.ProseMirror .editor-image.ProseMirror-selectednode {
  outline: 3px solid #3b82f6;
  outline-offset: 2px;
}

/* Estilos adicionales para imágenes redimensionadas */
.ProseMirror img[style*="width"] {
  max-width: 100%;
}

/* Estilos de alineación de imágenes */
.ProseMirror img[data-alignment="left"] {
  display: block;
  margin-left: 0;
  margin-right: auto;
}

.ProseMirror img[data-alignment="center"] {
  display: block;
  margin-left: auto;
  margin-right: auto;
}

.ProseMirror img[data-alignment="right"] {
  display: block;
  margin-left: auto;
  margin-right: 0;
}

/* Estilos para videos de YouTube */
.ProseMirror div[data-youtube-video] {
  margin: 1rem 0;
}

.ProseMirror div[data-youtube-video] iframe {
  border-radius: 8px;
  max-width: 100%;
}

.ProseMirror div[data-youtube-video].ProseMirror-selectednode iframe {
  outline: 3px solid #3b82f6;
  outline-offset: 2px;
}

/* Estilos de alineación de videos de YouTube */
.ProseMirror div[data-youtube-video][data-alignment="left"] {
  display: flex;
  justify-content: flex-start;
}

.ProseMirror div[data-youtube-video][data-alignment="center"] {
  display: flex;
  justify-content: center;
}

.ProseMirror div[data-youtube-video][data-alignment="right"] {
  display: flex;
  justify-content: flex-end;
}

</style>