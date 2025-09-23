import { useTranslation } from 'react-i18next';
import { useCallback, useMemo } from 'react';

export type SupportedLanguage = 'tr' | 'en';

export interface LanguageOption {
  code: SupportedLanguage;
  label: string;
  flag: string;
  nativeName: string;
}

export const useLanguage = () => {
  const { t, i18n } = useTranslation();

  const supportedLanguages: LanguageOption[] = useMemo(
    () => [
      {
        code: 'tr',
        label: t('language.turkish'),
        flag: '🇹🇷',
        nativeName: 'Türkçe',
      },
      {
        code: 'en',
        label: t('language.english'),
        flag: '🇬🇧',
        nativeName: 'English',
      },
    ],
    [t]
  );

  const currentLanguage = i18n.language as SupportedLanguage;

  const currentLanguageInfo = useMemo(
    () => supportedLanguages.find(lang => lang.code === currentLanguage) || supportedLanguages[0],
    [supportedLanguages, currentLanguage]
  );

  const changeLanguage = useCallback(
    (languageCode: SupportedLanguage) => {
      i18n.changeLanguage(languageCode);
    },
    [i18n]
  );

  const isLanguageActive = useCallback(
    (languageCode: SupportedLanguage) => {
      return currentLanguage === languageCode;
    },
    [currentLanguage]
  );

  const toggleLanguage = useCallback(() => {
    const currentIndex = supportedLanguages.findIndex(lang => lang.code === currentLanguage);
    const nextIndex = (currentIndex + 1) % supportedLanguages.length;
    changeLanguage(supportedLanguages[nextIndex].code);
  }, [supportedLanguages, currentLanguage, changeLanguage]);

  const detectAndSetBrowserLanguage = useCallback(() => {
    const browserLang = navigator.language.split('-')[0] as SupportedLanguage;
    const isSupported = supportedLanguages.some(lang => lang.code === browserLang);

    if (isSupported) {
      changeLanguage(browserLang);
      return true;
    }
    return false;
  }, [supportedLanguages, changeLanguage]);

  return {
    currentLanguage,
    currentLanguageInfo,
    supportedLanguages,

    changeLanguage,
    isLanguageActive,
    toggleLanguage,
    detectAndSetBrowserLanguage,

    t,
    i18n,
  };
};
