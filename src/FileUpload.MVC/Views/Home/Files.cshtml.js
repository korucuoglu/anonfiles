const app = {

    data() {
        return {
            datas: [],
        }
    },

    mounted() {


        this.GetFiles();

    },

    methods: {

        deleteFile(file, fileId) {

            axios({
                method: "delete",
                url: "/Home/Delete/" + fileId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
            }).then(function (response) {
                if (response.data.isSuccessful) {
                    this.datas = [...this.datas].filter((data) => data != file);
                } else {
                    alert("silme sırasında hata meydana geldi");
                }

            });

        },

        GetFiles() {
            let dataList = '@Html.Raw(Json.Serialize(Model.DataModel))';
            this.datas = JSON.parse(dataList);
            console.log(this.datas);

        },

        Download(fileId) {
            axios({
                method: "get",
                url: "/Home/GetLink/" + fileId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
            }).then(function (response) {
                if (response.data.isSuccessful) {
                    window.location.href = response.data.data;
                }

            });
        }

    },

}

Vue.createApp(app).mount('#app');