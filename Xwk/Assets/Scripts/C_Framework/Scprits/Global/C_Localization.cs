using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_Localization
    {
        private static string m_strLanguage = "";
        private static string StrLanguage
        {
            get
            {
                if (string.IsNullOrEmpty(m_strLanguage))
                    m_strLanguage = C_Save.LoadString("language_zh", C_LocalPath.StreamingAssetsConfigPath);

                return m_strLanguage;
            }
        }

        public static string GetLocalization(string key)
        {
            if (key.Contains("LOACAL_"))
                return C_Json.GetJsonKeyString(StrLanguage, key);

            return key;
        }

        public static string GetLocalizationERROR(int key)
        {
            return C_Json.GetJsonKeyString(StrLanguage, "LOACAL_ERROR_" + key);
        }
    }
}