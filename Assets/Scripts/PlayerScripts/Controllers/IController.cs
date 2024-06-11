using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public interface IController
    {
        public void OnUpdate();
        public void OnFixedUpdate();
    }
}
