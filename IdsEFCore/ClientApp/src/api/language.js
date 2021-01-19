import request from '@/utils/request'
var languageApi = {
    getLanglist (params) {
        params = params || {}
        var Code = params.Code || ''
        var Description = params.Description || ''
        return request({
            url: `api/Language/GetLanguageList?Code=${Code}&Description=${Description}`,
            method: 'get'
        })
    },
    getLanType (id) {
        return request({
            url: `api/Language/GetLanType?Id=${id}`,
            method: 'get'
        })
    },
    addLanType (data) {
        return request({
            url: `api/Language/AddLanguageType`,
            method: 'post',
            data
        })
    },
    updateLanType (data) {
        return request({
            url: `api/Language/UpdateLanguageType`,
            method: 'put',
            data
        })
    },
    DeleteLanType (id) {
        return request({
            url: `api/Language/DeleteLanType?Id=${id}`,
            method: 'delete'
        })
    },
    getTranslate (code) {
        return request({
            url: `api/Language/GetTranslateFromLanguage/${code}`,
            method: 'get'
        })
    },
    getTranslateFromId (id) {
        return request({
            url: `api/Language/GetTranslateFromId?Id=${id}`,
            method: 'get'
        })
    },
    getAllTranslate (data) {
        var Code = data.Code || ''
        var Value = data.Value || ''
        return request({
            url: `api/Language/GetAllTranslate?Code=${Code}&Value=${Value}&pageSize=${data.pageSize}&page=${data.page}`,
            method: 'get'
        })
    },
    addTranslate (data) {
        return request({
            url: `api/Language/AddTranslate`,
            method: 'post',
            data
        })
    },
    updateTranslate (data) {
        return request({
            url: `api/Language/UpdateTranslate`,
            method: 'put',
            data
        })
    },
    DeleteTranslate (ids) {
        return request({
            url: `api/Language/DeleteTranslate`,
            method: 'delete',
            data: ids
        })
    }
}
export default languageApi
