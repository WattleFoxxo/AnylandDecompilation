using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class AutoCompletion
{
	// Token: 0x06000A4B RID: 2635 RVA: 0x00046E6C File Offset: 0x0004526C
	public List<AutoCompletionData> GetBehaviorScriptCompletions(string textEntered)
	{
		this.lastUsedPageNumber = this.pageNumber;
		this.isFullLastWordReplacement = false;
		this.isSoundSearch = false;
		this.isSoundRelated = false;
		this.isLoopSoundRelated = false;
		this.pageCount = 1;
		this.completions = new List<AutoCompletionData>();
		string text = string.Concat(new string[]
		{
			Environment.NewLine,
			"The strength is also the effect radius in meters, with force",
			Environment.NewLine,
			"decaying by distance. Optionally, add a name filter of what to affect ",
			Environment.NewLine,
			"(may contain only a name part; * resets to match anything)."
		});
		string text2 = string.Concat(new string[]
		{
			"Forward-only applies the force in the forward direction.",
			Environment.NewLine,
			"Negative strength is the same as switching attract & repel. ",
			Environment.NewLine,
			"Parameters can be in any order."
		});
		List<string> list = new List<string>
		{
			"starts", "touched", "triggered", "neared", "consumed", "walked_into", "pointed_at", "looked_at", "taken", "grabbed",
			"shaken", "talked_from", "talked_to", "blown_at", "turned_around", "high_speed", "let_go", "raised", "lowered", "hit",
			"someone_in_vicinity", "someone_new_in_vicinity", "touch_ends", "trigger_let_go", "destroyed"
		};
		List<string> list2 = new List<string> { "told", "told_by_nearby", "told_by_any", "told_by_body" };
		textEntered = BehaviorScriptParser.NormalizeLine(textEntered, CreationHelper.thingBeingEdited.GetComponent<Thing>(), true);
		char[] array = new char[] { ' ' };
		string[] array2 = textEntered.Split(array, StringSplitOptions.RemoveEmptyEntries);
		if (array2.Length == 0)
		{
			this.Add("when ", string.Empty, string.Empty);
			if (!string.IsNullOrEmpty(CreationHelper.lastScriptLineEntered))
			{
				this.Add(CreationHelper.lastScriptLineEntered, "This line is always the last one you entered", string.Empty);
			}
		}
		else
		{
			string text3 = array2[array2.Length - 1];
			string text4 = string.Empty;
			if (array2.Length >= 2)
			{
				text4 = array2[array2.Length - 2];
			}
			switch (text3)
			{
			case "when":
			case "when_any_state":
				this.pageCount = 8;
				if (this.pageNumber == 1)
				{
					this.Add(" starts then ", "Triggers when the state starts", string.Empty);
					this.Add(" touched then ", "Triggers when the part is touched by the hand or a held thing", string.Empty);
					this.Add(" touches [thing name]", "Triggers when touched by a thing of that name", string.Empty);
					this.Add(" triggered then ", "When a holdable is held (or leg is moved) & its trigger pressed", string.Empty);
					if (!string.IsNullOrEmpty(CreationHelper.lastScriptLineEntered))
					{
						this.Add(CreationHelper.lastScriptLineEntered, "This line is always the last one you entered", string.Empty);
					}
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" neared then ", "When someone stands near this", string.Empty);
					this.Add(" gets [name]", "takes something when touched by it", string.Empty);
					this.Add(" told ", "When getting a \"tell ..\" message from another part", string.Empty);
					this.Add(" consumed then ", "When the thing is close to the mouth", string.Empty);
					this.Add(" walked into then ", "When one laser-transports on something or walks into it", string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					this.Add(" pointed at then ", "When one points the index finger at something", string.Empty);
					this.Add(" looked at then ", "When one directly looks at this", string.Empty);
					this.Add(" taken then ", "When the Holdable or Movable is picked up", string.Empty);
					this.Add(" grabbed then ", "When a non-holdable but grabbable, like a drawer knob, is grabbed", string.Empty);
					this.Add(" shaken then ", "When the holdable thing is picked up and shaken", string.Empty);
				}
				else if (this.pageNumber == 4)
				{
					this.Add(" talked from then ", "One's head when one is speaking", string.Empty);
					this.Add(" talked to then ", "When one looks at something or someone and speaks", string.Empty);
					this.Add(" blown at then ", "When one blows at something or makes sound while near mouth", string.Empty);
					this.Add(" turned around then ", "When a holdable is turned onto its head", string.Empty);
					this.Add(" high speed then ", "When the holdable thing is moved at high speed", string.Empty);
				}
				else if (this.pageNumber == 5)
				{
					this.Add(" let go then ", "When the Holdable or Movable is dropped", string.Empty);
					this.Add(" raised then ", "When one raises up a holdable", string.Empty);
					this.Add(" lowered then ", "When one lowers down a holdable", string.Empty);
					this.Add(" hit then ", "When hit by something at high speed", string.Empty);
					this.Add(" hitting [thing name]", "When hitting or being hit by something of that name at high speed", string.Empty);
				}
				else if (this.pageNumber == 6)
				{
					this.Add(" someone in vicinity then ", "When someone gets in vicinity", string.Empty);
					this.Add(" someone new in vicinity then ", "When someone gets in vicinity the first time of that area visit", string.Empty);
					this.Add(" touch ends then ", "When something stops touching", string.Empty);
					this.Add(" trigger let go then ", "When a holdable is held/ leg moved & one stops pressing the trigger", string.Empty);
					this.Add(" destroyed ", "When a thing ends, like a mug shattering on impact", string.Empty);
				}
				else if (this.pageNumber == 7)
				{
					this.Add(" any part ", "When something happens to any part of the thing or its sub-things", string.Empty);
					if (text3 == "when")
					{
						this.Add(" any state ", "Applies to all states of this part", string.Empty);
					}
					this.Add(" hears [speech]", "When this is spoken in vicinity (when attached, spoken by oneself)", string.Empty);
					this.Add(" enable setting ", "When a setting is enabled via dialog or script", string.Empty);
					this.Add(" disable setting ", "When a setting is disabled via dialog or script", string.Empty);
				}
				else if (this.pageNumber == 8)
				{
					this.Add(" typed ", "When text was typed. Can be attached & used with e.g. \"... say [typed]\"", string.Empty);
					this.Add(" is ", "When a value changes (you can also use e.g. \"when touched and is...\")", string.Empty);
				}
				goto IL_4D38;
			case "destroyed":
				this.Add(" then ", "When a thing ends, like a mug shattering on impact", string.Empty);
				this.Add(" restores then ", "When a \"destroy all parts with 30s\" type placement restores", string.Empty);
				goto IL_4D38;
			case "hears":
				this.Add(" [speech]", "When this is spoken in vicinity (when attached, spoken by oneself)", string.Empty);
				this.Add(" anywhere [speech]", "Hears area-wide at any distance", string.Empty);
				goto IL_4D38;
			case "touches":
				this.Add(" [thing name]", "When touched by a thing (or a \"call me\"-named part) of that name", string.Empty);
				this.Add(" hand", "When touched by someone's hand (or a thing called \"hand\")", string.Empty);
				this.Add(" face", "When touched by someone's face (or a thing called \"face\")." + Environment.NewLine + "For edibles, you can also use \"When consumed\"", string.Empty);
				goto IL_4D38;
			case "any_part":
				this.pageCount = 2;
				if (this.pageNumber == 1)
				{
					this.Add(" touched then ", "Triggers when any part is touched by the hand or a held Thing", string.Empty);
					this.Add(" touches [thing name]", "Triggers when any part is touched by a thing of that name", string.Empty);
					this.Add(" consumed then ", "When any part of the Thing is close to the mouth", string.Empty);
					this.Add(" hit then ", "When any part is hit by something at high speed", string.Empty);
					this.Add(" hitting [thing name]", "When any part hits or is being hit by something of that name", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" blown at then ", "When one blows at something or makes sound while near mouth", string.Empty);
					this.Add(" pointed at then ", "When one points the index finger at something", string.Empty);
					this.Add(" looked at then ", "When one directly looks at any part of the thing", string.Empty);
				}
				goto IL_4D38;
			case "controlled":
				this.pageCount = 2;
				if (this.pageNumber == 1)
				{
					this.Add(" forward ", "When the stick control for a Controllable is moved in this direction (optional)", string.Empty);
					this.Add(" backward ", "When the stick control for a Controllable is moved in this direction (optional)", string.Empty);
					this.Add(" left ", "When the stick control for a Controllable is moved in this direction (optional)", string.Empty);
					this.Add(" right ", "When the stick control for a Controllable is moved in this direction (optional)", string.Empty);
					this.Add(" up ", "When the stick control for a Controllable is moved in this direction (optional)", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" down ", "When the stick control for a Controllable is moved in this direction (optional)", string.Empty);
				}
				goto IL_4D38;
			case "when_is":
				this.pageCount = 3;
				if (this.pageNumber == 1)
				{
					this.Add(" somevalue >= 2", "When the value named \"somevalue\" of this thing is equal or over 2", string.Empty);
					this.Add(" somevalue = 2", "When the value named \"somevalue\" of this thing is equal or over 2", string.Empty);
					this.Add(" somevalue == 2", "\"=\" and \"==\" both mean equal", string.Empty);
					this.Add(" area.somevalue == 2", "When the area value named \"area.somevalue\" reaches 2", string.Empty);
					this.Add(" somestate", "When value \"somestate\" is true/ not 0 (you can also use \"not somestate\")", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" value >= 2 and state", "When all of these are true", string.Empty);
					this.Add(" value >= 2 and not state", "Use \"not\" to check for a value being untrue", string.Empty);
					this.Add(" value >= 2 or state", "When any of these is true", string.Empty);
					this.Add(" value != 2", "Check for unequal (same as \"<>\")", string.Empty);
					this.Add(" value <> 2", "Check for unequal (same as \"!=\")", string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					this.Add(" value < 2", "Check for lower than", string.Empty);
					this.Add(" value > 2", "Check for larger than", string.Empty);
				}
				goto IL_4D38;
			case "told":
				this.Add(" [e.g. pressed] then ", "Receives messages from other parts of this thing," + Environment.NewLine + "sent via \"then tell\"", string.Empty);
				this.Add(" by nearby ", "Also receives messages from other things nearby," + Environment.NewLine + "sent via \"then tell nearby\"", string.Empty);
				this.Add(" by any ", "Also receives messages from other things in the whole area," + Environment.NewLine + "sent via \"then tell any\"", string.Empty);
				this.Add(" by body ", "Only receives messages from parts of this body," + Environment.NewLine + "sent via \"tell body\"", string.Empty);
				goto IL_4D38;
			case "told_by_nearby":
				this.Add(" [e.g. pressed] then ", "When receiving this from a \"... then tell nearby ...\" command", string.Empty);
				goto IL_4D38;
			case "told_by_any":
				this.Add(" [e.g. pressed] then ", "When receiving this from a \"... then tell any ...\" command", string.Empty);
				this.Add(" track played [sound name]", "When a recorded track plays this back, e.g. \"tone c3\"", string.Empty);
				goto IL_4D38;
			case "told_by_body":
				this.pageCount = 9;
				if (this.pageNumber == 1)
				{
					this.Add(" [e.g. pressed] then ", "Listen for any tell sent from other parts via \"tell body\"", string.Empty);
					this.Add(" inventory opened left then ", "You automatically get this told by your body as it happens", string.Empty);
					this.Add(" inventory opened right then ", "You automatically get this told by your body as it happens", string.Empty);
					this.Add(" inventory closed left then ", "You automatically get this told by your body as it happens", string.Empty);
					this.Add(" inventory closed right then ", "You automatically get this told by your body as it happens", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" hand touches left then ", "You automatically get this told by your body as it happens", string.Empty);
					this.Add(" hand touches right then ", "You automatically get this told by your body as it happens", string.Empty);
					this.Add(" ping received ", "You automatically get this told by your body as it happens", string.Empty);
					this.Add(" ping received from [person name]", "You automatically get this told by your body as it happens", string.Empty);
					this.Add(" ping sent ", "You automatically get this told by your body as it happens", string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					this.Add(" moving ", "When moving with the teleport laser", string.Empty);
					this.Add(" moving fast ", "When moving over 1.5 meter with the teleport laser", string.Empty);
					this.Add(" moving very fast ", "When moving over 3 meter with the teleport laser", string.Empty);
					this.Add(" someone was born ", "You automatically get this told by your body as it happens", string.Empty);
					this.Add(" someone arrived in area ", "You automatically get this told by your body as it happens", string.Empty);
				}
				else if (this.pageNumber == 4)
				{
					this.Add(" hand triggered left then ", "When someone starts holding the trigger on their left hand", string.Empty);
					this.Add(" hand triggered right then ", "When someone starts holding the trigger on their right hand", string.Empty);
					this.Add(" hand trigger let go left then ", "When someone stops holding the trigger on their left hand", string.Empty);
					this.Add(" hand trigger let go right then ", "When someone stops holding the trigger on their right hand", string.Empty);
					this.Add(" both hands triggered then ", "When triggering both hands. Gets sent before left/ right tells", string.Empty);
				}
				else if (this.pageNumber == 5)
				{
					this.Add(" hand touches hand then ", "When your hand touches someone else's hand, like for a high-five or handshake", string.Empty);
					this.Add(" left hand touches hand", "When your left hand touches someone else's hand", string.Empty);
					this.Add(" right hand touches hand", "When your right hand touches someone else's hand", string.Empty);
					this.Add(" leg move started left then ", "When you start moving your left leg.", string.Empty);
					this.Add(" leg move started right then ", "When you start moving your right leg.", string.Empty);
				}
				else if (this.pageNumber == 6)
				{
					this.Add(" leg move ended left then ", "When you stop moving your left leg.", string.Empty);
					this.Add(" leg move ended right then ", "When you stop moving your right leg.", string.Empty);
					this.Add(" leg triggered left then ", "When starting to hold the left trigger while leg moving", string.Empty);
					this.Add(" leg triggered right then ", "When starting to hold the right trigger while leg moving", string.Empty);
					this.Add(" leg trigger let go left then ", "When stopping to hold the left trigger while leg moving", string.Empty);
				}
				else if (this.pageNumber == 7)
				{
					this.Add(" leg trigger let go right then ", "When stopping to hold the right trigger while leg moving", string.Empty);
					this.Add(" jumping then ", "When someone jumps (currently during desktop-mode only)", string.Empty);
					this.Add(" consumed then ", "When you eat something, i.e. your mouth triggers \"when consumed\"", string.Empty);
					this.Add(" dialog ", "Tells about dialogs opening or closing", string.Empty);
					this.Add(" context laser started left then ", "When one starts pressing the left hand context laser", string.Empty);
				}
				else if (this.pageNumber == 8)
				{
					this.Add(" context laser started right then ", "When one starts pressing the right hand context laser", string.Empty);
					this.Add(" context laser ended left then ", "When one stops pressing the left hand context laser", string.Empty);
					this.Add(" context laser ended right then ", "When one stops pressing the right hand context laser", string.Empty);
					this.Add(" flying then ", "When one moves into air in Change Things mode or zero gravity", string.Empty);
					this.Add(" deleting then ", "When triggering the delete action (e.g. the Vive grip button).", string.Empty);
				}
				else if (this.pageNumber == 9)
				{
					this.Add(" deleting left then ", "When triggering delete on the left hand (triggers before \"deleting\").", string.Empty);
					this.Add(" deleting right then ", "When triggering delete on the right hand (triggers before \"deleting\").", string.Empty);
					this.Add(" deleting ends then ", "When the delete action (e.g. the Vive grip button) ended.", string.Empty);
					this.Add(" deleting ends left then ", "When the delete action using the left hand ended.", string.Empty);
					this.Add(" deleting ends right then ", "When the delete action using the right hand ended.", string.Empty);
				}
				goto IL_4D38;
			case "dialog":
				if (text4 == "told_by_body")
				{
					this.pageCount = 2;
					if (this.pageNumber == 1)
					{
						this.Add(" opened left then ", "When no dialog was open and any dialog is opened now.", string.Empty);
						this.Add(" opened right then ", "When no dialog was open and any dialog is opened now.", string.Empty);
						this.Add(" closed left then ", "When a dialog is closed and no dialog is open anymore.", string.Empty);
						this.Add(" closed right then ", "When a dialog is closed and no dialog is open anymore.", string.Empty);
					}
					else if (this.pageNumber == 2)
					{
						this.Add(" [name] opened left then ", "Use names like \"create\", \"area\", \"thing part attributes\", \"material\".", string.Empty);
						this.Add(" [name] opened right then ", "Use names like \"create\", \"area\", \"thing part attributes\", \"material\".", string.Empty);
						this.Add(" [name] closed left then ", "Use names like \"create\", \"area\", \"thing part attributes\", \"material\".", string.Empty);
						this.Add(" [name] closed left then ", "Use names like \"create\", \"area\", \"thing part attributes\", \"material\".", string.Empty);
					}
				}
				goto IL_4D38;
			case "then":
				this.pageCount = 8;
				if (this.pageNumber == 1)
				{
					this.Add(" become [state]", "Goes to the state of that number", string.Empty);
					this.Add(" play ", "Plays a sound", string.Empty);
					this.Add(" tell ", "Sends a message which other parts can receive with \"when told ..\"", string.Empty);
					this.Add(" send ", "Sends to another area", string.Empty);
					this.Add(" emit ", "Emits a thing", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" loop [name]", "Loops sounds. Use \"with 100\" for volume + \".. surround\", \".. half-surround\"", string.Empty);
					this.Add(" end loop", "Ends the current sound started with \"loop\"", string.Empty);
					this.Add(" call me [name]", "Names this part's state. Checkable via e.g. \"when touched by blade\"", string.Empty);
					this.Add(" give haptic feedback", "Provide a haptic buzz for arm attachments or held things", string.Empty);
					this.Add(" let go", "Drops this item if currently held", string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					this.Add(" all parts ", "Do something specific with all parts", string.Empty);
					this.Add(" stop all parts ", "Stop doing something with all parts", string.Empty);
					this.Add(" destroy ", "Removes itself or other things", string.Empty);
					this.Add(" propel forward", "When thrown, moves forward with a given force", string.Empty);
					this.Add(" rotate forward", "When thrown, rotates forward with a given force", string.Empty);
				}
				else if (this.pageNumber == 4)
				{
					this.Add(" allow ", "Allows certain extra rights in the area", string.Empty);
					this.Add(" disallow ", "Disallows certain extra rights in the area", string.Empty);
					this.Add(" show ", "Shows a board, thread, dialog, video, line or name tags", string.Empty);
					this.Add(" do ", "Lets you create tools to change a thing part during creation", " creation ");
					this.Add(" go to inventory page [number] ", "If the inventory is currently open, moves to page 1-100 in it", string.Empty);
				}
				else if (this.pageNumber == 5)
				{
					this.Add(" add crumbles", "Shows crumbles falling off", string.Empty);
					this.Add(" type \"[text]\"", "Adds this text line (written in quotes) to a part script", string.Empty);
					this.Add(" set ", "Sets e.g. speed & snap angles or (for start event) light & voice", string.Empty);
					this.Add(" add speed ", "1 or 3 numbers, like 200.5, to add to speed of emitted/ thrown things", string.Empty);
					this.Add(" multiply speed ", "1 or 3 numbers, like 2.5, to multiply speed of emitted/ thrown things", string.Empty);
				}
				else if (this.pageNumber == 6)
				{
					this.Add(" change head", "Attaches a head, which may attach a body", string.Empty);
					this.Add(" resize nearby to ", "Resizes people to given % (needs to be placed)", string.Empty);
					this.Add(" stream ", "Commands relating to a camera stream", string.Empty);
					this.Add(" say [speech]", "Placeholders supported. \"When starts then set voice ...\" adjusts voice", string.Empty);
					this.Add(" write [text]", "Changes this part's text to this. Placeholders are supported", string.Empty);
				}
				else if (this.pageNumber == 7)
				{
					this.Add(" trail", "Start or end a trail", string.Empty);
					this.Add(" project", "Moves this part onto the next surface ahead", string.Empty);
					this.Add(" reset", "Commands to reset states, variables, deletions and more", string.Empty);
					this.Add(" enable setting ", "Enables a setting. Requires attaching the thing to you", string.Empty);
					this.Add(" disable setting ", "Disables a setting. Requires attaching the thing to you", string.Empty);
				}
				else if (this.pageNumber == 8)
				{
					this.Add(" turn ", "Sets whether the part is visible/ collidable", string.Empty);
					this.Add(" is ", "Sets a variable", string.Empty);
				}
				goto IL_4D38;
			case "destroy":
				this.Add(" all parts", "Destroys itself and its own whole thing (\"reset area\" reverts" + Environment.NewLine + "deletions)", string.Empty);
				this.Add(" nearby", "Destroys other things nearby at a given radius to surfaces and" + Environment.NewLine + "maximum thing size", string.Empty);
				goto IL_4D38;
			case "trail":
				this.Add(" start", "Starts a trail left behind this part, using the part materials & colors", string.Empty);
				this.Add(" start with 10s", string.Concat(new object[]
				{
					"Sets a trail duration of e.g. 10 seconds (default is 15s, maximum is ",
					60f,
					"s).",
					Environment.NewLine,
					"The \"s\" is optional"
				}), string.Empty);
				this.Add(" start with thick-start", "Lets the line start at its normal width instead of sharply", string.Empty);
				this.Add(" start with thick-end", "Lets the line end at its normal width instead of sharply", string.Empty);
				this.Add(" end", "Ends the current trail", string.Empty);
				goto IL_4D38;
			case "project":
				this.pageCount = 2;
				if (this.pageNumber == 1)
				{
					ProjectPartSettings projectPartSettings = new ProjectPartSettings();
					this.Add(" with 100% reach", "The distance percentage between start and the surface ahead" + Environment.NewLine + "(from 0-1000)", string.Empty);
					this.Add(" with 50.5 max", string.Concat(new object[]
					{
						"Maximum distance in meters at which to look for surfaces, ",
						Environment.NewLine,
						"from 0.01-10000 (default: ",
						projectPartSettings.maxDistance,
						")"
					}), string.Empty);
					this.Add(" with 1000 default", "The distance in meters to use when no surface is hit." + Environment.NewLine + "(Default: 0, Max: 1000)", string.Empty);
					this.Add(" with 0% reach", "Ends the projection", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" with alignment", "Aligns the projected part to the surface direction", string.Empty);
					this.Add(" with counter-alignment", "Aligns the projected part away from the surface direction", string.Empty);
				}
				goto IL_4D38;
			case "show_line":
				this.Add(" with 0.1 width", string.Concat(new object[] { "Optionally sets the line width from 0-", 10f, " (default: ", 0.01f, ")" }), string.Empty);
				this.Add(" with 0.1 start-width", "Sets the start width at the thing's core", string.Empty);
				this.Add(" with 0.1 end-width", "Sets the end width at this part", string.Empty);
				this.Add(" with 0 width", "Setting any width to 0 removes the line", string.Empty);
				goto IL_4D38;
			case "emit":
				this.Add(" [drop from inventory]", "Emits a thing. Optionally add e.g. \"with 10%\" (0-100) to set speed", string.Empty);
				this.Add(" gravity-free [drop from inventory]", "Emits something at a given speed with no gravity", string.Empty);
				goto IL_4D38;
			case "do":
				this.Add(" creation part ", "Lets you create tools to change a thing part during creation", string.Empty);
				this.Add(" all creation parts ", "Lets you create tools to change thing parts during creation", string.Empty);
				goto IL_4D38;
			case "change_head":
				this.Add(" to [drop from inventory]", "Attaches a head, which may attach a body. Must be placed or attached", string.Empty);
				this.Add("s to [drop from inventory]", "Same as \"change head\" but for everyone around", string.Empty);
				goto IL_4D38;
			case "add_crumbles":
				this.Add(" for all parts", "Optionally, shows crumbles falling off all parts", string.Empty);
				goto IL_4D38;
			case "destroy_all_parts":
			case "destroy_nearby":
				this.Add(" with ", "Optionally, for more destruction options", string.Empty);
				goto IL_4D38;
			case "reset":
			{
				this.pageCount = 2;
				string text5 = Environment.NewLine + "(needs to be placed)";
				string text6 = Environment.NewLine + "(needs to be attached to body)";
				if (this.pageNumber == 1)
				{
					this.Add(" area", "Resets states, thing & area.* variables, deletions & Movable positions" + text5, string.Empty);
					this.Add(" persons", "Resets person.* variables of everyone currently here" + text5, string.Empty);
					this.Add(" position", "Resets thing to its original placement position, e.g. for Movables" + text5, string.Empty);
					this.Add(" rotation", "Resets thing to its original placement rotation, e.g. for Movables" + text5, string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" body", "Removes all of one's attached body parts", string.Empty);
					this.Add(" legs to default", "Resets leg attachments to their universal default positions" + text6, string.Empty);
					this.Add(" legs to body default", "Resets leg attachments to the auto-eqipped body default " + Environment.NewLine + "positions, or if unavailable, the universal ones" + text6, string.Empty);
				}
				goto IL_4D38;
			}
			case "is":
				this.pageCount = 7;
				if (this.pageNumber == 1)
				{
					this.Add(" somevalue = 10 * (othervalue + 0.5)", string.Concat(new object[]
					{
						"Sets thing value \"somevalue\" (persists until everyone leaves).",
						Environment.NewLine,
						"Numbers are set to 0 at start. Up to ",
						50,
						" per tick are calculated."
					}), string.Empty);
					this.Add(" area.somevalue = 10.5", "A math expression to set area number \"somevalue\"" + Environment.NewLine + "(persists until everyone leaves)", string.Empty);
					this.Add(" somestate", "Sets \"somestate\" to be true (which equals 1)", string.Empty);
					this.Add(" not somestate", "Sets \"somestate\" to be false (which equals 0)", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" person.somevalue = 2", "Sets value for a person (who has this attached or is closest) in that" + Environment.NewLine + "area, e.g. person.gold=5 (semi-persisting & can't be read by other areas)", string.Empty);
					this.Add(" value = random(min max)", "Get a random number between first & second parameters (inclusive)", string.Empty);
					this.Add(" value = randomfloat(min max)", "Get a random floating point number between first & second (inclusive)", string.Empty);
					this.Add(" value = absolute(value)", "Gets the positive number, e.g. -17.5 turns into 17.5", string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					this.Add(" value = round(value)", "Rounds a value to the nearest integer", string.Empty);
					this.Add(" value = floor(value)", "Rounds the value down", string.Empty);
					this.Add(" value = ceil(value)", "Rounds the value up", string.Empty);
					this.Add(" value = smaller(a b)", "Gets the smaller of the two number parameters", string.Empty);
				}
				else if (this.pageNumber == 4)
				{
					this.Add(" value = larger(a b)", "Gets the larger of the two number parameters", string.Empty);
					this.Add(" value = sqrt(value)", "Returns the square root", string.Empty);
					this.Add(" value = exp(value)", "Returns Euler's number e raised to the specified power", string.Empty);
					this.Add(" value = sin(value)", "Returns the sine of the specified angle", string.Empty);
				}
				else if (this.pageNumber == 5)
				{
					this.Add(" value = cos(value)", "Returns the cosine of the specified angle", string.Empty);
					this.Add(" value = tan(value)", "Returns the tangent of the specified angle", string.Empty);
					this.Add(" value = log(value)", "Returns the natural base e logarithm of a number", string.Empty);
					this.Add(" value = mod(number divider)", "Returns the remainder of number divided by divider (modulo operation)", string.Empty);
				}
				else if (this.pageNumber == 6)
				{
					this.Add(" value += [millisecond] - [x]", "Uses available placeholder values if they return numbers, like" + Environment.NewLine + "[hour] [local hour] [second] [x] [y] [people count]", string.Empty);
					this.Add(" value += 2", "Increases the number named \"value\" by 2", string.Empty);
					this.Add(" value -= 2", "Decreases the number named \"value\" by 2", string.Empty);
					this.Add(" value *= 2", "Multiplies the number named \"value\" by 2", string.Empty);
				}
				else if (this.pageNumber == 7)
				{
					this.Add(" value /= 2", "Divides the value by 2. Division by 0 is ignored", string.Empty);
					this.Add(" value++", "Increases the number named \"value\" by 1", string.Empty);
					this.Add(" value--", "Decreases the number named \"value\" by 1", string.Empty);
				}
				goto IL_4D38;
			case "set":
			{
				bool flag = array2.Length >= 2 && array2[1] == "starts";
				this.pageCount = 3;
				if (this.pageNumber == 1)
				{
					this.Add(" speed ", "1 or 3 numbers, like 200.5, to set speed of emitted/ thrown things", string.Empty);
					this.Add(" constant rotation to ", "Sets a rotation for this part (without it needing to be thrown).", string.Empty);
					this.Add(" snap angles to ", "Use a custom snap value. Needs to be on body", string.Empty);
					this.Add(" camera ", "Set the desktop camera position or follow smoothing", string.Empty);
					this.Add(" gravity to ", "Sets the gravity for the area", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" area visibility to ", "Limits the distance at which things can be seen", string.Empty);
					this.Add(" person as authority", "Makes one sync authority (needs placement or editor attachment)." + Environment.NewLine + "Otherwise, by default, that's whoever has been in the area longest.", string.Empty);
					this.Add(" quest", "Handles quest setting", string.Empty);
					this.Add(" attract [strength]", "Attracts thrown/ emitted items with certain strength", string.Empty);
					this.Add(" repel [strength]", "Opposite of attract, pushes away thrown/ emitted items", string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					string text7 = Environment.NewLine + "Requires placement. Resets when transporting to new area.";
					this.Add(" run speed [number]", string.Concat(new object[]
					{
						"The Desktop mode forward speed when pressing Shift, ",
						2f,
						".0 - 100.0",
						Environment.NewLine,
						"or keyword \"default\" (default is ",
						4.5f,
						").",
						text7
					}), string.Empty);
					this.Add(" jump speed [number]", string.Concat(new object[]
					{
						"The desktop mode upwards speed when pressing Space, 0.0 - 100.0",
						Environment.NewLine,
						"or keyword \"default\" (default is ",
						5f,
						").",
						text7
					}), string.Empty);
					this.Add(" slidiness [number]", "The desktop mode speed slidiness, 0.0 - 100.0" + Environment.NewLine + "or keyword \"default\" (default is 0)." + text7, string.Empty);
					if (flag)
					{
						this.Add(" light ", "Sets further light properties, like intensity or range", string.Empty);
						this.Add(" voice ", "Sets the voice properties for the \"say\" command", string.Empty);
					}
				}
				goto IL_4D38;
			}
			case "set_attract":
			{
				string text8 = string.Concat(new object[] { "Attracts thrown/ emitted items with strength of up to ", 1000f, ".", text });
				this.Add(" 10", text8, string.Empty);
				this.Add(" thingname 50", text8, string.Empty);
				this.Add(" thingname -20.5 forward-only", text2, string.Empty);
				this.Add(" 0", "Stops attracting thrown/ emitted items", string.Empty);
				goto IL_4D38;
			}
			case "set_repel":
			{
				string text9 = string.Concat(new object[] { "Pushes away thrown/ emitted items with strength of up to ", 1000f, ".", text });
				this.Add(" 10", text9, string.Empty);
				this.Add(" thingname 50", text9, string.Empty);
				this.Add(" thingname -20.5 forward-only", text2, string.Empty);
				this.Add(" 0", "Stops repelling thrown/ emitted items", string.Empty);
				goto IL_4D38;
			}
			case "set_quest":
			{
				string text10 = string.Concat(new string[]
				{
					Environment.NewLine,
					"Quests are started by reading board thread-starter comments like ",
					Environment.NewLine,
					"[quest: someareaname - somequestname], e.g. ",
					Environment.NewLine,
					"[quest: dungeon - king sceptre].",
					Environment.NewLine,
					"Command requires placement in the respective area."
				});
				this.Add(" achieve [quest name]", "Marks a quest as achieved. " + text10, string.Empty);
				this.Add(" unachieve [quest name]", "Marks a quest as unachieved, but still running. " + text10, string.Empty);
				this.Add(" remove [quest name]", "Fully removes a quest. " + text10, string.Empty);
				goto IL_4D38;
			}
			case "set_constant_rotation_to":
			{
				string text11 = "Sets the x y z rotation speed for the part in degrees per second." + Environment.NewLine + "Starts when done.";
				this.Add("10 0 0", text11, string.Empty);
				this.Add("0 -20.5 0", text11, string.Empty);
				goto IL_4D38;
			}
			case "set_area_visibility_to":
			{
				string text12 = string.Concat(new string[]
				{
					"Limits the distance at which Things can be seen to this many meters.",
					Environment.NewLine,
					"Distance is measured from head to Thing center. Min. 2.5, \"m\" is optional.",
					Environment.NewLine,
					"\"Show if far away too\" Things are exempt. Large values do not increase ",
					Environment.NewLine,
					" visibility."
				});
				this.Add(" 15m", text12, string.Empty);
				this.Add(" 30m", text12, string.Empty);
				this.Add(" default", "Removes the special visibility limit again", string.Empty);
				goto IL_4D38;
			}
			case "set_camera":
				this.Add(" position to ", "Set the desktop camera position", string.Empty);
				this.Add(" following to ", "Set the desktop camera follow smoothing", string.Empty);
				goto IL_4D38;
			case "set_camera_position_to":
				this.pageCount = 2;
				if (this.pageNumber == 1)
				{
					this.Add(" default", "Sets this position (needs to be on body). Default: Own VR view)", string.Empty);
					this.Add(" optimized view", "Sets this position (needs to be on body). Default: Own VR view)", string.Empty);
					this.Add(" view from behind", "Sets this position (needs to be on body). Default: Own VR view)", string.Empty);
					this.Add(" view from further behind", "Sets this position (needs to be on body). Default: Own VR view)", string.Empty);
					this.Add(" bird's eye", "Sets this position (needs to be on body). Default: Own VR view)", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" looking at me", "Sets this position (needs to be on body). Default: Own VR view)", string.Empty);
					this.Add(" left hand", "Sets this position (needs to be on body). Default: Own VR view)", string.Empty);
					this.Add(" right hand", "Sets this position (needs to be on body). Default: Own VR view)", string.Empty);
				}
				goto IL_4D38;
			case "set_camera_following_to":
				this.Add(" default", "The camera follows directly without smoothing (needs to be on body)", string.Empty);
				this.Add(" smoothly", "The camera will follow smoothly (needs to be on body)", string.Empty);
				this.Add(" very smoothly", "The camera will follow very smoothly (needs to be on body)", string.Empty);
				this.Add(" none", "The camera will remain statically in its current place (needs to be on body)", string.Empty);
				goto IL_4D38;
			case "set_snap_angles_to":
				this.Add(" [number]", "E.g. 45 (normal: 90, soft: 22.5). Must be on body. Resets on area change", string.Empty);
				this.Add(" default", "Unsets previously set angles and uses the current default snap settings", string.Empty);
				goto IL_4D38;
			case "set_light":
				this.Add(" intensity ", "sets the strength of applied light materials from 0-100 (default: 12.5)", string.Empty);
				this.Add(" range ", "sets the distance applied light materials shine from 0-" + 10000f + " meter", string.Empty);
				this.Add(" cone size ", "sets the cone size for spotlights from 0-" + Universe.maxLightConeSize, string.Empty);
				goto IL_4D38;
			case "set_voice":
				this.Add(" female 50% 10 pitch 5 speed", "sets to female, 50% volume, +10 pitch, +5 speed", string.Empty);
				this.Add(" -5 speed", "Adjusts the speech rate from -10 to 10", string.Empty);
				this.Add(" male ", "Sets the voice to male or female", string.Empty);
				this.Add(" 50% ", "Volume from 0-200% (default 100%). Considers Thing's \"Surround Sound\"", string.Empty);
				this.Add(" 5 pitch ", "Adjusts the pitch from -10 to 10", string.Empty);
				goto IL_4D38;
			case "set_gravity_to":
				this.Add(" [x y z]", string.Concat(new object[]
				{
					"Sets the area gravity. Default is 0 ",
					-9.81f,
					" 0. ",
					Environment.NewLine,
					"Each value can range from -",
					1000f,
					" to ",
					1000f,
					"."
				}), string.Empty);
				this.Add(" default", "Sets the area gravity to the default 0 " + -9.81f + " 0.", string.Empty);
				goto IL_4D38;
			case "resize_nearby_to":
			{
				string[] array3 = new string[5];
				array3[0] = "Use ";
				int num2 = 1;
				int num = 1;
				array3[num2] = num.ToString();
				array3[2] = "-";
				array3[3] = 2500.ToString();
				array3[4] = "% (% optional; needs to be placed & 10% diff. to default)";
				string text13 = string.Concat(array3);
				this.Add(" 50% ", text13, string.Empty);
				this.Add(" 150 ", text13, string.Empty);
				this.Add(" " + 1, text13, string.Empty);
				this.Add(" " + 2500 + "% ", text13, string.Empty);
				this.Add(" 100% ", "Resets size to default (needs to be placed)", string.Empty);
				goto IL_4D38;
			}
			case "stream":
				this.Add(" to nearest", "Becomes a camera and streams to nearest video screen", string.Empty);
				this.Add(" to [thing name]", "Streams to the name of the thing which has a video surface", string.Empty);
				this.Add(" to desktop", "Streams to the desktop screen", string.Empty);
				this.Add(" stop", "Stops the stream", string.Empty);
				goto IL_4D38;
			case "send":
				this.pageCount = 2;
				if (this.pageNumber == 1)
				{
					this.Add(" nearby to [area name]", "placements & attachm. transport there. You can add \"onto [thing name]\"", string.Empty);
					this.Add(" nearby onto [thing name]", "sends onto closest thing of that name. Also \"at n degrees\" in steps of 45", string.Empty);
					this.Add(" one nearby to [area name]", "Like \"send nearby\", but only for e.g. closest or button-touching person", string.Empty);
					this.Add(" one nearby onto [thing name]", "sends onto closest thing of that name. Also \"at n degrees\" in steps of 45", string.Empty);
					this.Add(" all to [area name]", "placements & attachm. transport there. You can add \"onto [thing name]\"", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" all onto [thing name]", "sends everyone onto the closest thing of that name", string.Empty);
				}
				goto IL_4D38;
			case "set_light_intensity":
			{
				float num3 = 1f / Universe.maxLightIntensity * 100f;
				this.Add(" [number]", string.Empty, string.Empty);
				this.Add(" 0", string.Empty, string.Empty);
				this.Add(" " + num3, "this is the default light intensity", string.Empty);
				this.Add(" 50", string.Empty, string.Empty);
				this.Add(" 100", "this is the maximum light intensity", string.Empty);
				goto IL_4D38;
			}
			case "set_light_range":
				this.Add(" [number]", string.Empty, string.Empty);
				this.Add(" 0", string.Empty, string.Empty);
				this.Add(" 50", string.Empty, string.Empty);
				this.Add(" 100", string.Empty, string.Empty);
				this.Add(" " + 10000f, "this is the maximum distance lights can shine", string.Empty);
				goto IL_4D38;
			case "set_light_cone_size":
				this.Add(" [number]", string.Empty, string.Empty);
				this.Add(" 0", string.Empty, string.Empty);
				this.Add(" 50", string.Empty, string.Empty);
				this.Add(" 100", string.Empty, string.Empty);
				this.Add(" " + Universe.maxLightConeSize, "this is the maximum light cone size", string.Empty);
				goto IL_4D38;
			case "all_parts":
				this.pageCount = 2;
				if (this.pageNumber == 1)
				{
					this.Add(" face someone", "Rotates the whole thing towards the head of the closest person", string.Empty);
					this.Add(" face someone else", "Rotates the whole thing towards the head of the second-closest person", string.Empty);
					this.Add(" face up", "Rotates the whole thing upwards", string.Empty);
					this.Add(" face empty hand", "Rotates towards the closest empty hand", string.Empty);
					this.Add(" face empty hand while held", "Rotates towards the closest empty hand while being held", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" face nearest [thing name]", "Rotates towards the closest thing of that name", string.Empty);
					this.Add(" face nearest [thing name]", "Rotates towards closest thing while locking world or local rotation," + Environment.NewLine + "e.g. \"lock local xy\" or \"lock x lock z\".", " lock [...]");
					this.Add(" face view", "Rotates the thing towards the view of the eyes/ default camera", string.Empty);
				}
				goto IL_4D38;
			case "stop_all_parts":
				this.Add(" face someone", "Stops rotating towards closest or second-closest person", string.Empty);
				this.Add(" face empty hand", "Stops rotating towards the empty hand", string.Empty);
				this.Add(" face up", "Stops rotating the whole thing upwards", string.Empty);
				this.Add(" face nearest", "Stops rotating towards the closest thing of a name", string.Empty);
				this.Add(" face view", "Stops rotating towards the view of the eyes/ default camera", string.Empty);
				goto IL_4D38;
			case "turn":
			case "turn_thing":
				this.pageCount = 2;
				if (this.pageNumber == 1)
				{
					if (text3 != "turn_thing")
					{
						this.Add(" thing ", "Relates to the whole thing", string.Empty);
					}
					this.Add(" on", "Enables both collision & visibility for this part", string.Empty);
					this.Add(" off", "Disables both collision & visibility for this parts", string.Empty);
					this.Add(" visible", "Turns visible", string.Empty);
					this.Add(" invisible", "Turns invisible (unless \"See invisible as editor\" is on)", string.Empty);
				}
				else
				{
					this.Add(" collidable", "Turns collidable", string.Empty);
					this.Add(" uncollidable", "Turns uncollidable (unless \"Touch uncollidable as editor\" is on or the inventory shows)", string.Empty);
					this.Add(" sub-thing [optional name]", "Relates to this part's included sub-things of this name or any name", string.Empty);
				}
				goto IL_4D38;
			case "send_nearby_to":
			case "send_one_nearby_to":
				this.Add(" [enter an area]", "transports to an area of that name, e.g. \"send nearby to cafe\"", string.Empty);
				this.Add(" [area] onto [thing]", "sends onto closest thing of that name. Also \"at n degrees\" in steps of 45", string.Empty);
				this.Add(" [area] via 7.5s [area]", "optionally, transits like \"send nearby onto cafe via 5s train\" (1-150s)", string.Empty);
				this.Add(" previous", "transports to the previous area (while skipping \"via\"-transit areas)", string.Empty);
				this.Add(" current", "transports to the current area, useful e.g. when combined with \"via\"", string.Empty);
				this.Add(" [closest held]", "transports to an area by the name of the closest held thing, if any", string.Empty);
				goto IL_4D38;
			case "do_creation_part":
			case "do_all_creation_parts":
				this.pageCount = 6;
				if (this.pageNumber == 1)
				{
					this.Add(" move 0.0 0.0 0.0", "Moves a part's x-y-z coordinates relative to the world", string.Empty);
					this.Add(" move local 0.0 0.0 0.0", "Moves a part's x-y-z coordinates relative to itself", string.Empty);
					this.Add(" move random 0.0 0.0 0.0", "Moves a part's x-y-z coordinates randomly relative to the world", string.Empty);
					this.Add(" move local random 0.0 0.0 0.0", "Moves a part's x-y-z coordinates randomly relative to itself", string.Empty);
					this.Add(" rotate 0.0 0.0 0.0", "Rotates a part's x-y-z coordinates relative to the world", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" rotate local 0 0 0", "Rotates a part's x-y-z coordinates relative to itself", string.Empty);
					this.Add(" rotate random 0 0 0", "Rotates a part's x-y-z coordinates randomly", string.Empty);
					this.Add(" rotate local random 0 0 0", "Rotates a part's x-y-z coordinates relative to itself", string.Empty);
					this.Add(" scale local 0.0 0.0 0.0", "Scales a part's x-y-z coordinates relative to itself", string.Empty);
					this.Add(" scale local random 0.0 0.0 0.0", "Scales a part's x-y-z coordinates randomly relative to itself", string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					this.Add(" color 0 0 0", "Adjusts the red, green and blue values from 0-255", string.Empty);
					this.Add(" color random 0 0 0", "Adjusts the red, green and blue values randomly", string.Empty);
					this.Add(" saturation 0.0", "Adjusts the color saturation from 0-1", string.Empty);
					this.Add(" hue 0.0", "Adjusts the color hue from 0-1", string.Empty);
					this.Add(" hue random 0.0", "Adjusts the color hue randomly", string.Empty);
				}
				else if (this.pageNumber == 4)
				{
					this.Add(" lightness 0.0", "Adjusts the color lightness from 0-1", string.Empty);
					this.Add(" duplicate 0.0 0.0 0.0", "Copies & moves to the x-y-z coordinates relative to the world", string.Empty);
					this.Add(" duplicate local 0.0 0.0 0.0", "Copies & moves to the x-y-z coordinates relative to itself", string.Empty);
					this.Add(" duplicate random 0.0 0.0 0.0", "Copies & moves to the x-y-z coordinates randomly relative to the world", string.Empty);
					this.Add(" duplicate local random 0.0 0.0 0.0", "Copies & moves to the x-y-z coordinates randomly relative to itself", string.Empty);
				}
				else if (this.pageNumber == 5)
				{
					this.Add(" material", "Sets to different materials", string.Empty);
					this.Add(" become 1", "Sets to a given state if it exists", string.Empty);
					this.Add(" become stopped 1", "Sets to a given state while ignoring its \"when starts...\"", string.Empty);
					this.Add(" insert state", "Inserts a new cell after the current one", string.Empty);
					this.Add(" remove state", "Removes the current cell, unless it's the last remaining one", string.Empty);
				}
				else if (this.pageNumber == 6)
				{
					this.Add(" undo", "Reverts the last action, if any", string.Empty);
				}
				goto IL_4D38;
			case "do_creation_part_material":
			case "do_all_creation_parts_material":
				this.pageCount = 3;
				if (this.pageNumber == 1)
				{
					this.Add(" default", "Sets to this material", string.Empty);
					this.Add(" metallic", "Sets to this material", string.Empty);
					this.Add(" very metallic", "Sets to this material", string.Empty);
					this.Add(" dark metallic", "Sets to this material", string.Empty);
					this.Add(" bright metallic", "Sets to this material", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" glow", "Sets to this material", string.Empty);
					this.Add(" plastic", "Sets to this material", string.Empty);
					this.Add(" unshiny", "Sets to this material", string.Empty);
					this.Add(" transparent", "Sets to this material", string.Empty);
					this.Add(" transparent glossy", "Sets to this material", string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					this.Add(" transparent glossy metallic", "Sets to this material", string.Empty);
					this.Add(" very transparent", "Sets to this material", string.Empty);
					this.Add(" very transparent glossy", "Sets to this material", string.Empty);
					this.Add(" slightly transparent", "Sets to this material", string.Empty);
					this.Add(" transparent texture", "Sets to this material", string.Empty);
				}
				goto IL_4D38;
			case "allow":
			{
				this.pageCount = 3;
				AreaRights areaRights = new AreaRights();
				if (this.pageNumber == 1)
				{
					this.Add(" emitted climbing", "Allow teleporting onto thrown/ emitted things." + this.AddDefaultRight(areaRights.emittedClimbing), string.Empty);
					this.Add(" emitted transporting", "Allow thrown/ emitted things to send to areas/ locations." + this.AddDefaultRight(areaRights.emittedTransporting), string.Empty);
					this.Add(" moving through obstacles", "Allow peeking & walking through e.g. walls." + this.AddDefaultRight(areaRights.movingThroughObstacles), string.Empty);
					this.Add(" vision in obstacles", "Allows seeing & moving if one's head happens to get inside objects." + this.AddDefaultRight(areaRights.visionInObstacles), string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" invisibility", "Hides one's spheres & tag in the area (with disclaimer)." + this.AddDefaultRight(areaRights.invisibility), string.Empty);
					this.Add(" any person size", "Allows non-editors to resize like editors, i.e. up to " + 2500.ToString() + "%." + this.AddDefaultRight(areaRights.anyPersonSize), string.Empty);
					this.Add(" highlighting", "Allows to highlight certain things via the Area Highlight dialog." + this.AddDefaultRight(areaRights.highlighting), string.Empty);
					this.Add(" amplified speech", "Allows speech amplifying." + this.AddDefaultRight(areaRights.amplifiedSpeech), string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					this.Add(" any destruction", "Allows placements to be destroyed via \"destroy nearby\"." + this.AddDefaultRight(areaRights.anyDestruction), string.Empty);
					this.Add(" web browsing", "Allows web pages to be shown on screens." + this.AddDefaultRight(areaRights.webBrowsing), string.Empty);
					this.Add(" untargeted attract and repel", "Allows \"set attract/ repel\" commands without name filters." + this.AddDefaultRight(areaRights.untargetedAttractThings), string.Empty);
					this.Add(" build animations", "Allow non-editors to start build animations (collisions remain)." + this.AddDefaultRight(areaRights.slowBuildCreation), string.Empty);
				}
				goto IL_4D38;
			}
			case "disallow":
			{
				this.pageCount = 3;
				AreaRights areaRights2 = new AreaRights();
				if (this.pageNumber == 1)
				{
					this.Add(" emitted climbing", "Disallow teleporting onto thrown/ emitted things." + this.AddDefaultRight(areaRights2.emittedClimbing), string.Empty);
					this.Add(" emitted transporting", "Disallow thrown/ emitted things to send to areas/ locations." + this.AddDefaultRight(areaRights2.emittedTransporting), string.Empty);
					this.Add(" moving through obstacles", "Disallows peeking & walking through e.g. walls." + this.AddDefaultRight(areaRights2.emittedTransporting), string.Empty);
					this.Add(" vision in obstacles", "Disallows seeing & moving if one's head happens to get inside objects." + this.AddDefaultRight(areaRights2.visionInObstacles), string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" invisibility", "Disallow hiding one's spheres & tag in the area." + this.AddDefaultRight(areaRights2.invisibility), string.Empty);
					this.Add(" any person size", string.Concat(new string[]
					{
						"Disallows non-editors to resize like editors, i.e. limits to ",
						1.ToString(),
						"%-",
						150.ToString(),
						"%.",
						this.AddDefaultRight(areaRights2.anyPersonSize)
					}), string.Empty);
					this.Add(" highlighting", "Disallows to highlight certain things via the Area Highlight dialog." + this.AddDefaultRight(areaRights2.highlighting), string.Empty);
					this.Add(" amplified speech", "Disallows speech amplifying." + this.AddDefaultRight(areaRights2.amplifiedSpeech), string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					this.Add(" any destruction", "Disallows placements to be destroyed via \"destroy nearby\"." + this.AddDefaultRight(areaRights2.anyDestruction), string.Empty);
					this.Add(" web browsing", "Disallows web pages to be shown on screens." + this.AddDefaultRight(areaRights2.webBrowsing), string.Empty);
					this.Add(" untargeted attract and repel", "Disallows \"attract/ repel\" commands without or < 3 letters name filters." + this.AddDefaultRight(areaRights2.untargetedAttractThings), string.Empty);
					this.Add(" build animations", "Disallow non-editors to start build animations." + this.AddDefaultRight(areaRights2.slowBuildCreation), string.Empty);
				}
				goto IL_4D38;
			}
			case "become":
			case "become_untweened":
			case "become_unsoftened":
			case "become_soft_start":
			case "become_soft_end":
				this.pageCount = ((!(text3 == "become")) ? 1 : 2);
				if (this.pageNumber == 1)
				{
					this.Add(" next", "Goes to the next state, or the first if end of states reached", string.Empty);
					this.Add(" previous", "Goes to the previous state, or the last if beginning of states reached", string.Empty);
					if (CreationHelper.thingPartWhoseStatesAreEdited != null)
					{
						ThingPart component = CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<ThingPart>();
						int num4 = 0;
						int num5 = 0;
						while (num5 < 50 && num4 < 1)
						{
							if (num5 != component.currentState)
							{
								this.Add(" " + (num5 + 1), string.Empty, string.Empty);
								num4++;
							}
							num5++;
						}
					}
					if (text3 == "become")
					{
						this.Add(" untweened ", "Uses no transition animation between these states", string.Empty);
						this.Add(" unsoftened ", "A steady transition (also see attribute \"Unsoftened animations\")", string.Empty);
					}
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" soft start ", "Only animation start is soft (by default, start + end are soft)", string.Empty);
					this.Add(" soft end ", "Only animation end is soft (by default, start + end are soft)", string.Empty);
					this.Add(" 3 via 2 ", "Uses e.g. state 2 as intermediate for curvier movement" + Environment.NewLine + "(combinable; put command last)", string.Empty);
					this.Add(" current", "Goes again to the current state." + Environment.NewLine + "Can be used along with \"when any state\".", string.Empty);
				}
				goto IL_4D38;
			case "tell":
				this.pageCount = 3;
				if (this.pageNumber == 1)
				{
					this.Add(" [e.g. pressed] ", "Sends a message to other parts of this thing, receivable via \"when told\"." + Environment.NewLine + "Also see anyland.com/scripting for an overview.", string.Empty);
					this.Add(" nearby ", "Also sends message to other things nearby," + Environment.NewLine + "receivable via \"when told by nearby\".", string.Empty);
					this.Add(" any ", "Also sends messages to any thing in the whole area," + Environment.NewLine + "receivable via \"when told by any\".", string.Empty);
					this.Add(" body ", "If attached, sends message to other parts of this body & holdables," + Environment.NewLine + "receivable via \"when told by body\".", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" first of any ", "Sends a message to the closest reacting other thing," + Environment.NewLine + "receivable via \"when told by any\".", string.Empty);
					this.Add(" in front ", "Sends a message to all parts in front, receivable via \"when told by any\"." + Environment.NewLine + "Use \"...\" -> \"Show Direction\" for the part to see the forward line.", string.Empty);
					this.Add(" first in front ", "Messages the first part ahead, receivable via \"when told by any\"." + Environment.NewLine + "Use \"...\" -> \"Show Direction\" for the part to see the forward line.", string.Empty);
					this.Add(" web ", "Calls the web page JavaScript function AnylandTold(s, isAuthority) " + Environment.NewLine + "for a screen of this part's thing. Authority is 1 person in the area.", string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					this.Add(" any web ", "Calls web page JavaScript function AnylandToldByAny(s, isAuthority)" + Environment.NewLine + "for any thing in the area. Authority is 1 person in the area.", string.Empty);
				}
				goto IL_4D38;
			case "tell_nearby":
				this.Add(" [e.g. pressed] ", "Your message, e.g. \"pressed\", sent to things nearby," + Environment.NewLine + "receivable with \"when told\".", string.Empty);
				goto IL_4D38;
			case "tell_any":
				this.Add(" [e.g. pressed] ", "Your message content, e.g. \"pressed\", sent to things in the whole area," + Environment.NewLine + "receivable with \"when told by any\".", string.Empty);
				goto IL_4D38;
			case "tell_body":
				this.Add(" [e.g. pressed] ", "Your message content, e.g. \"pressed\", sent to body parts & holdables," + Environment.NewLine + "receivable with \"when told by body\".", string.Empty);
				goto IL_4D38;
			case "tell_first_of_any":
				this.Add(" [e.g. pressed] ", "Your message content, e.g. \"pressed\", sent to the closest active receiving thing," + Environment.NewLine + "receivable with \"when told by any\".", string.Empty);
				goto IL_4D38;
			case "tell_in_front":
				this.Add(" [e.g. pressed] ", string.Concat(new string[]
				{
					"Your message content, e.g. \"pressed\", sent to all things in front,",
					Environment.NewLine,
					"receivable with \"when told by any\". \"Can pass through\" Things are exempted.",
					Environment.NewLine,
					"Use \"...\" -> \"Show Direction\" for the part to see the forward line."
				}), string.Empty);
				goto IL_4D38;
			case "show":
			{
				this.pageCount = 3;
				string text14 = Environment.NewLine + "Requires placement/ attachment and one's action (e.g. touch).";
				if (this.pageNumber == 1)
				{
					this.Add(" board [name]", "Opens a dialog with the board by that name." + text14, string.Empty);
					this.Add(" thread [link]", "Opens a board thread for that link (copy it from the back of threads)." + text14, string.Empty);
					this.Add(" video controls", "Shows the controls dialog for the closest nearby video screen", string.Empty);
					this.Add(" camera controls", "Shows the controls dialog for the closest nearby camera", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" slideshow controls", "Shows the controls dialog for the closest nearby slideshow screen", string.Empty);
					this.Add(" video [url]", "Plays the YouTube URL pasted (you can also copy it from the video" + Environment.NewLine + "controls backside). Optionally add e.g. \"with 0.15%\" to adjust volume.", string.Empty);
					this.Add(" web [url]", "Loads URL in a browser on the nearest screen. Add \"with\" for more. Page" + Environment.NewLine + "JavaScript can use AnylandTell(text), AnylandTellAny(text) & AnylandClosePage().", string.Empty);
					this.Add(" chat keyboard", "Lets you enter a text chat line", string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					this.Add(" line", "Shows a line from the thing's center to this part", string.Empty);
					this.Add(" name tags", "Shows name tags again for a while (optionally add e.g. \"100s\").", string.Empty);
					this.Add(" areas [search]", "Opens an area search results dialog for your provided keywords." + text14, string.Empty);
					this.Add(" inventory", "Opens one's inventory." + text14, string.Empty);
				}
				goto IL_4D38;
			}
			case "play":
				this.pageCount = (int)Mathf.Floor((float)Managers.soundLibraryManager.names.Count / 5f) + 2;
				if (this.pageNumber <= 3)
				{
					int num6 = this.pageNumber;
					if (num6 != 1)
					{
						if (num6 != 2)
						{
							if (num6 == 3)
							{
								this.Add(" guitar1", "This instrument has a whole set of sounds", string.Empty);
								this.Add(" guitar2", "This instrument has a whole set of sounds", string.Empty);
								this.Add(" guitar3", "This instrument has a whole set of sounds", string.Empty);
								this.Add(" guitar4", "This instrument has a whole set of sounds", string.Empty);
							}
						}
						else
						{
							this.Add(" tone", "This instrument has a whole set of sounds", string.Empty);
							this.Add(" piano2", "This instrument has a whole set of sounds", string.Empty);
							this.Add(" bell", "This instrument has a whole set of sounds", string.Empty);
							this.Add(" trumpet", "This instrument has a whole set of sounds", string.Empty);
							this.Add(" fiddle", "This instrument has a whole set of sounds", string.Empty);
						}
					}
					else
					{
						this.Add(" [your search text]", "Search through " + Managers.soundLibraryManager.names.Count + " sounds", string.Empty);
						this.Add(" button click", string.Empty, string.Empty);
						this.Add(" doorbell", string.Empty, string.Empty);
						this.Add(" doorbell with", "Optionally, add \"with\" to adjust volume, pitch and more", string.Empty);
						this.Add(" track [play to record]", "Starts recording when selected. Play on any Anyland instrument", string.Empty);
					}
				}
				else
				{
					Managers.soundLibraryManager.SortIfNeeded();
					int num7 = 0;
					int num8 = (this.pageNumber - 4) * 5;
					int num9 = num8 + 5 - 1;
					foreach (string text15 in Managers.soundLibraryManager.names)
					{
						if (num7 >= num8 && num7 <= num9)
						{
							this.Add(text15, string.Empty, string.Empty);
						}
						num7++;
					}
				}
				this.isSoundRelated = true;
				goto IL_4D38;
			case "propel_forward":
			case "rotate_forward":
			{
				string text16 = "optional force in % from -100 to 100 (default is 10; 0 stops)";
				this.Add(" with 5", text16, string.Empty);
				this.Add(" with 10", text16, string.Empty);
				this.Add(" with 100", text16, string.Empty);
				this.Add(" with -10", text16, string.Empty);
				this.Add(" with 0", text16, string.Empty);
				goto IL_4D38;
			}
			case "set_speed":
				this.Add(" [number]", "use a number to set the velocity to (-1000 to 1000)", string.Empty);
				this.Add(" 0", "this would brake the emitted or thrown thing", string.Empty);
				this.Add(" 50.5", "would set a speed of 50.5", string.Empty);
				this.Add(" 0 50 -10.5", "use 3 values to set each of x y z", string.Empty);
				goto IL_4D38;
			case "add_speed":
				this.Add(" [number]", "use a number to add a force to velocity (-1000 to 1000)", string.Empty);
				this.Add(" 50.5", "would add a force of 50.5 to each of x y z", string.Empty);
				this.Add(" 0 50 -10.5", "use 3 values to set each of x y z", string.Empty);
				goto IL_4D38;
			case "multiply_speed":
				this.Add(" [number]", "use a number to multiply the speed by (maximum 1000)", string.Empty);
				this.Add(" 0", "this would brake the emitted or thrown thing", string.Empty);
				this.Add(" 0.1", "would slow down the emitted or thrown thing", string.Empty);
				this.Add(" 2", "would double the speed", string.Empty);
				this.Add(" 0 2.5 -1", "use 3 values to multiply each of x y z", string.Empty);
				goto IL_4D38;
			case "enable_setting":
			case "disable_setting":
			{
				this.pageCount = 6;
				string text17 = ((!(text4 == "when")) ? string.Empty : " then");
				string text18 = Environment.NewLine + "(script needs to be attached to one's body)";
				if (this.pageNumber == 1)
				{
					this.Add(" microphone" + text17, "Whether the microphone is active" + text18, string.Empty);
					this.Add(" see invisible" + text17, "Whether one can see invisible as editor" + text18, string.Empty);
					this.Add(" touch uncollidable" + text17, "Whether one can touch uncollidables as editor" + text18, string.Empty);
					this.Add(" lower graphics quality" + text17, "Whether graphics quality is lowered to improve performance" + text18, string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" fly" + text17, "Whether one can fly as editor" + text18, string.Empty);
					this.Add(" findable" + text17, "Whether one's current area is shown in the friends list" + text18, string.Empty);
					this.Add(" stop alerts" + text17, "Whether pings and newborn alerts are shown" + text18, string.Empty);
					this.Add(" snap angles" + text17, "Whether during creation, angles snap" + text18, string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					this.Add(" soft snap angles" + text17, "Whether during creation, angles soft snap" + text18, string.Empty);
					this.Add(" lock angles" + text17, "Whether to fully have rotation locked for editing" + text18, string.Empty);
					this.Add(" snap position" + text17, "Whether during creation, positions snap along an axis" + text18, string.Empty);
					this.Add(" lock position" + text17, "Whether to fully have position locked for editing" + text18, string.Empty);
				}
				else if (this.pageNumber == 4)
				{
					this.Add(" scale all parts" + text17, "Whether during creation, all parts scale uniformly together" + text18, string.Empty);
					this.Add(" scale each part uniformly" + text17, "Whether during creation, each individual part scales uniformly" + text18, string.Empty);
					this.Add(" finetune position" + text17, "Whether during creation, movements are finer" + text18, string.Empty);
					this.Add(" symmetry sideways" + text17, "Whether sideways-symmetric parts should be added" + text18, string.Empty);
				}
				else if (this.pageNumber == 5)
				{
					this.Add(" symmetry vertical" + text17, "Whether vertically-symmetric parts should be added" + text18, string.Empty);
					this.Add(" symmetry depth" + text17, "Whether depth-symmetric parts should be added" + text18, string.Empty);
					this.Add(" show grid" + text17, "Whether a grid is shown in the area for more precise building" + text18, string.Empty);
					this.Add(" snap things to grid" + text17, "Whether placements snap to the grid for more precise building" + text18, string.Empty);
				}
				else if (this.pageNumber == 6)
				{
					this.Add(" snap thing angles" + text17, "Whether generally, placement angles snap (overriding Thing settings)" + text18, string.Empty);
					this.Add(" snap thing position" + text17, "If generally, placements move on an axis (overriding Thing settings)" + text18, string.Empty);
					this.Add(" ignore thing snapping" + text17, "Whether generally, placement thing snappings will be ignored" + text18, string.Empty);
					this.Add(" extra effects in vr" + text17, "Toggles extra effects in VR (may lag; needs one to be >1 day old)" + text18, string.Empty);
				}
				goto IL_4D38;
			}
			}
			string text19 = textEntered;
			if (textEntered.Contains(","))
			{
				string[] array4 = Misc.Split(textEntered, ",", StringSplitOptions.RemoveEmptyEntries);
				text19 = array4[array4.Length - 1];
			}
			bool flag2 = text19.Contains("destroy_all_parts") || text19.Contains("destroy_nearby");
			if (flag2 && text19.Contains(" with"))
			{
				this.pageCount = ((!text19.Contains("destroy_nearby")) ? 3 : 4);
				bool flag3 = text19.Contains("burst");
				string text20 = ((!flag3) ? " burst" : string.Empty);
				if (this.pageNumber == 1)
				{
					if (!flag3)
					{
						this.Add(" burst", "Optional. Destroys the thing by breaking it into parts", string.Empty);
					}
					this.Add(text20 + " 10 force", "Optional. Applies burst force (0-1000)", string.Empty);
					this.Add(text20 + " gravity-free", "Optional. Applies zero gravity", string.Empty);
					this.Add(" 30s restore", "Optional, for placements. Makes thing reappear after given seconds", string.Empty);
					this.Add(text20 + " 50 parts", "Sets the max. # of parts to break into (1-250, default 30)", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(text20 + " 2 force 5s restore", "You can combine parameters", string.Empty);
					if (!flag3)
					{
						this.Add(" 20 parts", "Note writing \"burst\" is optional when using burst-tuning commands", string.Empty);
					}
					this.Add(" 5 grow", "Breaks into growing parts at given speed (0.01 to 100)", string.Empty);
					this.Add(" 5 shrink", "Breaks into shrinking parts at given speed (0.01 to 100)", string.Empty);
					this.Add(" 10s disappear", "Seconds after which burst parts start to disappear (0.1-60, default 12.5)", string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					this.Add(" bouncy", "Makes burst parts bounce on collisions", string.Empty);
					this.Add(" slidy", "Makes burst parts slide on collisions", string.Empty);
					this.Add(" uncollidable", "Makes the burst parts pass through objects", string.Empty);
					this.Add(" self-uncollidable", "Makes the burst parts pass through each other", string.Empty);
				}
				else if (this.pageNumber == 4)
				{
					this.Add(" radius", "The destruction radius for nearby (default: " + 0.2f + "m)", string.Empty);
					this.Add(" max-size", "Size limit at which things still get destroyed (default: " + 0.2f + "m)", string.Empty);
				}
			}
			else if (text4 == "when" && list.Contains(text3))
			{
				this.Add(" then ", string.Empty, string.Empty);
			}
			else if (text3 == "loop" && !textEntered.Contains(" play "))
			{
				this.pageCount = (int)Mathf.Floor((float)Managers.soundLibraryManager.loopNames.Count / 5f) + 1;
				int num10 = 0;
				int num11 = (this.pageNumber - 1) * 5;
				int num12 = num11 + 5 - 1;
				foreach (string text21 in Managers.soundLibraryManager.loopNames)
				{
					if (num10 >= num11 && num10 <= num12)
					{
						this.Add(" " + text21, string.Empty, string.Empty);
					}
					num10++;
				}
				this.isLoopSoundRelated = true;
			}
			else if (text4 == "become" || text4 == "become_untweened" || text4 == "become_unsoftened" || text4 == "become_soft_start" || text4 == "become_soft_end")
			{
				string text22 = string.Concat(new object[]
				{
					"Optional seconds (up to ",
					30f,
					", unless \"Persist States\" is used) ",
					Environment.NewLine,
					"to go to the state (\"s\" optional)"
				});
				this.Add(" in 1s", text22, string.Empty);
				this.Add(" in 0.2s", text22, string.Empty);
				this.Add(" in 0.5s", text22, string.Empty);
				this.Add(" in 2", text22, string.Empty);
				this.Add(" in ", text22, string.Empty);
			}
			else if (text4 == "emit" || (textEntered.Contains(" emit ") && !textEntered.Contains("[")))
			{
				string text23 = "Optionally, the emit speed in percent from 0 to 100";
				this.Add(" with 0", text23, string.Empty);
				this.Add(" with 0.1", text23, string.Empty);
				this.Add(" with 10%", text23, string.Empty);
				this.Add(" with 100", text23, string.Empty);
			}
			else if (text19.Contains(" play ") && text3 != "with")
			{
				string[] array5 = Misc.Split(text19, " play ", StringSplitOptions.None);
				Managers.soundLibraryManager.SortIfNeeded();
				string text24 = array5[array5.Length - 1];
				List<string> searchResults = Managers.soundLibraryManager.GetSearchResults(text24);
				this.pageCount = (int)Mathf.Floor((float)searchResults.Count / 6f) + 1;
				int num13 = 0;
				int num14 = (this.pageNumber - 1) * 6;
				int num15 = num14 + 6 - 1;
				foreach (string text25 in searchResults)
				{
					if (num13 >= num14 && num13 <= num15)
					{
						this.Add(" " + text25, string.Empty, string.Empty);
					}
					num13++;
				}
				this.isSoundSearch = true;
				this.isSoundRelated = true;
			}
			else if (text19.Contains("play_track") && text19.Contains(" with"))
			{
				this.Add(" 125% ", "Optionally plays at 125% volume (from 1-500). Combinable", string.Empty);
				this.Add(" loop ", "Loops the track as long as it's in this state number. Combinable", string.Empty);
				this.Add(" 0.25s quantization ", "Quantizes play rythm into e.g. 0.25 seconds. Combinable", string.Empty);
				this.Add(" 1.5 speed ", "Plays track at e.g. 1.5x its speed (after quantization). Combinable", string.Empty);
			}
			else if (text19.Contains(" then play ") && text3 == "with")
			{
				this.pageCount = 5;
				if (this.pageNumber == 1)
				{
					this.Add(" 50% ", "Optionally plays at 50% volume (from 1-500). Combinable", string.Empty);
					this.Add(" 150% ", "Optionally plays at a 150% volume (from 1-500). Combinable", string.Empty);
					this.Add(" echo 50% low-pitch ", "You can combine keywords, like in this example", string.Empty);
					this.Add(" very-low-pitch ", "Plays at a very low pitch. Combinable with other keywords", string.Empty);
					this.Add(" low-pitch ", "Plays at a low pitch. Combinable with other keywords", string.Empty);
				}
				else if (this.pageNumber == 2)
				{
					this.Add(" high-pitch ", "Plays at a high pitch. Combinable", string.Empty);
					this.Add(" very-high-pitch ", "Plays at a very high pitch. Combinable", string.Empty);
					this.Add(" varied-pitch ", "Plays with a slightly randomized pitch. Combinable", string.Empty);
					this.Add(" very-varied-pitch ", "Plays with a very randomized pitch. Combinable", string.Empty);
					this.Add(" -1.0 octaves ", "Decreases pitch by an octave, use e.g. 1, -0.26, and \"octave[s]\". Combinable", string.Empty);
				}
				else if (this.pageNumber == 3)
				{
					this.Add(" 1.0 octaves ", "Increases pitch by an octave, use e.g. 1, -0.26, and \"octave[s]\". Combinable", string.Empty);
					this.Add(" echo ", "Adds echo to the sound. Combinable", string.Empty);
					this.Add(" low-pass ", "Filter to only allow low frequencies. Combinable", string.Empty);
					this.Add(" high-pass ", "Filter to only high frequencies. Combinable", string.Empty);
					this.Add(" stretch ", "Stretches the sound. Combinable", string.Empty);
				}
				else if (this.pageNumber == 4)
				{
					this.Add(" reversal ", "Plays sound backwards. Combinable, but will ignore pitch settings", string.Empty);
					this.Add(" 5 repeats ", "Repeats the sound this often, e.g. \"1 repeat\" (up to 50). Combinable", string.Empty);
					this.Add(" 0.5s delay ", "A pause before the sound starts, e.g. \"2.4s\" (\"s\" optional)", string.Empty);
					this.Add(" 0.5s skip ", "Omits this many seconds from the sound start, e.g. \"2.4s\" (\"s\" optional)", string.Empty);
					this.Add(" 0.5s duration ", "After how many seconds the sound is stopped, e.g. \"2.4s\" (\"s\" optional)", string.Empty);
				}
				else if (this.pageNumber == 5)
				{
					this.Add(" surround ", "Full volume at any distance (also see \"Surround sound\" Thing attribute)", string.Empty);
				}
			}
			else if (text19.Contains("show_web") && text19.Contains(" with"))
			{
				this.Add(" 200% zoom", "Optionally sets a zoom for the web page (\"%\" can be omitted)", string.Empty);
				this.Add(" navigation-free", "Disallows editing the URL, paging back & forward, or switching to videos", string.Empty);
				this.Add(" cursor-free", "Hides the cursor & removes ability to click", string.Empty);
				this.Add(" unsynced", "Won't synchronize changes to the URL among everyone around", string.Empty);
			}
			else if (!textEntered.Contains(" then") && (this.StringContainsOneOf(textEntered, list2) || textEntered.Contains("when touches ") || textEntered.Contains("when gets ") || textEntered.Contains("when hitting ") || textEntered.Contains("when hears ")))
			{
				this.Add(" then ", string.Empty, string.Empty);
			}
		}
		IL_4D38:
		return this.completions;
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x0004BBE0 File Offset: 0x00049FE0
	public List<AutoCompletionData> GetThingTagStringsCompletions(string textEntered)
	{
		this.completions = new List<AutoCompletionData>();
		int num = 0;
		foreach (string text in ThingTagsDialog.recentlyEnteredInput)
		{
			this.completions.Add(new AutoCompletionData(text, "These are tags you recently entered and can use again if you want", string.Empty));
			if (++num >= 6)
			{
				break;
			}
		}
		return this.completions;
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x0004BC74 File Offset: 0x0004A074
	public List<AutoCompletionData> GetFindAreasStringsCompletions(string textEntered)
	{
		this.completions = new List<AutoCompletionData>();
		int num = 0;
		foreach (string text in FindAreasDialog.recentlyEnteredInput)
		{
			this.completions.Add(new AutoCompletionData(text, string.Empty, string.Empty));
			if (++num >= 6)
			{
				break;
			}
		}
		return this.completions;
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x0004BD08 File Offset: 0x0004A108
	public List<AutoCompletionData> GetCreationTextStringsCompletions(string textEntered)
	{
		this.lastUsedPageNumber = this.pageNumber;
		this.pageCount = 1;
		this.completions = new List<AutoCompletionData>();
		if (textEntered.Length >= 1 && textEntered[textEntered.Length - 1] == '[')
		{
			this.pageCount = 6;
			switch (this.pageNumber)
			{
			case 1:
				this.completions.Add(new AutoCompletionData("year]", "The current year number", string.Empty));
				this.completions.Add(new AutoCompletionData("month]", "The current month number", string.Empty));
				this.completions.Add(new AutoCompletionData("day]", "The current date's day number", string.Empty));
				this.completions.Add(new AutoCompletionData("hour]", "The current Manyland/ UTC hour", string.Empty));
				this.completions.Add(new AutoCompletionData("hour 12]", "The current Manyland/ UTC hour in 12 hours format", string.Empty));
				break;
			case 2:
				this.completions.Add(new AutoCompletionData("minute]", "The current Manyland/ UTC minute", string.Empty));
				this.completions.Add(new AutoCompletionData("second]", "The current time's seconds", string.Empty));
				this.completions.Add(new AutoCompletionData("millisecond]", "The current time's milliseconds", string.Empty));
				this.completions.Add(new AutoCompletionData("local hour]", "The local time hour for oneself (needs attachment)", string.Empty));
				this.completions.Add(new AutoCompletionData("local hour 12]", "The local time hour in 12 hours format (needs attachment)", string.Empty));
				break;
			case 3:
				this.completions.Add(new AutoCompletionData("month/ day/ hour etc. unpadded]", "The unpadded variant (e.g. \"1\" instead of \"01\")", string.Empty));
				this.completions.Add(new AutoCompletionData("person]", "Name of the person who has this body part sticky attached", string.Empty));
				this.completions.Add(new AutoCompletionData("own person]", "One's own name, in a personal view different on each client", string.Empty));
				this.completions.Add(new AutoCompletionData("closest person]", "Name of the person closest to this", string.Empty));
				this.completions.Add(new AutoCompletionData("closest held]", "Name of the held item closest to this", string.Empty));
				break;
			case 4:
				this.completions.Add(new AutoCompletionData("area name]", "Name of the current area", string.Empty));
				this.completions.Add(new AutoCompletionData("thing name]", "Name of this thing", string.Empty));
				this.completions.Add(new AutoCompletionData("x]", "The area position x coordinate of this thing", string.Empty));
				this.completions.Add(new AutoCompletionData("y]", "The area position y coordinate of this thing", string.Empty));
				this.completions.Add(new AutoCompletionData("z]", "The area position z coordinate of this thing", string.Empty));
				break;
			case 5:
				this.completions.Add(new AutoCompletionData("people names]", "A list of people currently in the area", string.Empty));
				this.completions.Add(new AutoCompletionData("people count]", "How many people there currently are in the area", string.Empty));
				this.completions.Add(new AutoCompletionData("proximity]", "Distance in meters to the next thing ahead", string.Empty));
				this.completions.Add(new AutoCompletionData("typed]", "What was typed using the \"t\" key or \"show chat keyboard\" command", string.Empty));
				break;
			case 6:
				this.completions.Add(new AutoCompletionData("thing values]", "Lists all this thing's current variable values (0's may be omitted)", string.Empty));
				this.completions.Add(new AutoCompletionData("area values]", "Lists all area.* variables (0's may be omitted). Needs placement or editor rights", string.Empty));
				this.completions.Add(new AutoCompletionData("person values]", "Lists all person.* variables of closest person. Needs placement or editor rights", string.Empty));
				this.completions.Add(new AutoCompletionData("person... values]", "Lists everyone's person.* variables of this name. Needs placement or editor rights", string.Empty));
				this.completions.Add(new AutoCompletionData("... value]", "The value of a variable by this name, e.g. [gold value] or [area.lumber value]", string.Empty));
				break;
			}
		}
		return this.completions;
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x0004C130 File Offset: 0x0004A530
	public List<AutoCompletionData> GetEditThingPartNameStringsCompletions(string textEntered)
	{
		this.lastUsedPageNumber = this.pageNumber;
		this.pageCount = 1;
		this.completions = new List<AutoCompletionData>();
		if (textEntered == string.Empty)
		{
			this.pageCount = 2;
			int num = this.pageNumber;
			if (num != 1)
			{
				if (num == 2)
				{
					this.completions.Add(new AutoCompletionData("hand grip", string.Empty, string.Empty));
					this.completions.Add(new AutoCompletionData("hand grab zone", string.Empty, string.Empty));
					this.completions.Add(new AutoCompletionData("eyelid", string.Empty, string.Empty));
					this.completions.Add(new AutoCompletionData("mouth", string.Empty, string.Empty));
					this.completions.Add(new AutoCompletionData("animation", string.Empty, string.Empty));
				}
			}
			else
			{
				this.completions.Add(new AutoCompletionData("thumb", string.Empty, string.Empty));
				this.completions.Add(new AutoCompletionData("index finger", string.Empty, string.Empty));
				this.completions.Add(new AutoCompletionData("middle finger", string.Empty, string.Empty));
				this.completions.Add(new AutoCompletionData("ring finger", string.Empty, string.Empty));
				this.completions.Add(new AutoCompletionData("little finger", string.Empty, string.Empty));
			}
		}
		return this.completions;
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x0004C2D4 File Offset: 0x0004A6D4
	public List<AutoCompletionData> GetBrowserFavoritesCompletions(string textEntered)
	{
		this.completions = new List<AutoCompletionData>();
		this.lastUsedPageNumber = this.pageNumber;
		this.pageCount = 1;
		if (textEntered == string.Empty)
		{
			Dictionary<string, string> favorites = Managers.browserManager.GetFavorites();
			this.pageCount = (int)Mathf.Ceil((float)favorites.Count / 6f);
			int num = (this.pageNumber - 1) * 6;
			int num2 = num + 6 - 1;
			int num3 = 0;
			foreach (KeyValuePair<string, string> keyValuePair in favorites)
			{
				if (num3 >= num && num3 <= num2)
				{
					string text = keyValuePair.Key;
					text = text.Replace("https://www.", string.Empty);
					text = text.Replace("https://", string.Empty);
					text = text.Replace("http://www.", string.Empty);
					text = text.Replace("http://", string.Empty);
					if (text.EndsWith("/"))
					{
						text = text.Remove(text.Length - 1);
					}
					this.completions.Add(new AutoCompletionData(text, string.Empty, string.Empty));
				}
				num3++;
			}
		}
		return this.completions;
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x0004C438 File Offset: 0x0004A838
	private bool StringContainsOneOf(string s, List<string> matches)
	{
		bool flag = false;
		foreach (string text in matches)
		{
			if (s.Contains(text))
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x0004C4A0 File Offset: 0x0004A8A0
	private void Add(string completion, string help = "", string addToCompletionDisplay = "")
	{
		if (this.completions.Count < 6)
		{
			this.completions.Add(new AutoCompletionData(completion, help, addToCompletionDisplay));
		}
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x0004C4C6 File Offset: 0x0004A8C6
	private string AddDefaultRight(bool? rightBool)
	{
		return "\nSetting change requires placement. Default: " + ((!(rightBool == true)) ? "disallowed" : "allowed") + "\n(Current rights are shown at Area Dialog -> ... -> Rights.)";
	}

	// Token: 0x040007B1 RID: 1969
	public bool isFullLastWordReplacement;

	// Token: 0x040007B2 RID: 1970
	public bool isSoundSearch;

	// Token: 0x040007B3 RID: 1971
	public bool isSoundRelated;

	// Token: 0x040007B4 RID: 1972
	public bool isLoopSoundRelated;

	// Token: 0x040007B5 RID: 1973
	public int pageCount = 1;

	// Token: 0x040007B6 RID: 1974
	public int pageNumber = 1;

	// Token: 0x040007B7 RID: 1975
	public int lastUsedPageNumber = 1;

	// Token: 0x040007B8 RID: 1976
	public const int maxCompletionsPerPage = 6;

	// Token: 0x040007B9 RID: 1977
	private List<AutoCompletionData> completions = new List<AutoCompletionData>();
}
