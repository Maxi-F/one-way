using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Scripts
{
    [Serializable]
    public class CreditSection
    {
        public string title;
        public string[] members;
    }
    
    [CreateAssetMenu(menuName = "Create CreditsDataConfig", fileName = "CreditsDataConfig", order = 0)]
    public class CreditsDataConfig : ScriptableObject
    {
        public List<CreditSection> credits;
    }
}
