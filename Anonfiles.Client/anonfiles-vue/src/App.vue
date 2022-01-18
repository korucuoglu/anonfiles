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
            class="block w-full text-sm text-slate-500 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-green-200 file:text-green-600 hover:file:bg-green-600 hover:file:text-white"
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
        <tr v-for="item in datas" :key="item.fileId">
          <td class="border-b border-slate-100 dark:border-slate-700 md:p-4 p-2 text-slate-500 dark:text-slate-400 pl-2">
            {{ item.file.name }}
          </td>
          <td class="border-b border-slate-100 dark:border-slate-700 md:p-4 p-2 text-slate-500 dark:text-slate-400">{{ (item.file.size / 1000000).toFixed(2) }}mb</td>
          <td class="border-b border-slate-100 dark:border-slate-700 md:p-4 p-2 text-slate-500 dark:text-slate-400">
            {{ item.status }}
          </td>
          <td class="border-b border-slate-100 dark:border-slate-700 md:p-4 p-2 text-slate-500 dark:text-slate-400 overflow text-right">
            <button
              v-if="item.status != 'Loaded'"
              @click.prevent="deleteFile(item)"
              :disabled="item.status == 'Starting'"
              class="px-4 py-1 text-md text-red-600 font-semibold rounded-sm border border-red-600 hover:text-white hover:bg-red-600 hover:border-transparent focus:outline-none focus:ring-2 focus:ring-red-400 focus:ring-offset-2 disabled:bg-red-100"
            >
              Delete
            </button>

            <a
              v-if="item.status == 'Loaded'"
              :href="localAddress + '/api/data/direct/' + item.fileId"
              class="px-4 py-1 text-md text-green-600 font-semibold rounded-sm border border-green-600 hover:text-white hover:bg-green-600 hover:border-transparent focus:outline-none focus:ring-2 focus:ring-green-400 focus:ring-offset-2"
            >
              Indir
            </a>
          </td>
        </tr>
      </tbody>
    </table>
    <div class="flex lg:w-1/2 w-full justify-end mt-10 mb-16">
      <button
        @click.prevent="addDocs"
        v-if="inputFilesLength > 0"
        class="px-4 py-1 text-md text-blue-400 font-semibold rounded-sm border border-blue-400 hover:text-white hover:bg-blue-600 hover:border-transparent focus:outline-none focus:ring-2 focus:ring-blue-400 focus:ring-offset-2"
      >
        Upload ({{ inputFilesLength }})
      </button>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { computed, ref } from "vue";

let datas = ref([]);

const localAddress = "https://localhost:5001";

const connection = new HubConnectionBuilder().withUrl("https://localhost:5001/hub").build();

connection.on("filesUploaded", function (model) {
  const data = datas.value.find((data) => data.file.name == model.fileName);
  data.fileId = model.fileId;

  if (model.success) data.status = "Loaded";
  else data.status = "Error";
});

connection.on("filesUploadedStarting", function (fileName) {
  const data = datas.value.find((data) => data.file.name == fileName);
  data.status = "Starting";
});

connection
  .start()
  .then(function () {})
  .catch((error) => {
    console.error(error.message);
  });

const getInputFiles = (event) => {
  for (let i = 0; i < event.target.files.length; i++) {
    datas.value.push({
      fileId: "",
      status: "Unstarted",
      message: "",
      file: event.target.files[i],
    });
  }
};

const inputFilesLength = computed(() => {
  return datas.value.length || 0;
});

const deleteFile = (file) => {
  datas.value = [...datas.value].filter((inputFile) => inputFile != file);
};

const addDocs = (e) => {
  var formData = new FormData();

  for (var i = 0; i < datas.value.length; i++) {
    formData.append("files", datas.value[i].file);
  }

  axios({
    method: "post",
    url: "https://localhost:5001/api/test/upload",
    data: formData,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
  }).then(function (response) {
    console.log(response);
  });
};
</script>

<style></style>
