using BusinessShared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Text.RegularExpressions;
using System.Xml;
using WPFShared;

namespace GimaSoft.Business.GINA
{
    // Token: 0x02000036 RID: 54
    public class Package : BindableObject
    {
        // Token: 0x170000CD RID: 205
        // (get) Token: 0x0600025D RID: 605 RVA: 0x0000B9AA File Offset: 0x00009BAA
        public static ObservableCollection<ShareDetectedEventArgs> SharesDetected => Package._SharesDetected;

        // Token: 0x170000CE RID: 206
        // (get) Token: 0x0600025E RID: 606 RVA: 0x0000B9B1 File Offset: 0x00009BB1
        public static ObservableCollection<ShareDetectedEventArgs> SharesProcessed => Package._SharesProcessed;

        // Token: 0x170000CF RID: 207
        // (get) Token: 0x0600025F RID: 607 RVA: 0x0000B9B8 File Offset: 0x00009BB8
        public static ObservableCollection<Guid> SharesCreated => Package._SharesCreated;

        // Token: 0x06000260 RID: 608 RVA: 0x0000BA58 File Offset: 0x00009C58
        private static IEnumerable<Package.PrerecordedFile> GetFiles(IEnumerable<TriggerGroup> groups, IEnumerable<Trigger> triggers)
        {
            var list = new List<Trigger>();
            var list2 = new List<Package.PrerecordedFile>();
            if (groups != null)
            {
                list.AddRange(groups.SelectMany((TriggerGroup o) => o.DescendantTree).SelectMany((TriggerGroup o) => o.Triggers));
            }
            if (triggers != null)
            {
                list.AddRange(triggers);
            }
            var list3 = (from o in (from o in triggers.SelectMany((Trigger o) => new Trigger[] { o, o.TimerEndingTrigger, o.TimerEndedTrigger })
                                    where o != null && !string.IsNullOrWhiteSpace(o.MediaFileName)
                                    select o.MediaFileName).Distinct<string>()
                         group o by o.ToLower() into o
                         select o.First<string>() into o
                         where File.Exists(o)
                         select o).ToList<string>();
            foreach (var text in list3)
            {
                var fileName2 = Path.GetFileName(text);
                var fileName = fileName2;
                var num = 1;
                while (list2.Any((Package.PrerecordedFile o) => o.Filename.ToLower() == fileName.ToLower()))
                {
                    fileName = string.Format("{0}_{1}", fileName2, num++);
                }
                list2.Add(new Package.PrerecordedFile
                {
                    Filename = fileName,
                    FullPath = text,
                    FileId = list2.Count + 1
                });
            }
            return list2;
        }

        // Token: 0x06000261 RID: 609 RVA: 0x0000BC8C File Offset: 0x00009E8C
        private static TriggerGroup EnsurePackageGroup(Package pkg, TriggerGroup group)
        {
            if (group == null)
            {
                return null;
            }
            var stack = new Stack<TriggerGroup>();
            while (group.ParentGroup != null)
            {
                stack.Push(group);
                group = group.ParentGroup;
            }
            var triggerGroup = pkg.RootGroup;
            while (stack.Any<TriggerGroup>())
            {
                group = stack.Pop();
                var triggerGroup2 = triggerGroup.Groups.FirstOrDefault((TriggerGroup o) => string.Compare(o.Name, group.Name, true) == 0);
                if (triggerGroup2 == null)
                {
                    triggerGroup2 = triggerGroup.AddGroup(group.CloneShallow(), null);
                }
                triggerGroup = triggerGroup2;
            }
            return triggerGroup;
        }

        // Token: 0x06000262 RID: 610 RVA: 0x0000BD90 File Offset: 0x00009F90
        public static Package CreatePackage(IEnumerable<ITriggerLibraryEntry> entries)
        {
            if (entries == null || !entries.Any<ITriggerLibraryEntry>())
            {
                return null;
            }
            if (entries.Any((ITriggerLibraryEntry o) => !(o is Trigger) && !(o is TriggerGroup)))
            {
                throw new NotSupportedException("This method only works with TriggerGroup and Trigger objects.");
            }
            var package = new Package
            {
                MediaFiles = Package.GetFiles(entries.Where((ITriggerLibraryEntry o) => o is TriggerGroup).Cast<TriggerGroup>(), entries.Where((ITriggerLibraryEntry o) => o is Trigger).Cast<Trigger>()).ToList<Package.PrerecordedFile>()
            };
            foreach (var triggerGroup in entries.Where((ITriggerLibraryEntry o) => o is TriggerGroup).Cast<TriggerGroup>())
            {
                var triggerGroup2 = Package.EnsurePackageGroup(package, triggerGroup.ParentGroup);
                _ = triggerGroup2.AddGroup(triggerGroup.Clone(), null);
                triggerGroup.Merge(triggerGroup2, true);
            }
            foreach (var trigger in entries.Where((ITriggerLibraryEntry o) => o is Trigger).Cast<Trigger>())
            {
                var triggerGroup3 = Package.EnsurePackageGroup(package, trigger.ParentGroup);
                _ = triggerGroup3.AddTrigger(trigger.Clone(), null);
            }
            return package;
        }

        // Token: 0x06000263 RID: 611 RVA: 0x0000BF50 File Offset: 0x0000A150
        public static string GetPackagePasteText(Guid sessionId)
        {
            return string.Format("{{GINA:{0}}}", sessionId.ToString());
        }

        // Token: 0x06000264 RID: 612 RVA: 0x0000C098 File Offset: 0x0000A298
        private static IEnumerable<Trigger> GetTriggersFromEQ(string filename, string triggerSetName)
        {
            var list = new List<Trigger>();
            //string text = null;
            //if (File.Exists(filename))
            //{
            //	Package.<>c__DisplayClass28 CS$<>8__locals1 = new Package.<>c__DisplayClass28();
            //	using (StreamReader streamReader = new StreamReader(filename))
            //	{
            //		text = streamReader.ReadToEnd();
            //	}
            //	List<Package.EverquestMetadata> list2 = new List<Package.EverquestMetadata>();
            //	string text2 = Path.ChangeExtension(filename, Configuration.EverquestMetadataExtension);
            //	try
            //	{
            //		XmlDocument xmlDocument = new XmlDocument();
            //		xmlDocument.SafeLoad(text2);
            //		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/Metadata/Triggers/Trigger");
            //		foreach (XmlElement xmlElement in xmlNodeList.Cast<XmlElement>())
            //		{
            //			list2.Add(new Package.EverquestMetadata
            //			{
            //				Name = xmlElement.GetElementValue("Name", null),
            //				ID = xmlElement.GetElementValue("ID", 0),
            //				Pattern = xmlElement.GetElementValue("Pattern", null)
            //			});
            //		}
            //	}
            //	catch
            //	{
            //	}
            //	CS$<>8__locals1.matches = Package.EverquestAudioTriggerRegex.Matches(text).Cast<Match>();
            //	using (IEnumerator<Match> enumerator2 = CS$<>8__locals1.matches.GetEnumerator())
            //	{
            //		while (enumerator2.MoveNext())
            //		{
            //			Package.<>c__DisplayClass2b CS$<>8__locals2 = new Package.<>c__DisplayClass2b();
            //			CS$<>8__locals2.CS$<>8__locals29 = CS$<>8__locals1;
            //			CS$<>8__locals2.match = enumerator2.Current;
            //			Trigger trig = new Trigger
            //			{
            //				Name = CS$<>8__locals2.match.Groups["pattern"].Value,
            //				TriggerText = CS$<>8__locals2.match.Groups["pattern"].Value,
            //				MediaFileName = Path.Combine(Configuration.Current.EverquestFolder, "AudioTriggers", triggerSetName, CS$<>8__locals2.match.Groups["sound"].Value) + ".wav",
            //				PlayMediaFile = true
            //			};
            //			Package.EverquestMetadata everquestMetadata = list2.FirstOrDefault((Package.EverquestMetadata o) => o.Pattern.ToLower() == trig.TriggerText.ToLower());
            //			if (everquestMetadata == null)
            //			{
            //				everquestMetadata = (from o in list2
            //					where o.Pattern.ToLower().Contains(trig.TriggerText.ToLower()) || trig.TriggerText.ToLower().Contains(o.Pattern.ToLower())
            //					where !CS$<>8__locals2.CS$<>8__locals29.matches.Any((Match n) => n != CS$<>8__locals2.match && n.Groups["pattern"].Value.ToLower() == o.Pattern.ToLower())
            //					select o).FirstOrDefault<Package.EverquestMetadata>();
            //			}
            //			if (everquestMetadata != null)
            //			{
            //				trig.Name = everquestMetadata.Name;
            //				list2.Remove(everquestMetadata);
            //			}
            //			list.Add(trig);
            //		}
            //	}
            //}
            return list;
        }

        // Token: 0x06000265 RID: 613 RVA: 0x0000C390 File Offset: 0x0000A590
        private static void WriteEQMetadataFile(string filename, IEnumerable<Package.EverquestMetadata> metadata)
        {
            var xmlDocument = new XmlDocument();
            _ = xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, null));
            var xmlElement = xmlDocument.CreateElement("Metadata");
            _ = xmlDocument.AppendChild(xmlElement);
            _ = xmlElement.AppendChild(xmlDocument.NewElement("LastGINAWrite", DateTime.Now.ToString()));
            var xmlElement2 = xmlDocument.CreateElement("Triggers");
            _ = xmlElement.AppendChild(xmlElement2);
            foreach (var everquestMetadata in metadata)
            {
                var xmlElement3 = xmlDocument.CreateElement("Trigger");
                _ = xmlElement3.AppendChild(xmlDocument.NewElement("Name", everquestMetadata.Name));
                _ = xmlElement3.AppendChild(xmlDocument.NewElement("ID", everquestMetadata.ID));
                _ = xmlElement3.AppendChild(xmlDocument.NewElement("Pattern", everquestMetadata.Pattern));
                _ = xmlElement2.AppendChild(xmlElement3);
            }
            xmlDocument.Save(Path.ChangeExtension(filename, Configuration.EverquestMetadataExtension));
        }

        // Token: 0x06000266 RID: 614 RVA: 0x0000C4B8 File Offset: 0x0000A6B8
        public static Package OpenPackageFromEQTriggers(string filename, string triggerSetName)
        {
            var package = new Package();
            var triggerGroup = package.RootGroup.AddGroup(triggerSetName, null);
            var triggersFromEQ = Package.GetTriggersFromEQ(filename, triggerSetName);
            foreach (var trigger in triggersFromEQ)
            {
                _ = triggerGroup.AddTrigger(trigger, null);
            }
            return package;
        }

        // Token: 0x06000267 RID: 615 RVA: 0x0000C600 File Offset: 0x0000A800
        public static void SaveTriggersToEQTriggers(IEnumerable<Package.EQCharacterTriggerSet> characters, IEnumerable<Trigger> triggers)
        {
            var list = characters.Select((Package.EQCharacterTriggerSet o) => o.TriggerSet).Distinct<string>().ToList<string>();
            foreach (var text in from o in (from o in triggers
                                            where o.PlayMediaFile && !string.IsNullOrWhiteSpace(o.MediaFileName)
                                            select o.MediaFileName).Distinct<string>().ToList<string>()
                                 where File.Exists(o)
                                 select o)
            {
                foreach (var text2 in list)
                {
                    try
                    {
                        File.Copy(text, Path.Combine(Configuration.Current.EverquestFolder, "AudioTriggers", text2, Path.GetFileName(text)));
                    }
                    catch
                    {
                    }
                }
            }
            var speechAudioFormatInfo = new SpeechAudioFormatInfo(11000, AudioBitsPerSample.Sixteen, AudioChannel.Mono);
            var list2 = new List<string>();
            using (var speechSynthesizer = new SpeechSynthesizer())
            {
                foreach (var eqcharacterTriggerSet in characters)
                {
                    var text3 = Path.Combine(Configuration.Current.EverquestFolder, Configuration.EverquestUserDataFolder, string.Format(Configuration.EverquestATFileFormat, eqcharacterTriggerSet.TriggerSet, eqcharacterTriggerSet.Character, eqcharacterTriggerSet.Server));
                    var list3 = Package.GetTriggersFromEQ(text3, eqcharacterTriggerSet.TriggerSet).ToList<Trigger>();
                    var list4 = (from o in triggers
                                 where !o.EnableRegex
                                 select o.Clone()).ToList<Trigger>();
                    foreach (var trigger in list4)
                    {
                        trigger.TriggerText = trigger.TriggerText.Replace("{C}", eqcharacterTriggerSet.Character);
                    }
                    if (list4.Any<Trigger>())
                    {
                        var enumerable = list4.Where((Trigger o) => o.UseTextToVoice && !string.IsNullOrWhiteSpace(o.TextToVoiceText) && !o.EnableRegex);
                        foreach (var trigger2 in enumerable.ToList<Trigger>())
                        {
                            var text4 = Path.GetInvalidFileNameChars().Aggregate(trigger2.TextToVoiceText.Replace("{C}", eqcharacterTriggerSet.Character), (string o, char n) => o.Replace(n.ToString(), " "));
                            if (!string.IsNullOrWhiteSpace(text4))
                            {
                                var text5 = Path.Combine(Configuration.Current.EverquestFolder, Configuration.EverquestAudioTriggersFolder, eqcharacterTriggerSet.TriggerSet, text4 + ".wav");
                                trigger2.MediaFileName = text5;
                                if (!list2.Contains(text5.ToLower()))
                                {
                                    speechSynthesizer.SetOutputToWaveFile(text5, speechAudioFormatInfo);
                                    speechSynthesizer.Speak(trigger2.TextToVoiceText.Replace("{C}", eqcharacterTriggerSet.Character));
                                    list2.Add(text5.ToLower());
                                }
                            }
                        }
                        var list5 = new List<Trigger>();
                        var list6 = new List<Package.EverquestMetadata>();
                        if (File.Exists(text3) && !File.Exists(Path.ChangeExtension(text3, "bak")))
                        {
                            File.Copy(text3, Path.ChangeExtension(text3, "bak"));
                        }
                        using (var streamWriter = new StreamWriter(text3, false))
                        {
                            streamWriter.WriteLine("[Triggers]");
                            var num = 0;
                            using (var enumerator6 = list3.GetEnumerator())
                            {
                                while (enumerator6.MoveNext())
                                {
                                    Func<Trigger, bool> func = null;
                                    var fileTrig = enumerator6.Current;
                                    Trigger trigger3 = null;
                                    if (!string.IsNullOrWhiteSpace(fileTrig.Name))
                                    {
                                        _ = list4.FirstOrDefault((Trigger o) => o.Name.ToLower() == fileTrig.Name.ToLower());
                                    }
                                    if (trigger3 == null)
                                    {
                                        IEnumerable<Trigger> enumerable2 = list4;
                                        if (func == null)
                                        {
                                            func = (Trigger o) => o.TriggerText.ToLower() == fileTrig.TriggerText.ToLower();
                                        }
                                        trigger3 = enumerable2.FirstOrDefault(func);
                                    }
                                    if (trigger3 != null)
                                    {
                                        list5.Add(trigger3);
                                        _ = list4.Remove(trigger3);
                                    }
                                    else
                                    {
                                        list5.Add(fileTrig);
                                    }
                                }
                            }
                            list5.AddRange(list4);
                            foreach (var trigger4 in list5)
                            {
                                streamWriter.WriteLine("Pattern{0}={1}", num, trigger4.TriggerText);
                                streamWriter.WriteLine("Sound{0}={1}", num, string.IsNullOrWhiteSpace(trigger4.MediaFileName) ? "" : Path.GetFileNameWithoutExtension(trigger4.MediaFileName));
                                if (!string.IsNullOrWhiteSpace(trigger4.Name))
                                {
                                    list6.Add(new Package.EverquestMetadata
                                    {
                                        Name = trigger4.Name,
                                        ID = num,
                                        Pattern = trigger4.TriggerText
                                    });
                                }
                                num++;
                            }
                            streamWriter.WriteLine();
                            streamWriter.WriteLine("PatternCount={0}", num);
                            streamWriter.Close();
                            try
                            {
                                Package.WriteEQMetadataFile(text3, list6);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }

        // Token: 0x06000268 RID: 616 RVA: 0x0000CCC4 File Offset: 0x0000AEC4
        public static void CreateGamTextTriggerFile(string fileName, IEnumerable<Trigger> triggers)
        {
            using (var streamWriter = new StreamWriter(fileName, false))
            {
                foreach (var trigger in triggers)
                {
                    var gamTextTriggerTranslation = new GamTextTriggerTranslation(trigger);
                    if (gamTextTriggerTranslation.IsCompatible)
                    {
                        streamWriter.WriteLine(gamTextTriggerTranslation.Translation);
                    }
                }
            }
        }

        // Token: 0x06000269 RID: 617 RVA: 0x0000CD40 File Offset: 0x0000AF40
        public static Package OpenPackageFromGamTextTriggers(string filename)
        {
            var package = new Package();
            var triggerGroup = package.RootGroup.AddGroup("GamTextTriggers", null);
            var text = Path.Combine(Path.GetDirectoryName(filename), "Sound Files");
            var array = File.ReadAllLines(filename);
            foreach (var text2 in array)
            {
                var trigger = Trigger.CreateFromGamTextTriggerString(text2);
                if (trigger != null)
                {
                    trigger.Name = triggerGroup.GetUniqueTriggerName(trigger);
                    _ = triggerGroup.AddTrigger(trigger, null);
                    try
                    {
                        if (trigger.PlayMediaFile && !string.IsNullOrWhiteSpace(trigger.MediaFileName))
                        {
                            var text3 = Path.Combine(text, trigger.MediaFileName);
                            var text4 = Path.Combine(Configuration.Current.ImportedMediaFileFolder, trigger.MediaFileName);
                            if (File.Exists(text3) && !File.Exists(text4))
                            {
                                File.Copy(text3, text4);
                            }
                            if (File.Exists(text4))
                            {
                                trigger.MediaFileName = text4;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Configuration.LogDebug("Error copying files from GamTextTriggers folder: {0}", new object[] { ex.ToString() });
                    }
                }
            }
            return package;
        }

        // Token: 0x0600026A RID: 618 RVA: 0x0000CEA4 File Offset: 0x0000B0A4
        public static Package OpenPackage(byte[] bytes)
        {
            var package = new Package
            {
                MediaFiles = new List<Package.PrerecordedFile>()
            };
            var xmlDocument = new XmlDocument();
            var memoryStream = new MemoryStream();
            memoryStream.Write(bytes, 0, bytes.Length);
            using (var zipStorer = ZipStorer.Open(memoryStream, FileAccess.Read))
            {
                var list = zipStorer.ReadCentralDir();
                using (var memoryStream2 = new MemoryStream())
                {
                    _ = zipStorer.ExtractFile(list.Single((ZipStorer.ZipFileEntry o) => o.FilenameInZip == "ShareData.xml"), memoryStream2);
                    memoryStream2.Position = 0L;
                    xmlDocument.SafeLoad(memoryStream2);
                }
                foreach (var zipFileEntry in list.Where((ZipStorer.ZipFileEntry o) => o.FilenameInZip != "ShareData.xml"))
                {
                    using (var memoryStream3 = new MemoryStream())
                    {
                        _ = zipStorer.ExtractFile(zipFileEntry, memoryStream3);
                        package.MediaFiles.Add(new Package.PrerecordedFile
                        {
                            Data = memoryStream3.ToArray(),
                            Filename = zipFileEntry.FilenameInZip,
                            FileId = Convert.ToInt32(zipFileEntry.Comment)
                        });
                    }
                }
            }
            memoryStream.Dispose();
            var enumerable = xmlDocument.SelectNodes("/SharedData[1]/TriggerGroups[1]/TriggerGroup").Cast<XmlElement>();
            foreach (var xmlElement in enumerable)
            {
                _ = package.RootGroup.AddGroup(TriggerGroup.LoadFromXml(xmlElement, null, true, package.MediaFiles), null);
            }
            return package;
        }

        // Token: 0x0600026B RID: 619 RVA: 0x0000D12C File Offset: 0x0000B32C
        public static void AddDetectedShare(ShareDetectedEventArgs args)
        {
            if (!Package.SharesDetected.Any((ShareDetectedEventArgs o) => o.SessionId == args.SessionId) && !Package.SharesProcessed.Any((ShareDetectedEventArgs o) => o.SessionId == args.SessionId) && !Package.SharesCreated.Any((Guid o) => o == args.SessionId))
            {
                Package.SharesDetected.Add(args);
            }
        }

        // Token: 0x0600026C RID: 620 RVA: 0x0000D19E File Offset: 0x0000B39E
        public static void AddCreatedShare(Guid sessionId)
        {
            if (!Package.SharesCreated.Contains(sessionId))
            {
                Package.SharesCreated.Add(sessionId);
            }
        }

        // Token: 0x0600026D RID: 621 RVA: 0x0000D1E0 File Offset: 0x0000B3E0
        public static void MarkProcessedShare(ShareDetectedEventArgs args)
        {
            if (args.ShareType != PackageShareType.Repository && args.SessionId != Guid.Empty && !Package.SharesProcessed.Any((ShareDetectedEventArgs o) => args.SessionId == args.SessionId))
            {
                Package.SharesProcessed.Add(args);
            }
            _ = Package.SharesDetected.Remove(args);
        }

        // Token: 0x0600026E RID: 622 RVA: 0x0000D274 File Offset: 0x0000B474
        public static void DiscardShare(Guid sessionId)
        {
            var shareDetectedEventArgs = Package.SharesDetected.FirstOrDefault((ShareDetectedEventArgs o) => o.SessionId == sessionId);
            if (shareDetectedEventArgs != null)
            {
                _ = Package.SharesDetected.Remove(shareDetectedEventArgs);
            }
        }

        // Token: 0x0600026F RID: 623 RVA: 0x0000D2B4 File Offset: 0x0000B4B4
        public static void OpenRepository()
        {
            Package.SharesDetected.Insert(0, new ShareDetectedEventArgs(PackageShareType.Repository, Guid.Empty, null, null));
        }

        // Token: 0x06000270 RID: 624 RVA: 0x0000D2CE File Offset: 0x0000B4CE
        public static void OpenFilePackage(string filename)
        {
            Package.SharesDetected.Insert(0, new ShareDetectedEventArgs(PackageShareType.GINAPackageFile, Guid.NewGuid(), null, filename));
        }

        // Token: 0x06000271 RID: 625 RVA: 0x0000D2E8 File Offset: 0x0000B4E8
        public static void OpenGamTextTriggerFile(string filename)
        {
            Package.SharesDetected.Insert(0, new ShareDetectedEventArgs(PackageShareType.GamTextTriggersFile, Guid.NewGuid(), null, filename));
        }

        // Token: 0x06000272 RID: 626 RVA: 0x0000D302 File Offset: 0x0000B502
        public Package()
        {
            MediaFiles = new List<Package.PrerecordedFile>();
        }

        // Token: 0x170000D0 RID: 208
        // (get) Token: 0x06000273 RID: 627 RVA: 0x0000D320 File Offset: 0x0000B520
        public TriggerGroup RootGroup { get; } = new TriggerGroup();

        // Token: 0x170000D1 RID: 209
        // (get) Token: 0x06000274 RID: 628 RVA: 0x0000D328 File Offset: 0x0000B528
        // (set) Token: 0x06000275 RID: 629 RVA: 0x0000D330 File Offset: 0x0000B530
        public List<Package.PrerecordedFile> MediaFiles { get; set; }

        // Token: 0x06000276 RID: 630 RVA: 0x0000D38C File Offset: 0x0000B58C
        public void Merge(Package pkg)
        {
            var count = MediaFiles.Count;
            var list = pkg.RootGroup.DescendantTree.SelectMany((TriggerGroup o) => o.Triggers.Where((Trigger n) => n.NeedsMerge && n.MediaFileId != null)).ToList<Trigger>();
            foreach (var trigger in list)
            {
                trigger.MediaFileId += count;
            }
            foreach (var prerecordedFile in pkg.MediaFiles)
            {
                prerecordedFile.FileId += count;
                MediaFiles.Add(prerecordedFile);
            }
            foreach (var triggerGroup in pkg.RootGroup.Groups)
            {
                triggerGroup.Merge(RootGroup, true);
            }
        }

        // Token: 0x06000277 RID: 631 RVA: 0x0000D4EC File Offset: 0x0000B6EC
        public byte[] GetBytes()
        {
            var xmlDocument = new XmlDocument();
            _ = xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, null));
            var xmlElement = xmlDocument.CreateElement("SharedData");
            _ = xmlDocument.AppendChild(xmlElement);
            var xmlElement2 = xmlElement.OwnerDocument.CreateElement("TriggerGroups");
            _ = xmlElement.AppendChild(xmlElement2);
            foreach (var triggerGroup in RootGroup.Groups)
            {
                _ = triggerGroup.SaveToXml(xmlElement2, true, true, false, MediaFiles);
            }
            var memoryStream = xmlDocument.FormatToMemoryStream();
            var memoryStream2 = new MemoryStream();
            using (var zipStorer = ZipStorer.Create(memoryStream2, "Package"))
            {
                zipStorer.AddStream(ZipStorer.Compression.Deflate, "ShareData.xml", memoryStream, DateTime.Now, "");
                if (MediaFiles != null)
                {
                    foreach (var prerecordedFile in MediaFiles)
                    {
                        using (var fileStream = new FileStream(prerecordedFile.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            zipStorer.AddStream(ZipStorer.Compression.Deflate, prerecordedFile.Filename, fileStream, DateTime.Now, prerecordedFile.FileId.ToString());
                        }
                    }
                }
            }
            var array = memoryStream2.ToArray();
            memoryStream.Dispose();
            memoryStream2.Dispose();
            return array;
        }

        // Token: 0x04000114 RID: 276
        private static readonly Regex EverquestAudioTriggerRegex = new Regex(Configuration.EverquestTriggerFileRegex, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        // Token: 0x04000115 RID: 277
        private static readonly ObservableCollection<ShareDetectedEventArgs> _SharesDetected = new ObservableCollection<ShareDetectedEventArgs>();

        // Token: 0x04000116 RID: 278
        private static readonly ObservableCollection<ShareDetectedEventArgs> _SharesProcessed = new ObservableCollection<ShareDetectedEventArgs>();

        // Token: 0x04000117 RID: 279
        private static readonly ObservableCollection<Guid> _SharesCreated = new ObservableCollection<Guid>();

        // Token: 0x02000037 RID: 55
        public class PrerecordedFile
        {
            // Token: 0x170000D2 RID: 210
            // (get) Token: 0x06000292 RID: 658 RVA: 0x0000D6C5 File Offset: 0x0000B8C5
            // (set) Token: 0x06000293 RID: 659 RVA: 0x0000D6CD File Offset: 0x0000B8CD
            public string FullPath { get; set; }

            // Token: 0x170000D3 RID: 211
            // (get) Token: 0x06000294 RID: 660 RVA: 0x0000D6D6 File Offset: 0x0000B8D6
            // (set) Token: 0x06000295 RID: 661 RVA: 0x0000D6DE File Offset: 0x0000B8DE
            public string Filename { get; set; }

            // Token: 0x170000D4 RID: 212
            // (get) Token: 0x06000296 RID: 662 RVA: 0x0000D6E7 File Offset: 0x0000B8E7
            // (set) Token: 0x06000297 RID: 663 RVA: 0x0000D6EF File Offset: 0x0000B8EF
            public int FileId { get; set; }

            // Token: 0x170000D5 RID: 213
            // (get) Token: 0x06000298 RID: 664 RVA: 0x0000D6F8 File Offset: 0x0000B8F8
            // (set) Token: 0x06000299 RID: 665 RVA: 0x0000D700 File Offset: 0x0000B900
            public Guid? OriginalPackageId { get; set; }

            // Token: 0x170000D6 RID: 214
            // (get) Token: 0x0600029A RID: 666 RVA: 0x0000D709 File Offset: 0x0000B909
            // (set) Token: 0x0600029B RID: 667 RVA: 0x0000D711 File Offset: 0x0000B911
            public byte[] Data { get; set; }
        }

        // Token: 0x02000038 RID: 56
        public class EQCharacterTriggerSet
        {
            // Token: 0x170000D7 RID: 215
            // (get) Token: 0x0600029D RID: 669 RVA: 0x0000D722 File Offset: 0x0000B922
            // (set) Token: 0x0600029E RID: 670 RVA: 0x0000D72A File Offset: 0x0000B92A
            public string Server { get; set; }

            // Token: 0x170000D8 RID: 216
            // (get) Token: 0x0600029F RID: 671 RVA: 0x0000D733 File Offset: 0x0000B933
            // (set) Token: 0x060002A0 RID: 672 RVA: 0x0000D73B File Offset: 0x0000B93B
            public string Character { get; set; }

            // Token: 0x170000D9 RID: 217
            // (get) Token: 0x060002A1 RID: 673 RVA: 0x0000D744 File Offset: 0x0000B944
            // (set) Token: 0x060002A2 RID: 674 RVA: 0x0000D74C File Offset: 0x0000B94C
            public string TriggerSet { get; set; }
        }

        // Token: 0x02000039 RID: 57
        private class EverquestMetadata
        {
            // Token: 0x170000DA RID: 218
            // (get) Token: 0x060002A4 RID: 676 RVA: 0x0000D75D File Offset: 0x0000B95D
            // (set) Token: 0x060002A5 RID: 677 RVA: 0x0000D765 File Offset: 0x0000B965
            public string Name { get; set; }

            // Token: 0x170000DB RID: 219
            // (get) Token: 0x060002A6 RID: 678 RVA: 0x0000D76E File Offset: 0x0000B96E
            // (set) Token: 0x060002A7 RID: 679 RVA: 0x0000D776 File Offset: 0x0000B976
            public int ID { get; set; }

            // Token: 0x170000DC RID: 220
            // (get) Token: 0x060002A8 RID: 680 RVA: 0x0000D77F File Offset: 0x0000B97F
            // (set) Token: 0x060002A9 RID: 681 RVA: 0x0000D787 File Offset: 0x0000B987
            public string Pattern { get; set; }
        }
    }
}
