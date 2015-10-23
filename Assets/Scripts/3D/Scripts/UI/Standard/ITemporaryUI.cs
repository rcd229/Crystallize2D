using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface ITemporaryUI : ICloseable {}
public interface ITemporaryUI<I, O> : ITemporaryUI, IInitializable<I>, ICompleteable<O> {}
public interface ITemporaryUI<I> : ITemporaryUI<I, object>, IInitializable<I>, ICompleteable { }
