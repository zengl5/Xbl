using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class EntityMgr
{
    private static string c_EntityConfig = "";
    public static string EntityConfig
    {
        get
        {
            if (string.IsNullOrEmpty(c_EntityConfig))
                c_EntityConfig = C_Singleton<GameResMgr>.GetInstance().LoadString("entity_config.json", "entity", "config", "c_framework/entity/config/");

            return c_EntityConfig;
        }
    }

    public static GameObject CreateCharacter(int entityID)
    {
        return CreateCharacter(entityID.ToString());
    }

    public static GameObject CreateCharacter(string entityID)
    {
        string characterConfigName = C_Json.GetJsonKeyString(EntityConfig, entityID);
        if (!string.IsNullOrEmpty(characterConfigName))
        {
            string prefabConfig = C_Singleton<GameResMgr>.GetInstance().LoadString("prefab_config.json", "entity", "character", "c_framework/entity/character/" + characterConfigName + "/config/");
            if (!string.IsNullOrEmpty(prefabConfig))
            {
                string prefabName = C_Json.GetJsonKeyString(prefabConfig, "main_prefab");
                if (!string.IsNullOrEmpty(prefabName))
                    return C_Singleton<GameResMgr>.GetInstance().LoadResource_GameObject(prefabName, "PackagingResources/c_framework/entity/character/" + characterConfigName + "/prefab/");
            }
        }

        return null;
    }
}
