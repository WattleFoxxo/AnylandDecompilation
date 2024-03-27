using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200027A RID: 634
public class ThingPartState
{
	// Token: 0x06001819 RID: 6169 RVA: 0x000DCFD0 File Offset: 0x000DB3D0
	public void ParseScriptLinesIntoListeners(Thing parentThing, ThingPart parentThingPart, bool doDebug = false)
	{
		this.listeners = new List<StateListener>();
		for (int i = 0; i < this.scriptLines.Count; i++)
		{
			this.listeners.Add(BehaviorScriptParser.GetStateListenerFromScriptLine(this.scriptLines[i], parentThing, parentThingPart));
		}
	}

	// Token: 0x0600181A RID: 6170 RVA: 0x000DD024 File Offset: 0x000DB424
	public bool ContainsOnAnyListener()
	{
		bool flag = false;
		foreach (StateListener stateListener in this.listeners)
		{
			StateListener.EventType eventType = stateListener.eventType;
			if (eventType == StateListener.EventType.OnAnyPartTouches || eventType == StateListener.EventType.OnAnyPartConsumed || eventType == StateListener.EventType.OnAnyPartHitting)
			{
				flag = true;
			}
			if (flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x0400166E RID: 5742
	public Vector3 position = Vector3.zero;

	// Token: 0x0400166F RID: 5743
	public Vector3 rotation = Vector3.zero;

	// Token: 0x04001670 RID: 5744
	public Vector3 scale = Vector3.one;

	// Token: 0x04001671 RID: 5745
	public Color color = Color.white;

	// Token: 0x04001672 RID: 5746
	public string name;

	// Token: 0x04001673 RID: 5747
	public List<string> scriptLines = new List<string>();

	// Token: 0x04001674 RID: 5748
	public List<StateListener> listeners = new List<StateListener>();

	// Token: 0x04001675 RID: 5749
	public bool didTriggerStartEvent;

	// Token: 0x04001676 RID: 5750
	public Color particleSystemColor = Color.white;

	// Token: 0x04001677 RID: 5751
	public Dictionary<ParticleSystemProperty, float> particleSystemProperty;

	// Token: 0x04001678 RID: 5752
	public Color[] textureColors = new Color[]
	{
		Color.white,
		Color.white
	};

	// Token: 0x04001679 RID: 5753
	public Dictionary<TextureProperty, float>[] textureProperties = new Dictionary<TextureProperty, float>[2];
}
