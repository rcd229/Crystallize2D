using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface JoinedGameDataRef<T1, T2> {
    T1 GameDataInstance { get; }
    T2 PlayerDataInstance { get; }
}

public abstract class IDRef<T0, T1, T2> {
    public T0 ID { get; private set; }

    public abstract bool IsNull { get; }

    public IDRef(T0 id) {
        ID = id;
    }
}

public abstract class IntIDRef<T1, T2> : IDRef<int, T1, T2> {
    public override bool IsNull { get { return ID == -1; } }

    public IntIDRef(int id) : base(id) { }
}

public abstract class GuidRef<T1, T2> : IDRef<Guid, T1, T2> {
    public override bool IsNull { get { return ID == Guid.Empty; } }

    public GuidRef(Guid id) : base(id) { }
}
