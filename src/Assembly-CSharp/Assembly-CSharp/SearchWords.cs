using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020001C6 RID: 454
public static class SearchWords
{
	// Token: 0x06000DE2 RID: 3554 RVA: 0x0007C5EC File Offset: 0x0007A9EC
	static SearchWords()
	{
		SearchWords.InitializeNativeWords();
		SearchWords.customWords = new List<string>();
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x0007C5FD File Offset: 0x0007A9FD
	// (set) Token: 0x06000DE4 RID: 3556 RVA: 0x0007C604 File Offset: 0x0007AA04
	public static List<string> nativeWords { get; private set; }

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x0007C60C File Offset: 0x0007AA0C
	// (set) Token: 0x06000DE6 RID: 3558 RVA: 0x0007C613 File Offset: 0x0007AA13
	public static List<string> customWords { get; private set; }

	// Token: 0x06000DE7 RID: 3559 RVA: 0x0007C61B File Offset: 0x0007AA1B
	public static void InitializeCustomWordsFromString(string wordsString)
	{
		if (!string.IsNullOrEmpty(wordsString))
		{
			SearchWords.customWords = Misc.Split(wordsString, ",", StringSplitOptions.RemoveEmptyEntries).ToList<string>();
		}
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x0007C63E File Offset: 0x0007AA3E
	public static string GetCustomWordsString()
	{
		return string.Join(",", SearchWords.customWords.ToArray());
	}

	// Token: 0x06000DE9 RID: 3561 RVA: 0x0007C654 File Offset: 0x0007AA54
	public static string AddCustomSearchWord(string word)
	{
		string text = null;
		if (!string.IsNullOrEmpty(word))
		{
			word = SearchWords.NormalizeWord(word);
			if (SearchWords.nativeWords.Contains(word))
			{
				text = "✔  Word already in base list";
			}
			else if (SearchWords.customWords.Contains(word))
			{
				text = "✔  Word already in your list";
			}
			else if (word != string.Empty && !SearchWords.customWords.Contains(word))
			{
				SearchWords.customWords.Add(word);
				SearchWords.customWords.Sort();
			}
		}
		return text;
	}

	// Token: 0x06000DEA RID: 3562 RVA: 0x0007C6E4 File Offset: 0x0007AAE4
	public static void RemoveCustomSearchWord(string word)
	{
		word = SearchWords.NormalizeWord(word);
		int num = SearchWords.customWords.IndexOf(word);
		if (num >= 0)
		{
			SearchWords.customWords.RemoveAt(num);
		}
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x0007C717 File Offset: 0x0007AB17
	public static string NormalizeWord(string word)
	{
		return word.Trim().ToLower();
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x0007C724 File Offset: 0x0007AB24
	public static void ShowDupes()
	{
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x0007C726 File Offset: 0x0007AB26
	public static string SpeechToText(string speech)
	{
		speech = speech.Replace("find ", speech);
		speech = speech.Replace("search ", speech);
		speech = speech.Replace("get ", speech);
		return speech;
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x0007C753 File Offset: 0x0007AB53
	public static string[] GetAllWords()
	{
		return SearchWords.nativeWords.Concat(SearchWords.customWords).ToArray<string>();
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x0007C76C File Offset: 0x0007AB6C
	private static void InitializeNativeWords()
	{
		SearchWords.nativeWords = new List<string>
		{
			"next", "last", "next page", "last page", "furniture", "chair", "wall", "window", "fireball", "door",
			"body", "digital", "experience", "key", "sword", "shield", "shoes", "pants", "face", "nose",
			"eye", "box", "cable", "electricity", "internet", "home", "information", "car", "mail", "world",
			"technology", "software", "system", "university", "office", "education", "number", "cookie", "science", "computer",
			"network", "guide", "area", "place", "staff", "design", "city", "school", "book", "phone",
			"government", "video", "control", "family", "note", "future", "store", "post", "board", "human",
			"sign", "travel", "point", "ball", "cube", "pyramid", "ground", "ceiling", "lamp", "bulb",
			"light", "mountain", "tree", "flower", "plant", "grass", "room", "art", "environment", "power",
			"building", "plate", "chopsticks", "bowl", "dress", "red", "pink", "green", "blue", "black",
			"white", "purple", "yellow", "brown", "gray", "flowers", "avatar", "baby", "funny", "sad",
			"cute", "house", "language", "space", "fiction", "star", "war", "wars", "mouse", "dog",
			"cat", "rat", "lion", "animal", "insect", "bird", "eagle", "pigeon", "worm", "hole",
			"cheese", "bread", "food", "fruit", "pineapple", "bush", "rock", "stone", "apple", "peach",
			"eating", "document", "notice", "jail", "cell", "mattress", "mat", "woman", "man", "old",
			"young", "hat", "helmet", "weapon", "barrel", "munition", "saber", "lightsaber", "toy", "bucket",
			"water", "ocean", "desert", "island", "fire", "ice", "earth", "sky", "cloud", "candy",
			"banana", "movies", "thirties", "fourties", "fifties", "sixties", "seventies", "eighties", "nineties", "online",
			"forum", "target", "arrow", "bow", "magic", "wizard", "witch", "spell", "castle", "medieval",
			"futuristic", "carpet", "wool", "fur", "hand", "finger", "leg", "belly", "image", "photo",
			"screenshot", "calendar", "shop", "item", "group", "wax", "candle", "torch", "watch", "clock",
			"desk", "keyboard", "tablet", "speaker", "head", "headphone", "drugs", "smoking", "cigar", "cigaret",
			"music", "sound", "wave", "effect", "particles", "material", "color", "glasses", "mouth", "teeth",
			"tongue", "nail", "action", "animation", "fast", "slow", "pose", "posable", "clonable", "camera",
			"screen", "flat", "round", "bouncy", "device", "gimmick", "tasty", "dirt", "large", "tall",
			"small", "tiny", "moving", "tutorial", "sport", "money", "dollar bill", "coin", "president", "politics",
			"compass", "direction", "thumb", "street", "diamond", "valuable", "gold", "treasure", "treasure chest", "secret",
			"poster", "cassette", "tape", "model", "adult", "underwear", "crime", "criminal", "police", "electronic",
			"day", "night", "sleep", "watching", "universe", "popular", "celebrity", "medical", "communication", "writing",
			"knowledge", "story", "collection", "magazine", "advertisement", "person", "metal", "plastic", "liquid", "poison",
			"wood", "skin", "bone", "glass", "broken", "broom", "hoover", "robot", "artificial", "machine",
			"wire", "roof", "drink", "coffee", "beer", "soda", "sewer", "fan", "switch", "button",
			"holdable", "ventilation", "glowing", "nature", "hot", "cold", "warm", "snow", "rain", "moon",
			"planet", "ship", "spaceship", "seat", "couch", "bed", "headset", "controller", "gadget", "road",
			"line", "abstract", "dangerous", "useful", "energy", "foreign", "front", "top", "decorative", "decoration",
			"sparkle", "entertainment", "cinema", "safety", "record", "campus", "college", "senior", "navigation", "love",
			"hardware", "swimming", "radio", "card", "party", "dice", "display", "brick", "toilet", "shower",
			"bath", "bathtub", "equipment", "pencil", "telephone", "china", "russia", "america", "japan", "plane",
			"airplane", "boat", "hover", "tunnel", "shovel", "hammer", "saw", "pistol", "sharp", "blinking",
			"fog", "park", "gallery", "tire", "gift", "corner", "parcel", "package", "brush", "paint",
			"painting", "culture", "finance", "center", "mobile", "suite", "backpack", "back", "underground", "connection",
			"marker", "chocolate", "cake", "cherry", "lemon", "spice", "spicy", "fork", "spoon", "knife",
			"rotating", "jumping", "falling", "spider", "monster", "orc", "dragon", "fantasy", "elf", "emitter",
			"manual", "floor", "lab", "chemistry", "explosion", "effects", "television", "train", "ride", "carnival",
			"mask", "cloak", "knight", "prince", "princess", "king", "queen", "leather", "award", "trophy",
			"nift", "christmas", "celebration", "season", "horror", "shock", "fear", "laughter", "twilight", "doctor",
			"booth", "ghost", "unicorn", "horn", "sand", "fish", "whale", "dolphin", "creature", "illusion",
			"medicine", "iphone", "ipad", "french", "butter", "breakfast", "dinner", "lunch", "knob", "lever",
			"hotel", "picture", "child", "mind", "brain", "cellar", "wooden", "metallic", "armor", "wireless",
			"cross", "circle", "hour", "teaching", "interface", "heart", "laboratory", "bad", "good", "bank",
			"core", "ring", "storage", "safe", "modern", "oldfashioned", "entrance", "host", "voice", "bar",
			"platform", "mirror", "error", "stop sign", "alley", "prize", "letter", "architecture", "industrial", "death",
			"life", "transportation", "zip", "memory", "town", "player", "professor", "devil", "angel", "truck",
			"detergent", "powder", "washing", "cleaning", "scanner", "laser", "rail", "treadmill", "structure", "deco",
			"tap", "stream", "cream", "dream", "military", "museum", "germany", "printer", "monitor", "fair",
			"figure", "super", "bat", "hulk", "iron", "agent", "stage", "microphone", "fashion", "makeup",
			"lipstick", "stick", "river", "socket", "plug", "lake", "officer", "beach", "well", "deep",
			"emergency", "block", "bag", "chemical", "hill", "path", "golf", "football", "biology", "african",
			"hospital", "beautiful", "winter", "summer", "spring", "autumn", "handle", "cash", "flow", "peace",
			"ruler", "script", "scripting", "logo", "sheet", "transparent", "graphic", "indian", "blood", "church",
			"italian", "frame", "crown", "telescope", "satellite", "pretty", "math", "branch", "bicycle", "motor",
			"bus", "racing", "flying", "astronaut", "holiday", "nuclear", "gateway", "kitchen", "marine", "warning",
			"ray", "vacation", "market", "girl", "boy", "famous", "disk", "hard", "soft", "temperature",
			"banner", "village", "bridge", "female", "male", "newborn", "background", "woods", "tower", "skyscraper",
			"antenna", "map", "patterns", "drive", "fly", "wedding", "marriage", "icon", "counter", "arm",
			"astronomy", "balcony", "violin", "guitar", "trumpet", "drums", "instrument", "bike", "bikini", "hamburger",
			"t shirt", "badge", "vegetables", "salad", "rice", "noodles", "pasta", "wine", "canoe", "socks",
			"cereal", "health", "chrome", "classic", "vintage", "clerk", "cockpit", "cup", "crop", "diner",
			"dollar", "disco", "dance", "drawer", "ear", "dwarf", "giant", "weather", "egg", "exit",
			"eyebrows", "hair", "faucet", "farm", "figurine", "fence", "net", "garbage", "trash", "girlfriend",
			"boyfriend", "helicopter", "honey", "hunting", "rifle", "ice cream", "jeep", "jeans", "organ", "kiss",
			"ladder", "stairs", "staircase", "leaf", "loft", "marble", "melody", "song", "muscular", "mud",
			"newspaper", "cow", "pie", "wallet", "pocket", "price", "shelf", "prison", "leash", "slingshot",
			"offensive", "rainbow", "riddle", "saloon", "shoulder", "shore", "sled", "snail", "soap", "sink",
			"spaghetti", "spandex", "spike", "spinach", "spiral", "spotlight", "spark", "submarine", "rotate", "subway",
			"mechanical", "tail", "basket", "tea", "tissue", "throne", "tomato", "potato", "toothbrush", "toothpaste",
			"traffic", "triangle", "square", "geometry", "tuxedo", "skirt", "waste", "waffle", "weird", "wind",
			"wolf", "exciting", "lava", "heat", "oven", "bench", "yacht", "yogurt", "horse", "cage",
			"aquarium", "collector", "alien", "pot", "dishes", "rocket", "leaves", "shoe", "halloween", "costume",
			"relaxing", "microwave", "clothes", "table", "sports", "remote", "doll", "playground", "scary", "happy",
			"birthday", "greeting", "scenery", "cyberspace", "virtual reality", "science fiction", "fries", "french fries", "gun", "handgun",
			"machine gun"
		};
	}
}
