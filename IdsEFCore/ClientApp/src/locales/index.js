import Vue from 'vue'
import VueI18n from 'vue-i18n'
import storage from 'store'
import moment from 'moment'
import languageApi from '@/api/language'
// default lang
import enUS from './lang/en-US'

Vue.use(VueI18n)

export const defaultLang = 'en-US'

const messages = {
  'en-US': {
    ...enUS
  }
}

const i18n = new VueI18n({
  silentTranslationWarn: true,
  locale: defaultLang,
  fallbackLocale: defaultLang,
  messages
})

const loadedLanguages = [defaultLang]

function setI18nLanguage (lang) {
  i18n.locale = lang
  // request.headers['Accept-Language'] = lang
  document.querySelector('html').setAttribute('lang', lang)
  return lang
}

export function loadLanguageAsync (lang = defaultLang) {
  return new Promise(async resolve => {
    // 缓存语言设置
    storage.set('lang', lang)
    console.log('切换语言', lang)
    // 去查询应用+语言包的翻译内容
    var local = await import(/* webpackChunkName: "lang-[request]" */ `./lang/${lang}`)
    var server = await languageApi.getTranslate(lang) || []
    Promise.all([local, server]).then(res => {
      server = server || []
      const locale = local.default
      server.map(e => {
        locale[e.TranslateCode] = e.TranslateContent
      })
      i18n.setLocaleMessage(lang, locale)
      loadedLanguages.push(lang)
      moment.updateLocale(locale.momentName, locale.momentLocale)
      return setI18nLanguage(lang)
    })
    return resolve(setI18nLanguage(lang))
  })
}

export function i18nRender (key) {
  return i18n.t(`${key}`)
}

export default i18n
