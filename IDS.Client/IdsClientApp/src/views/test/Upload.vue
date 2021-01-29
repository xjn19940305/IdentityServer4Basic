<template>
  <a-upload
    name="file"
    :multiple="true"
    action="https://localhost:5001/api/UserMedicalWithProject/ImportWhiteList"
    :headers="headers"
    @change="handleChange"
  >
    <a-button> <a-icon type="upload" /> Click to Upload </a-button>
  </a-upload>
</template>
<script>
export default {
  data () {
    return {
      headers: {
        authorization: 'authorization-text'
      }
    }
  },
  methods: {
    handleChange (info) {
      if (info.file.status !== 'uploading') {
        console.log(info.file, info.fileList)
      }
      if (info.file.status === 'done') {
        this.$message.success(`${info.file.name} file uploaded successfully`)
      } else if (info.file.status === 'error') {
        this.$message.error(`${info.file.name} file upload failed.`)
      }
    }
  }
}
</script>
