// ***Copyright © 2017 Doxel aka Ng Bob Shoaun. All Rights Reserved.***

using System;
using System.Collections.Generic;

public class EventManager : Singleton<EventManager> {

	private readonly Dictionary<Event, Action> events;

	public EventManager () {
		events = new Dictionary<Event, Action> ();
	}

	public void Broadcast (Event eventKey) {
		Action eventHandler;
		if (events.TryGetValue (eventKey, out eventHandler))
			eventHandler ();
	}

	public void Subscribe (Event eventKey, Action method) {
		Action eventHandler;
		if (events.TryGetValue (eventKey, out eventHandler))
			eventHandler += method;
		else
			events.Add (eventKey, method);
	}

	public void Unsubscribe (Event eventKey, Action method) {
		Action eventHandler;
		if (events.TryGetValue (eventKey, out eventHandler))
			eventHandler -= method;
	}

}

public class EventManager<T> : Singleton<EventManager<T>> {

	private readonly Dictionary<Event, Action<T>> events;

	public EventManager () {
		events = new Dictionary<Event, Action<T>> ();
	}

	public void Broadcast (Event eventKey, T argument) {
		Action<T> eventHandler;
		if (events.TryGetValue (eventKey, out eventHandler))
			eventHandler (argument);
	}

	public void Subscribe (Event eventKey, Action<T> method) {
		Action<T> eventHandler;
		if (events.TryGetValue (eventKey, out eventHandler))
			eventHandler += method;
		else
			events.Add (eventKey, method);
	}

	public void Unsubscribe (Event eventKey, Action<T> method) {
		Action<T> eventHandler;
		if (events.TryGetValue (eventKey, out eventHandler))
			eventHandler -= method;
	}

}

public class EventManager<T1, T2> : Singleton<EventManager<T1, T2>> {

	private readonly Dictionary<Event, Action<T1, T2>> events;

	public EventManager () {
		events = new Dictionary<Event, Action<T1, T2>> ();
	}

	public void Broadcast (Event eventKey, T1 argument1, T2 argument2) {
		Action<T1,T2> eventHandler;
		if (events.TryGetValue (eventKey, out eventHandler))
			eventHandler (argument1, argument2);
	}

	public void Subscribe (Event eventKey, Action<T1, T2> method) {
		if (events.ContainsKey (eventKey))
			events [eventKey] += method;
		else
			events.Add (eventKey, method);
	}

	public void Unsubscribe (Event eventKey, Action<T1, T2> method) {
		if (events.ContainsKey (eventKey))
			events [eventKey] -= method;
	}

}

public class EventManager<T1, T2, T3> : Singleton<EventManager<T1, T2, T3>> {
	
	private readonly Dictionary<Event, Action<T1, T2, T3>> events;

	public EventManager () {
		events = new Dictionary<Event, Action<T1, T2, T3>> ();
	}

	public void Broadcast (Event eventKey, T1 argument1, T2 argument2, T3 argument3) {
		Action<T1, T2, T3> eventHandler;
		if (events.TryGetValue (eventKey, out eventHandler))
			eventHandler (argument1, argument2, argument3);
	}

	public void Subscribe (Event eventKey, Action<T1, T2, T3> method) {
		if (events.ContainsKey (eventKey))
			events [eventKey] += method;
		else
			events.Add (eventKey, method);
	}

	public void Unsubscribe (Event eventKey, Action<T1, T2, T3> method) {
		if (events.ContainsKey (eventKey))
			events [eventKey] -= method;
	}

}

public enum Event {
	InStation
}