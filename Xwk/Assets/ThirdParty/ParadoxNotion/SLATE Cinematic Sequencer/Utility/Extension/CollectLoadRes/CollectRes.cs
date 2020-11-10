using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate
{
    public class CollectRes : MonoBehaviour, ICollectRes
    {
        public virtual string getAffectedRes()
        {
           

            return "";
        }
    }
}
