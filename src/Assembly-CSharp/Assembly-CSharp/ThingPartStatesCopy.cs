using System;
using System.Collections.Generic;

// Token: 0x0200027B RID: 635
public class ThingPartStatesCopy
{
	// Token: 0x0600181C RID: 6172 RVA: 0x000DD0BC File Offset: 0x000DB4BC
	public void CopySingleFromThingPart(ThingPart thingPart)
	{
		this.maxState = thingPart.states.Count;
		this.stateScriptLines = new string[1, 100];
		int currentState = thingPart.currentState;
		ThingPartState thingPartState = thingPart.states[currentState];
		for (int i = 0; i < thingPartState.scriptLines.Count; i++)
		{
			this.stateScriptLines[0, i] = thingPartState.scriptLines[i];
		}
	}

	// Token: 0x0600181D RID: 6173 RVA: 0x000DD134 File Offset: 0x000DB534
	public void CopyAllFromThingPart(ThingPart thingPart)
	{
		this.maxState = thingPart.states.Count;
		this.stateScriptLines = new string[50, 100];
		for (int i = 0; i < thingPart.states.Count; i++)
		{
			ThingPartState thingPartState = thingPart.states[i];
			for (int j = 0; j < thingPartState.scriptLines.Count; j++)
			{
				this.stateScriptLines[i, j] = thingPartState.scriptLines[j];
			}
		}
	}

	// Token: 0x0600181E RID: 6174 RVA: 0x000DD1C0 File Offset: 0x000DB5C0
	public void PasteSingleToThingPart(ThingPart thingPart)
	{
		Thing component = thingPart.transform.parent.GetComponent<Thing>();
		int currentState = thingPart.currentState;
		thingPart.states[currentState].scriptLines = new List<string>();
		thingPart.states[currentState].listeners = new List<StateListener>();
		for (int i = 0; i < 100; i++)
		{
			string text = this.stateScriptLines[0, i];
			if (!string.IsNullOrEmpty(text))
			{
				thingPart.states[currentState].scriptLines.Add(text);
			}
		}
		thingPart.states[currentState].ParseScriptLinesIntoListeners(component, thingPart, false);
		thingPart.SetStatePropertiesByTransform(false);
	}

	// Token: 0x0600181F RID: 6175 RVA: 0x000DD270 File Offset: 0x000DB670
	public void PasteAllToThingPart(ThingPart thingPart)
	{
		thingPart.currentState = 0;
		thingPart.currentStateTarget = -1;
		thingPart.states = new List<ThingPartState>();
		Thing component = thingPart.transform.parent.GetComponent<Thing>();
		for (int i = 0; i < this.maxState; i++)
		{
			thingPart.states.Add(new ThingPartState());
			thingPart.currentState = i;
			thingPart.SetStatePropertiesByTransform(false);
			for (int j = 0; j < 100; j++)
			{
				string text = this.stateScriptLines[i, j];
				if (text != string.Empty && text != null)
				{
					thingPart.states[i].scriptLines.Add(text);
					thingPart.states[i].ParseScriptLinesIntoListeners(component, thingPart, false);
				}
			}
		}
		thingPart.currentState = 0;
	}

	// Token: 0x0400167A RID: 5754
	private string[,] stateScriptLines;

	// Token: 0x0400167B RID: 5755
	private int maxState;
}
