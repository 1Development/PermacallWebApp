namespace PermacallTools.Models.WoWGuild
{

    public class WoWCharacter
    {
        public long lastModified { get; set; }
        public string name { get; set; }
        public string realm { get; set; }
        public string battlegroup { get; set; }
        public int _class { get; set; }
        public int race { get; set; }
        public int gender { get; set; }
        public int level { get; set; }
        public int achievementPoints { get; set; }
        public string thumbnail { get; set; }
        public string calcClass { get; set; }
        public int faction { get; set; }
        public Items items { get; set; }
        public int totalHonorableKills { get; set; }
    }

    public class Items
    {
        public Items()
        {
            this.head = new Head() { itemLevel = 0, quality = 0};
            this.neck = new Neck() { itemLevel = 0, quality = 0 };
            this.shoulder = new Shoulder() { itemLevel = 0, quality = 0 };
            this.back = new Back() { itemLevel = 0, quality = 0 };
            this.chest = new Chest() { itemLevel = 0, quality = 0 };
            this.shirt = new Shirt() { itemLevel = 0, quality = 0 };
            this.wrist = new Wrist() { itemLevel = 0, quality = 0 };
            this.hands = new Hands() { itemLevel = 0, quality = 0 };
            this.waist = new Waist() { itemLevel = 0, quality = 0 };
            this.legs = new Legs() { itemLevel = 0, quality = 0 };
            this.feet = new Feet() { itemLevel = 0, quality = 0 };
            this.finger1 = new Finger1() { itemLevel = 0, quality = 0 };
            this.finger2 = new Finger2() { itemLevel = 0, quality = 0 };
            this.trinket1 = new Trinket1() { itemLevel = 0, quality = 0 };
            this.trinket2 = new Trinket2() { itemLevel = 0, quality = 0 };
            this.mainHand = new Mainhand() { itemLevel = 0, quality = 0 };
        }

        public int averageItemLevel { get; set; }
        public int averageItemLevelEquipped { get; set; }
        public Head head { get; set; }
        public Neck neck { get; set; }
        public Shoulder shoulder { get; set; }
        public Back back { get; set; }
        public Chest chest { get; set; }
        public Shirt shirt { get; set; }
        public Wrist wrist { get; set; }
        public Hands hands { get; set; }
        public Waist waist { get; set; }
        public Legs legs { get; set; }
        public Feet feet { get; set; }
        public Finger1 finger1 { get; set; }
        public Finger2 finger2 { get; set; }
        public Trinket1 trinket1 { get; set; }
        public Trinket2 trinket2 { get; set; }
        public Mainhand mainHand { get; set; }
    }

    public class Head
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams tooltipParams { get; set; }
        public Stat[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public int[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance appearance { get; set; }

        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance
    {
    }

    public class Stat
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Neck
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams1 tooltipParams { get; set; }
        public Stat1[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public int[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance1 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams1
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance1
    {
    }

    public class Stat1
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Shoulder
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams2 tooltipParams { get; set; }
        public Stat2[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public int[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance2 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams2
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance2
    {
    }

    public class Stat2
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Back
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams3 tooltipParams { get; set; }
        public Stat3[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public int[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance3 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams3
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance3
    {
    }

    public class Stat3
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Chest
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams4 tooltipParams { get; set; }
        public Stat4[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public object[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance4 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams4
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance4
    {
    }

    public class Stat4
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Shirt
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams5 tooltipParams { get; set; }
        public object[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public object[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance5 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams5
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance5
    {
    }

    public class Wrist
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams6 tooltipParams { get; set; }
        public Stat5[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public int[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance6 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams6
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance6
    {
    }

    public class Stat5
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Hands
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams7 tooltipParams { get; set; }
        public Stat6[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public int[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance7 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams7
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance7
    {
    }

    public class Stat6
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Waist
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams8 tooltipParams { get; set; }
        public Stat7[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public int[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance8 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams8
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance8
    {
    }

    public class Stat7
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Legs
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams9 tooltipParams { get; set; }
        public Stat8[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public int[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance9 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams9
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance9
    {
    }

    public class Stat8
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Feet
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams10 tooltipParams { get; set; }
        public Stat9[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public int[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance10 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams10
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance10
    {
    }

    public class Stat9
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Finger1
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams11 tooltipParams { get; set; }
        public Stat10[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public int[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance11 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams11
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance11
    {
    }

    public class Stat10
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Finger2
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams12 tooltipParams { get; set; }
        public Stat11[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public int[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance12 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams12
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance12
    {
    }

    public class Stat11
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Trinket1
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams13 tooltipParams { get; set; }
        public Stat12[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public int[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance13 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams13
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance13
    {
    }

    public class Stat12
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Trinket2
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams14 tooltipParams { get; set; }
        public Stat13[] stats { get; set; }
        public int armor { get; set; }
        public string context { get; set; }
        public int[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public object[] artifactTraits { get; set; }
        public object[] relics { get; set; }
        public Appearance14 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams14
    {
        public int timewalkerLevel { get; set; }
    }

    public class Appearance14
    {
    }

    public class Stat13
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Mainhand
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int quality { get; set; }
        public int itemLevel { get; set; }
        public Tooltipparams15 tooltipParams { get; set; }
        public Stat14[] stats { get; set; }
        public int armor { get; set; }
        public Weaponinfo weaponInfo { get; set; }
        public string context { get; set; }
        public int[] bonusLists { get; set; }
        public int artifactId { get; set; }
        public int displayInfoId { get; set; }
        public int artifactAppearanceId { get; set; }
        public Artifacttrait[] artifactTraits { get; set; }
        public Relic[] relics { get; set; }
        public Appearance15 appearance { get; set; }
        public string rarityColor
        {
            get
            {
                if (quality == 1) return "#ffffff";
                if (quality == 2) return "#1eff00";
                if (quality == 3) return "#0070dd";
                if (quality == 4) return "#a335ee";
                if (quality == 5) return "#ff8000";
                if (quality == 6) return "#e6cc80";
                if (quality == 7) return "#e6cc80";
                if (quality == 8) return "#e6cc80";

                return "#9d9d9d";
            }
        }
    }

    public class Tooltipparams15
    {
        public int gem0 { get; set; }
        public int gem1 { get; set; }
        public int timewalkerLevel { get; set; }
    }

    public class Weaponinfo
    {
        public Damage damage { get; set; }
        public float weaponSpeed { get; set; }
        public float dps { get; set; }
    }

    public class Damage
    {
        public int min { get; set; }
        public int max { get; set; }
        public float exactMin { get; set; }
        public float exactMax { get; set; }
    }

    public class Appearance15
    {
        public int itemAppearanceModId { get; set; }
    }

    public class Stat14
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Artifacttrait
    {
        public int id { get; set; }
        public int rank { get; set; }
    }

    public class Relic
    {
        public int socket { get; set; }
        public int itemId { get; set; }
        public int context { get; set; }
        public int[] bonusLists { get; set; }
    }
}