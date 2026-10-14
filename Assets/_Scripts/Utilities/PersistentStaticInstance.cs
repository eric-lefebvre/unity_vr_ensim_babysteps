using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PersistentStaticInstance<T> : StaticInstance<T> where T : MonoBehaviour {
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
