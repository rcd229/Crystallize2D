using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// All objects that consumes energy in the daily cycle should implement this interface
/// IEnergyConsumer represents anything that reports an energy cost when interaction happens
/// OnInteraction should raise the amount of energy spent in this interaction
/// </summary>
public interface IEnergyConsumer {
	event EventHandler<EventArgs<int>> OnInteraction;
}
