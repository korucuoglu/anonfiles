<template>
  <div class="w-full justify-center items-center flex p-4 overflow-auto flex-col">
    <div class="lg:w-1/2 w-full">
      <form class="flex items-center space-x-6 mb-6 justify-between">
        <label class="block self-start">
          <span class="sr-only">Dosya Seç</span>
          <input
            type="file"
            multiple
            @change="getInputFiles($event)"
            class="
              block
              w-full
              text-sm text-slate-500
              file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-green-200 file:text-green-600
              hover:file:bg-green-600 hover:file:text-white
            "
          />
        </label>
      </form>
    </div>
    <table class="border-collapse table-fixed lg:w-1/2 w-full text-sm">
      <thead>
        <tr>
          <th class="border-b dark:border-slate-600 font-medium md:p-4 p-2 pt-0 pb-3 text-slate-400 dark:text-slate-200 text-left">File Name</th>
          <th class="border-b dark:border-slate-600 font-medium md:p-4 p-2 pt-0 pb-3 text-slate-400 dark:text-slate-200 text-left">Size</th>
          <th class="border-b dark:border-slate-600 font-medium md:p-4 p-2 pt-0 pb-3 text-slate-400 dark:text-slate-200 text-left">Status</th>
          <th class="border-b dark:border-slate-600 font-medium md:p-4 p-2 pt-0 pb-3 text-slate-400 dark:text-slate-200 text-left"></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, index) in inputFiles" :key="index">
          <td class="border-b border-slate-100 dark:border-slate-700 md:p-4 p-2 text-slate-500 dark:text-slate-400 pl-2">{{ item.name }}</td>
          <td class="border-b border-slate-100 dark:border-slate-700 md:p-4 p-2 text-slate-500 dark:text-slate-400">{{ item.size / 1000000 }}mb</td>
          <td class="border-b border-slate-100 dark:border-slate-700 md:p-4 p-2 text-slate-500 dark:text-slate-400"></td>
          <td class="border-b border-slate-100 dark:border-slate-700 md:p-4 p-2 text-slate-500 dark:text-slate-400 overflow text-right">
            <button
              @click.prevent="deleteFile(item)"
              class="
                px-4
                py-1
                text-md text-red-600
                font-semibold
                rounded-sm
                border border-red-600
                hover:text-white hover:bg-red-600 hover:border-transparent
                focus:outline-none focus:ring-2 focus:ring-red-400 focus:ring-offset-2
              "
            >
              Delete
            </button>
          </td>
        </tr>
      </tbody>
    </table>
    <div class="flex lg:w-1/2 w-full justify-end mt-10 mb-16">
      <button
        @click="addDocs"
        v-if="inputFilesLength > 0"
        class="
          px-4
          py-1
          text-md text-blue-400
          font-semibold
          rounded-sm
          border border-blue-400
          hover:text-white hover:bg-blue-600 hover:border-transparent
          focus:outline-none focus:ring-2 focus:ring-blue-400 focus:ring-offset-2
        "
      >
        Upload ({{ inputFilesLength }})
      </button>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { computed, ref } from "vue";

let inputFiles = ref("");

const getInputFiles = (event) => {
  inputFiles.value = event.target.files;
};

const inputFilesLength = computed(() => {
  return inputFiles.value.length || 0;
});

const deleteFile = (file) => {
  inputFiles.value = [...inputFiles.value].filter((inputFile) => inputFile != file);
};

const addDocs = (e) => {
  e.preventDefault();

  var formData = new FormData();

  for (var i = 0; i < inputFiles.value.length; i++) {
    formData.append("files", inputFiles.value[i]);
  }

  axios({
    method: "post",
    url: "https://localhost:5010/home/test",
    data: formData,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
  }).then(function (response) {
    console.log(response);
  });
};
</script>

<style>
</style>