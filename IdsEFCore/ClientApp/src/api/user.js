import request from '@/utils/request'
var UserApi = {
    getList (params) {
        params = params || {}
        var Account = params.Account || ''
        var Email = params.Email || ''
        var Phone = params.Phone || ''
        var NickName = params.NickName || ''
        return request({
            url: `api/User/GetList?Account=${Account}&NickName=${NickName}&Email=${Email}&Phone=${Phone}&pageSize=${params.pageSize}&page=${params.page}`,
            method: 'get'
        })
    },
    Add (data) {
        return request({
            url: `api/User/Add`,
            method: 'post',
            data
        })
    },
    Update (data) {
        return request({
            url: `api/User/Update`,
            method: 'put',
            data
        })
    },
    Delete (ids) {
        return request({
            url: `api/User/Delete`,
            method: 'delete',
            data: ids
        })
    },
    Get (UserId) {
        return request({
            url: `api/User/Get/${UserId}`,
            method: 'get'
        })
    },
    ResetPwd (UserId) {
        return request({
            url: `api/User/ResetPassword?UserId=${UserId}`,
            method: 'get'
        })
    },
    GetRoleIds (UserId) {
        return request({
            url: `api/User/GetRoleIdsFromUser?UserId=${UserId}`,
            method: 'get'
        })
    },
    SaveUserRole (data) {
        return request({
            url: `api/User/SaveUserRoles`,
            method: 'put',
            data
        })
    },
    Login (data) {
        return request({
            url: `Account/LoginAsync`,
            method: 'post',
            data
        })
    },
    GetConsentContext (returnUrl) {
        return request({
            url: `Account/GetConsentContext?returnUrl=${returnUrl}`,
            method: 'get'
        })
    },
    ProcessConsent (data) {
        return request({
            url: `Account/ProcessConsent`,
            method: 'post',
            data
        })
    },
    GetUserInfo (token) {
        return request({
            url: `api/User/GetUserInfo?token=${token}`,
            method: 'get'
        })
    }
}
export default UserApi
