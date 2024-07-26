using System;
using System.Collections.Generic;
using Manager;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Coins.JumpCoin
{
    public class JumpCoinPool : ObjectPool<NoteConfig, JumpCoinFactory>
    {}
}
