using BusinessShared;
using GimaSoft.Business.GINA.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml;
using WPFShared;

namespace GimaSoft.Business.GINA
{
    // Token: 0x02000007 RID: 7
    public class Configuration : BindableObject
    {
        // Token: 0x06000067 RID: 103 RVA: 0x000038FC File Offset: 0x00001AFC
        static Configuration()
        {
            try
            {
                if (File.Exists(Configuration.DebugLogFile))
                {
                    File.Delete(Configuration.DebugLogFile);
                }
            }
            catch
            {
            }
        }

        // Token: 0x17000021 RID: 33
        // (get) Token: 0x06000068 RID: 104 RVA: 0x00003968 File Offset: 0x00001B68
        // (set) Token: 0x06000069 RID: 105 RVA: 0x0000396F File Offset: 0x00001B6F
        public static Configuration Current { get; set; } = new Configuration();

        // Token: 0x17000022 RID: 34
        // (get) Token: 0x0600006A RID: 106 RVA: 0x00003977 File Offset: 0x00001B77
        public static string EverquestLogLineFormat => Settings.Default.EverquestLogLineFormat;

        // Token: 0x17000023 RID: 35
        // (get) Token: 0x0600006B RID: 107 RVA: 0x00003983 File Offset: 0x00001B83
        public static string EverquestShareFormat => Settings.Default.EverquestShareFormat;

        // Token: 0x17000024 RID: 36
        // (get) Token: 0x0600006C RID: 108 RVA: 0x0000398F File Offset: 0x00001B8F
        public static string DefaultShareFormat => Settings.Default.DefaultShareFormat;

        // Token: 0x17000025 RID: 37
        // (get) Token: 0x0600006D RID: 109 RVA: 0x0000399B File Offset: 0x00001B9B
        public static string DefaultShareServiceUri => Settings.Default.DefaultShareServiceUri;

        // Token: 0x17000026 RID: 38
        // (get) Token: 0x0600006E RID: 110 RVA: 0x000039A7 File Offset: 0x00001BA7
        public static string EverquestGameFile => Settings.Default.EverquestGameFile;

        // Token: 0x17000027 RID: 39
        // (get) Token: 0x0600006F RID: 111 RVA: 0x000039B3 File Offset: 0x00001BB3
        public static string EverquestAudioTriggersFolder => Settings.Default.EverquestAudioTriggersFolder;

        // Token: 0x17000028 RID: 40
        // (get) Token: 0x06000070 RID: 112 RVA: 0x000039BF File Offset: 0x00001BBF
        public static string EverquestUserDataFolder => Settings.Default.EverquestUserDataFolder;

        // Token: 0x17000029 RID: 41
        // (get) Token: 0x06000071 RID: 113 RVA: 0x000039CB File Offset: 0x00001BCB
        public static string EverquestUIFileRegex => Settings.Default.EverquestUIFileRegex;

        // Token: 0x1700002A RID: 42
        // (get) Token: 0x06000072 RID: 114 RVA: 0x000039D7 File Offset: 0x00001BD7
        public static string EverquestUIFileMask => Settings.Default.EverquestUIFileMask;

        // Token: 0x1700002B RID: 43
        // (get) Token: 0x06000073 RID: 115 RVA: 0x000039E3 File Offset: 0x00001BE3
        public static string EverquestATFileRegex => Settings.Default.EverquestATFileRegex;

        // Token: 0x1700002C RID: 44
        // (get) Token: 0x06000074 RID: 116 RVA: 0x000039EF File Offset: 0x00001BEF
        public static string EverquestATFileFormat => Settings.Default.EverquestATFileFormat;

        // Token: 0x1700002D RID: 45
        // (get) Token: 0x06000075 RID: 117 RVA: 0x000039FB File Offset: 0x00001BFB
        public static string EverquestATFileMask => Settings.Default.EverquestATFileMask;

        // Token: 0x1700002E RID: 46
        // (get) Token: 0x06000076 RID: 118 RVA: 0x00003A07 File Offset: 0x00001C07
        public static string EverquestTriggerFileRegex => Settings.Default.EverquestTriggerFileRegex;

        // Token: 0x1700002F RID: 47
        // (get) Token: 0x06000077 RID: 119 RVA: 0x00003A13 File Offset: 0x00001C13
        public static string EverquestLogFolder => Settings.Default.EverquestLogFolder;

        // Token: 0x17000030 RID: 48
        // (get) Token: 0x06000078 RID: 120 RVA: 0x00003A1F File Offset: 0x00001C1F
        public static string EverquestMetadataExtension => Settings.Default.EverquestMetadataExtension;

        // Token: 0x17000031 RID: 49
        // (get) Token: 0x06000079 RID: 121 RVA: 0x00003A2B File Offset: 0x00001C2B
        public static string GINAPackageFileExtension => Settings.Default.GINAPackageFileExtension;

        // Token: 0x17000032 RID: 50
        // (get) Token: 0x0600007A RID: 122 RVA: 0x00003A37 File Offset: 0x00001C37
        public static string GamTextTriggerExtension => Settings.Default.GamTextTriggerExtension;

        // Token: 0x17000033 RID: 51
        // (get) Token: 0x0600007B RID: 123 RVA: 0x00003A43 File Offset: 0x00001C43
        public static string GamTextTriggerImportRegex => Settings.Default.GamTextTriggerImportRegex;

        // Token: 0x17000034 RID: 52
        // (get) Token: 0x0600007C RID: 124 RVA: 0x00003A4F File Offset: 0x00001C4F
        public static string GamTextTriggerExportFormat => Settings.Default.GamTextTriggerExportFormat;

        // Token: 0x17000035 RID: 53
        // (get) Token: 0x0600007D RID: 125 RVA: 0x00003A5B File Offset: 0x00001C5B
        public static string GamTextTriggerClipboardFormat => Settings.Default.GamTextTriggerClipboardFormat;

        // Token: 0x17000036 RID: 54
        // (get) Token: 0x0600007E RID: 126 RVA: 0x00003A67 File Offset: 0x00001C67
        public static string GamTextTriggerShareRegex => Settings.Default.GamTextTriggerShareRegex;

        // Token: 0x17000037 RID: 55
        // (get) Token: 0x0600007F RID: 127 RVA: 0x00003A73 File Offset: 0x00001C73
        public static string LastKnownGoodExtension => Settings.Default.LastKnownGoodExtension;

        // Token: 0x17000038 RID: 56
        // (get) Token: 0x06000080 RID: 128 RVA: 0x00003A7F File Offset: 0x00001C7F
        public static string BadFileExtension => Settings.Default.BadFileExtension;

        // Token: 0x17000039 RID: 57
        // (get) Token: 0x06000081 RID: 129 RVA: 0x00003A8B File Offset: 0x00001C8B
        public static string GINAChangeLogURL => Settings.Default.GINAChangeLogURL;

        // Token: 0x1700003A RID: 58
        // (get) Token: 0x06000082 RID: 130 RVA: 0x00003A97 File Offset: 0x00001C97
        public static string EverquestGINACommandFormat => Settings.Default.EverquestGINACommandFormat;

        // Token: 0x1700003B RID: 59
        // (get) Token: 0x06000083 RID: 131 RVA: 0x00003AA3 File Offset: 0x00001CA3
        public static int EverquestFastSearchSkip => Settings.Default.EverquestFastSearchSkip;

        // Token: 0x1700003C RID: 60
        // (get) Token: 0x06000084 RID: 132 RVA: 0x00003AC0 File Offset: 0x00001CC0
        public static IEnumerable<string> InstalledVoices
        {
            get
            {
                if (Configuration.installedVoices == null)
                {
                    var speechSynthesizer = new SpeechSynthesizer();
                    Configuration.installedVoices = (from o in speechSynthesizer.GetInstalledVoices()
                                                     select o.VoiceInfo.Name into o
                                                     orderby o
                                                     select o).ToList<string>();
                }
                return Configuration.installedVoices;
            }
        }

        // Token: 0x1700003D RID: 61
        // (get) Token: 0x06000085 RID: 133 RVA: 0x00003B33 File Offset: 0x00001D33
        public static string DefaultVoiceName
        {
            get
            {
                if (!Configuration.InstalledVoices.Any<string>())
                {
                    return "";
                }
                return Configuration.InstalledVoices.First<string>();
            }
        }

        // Token: 0x1700003E RID: 62
        // (get) Token: 0x06000086 RID: 134 RVA: 0x00003B54 File Offset: 0x00001D54
        private static string UserDataDir
        {
            get
            {
                var text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify);
                text = Path.Combine(text, "GimaSoft");
                text = Path.Combine(text, "GINA");
                if (!Directory.Exists(text))
                {
                    _ = Directory.CreateDirectory(text);
                }
                return text;
            }
        }

        // Token: 0x06000087 RID: 135 RVA: 0x00003B98 File Offset: 0x00001D98
        private static string GetDataFolder()
        {
            try
            {
                if (Configuration.dataFolder == null)
                {
                    if (string.IsNullOrWhiteSpace(Settings.Default.DataFolder))
                    {
                        Settings.Default.DataFolder = Configuration.UserDataDir;
                        Settings.Default.Save();
                        Configuration.dataFolder = Settings.Default.DataFolder;
                    }
                    else
                    {
                        Configuration.dataFolder = Settings.Default.DataFolder;
                        using (File.Create(Path.Combine(Configuration.dataFolder, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
                        {
                        }
                    }
                }
            }
            catch
            {
                Settings.Default.DataFolder = Configuration.UserDataDir;
                Settings.Default.Save();
                Configuration.dataFolder = Settings.Default.DataFolder;
            }
            return Configuration.dataFolder;
        }

        // Token: 0x1700003F RID: 63
        // (get) Token: 0x06000088 RID: 136 RVA: 0x00003C6C File Offset: 0x00001E6C
        // (set) Token: 0x06000089 RID: 137 RVA: 0x00003C74 File Offset: 0x00001E74
        public static string DataFolder
        {
            get => Configuration.GetDataFolder();
            set
            {
                if (value == Configuration.GetDataFolder())
                {
                    return;
                }
                try
                {
                    using (File.Create(Path.Combine(value, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
                    {
                    }
                }
                catch
                {
                    return;
                }
                try
                {
                    var configFile = Configuration.ConfigFile;
                    var text = Path.Combine(value, Path.GetFileName(configFile));
                    if (File.Exists(configFile) && !File.Exists(text))
                    {
                        File.Move(configFile, text);
                    }
                }
                catch
                {
                }
                Settings.Default.DataFolder = value;
                Settings.Default.Save();
                Configuration.dataFolder = Settings.Default.DataFolder;
                _ = Configuration.LoadConfiguration();
            }
        }

        // Token: 0x17000040 RID: 64
        // (get) Token: 0x0600008A RID: 138 RVA: 0x00003D38 File Offset: 0x00001F38
        public static string ConfigFile => Path.Combine(Configuration.DataFolder, "GINAConfig.xml");

        // Token: 0x17000041 RID: 65
        // (get) Token: 0x0600008B RID: 139 RVA: 0x00003D49 File Offset: 0x00001F49
        public static string CrashLogFilePre => Path.Combine(Configuration.DataFolder, "GINACrash.log");

        // Token: 0x17000042 RID: 66
        // (get) Token: 0x0600008C RID: 140 RVA: 0x00003D5A File Offset: 0x00001F5A
        public static string CrashLogFilePost => Path.Combine(Configuration.DataFolder, "GINACrash.txt");

        // Token: 0x17000043 RID: 67
        // (get) Token: 0x0600008D RID: 141 RVA: 0x00003D6B File Offset: 0x00001F6B
        public static string DebugLogFile => Path.Combine(Configuration.DataFolder, "GINADebug.log");

        // Token: 0x0600008E RID: 142 RVA: 0x00003D7C File Offset: 0x00001F7C
        public static void LogDebug(string message)
        {
            Configuration.LogDebug(message, null);
        }

        // Token: 0x0600008F RID: 143 RVA: 0x00003D88 File Offset: 0x00001F88
        public static void LogDebug(string message, params object[] parameters)
        {
            if (Configuration.Current != null && Configuration.Current.EnableDebugLog)
            {
                File.AppendAllText(Configuration.DebugLogFile, string.Format("{0:yyyy/MM/dd hh:mm:ss:ffffff}\t{1}\r\n", DateTime.Now, string.Format(message, parameters ?? Configuration.emptyLogParameters)), Encoding.ASCII);
            }
        }

        // Token: 0x06000090 RID: 144 RVA: 0x00003E10 File Offset: 0x00002010
        private static void ProcessConfiguration(XmlDocument doc)
        {
            BehaviorGroup.Clear();
            TriggerGroup.Clear();
            TriggerCategory.Clear();
            GINACharacter.Clear();
            if (doc.SelectSingleNode("/Configuration/Settings[1]") is XmlElement xmlElement)
            {
                Configuration.Current.LoadFromXml(xmlElement);
            }
            var enumerable = doc.SelectNodes("/Configuration/BehaviorGroups/Behavior").Cast<XmlElement>();
            foreach (var xmlElement2 in enumerable)
            {
                _ = BehaviorGroup.LoadFromXml(xmlElement2, true);
            }
            BehaviorGroup.Init();
            var enumerable2 = doc.SelectNodes("/Configuration/Categories/Category").Cast<XmlElement>();
            foreach (var xmlElement3 in enumerable2)
            {
                _ = TriggerCategory.LoadFromXml(xmlElement3);
            }
            TriggerCategory.Init();
            var enumerable3 = doc.SelectNodes("/Configuration/TriggerGroups/TriggerGroup").Cast<XmlElement>();
            foreach (var xmlElement4 in enumerable3)
            {
                _ = TriggerGroup.LoadFromXml(xmlElement4, TriggerGroup.RootGroup, true, null);
            }
            TriggerGroup.Init();
            var enumerable4 = doc.SelectNodes("/Configuration/Characters/Character").Cast<XmlElement>();
            foreach (var xmlElement5 in enumerable4)
            {
                _ = GINACharacter.LoadFromXml(xmlElement5);
            }
            if (enumerable.Any<XmlElement>() && enumerable.First<XmlElement>().SelectSingleNode("FontColor[1]") != null)
            {
                foreach (var xmlElement6 in enumerable)
                {
                    var behaviorName = xmlElement6.GetElementValue("Name", null);
                    var behaviorType = xmlElement6.GetElementValue("BehaviorType", BehaviorTypes.Text);
                    var solidColorBrush = new SolidColorBrush(default(Color).SetFromHtml(xmlElement6.GetElementValue("FontColor", "Yellow"), "White"));
                    var color = default(Color).SetFromHtml(xmlElement6.GetElementValue("TimerBarColor", "Maroon"), "White");
                    var behaviorGroup = BehaviorGroup.All.SingleOrDefault((BehaviorGroup o) => o.BehaviorType == behaviorType && o.Name.ToLower() == behaviorName.ToLower());
                    if (behaviorGroup != null)
                    {
                        foreach (var triggerCategory in TriggerCategory.All)
                        {
                            if (triggerCategory.TextOverlay == behaviorGroup)
                            {
                                triggerCategory.TextStyleSource = InheritanceSources.None;
                                triggerCategory.TextStyle.FontColor = solidColorBrush;
                            }
                            if (triggerCategory.TimerOverlay == behaviorGroup)
                            {
                                triggerCategory.TimerStyleSource = InheritanceSources.None;
                                triggerCategory.TimerStyle.FontColor = solidColorBrush;
                                triggerCategory.TimerStyle.TimerBarColor = color;
                            }
                        }
                    }
                }
            }
        }

        // Token: 0x06000091 RID: 145 RVA: 0x00004178 File Offset: 0x00002378
        public static ConfigurationLoadResult LoadConfiguration()
        {
            var configurationLoadResult = ConfigurationLoadResult.Success;
            var xmlDocument = new XmlDocument();
            if (File.Exists(Configuration.ConfigFile))
            {
                try
                {
                    xmlDocument.SafeLoad(Configuration.ConfigFile);
                    Configuration.ProcessConfiguration(xmlDocument);
                    goto IL_00AA;
                }
                catch
                {
                    try
                    {
                        try
                        {
                            File.Copy(Configuration.ConfigFile, Path.ChangeExtension(Configuration.ConfigFile, Configuration.BadFileExtension), true);
                        }
                        catch
                        {
                        }
                        xmlDocument.SafeLoad(Path.ChangeExtension(Configuration.ConfigFile, Configuration.LastKnownGoodExtension));
                        Configuration.ProcessConfiguration(xmlDocument);
                        configurationLoadResult = ConfigurationLoadResult.UsedLastKnownGood;
                    }
                    catch
                    {
                        var manifestResourceStream = Assembly.GetEntryAssembly().GetManifestResourceStream("GimaSoft.GINA.Resources.DefaultSettings.xml");
                        xmlDocument.SafeLoad(manifestResourceStream);
                        Configuration.ProcessConfiguration(xmlDocument);
                        configurationLoadResult = ConfigurationLoadResult.FailedToDefault;
                    }
                    goto IL_00AA;
                }
            }
            var manifestResourceStream2 = Assembly.GetEntryAssembly().GetManifestResourceStream("GimaSoft.GINA.Resources.DefaultSettings.xml");
            xmlDocument.SafeLoad(manifestResourceStream2);
            Configuration.ProcessConfiguration(xmlDocument);
        IL_00AA:
            if (configurationLoadResult == ConfigurationLoadResult.Success)
            {
                try
                {
                    File.Copy(Configuration.ConfigFile, Path.ChangeExtension(Configuration.ConfigFile, Configuration.LastKnownGoodExtension), true);
                }
                catch
                {
                }
            }
            return configurationLoadResult;
        }

        // Token: 0x06000092 RID: 146 RVA: 0x00004288 File Offset: 0x00002488
        public static void SaveConfiguration(bool includeVersion = false)
        {
            var text = Configuration.ConfigFile;
            if (includeVersion)
            {
                text = Path.Combine(Path.GetDirectoryName(text), string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(text), Application.ProductVersion, Path.GetExtension(text)));
            }
            Configuration.SaveConfiguration(text);
        }

        // Token: 0x06000093 RID: 147 RVA: 0x000042CC File Offset: 0x000024CC
        public static void SaveConfiguration(string fileName)
        {
            lock (Configuration.saveLockObject)
            {
                Settings.Default.Save();
                var xmlDocument = new XmlDocument();
                _ = xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, null));
                var xmlElement = xmlDocument.CreateElement("Configuration");
                _ = xmlDocument.AppendChild(xmlElement);
                Configuration.Current.SaveToXml(xmlElement);
                var xmlElement2 = xmlDocument.CreateElement("BehaviorGroups");
                _ = xmlElement.AppendChild(xmlElement2);
                foreach (var behaviorGroup in BehaviorGroup.All)
                {
                    behaviorGroup.SaveToXml(xmlElement2);
                }
                var xmlElement3 = xmlDocument.CreateElement("Categories");
                _ = xmlElement.AppendChild(xmlElement3);
                foreach (var triggerCategory in TriggerCategory.All)
                {
                    triggerCategory.SaveToXml(xmlElement3);
                }
                var xmlElement4 = xmlDocument.CreateElement("TriggerGroups");
                _ = xmlElement.AppendChild(xmlElement4);
                foreach (var triggerGroup in TriggerGroup.RootGroup.Groups)
                {
                    _ = triggerGroup.SaveToXml(xmlElement4, true, true, true, null);
                }
                var xmlElement5 = xmlDocument.CreateElement("Characters");
                _ = xmlElement.AppendChild(xmlElement5);
                foreach (var ginacharacter in GINACharacter.All)
                {
                    ginacharacter.SaveToXml(xmlElement5);
                }
                xmlDocument.FormatToFile(fileName, 3, 250);
            }
        }

        // Token: 0x06000094 RID: 148 RVA: 0x00004500 File Offset: 0x00002700
        public Configuration()
        {
            MasterVolume = 100;
            MatchDisplayLimit = 100;
            ShareServiceUri = Configuration.DefaultShareServiceUri;
            AllowSharedPackages = true;
            AcceptShareLevel = ShareLevel.Anybody;
            AutoMergeShareLevel = ShareLevel.Nobody;
            ShareWhiteList = new List<string>();
            LogFormatRegex = new Regex(Configuration.EverquestLogLineFormat, RegexOptions.Compiled);
        }

        // Token: 0x17000044 RID: 68
        // (get) Token: 0x06000095 RID: 149 RVA: 0x0000458B File Offset: 0x0000278B
        // (set) Token: 0x06000096 RID: 150 RVA: 0x00004593 File Offset: 0x00002793
        public bool AllowSharedPackages
        {
            get => allowSharedPackages;
            set
            {
                allowSharedPackages = value;
                base.RaisePropertyChanged("AllowSharedPackages");
            }
        }

        // Token: 0x17000045 RID: 69
        // (get) Token: 0x06000097 RID: 151 RVA: 0x000045A7 File Offset: 0x000027A7
        // (set) Token: 0x06000098 RID: 152 RVA: 0x000045AF File Offset: 0x000027AF
        public bool AllowGamTextTriggerShares
        {
            get => allowGamTextTriggerShares;
            set
            {
                allowGamTextTriggerShares = value;
                base.RaisePropertyChanged("AllowGamTextTriggerShares");
            }
        }

        // Token: 0x17000046 RID: 70
        // (get) Token: 0x06000099 RID: 153 RVA: 0x000045C3 File Offset: 0x000027C3
        // (set) Token: 0x0600009A RID: 154 RVA: 0x000045CB File Offset: 0x000027CB
        public bool StopSearchingAfterFirstMatch
        {
            get => stopSearchingAfterFirstMatch;
            set
            {
                stopSearchingAfterFirstMatch = value;
                base.RaisePropertyChanged("StopSearchingAfterFirstMatch");
            }
        }

        // Token: 0x17000047 RID: 71
        // (get) Token: 0x0600009B RID: 155 RVA: 0x000045DF File Offset: 0x000027DF
        // (set) Token: 0x0600009C RID: 156 RVA: 0x000045E7 File Offset: 0x000027E7
        public bool ArchiveLogs
        {
            get => archiveLogs;
            set
            {
                archiveLogs = value;
                base.RaisePropertyChanged("ArchiveLogs");
            }
        }

        // Token: 0x17000048 RID: 72
        // (get) Token: 0x0600009D RID: 157 RVA: 0x000045FB File Offset: 0x000027FB
        // (set) Token: 0x0600009E RID: 158 RVA: 0x00004603 File Offset: 0x00002803
        public ArchiveMethods LogArchiveMethod
        {
            get => logArchiveMethod;
            set
            {
                logArchiveMethod = value;
                base.RaisePropertyChanged("LogArchiveMethod");
            }
        }

        // Token: 0x17000049 RID: 73
        // (get) Token: 0x0600009F RID: 159 RVA: 0x00004617 File Offset: 0x00002817
        // (set) Token: 0x060000A0 RID: 160 RVA: 0x0000461F File Offset: 0x0000281F
        public long LogArchiveThresholdSize
        {
            get => logArchiveThresholdSize;
            set
            {
                logArchiveThresholdSize = value;
                base.RaisePropertyChanged("LogArchiveThresholdSize");
            }
        }

        // Token: 0x1700004A RID: 74
        // (get) Token: 0x060000A1 RID: 161 RVA: 0x00004633 File Offset: 0x00002833
        // (set) Token: 0x060000A2 RID: 162 RVA: 0x0000463B File Offset: 0x0000283B
        public bool EnableProfiler
        {
            get => enableProfiler;
            set
            {
                enableProfiler = value;
                base.RaisePropertyChanged("EnableProfiler");
            }
        }

        // Token: 0x1700004B RID: 75
        // (get) Token: 0x060000A3 RID: 163 RVA: 0x0000464F File Offset: 0x0000284F
        // (set) Token: 0x060000A4 RID: 164 RVA: 0x0000465C File Offset: 0x0000285C
        public bool EnableSound
        {
            get => base.Get<bool>("EnableSound"); set => base.Set("EnableSound", value);
        }

        // Token: 0x1700004C RID: 76
        // (get) Token: 0x060000A5 RID: 165 RVA: 0x0000466F File Offset: 0x0000286F
        // (set) Token: 0x060000A6 RID: 166 RVA: 0x0000467C File Offset: 0x0000287C
        public int MasterVolume
        {
            get => base.Get<int>("MasterVolume"); set => base.Set("MasterVolume", value);
        }

        // Token: 0x1700004D RID: 77
        // (get) Token: 0x060000A7 RID: 167 RVA: 0x0000468F File Offset: 0x0000288F
        // (set) Token: 0x060000A8 RID: 168 RVA: 0x0000469C File Offset: 0x0000289C
        public bool EnableText
        {
            get => base.Get<bool>("EnableText"); set => base.Set("EnableText", value);
        }

        // Token: 0x1700004E RID: 78
        // (get) Token: 0x060000A9 RID: 169 RVA: 0x000046AF File Offset: 0x000028AF
        // (set) Token: 0x060000AA RID: 170 RVA: 0x000046BC File Offset: 0x000028BC
        public bool EnableTimers
        {
            get => base.Get<bool>("EnableTimers"); set => base.Set("EnableTimers", value);
        }

        // Token: 0x1700004F RID: 79
        // (get) Token: 0x060000AB RID: 171 RVA: 0x000046CF File Offset: 0x000028CF
        // (set) Token: 0x060000AC RID: 172 RVA: 0x000046DC File Offset: 0x000028DC
        public bool DisplayMatches
        {
            get => base.Get<bool>("DisplayMatches"); set => base.Set("DisplayMatches", value);
        }

        // Token: 0x17000050 RID: 80
        // (get) Token: 0x060000AD RID: 173 RVA: 0x000046EF File Offset: 0x000028EF
        // (set) Token: 0x060000AE RID: 174 RVA: 0x000046FC File Offset: 0x000028FC
        public int MatchDisplayLimit
        {
            get => base.Get<int>("MatchDisplayLimit"); set => base.Set("MatchDisplayLimit", value);
        }

        // Token: 0x17000051 RID: 81
        // (get) Token: 0x060000AF RID: 175 RVA: 0x0000470F File Offset: 0x0000290F
        // (set) Token: 0x060000B0 RID: 176 RVA: 0x0000471C File Offset: 0x0000291C
        public bool LogMatchesToFile
        {
            get => base.Get<bool>("LogMatchesToFile"); set => base.Set("LogMatchesToFile", value);
        }

        // Token: 0x17000052 RID: 82
        // (get) Token: 0x060000B1 RID: 177 RVA: 0x0000472F File Offset: 0x0000292F
        // (set) Token: 0x060000B2 RID: 178 RVA: 0x0000473C File Offset: 0x0000293C
        public string MatchLogFileName
        {
            get => base.Get<string>("MatchLogFileName"); set => base.Set("MatchLogFileName", value);
        }

        // Token: 0x17000053 RID: 83
        // (get) Token: 0x060000B3 RID: 179 RVA: 0x0000474A File Offset: 0x0000294A
        // (set) Token: 0x060000B4 RID: 180 RVA: 0x00004757 File Offset: 0x00002957
        public string ShareServiceUri
        {
            get => base.Get<string>("ShareServiceUri"); set => base.Set("ShareServiceUri", value);
        }

        // Token: 0x17000054 RID: 84
        // (get) Token: 0x060000B5 RID: 181 RVA: 0x00004765 File Offset: 0x00002965
        // (set) Token: 0x060000B6 RID: 182 RVA: 0x00004772 File Offset: 0x00002972
        public ShareLevel AcceptShareLevel
        {
            get => base.Get<ShareLevel>("AcceptShareLevel"); set => base.Set("AcceptShareLevel", value);
        }

        // Token: 0x17000055 RID: 85
        // (get) Token: 0x060000B7 RID: 183 RVA: 0x00004785 File Offset: 0x00002985
        // (set) Token: 0x060000B8 RID: 184 RVA: 0x00004792 File Offset: 0x00002992
        public ShareLevel AutoMergeShareLevel
        {
            get => base.Get<ShareLevel>("AutoMergeShareLevel"); set => base.Set("AutoMergeShareLevel", value);
        }

        // Token: 0x17000056 RID: 86
        // (get) Token: 0x060000B9 RID: 185 RVA: 0x000047A5 File Offset: 0x000029A5
        // (set) Token: 0x060000BA RID: 186 RVA: 0x000047B2 File Offset: 0x000029B2
        public List<string> ShareWhiteList
        {
            get => base.Get<List<string>>("ShareWhiteList"); set => base.Set("ShareWhiteList", value);
        }

        // Token: 0x17000057 RID: 87
        // (get) Token: 0x060000BB RID: 187 RVA: 0x000047C0 File Offset: 0x000029C0
        // (set) Token: 0x060000BC RID: 188 RVA: 0x000047CD File Offset: 0x000029CD
        public bool AutoUpdate
        {
            get => base.Get<bool>("AutoUpdate"); set => base.Set("AutoUpdate", value);
        }

        // Token: 0x17000058 RID: 88
        // (get) Token: 0x060000BD RID: 189 RVA: 0x000047E0 File Offset: 0x000029E0
        // (set) Token: 0x060000BE RID: 190 RVA: 0x00004835 File Offset: 0x00002A35
        public string ImportedMediaFileFolder
        {
            get
            {
                var text = base.Get<string>("ImportedMediaFileFolder");
                if (string.IsNullOrWhiteSpace(text) || !Directory.Exists(text))
                {
                    text = Path.Combine(Configuration.UserDataDir, "ImportedMediaFiles");
                    if (!Directory.Exists(text))
                    {
                        _ = Directory.CreateDirectory(text);
                    }
                    base.Set("ImportedMediaFileFolder", text);
                }
                return text;
            }

            set => base.Set("ImportedMediaFileFolder", value);
        }

        // Token: 0x17000059 RID: 89
        // (get) Token: 0x060000BF RID: 191 RVA: 0x00004843 File Offset: 0x00002A43
        // (set) Token: 0x060000C0 RID: 192 RVA: 0x00004850 File Offset: 0x00002A50
        public string EverquestFolder
        {
            get => base.Get<string>("EverquestFolder"); set => base.Set("EverquestFolder", value);
        }

        // Token: 0x1700005A RID: 90
        // (get) Token: 0x060000C1 RID: 193 RVA: 0x0000485E File Offset: 0x00002A5E
        // (set) Token: 0x060000C2 RID: 194 RVA: 0x0000486B File Offset: 0x00002A6B
        public RunModes RunMode
        {
            get => base.Get<RunModes>("RunMode"); set => base.Set("RunMode", value);
        }

        // Token: 0x1700005B RID: 91
        // (get) Token: 0x060000C3 RID: 195 RVA: 0x0000487E File Offset: 0x00002A7E
        // (set) Token: 0x060000C4 RID: 196 RVA: 0x0000488B File Offset: 0x00002A8B
        public DateTimeOffset? RepositoryLastViewed
        {
            get => base.Get<DateTimeOffset?>("RepositoryLastViewed"); set => base.Set("RepositoryLastViewed", value);
        }

        // Token: 0x1700005C RID: 92
        // (get) Token: 0x060000C5 RID: 197 RVA: 0x0000489E File Offset: 0x00002A9E
        // (set) Token: 0x060000C6 RID: 198 RVA: 0x000048AB File Offset: 0x00002AAB
        public DateTimeOffset? RepositoryLastViewedAtStartup
        {
            get => base.Get<DateTimeOffset?>("RepositoryLastViewedAtStartup"); set => base.Set("RepositoryLastViewedAtStartup", value);
        }

        // Token: 0x1700005D RID: 93
        // (get) Token: 0x060000C7 RID: 199 RVA: 0x000048BE File Offset: 0x00002ABE
        // (set) Token: 0x060000C8 RID: 200 RVA: 0x000048CB File Offset: 0x00002ACB
        public bool MinimizeToSystemTray
        {
            get => base.Get<bool>("MinimizeToSystemTray"); set => base.Set("MinimizeToSystemTray", value);
        }

        // Token: 0x1700005E RID: 94
        // (get) Token: 0x060000C9 RID: 201 RVA: 0x000048DE File Offset: 0x00002ADE
        // (set) Token: 0x060000CA RID: 202 RVA: 0x000048EB File Offset: 0x00002AEB
        public string ReferenceToSelf
        {
            get => base.Get<string>("ReferenceToSelf"); set => base.Set("ReferenceToSelf", value);
        }

        // Token: 0x1700005F RID: 95
        // (get) Token: 0x060000CB RID: 203 RVA: 0x000048F9 File Offset: 0x00002AF9
        // (set) Token: 0x060000CC RID: 204 RVA: 0x00004906 File Offset: 0x00002B06
        public bool CheckLibraryAtStartup
        {
            get => base.Get<bool>("CheckLibraryAtStartup"); set => base.Set("CheckLibraryAtStartup", value);
        }

        // Token: 0x17000060 RID: 96
        // (get) Token: 0x060000CD RID: 205 RVA: 0x00004919 File Offset: 0x00002B19
        // (set) Token: 0x060000CE RID: 206 RVA: 0x00004926 File Offset: 0x00002B26
        public string CTagClipboardReplacement
        {
            get => base.Get<string>("CTagClipboardReplacement"); set => base.Set("CTagClipboardReplacement", value);
        }

        // Token: 0x17000061 RID: 97
        // (get) Token: 0x060000CF RID: 207 RVA: 0x00004934 File Offset: 0x00002B34
        // (set) Token: 0x060000D0 RID: 208 RVA: 0x00004941 File Offset: 0x00002B41
        public bool EnableDebugLog
        {
            get => base.Get<bool>("EnableDebugLog"); set => base.Set("EnableDebugLog", value);
        }

        // Token: 0x17000062 RID: 98
        // (get) Token: 0x060000D1 RID: 209 RVA: 0x00004954 File Offset: 0x00002B54
        // (set) Token: 0x060000D2 RID: 210 RVA: 0x00004961 File Offset: 0x00002B61
        public bool CompressArchivedLogs
        {
            get => base.Get<bool>("CompressArchivedLogs"); set => base.Set("CompressArchivedLogs", value);
        }

        // Token: 0x17000063 RID: 99
        // (get) Token: 0x060000D3 RID: 211 RVA: 0x00004974 File Offset: 0x00002B74
        // (set) Token: 0x060000D4 RID: 212 RVA: 0x00004981 File Offset: 0x00002B81
        public string LogArchiveFolder
        {
            get => base.Get<string>("LogArchiveFolder"); set => base.Set("LogArchiveFolder", value);
        }

        // Token: 0x17000064 RID: 100
        // (get) Token: 0x060000D5 RID: 213 RVA: 0x0000498F File Offset: 0x00002B8F
        // (set) Token: 0x060000D6 RID: 214 RVA: 0x0000499C File Offset: 0x00002B9C
        public ArchiveSchedules LogArchiveSchedule
        {
            get => base.Get<ArchiveSchedules>("LogArchiveSchedule"); set => base.Set("LogArchiveSchedule", value);
        }

        // Token: 0x17000065 RID: 101
        // (get) Token: 0x060000D7 RID: 215 RVA: 0x000049AF File Offset: 0x00002BAF
        // (set) Token: 0x060000D8 RID: 216 RVA: 0x000049BC File Offset: 0x00002BBC
        public bool PurgeArchivedLogs
        {
            get => base.Get<bool>("PurgeArchivedLogs"); set => base.Set("PurgeArchivedLogs", value);
        }

        // Token: 0x17000066 RID: 102
        // (get) Token: 0x060000D9 RID: 217 RVA: 0x000049CF File Offset: 0x00002BCF
        // (set) Token: 0x060000DA RID: 218 RVA: 0x000049DC File Offset: 0x00002BDC
        public int ArchivePurgeDaysToKeep
        {
            get => base.Get<int>("ArchivePurgeDaysToKeep"); set => base.Set("ArchivePurgeDaysToKeep", value);
        }

        // Token: 0x17000067 RID: 103
        // (get) Token: 0x060000DB RID: 219 RVA: 0x000049EF File Offset: 0x00002BEF
        // (set) Token: 0x060000DC RID: 220 RVA: 0x000049FC File Offset: 0x00002BFC
        public int ProfilerRefreshInterval
        {
            get => base.Get<int>("ProfilerRefreshInterval"); set => base.Set("ProfilerRefreshInterval", value);
        }

        // Token: 0x17000068 RID: 104
        // (get) Token: 0x060000DD RID: 221 RVA: 0x00004A0F File Offset: 0x00002C0F
        // (set) Token: 0x060000DE RID: 222 RVA: 0x00004A17 File Offset: 0x00002C17
        public List<PhoneticTransform> PhoneticDictionary
        {
            get => phoneticDictionary;
            set
            {
                phoneticDictionary = value;
                base.RaisePropertyChanged("PhoneticDictionary");
            }
        }

        // Token: 0x17000069 RID: 105
        // (get) Token: 0x060000DF RID: 223 RVA: 0x00004A2B File Offset: 0x00002C2B
        // (set) Token: 0x060000E0 RID: 224 RVA: 0x00004A33 File Offset: 0x00002C33
        public Regex LogFormatRegex { get; set; }

        // Token: 0x1700006A RID: 106
        // (get) Token: 0x060000E1 RID: 225 RVA: 0x00004A3C File Offset: 0x00002C3C
        // (set) Token: 0x060000E2 RID: 226 RVA: 0x00004A50 File Offset: 0x00002C50
        public long ProcessorAffinity
        {
            get => (long)Process.GetCurrentProcess().ProcessorAffinity;
            set
            {
                value &= Convert.ToInt64(Math.Pow(2.0, Environment.ProcessorCount) - 1.0);
                Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)value;
                base.RaisePropertyChanged("ProcessorAffinity");
            }
        }

        // Token: 0x060000E3 RID: 227 RVA: 0x00004AA0 File Offset: 0x00002CA0
        public void LoadFromXml(XmlElement element)
        {
            EnableSound = element.GetElementValue("EnableSound", false);
            MasterVolume = element.GetElementValue("MasterVolume", 0);
            EnableText = element.GetElementValue("EnableText", false);
            EnableTimers = element.GetElementValue("EnableTimers", false);
            DisplayMatches = element.GetElementValue("DisplayMatches", true);
            MatchDisplayLimit = element.GetElementValue("MatchDisplayLimit", 0);
            LogMatchesToFile = element.GetElementValue("LogMatchesToFile", false);
            MatchLogFileName = element.GetElementValue("MatchLogFileName", null);
            ShareServiceUri = element.GetElementValue("ShareServiceUri", Configuration.DefaultShareServiceUri).Replace("http://", "https://");
            AutoUpdate = element.GetElementValue("AutoUpdate", true);
            ImportedMediaFileFolder = element.GetElementValue("ImportedMediaFileFolder", "");
            EverquestFolder = element.GetElementValue("EverquestFolder", "");
            RepositoryLastViewed = element.GetElementValue<DateTimeOffset?>("RepositoryLastViewed", null);
            RepositoryLastViewedAtStartup = RepositoryLastViewed;
            MinimizeToSystemTray = element.GetElementValue("MinimizeToSystemTray", false);
            CheckLibraryAtStartup = element.GetElementValue("CheckLibraryAtStartup", false);
            ReferenceToSelf = element.GetElementValue("ReferenceToSelf", "You");
            AllowSharedPackages = element.GetElementValue("AllowSharedPackages", true);
            AllowGamTextTriggerShares = element.GetElementValue("AllowGamTextTriggerShares", true);
            StopSearchingAfterFirstMatch = element.GetElementValue("StopSearchingAfterFirstMatch", false);
            AcceptShareLevel = element.GetElementValue("AcceptShareLevel", ShareLevel.Anybody);
            AutoMergeShareLevel = element.GetElementValue("AutoMergeShareLevel", ShareLevel.Nobody);
            CTagClipboardReplacement = element.GetElementValue("CTagClipboardReplacement", "{C}");
            EnableDebugLog = element.GetElementValue("EnableDebugLog", false);
            ArchiveLogs = element.GetElementValue("ArchiveLogs", false);
            CompressArchivedLogs = element.GetElementValue("CompressArchivedLogs", false);
            LogArchiveFolder = element.GetElementValue("LogArchiveFolder", null);
            LogArchiveMethod = element.GetElementValue("LogArchiveMethod", ArchiveMethods.BySize);
            LogArchiveSchedule = element.GetElementValue("LogArchiveSchedule", ArchiveSchedules.Monthly);
            LogArchiveThresholdSize = element.GetElementValue("LogArchiveThresholdSize", LogArchiveThresholdSize);
            PurgeArchivedLogs = element.GetElementValue("PurgeArchivedLogs", false);
            ArchivePurgeDaysToKeep = element.GetElementValue("ArchivePurgeDaysToKeep", 365);
            ProfilerRefreshInterval = element.GetElementValue("ProfilerRefreshInterval", 30);
            ProcessorAffinity = element.GetElementValue("ProcessorAffinity", ProcessorAffinity);
            ShareWhiteList.Clear();
            var xmlNodeList = element.SelectNodes("ShareWhiteList/Character");
            foreach (var xmlElement in xmlNodeList.Cast<XmlElement>())
            {
                ShareWhiteList.Add(xmlElement.InnerText);
            }
            var enumerable = element.SelectNodes("PhoneticTransforms/Transform").Cast<XmlElement>();
            foreach (var xmlElement2 in enumerable)
            {
                PhoneticDictionary.Add(PhoneticTransform.LoadFromXml(xmlElement2, true));
            }
        }

        // Token: 0x060000E4 RID: 228 RVA: 0x00004E10 File Offset: 0x00003010
        public void SaveToXml(XmlElement element)
        {
            var ownerDocument = element.OwnerDocument;
            var xmlElement = ownerDocument.CreateElement("Settings");
            _ = element.AppendChild(xmlElement);
            _ = xmlElement.AppendChild(ownerDocument.NewElement("EnableSound", EnableSound));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("MasterVolume", MasterVolume));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("EnableText", EnableText));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("EnableTimers", EnableTimers));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("DisplayMatches", DisplayMatches));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("MatchDisplayLimit", MatchDisplayLimit));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("LogMatchesToFile", LogMatchesToFile));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("MatchLogFileName", MatchLogFileName ?? ""));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("ShareServiceUri", ShareServiceUri ?? Configuration.DefaultShareServiceUri));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("AutoUpdate", AutoUpdate));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("ImportedMediaFileFolder", ImportedMediaFileFolder ?? ""));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("EverquestFolder", EverquestFolder ?? ""));
            if (RepositoryLastViewed != null)
            {
                _ = xmlElement.AppendChild(ownerDocument.NewElement("RepositoryLastViewed", RepositoryLastViewed));
            }
            _ = xmlElement.AppendChild(ownerDocument.NewElement("MinimizeToSystemTray", MinimizeToSystemTray));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("CheckLibraryAtStartup", CheckLibraryAtStartup));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("ReferenceToSelf", ReferenceToSelf ?? "You"));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("AllowSharedPackages", AllowSharedPackages));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("AllowGamTextTriggerShares", AllowGamTextTriggerShares));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("StopSearchingAfterFirstMatch", StopSearchingAfterFirstMatch));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("AcceptShareLevel", AcceptShareLevel));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("AutoMergeShareLevel", AutoMergeShareLevel));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("CTagClipboardReplacement", CTagClipboardReplacement ?? "{C}"));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("EnableDebugLog", EnableDebugLog));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("ArchiveLogs", ArchiveLogs));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("CompressArchivedLogs", CompressArchivedLogs));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("LogArchiveFolder", LogArchiveFolder ?? ""));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("LogArchiveMethod", LogArchiveMethod));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("LogArchiveSchedule", LogArchiveSchedule));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("LogArchiveThresholdSize", LogArchiveThresholdSize));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("PurgeArchivedLogs", PurgeArchivedLogs));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("ArchivePurgeDaysToKeep", ArchivePurgeDaysToKeep));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("ProfilerRefreshInterval", ProfilerRefreshInterval));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("ProcessorAffinity", ProcessorAffinity));
            var xmlNode = xmlElement.AppendChild(ownerDocument.CreateElement("ShareWhiteList"));
            foreach (var text in ShareWhiteList.Where((string o) => !string.IsNullOrWhiteSpace(o)))
            {
                _ = xmlNode.AppendChild(ownerDocument.NewElement("Character", text));
            }
            xmlNode = xmlElement.AppendChild(ownerDocument.CreateElement("PhoneticTransforms"));
            foreach (var phoneticTransform in PhoneticDictionary)
            {
                phoneticTransform.SaveToXml((XmlElement)xmlNode);
            }
        }

        // Token: 0x0400001A RID: 26
        public static string GamTextTriggerFastMatch = Settings.Default.GamTextTriggerFastMatch;

        // Token: 0x0400001B RID: 27
        private static IEnumerable<string> installedVoices;

        // Token: 0x0400001C RID: 28
        private static readonly object saveLockObject = new object();

        // Token: 0x0400001D RID: 29
        private static string dataFolder = null;

        // Token: 0x0400001E RID: 30
        private static readonly object[] emptyLogParameters = new object[0];

        // Token: 0x0400001F RID: 31
        private bool allowSharedPackages = true;

        // Token: 0x04000020 RID: 32
        private bool allowGamTextTriggerShares = true;

        // Token: 0x04000021 RID: 33
        private bool stopSearchingAfterFirstMatch = true;

        // Token: 0x04000022 RID: 34
        private bool archiveLogs;

        // Token: 0x04000023 RID: 35
        private ArchiveMethods logArchiveMethod;

        // Token: 0x04000024 RID: 36
        private long logArchiveThresholdSize = 104857600L;

        // Token: 0x04000025 RID: 37
        private bool enableProfiler;

        // Token: 0x04000026 RID: 38
        private List<PhoneticTransform> phoneticDictionary = new List<PhoneticTransform>();
    }
}
