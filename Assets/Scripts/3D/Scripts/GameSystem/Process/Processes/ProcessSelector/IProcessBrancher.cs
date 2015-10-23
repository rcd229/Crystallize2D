using UnityEngine;
using System.Collections;
using System;

interface IProcessBrancher
{
	IProcess SelectProcess<I, U, V>(ProcessFactoryRef<I, U> original, I input, ProcessExitCallback<U> originalCallback, ProcessExitCallback<V> alterCallback,IProcess parent);
	void SelectProcessOrUI<I, U, V>(UIFactoryRef<I, U> original, I input, Action<object, EventArgs<U>> originalCallback, ProcessExitCallback<V> alterCallback,IProcess parent);
}

