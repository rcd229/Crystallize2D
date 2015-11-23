using UnityEngine;
using System.Collections;
using System;

public abstract class SingletonUI<I, O, T> : UIMonoBehaviour, ITemporaryUI<I, O>
    where T : SingletonUI<I, O, T> {

    static T _instance;
    static UIFactoryRef<I, O> _factory = new UIFactoryRef<I, O>();

    static SingletonUI() {
        _factory.Set(GetInstance);
    }

    public static T GetInstance() {
        if (_instance) {
            Destroy(_instance.gameObject);
        }
        _instance = GameObjectUtil.GetResourceInstanceFromAttribute<T>();
        return _instance;
    }

    public static UIFactoryRef<I, O> GetFactory() {
        return _factory;
    }

    public abstract void Initialize(I args1);

    public virtual EventHandler<EventArgs<O>> Complete { get; set; }

    public virtual void Close() {
        Destroy(gameObject);
    }
}
