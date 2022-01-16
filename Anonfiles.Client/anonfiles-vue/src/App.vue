<template>
  <form>
    <label for="">
      dosya sec
      <input type="file" multiple @change="getInputFiles($event)" />
    </label>

    <br />
    <button @click="addDocs">ekle</button>

    <br /><br />

    <div class="datalist">
      <table style="border: 1px solid black">
        <thead>
          <tr>
            <th style="border: 1px solid black">File Name</th>
            <th style="border: 1px solid black">kaldir</th>
            <th style="border: 1px solid black">boyut</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(item, index) in inputFiles" :key="index">
            <td style="border: 1px solid black">{{ item.name }}</td>
            <td style="border: 1px solid black">
              <button @click.prevent="deleteFile(item)">Kaldir</button>
            </td>
            <td style="border: 1px solid black">
              {{ item.size / 1000000 }} mb
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </form>
</template>

<script setup>
import axios from 'axios'
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from '@microsoft/signalr'
import { ref } from 'vue'

let inputFiles = ref()

const connection = new HubConnectionBuilder()
  .withUrl('https://localhost:5001/hub')
  .build()

connection.on('receiveMessage', function (message) {
  console.log(message)
})

connection
  .start()
  .then(function () {
    console.log('başarılı')
  })
  .catch((error) => {
    console.error(error.message)
  })

const getInputFiles = (event) => {
  inputFiles.value = event.target.files
}

const deleteFile = (file) => {
  inputFiles.value = [...inputFiles.value].filter(
    (inputFile) => inputFile != file
  )
}

const addDocs = (e) => {
  e.preventDefault()

  var formData = new FormData()

  for (var i = 0; i < inputFiles.value.length; i++) {
    formData.append('files', inputFiles.value[i])
  }

  axios({
    method: 'post',
    url: 'https://localhost:5001/api/test',
    data: formData,
    contentType: 'application/json; charset=utf-8',
    dataType: 'json',
  }).then(function (response) {
    console.log(response.data)
  })
}
</script>

<style></style>
