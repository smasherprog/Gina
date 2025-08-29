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
        private static readonly Regex EverquestAudioTriggerRegex =
            new Regex(Configuration.EverquestTriggerFileRegex, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        public Package()
        {
            MediaFiles = new List<PrerecordedFile>();
        }

        #region Public static observable collections
        public static ObservableCollection<ShareDetectedEventArgs> SharesDetected { get; } = new ObservableCollection<ShareDetectedEventArgs>();
        public static ObservableCollection<ShareDetectedEventArgs> SharesProcessed { get; } = new ObservableCollection<ShareDetectedEventArgs>();
        public static ObservableCollection<Guid> SharesCreated { get; } = new ObservableCollection<Guid>();
        #endregion

        public TriggerGroup RootGroup { get; } = new TriggerGroup();
        public List<PrerecordedFile> MediaFiles { get; set; }


        private static IEnumerable<PrerecordedFile> GetFiles(IEnumerable<TriggerGroup> groups, IEnumerable<Trigger> triggers)
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

            var current = pkg.RootGroup;
            while (stack.Any())
            {
                var next = stack.Pop();
                var existing = current.Groups.FirstOrDefault(o => string.Equals(o.Name, next.Name, StringComparison.OrdinalIgnoreCase));
                if (existing == null)
                {
                    existing = current.AddGroup(next.CloneShallow(), null);
                }
                current = existing;
            }

            return current;
        }

        public static Package CreatePackage(IEnumerable<ITriggerLibraryEntry> entries)
        {
            if (entries == null || !entries.Any())
            {
                return null;
            }

            if (entries.Any((ITriggerLibraryEntry o) => !(o is Trigger) && !(o is TriggerGroup)))
            {
                throw new NotSupportedException("This method only works with TriggerGroup and Trigger objects.");
            }

            var pkg = new Package
            {
                MediaFiles = GetFiles(
                    entries.Where(o => o is TriggerGroup).Cast<TriggerGroup>(),
                    entries.Where(o => o is Trigger).Cast<Trigger>()).ToList()
            };

            foreach (var group in entries.Where(o => o is TriggerGroup).Cast<TriggerGroup>())
            {
                var parent = EnsurePackageGroup(pkg, group.ParentGroup);
                var added = parent.AddGroup(group.Clone(), null);
                group.Merge(added, true);
            }

            foreach (var trig in entries.Where(o => o is Trigger).Cast<Trigger>())
            {
                var parent = EnsurePackageGroup(pkg, trig.ParentGroup);
                _ = parent.AddTrigger(trig.Clone(), null);
            }

            return pkg;
        }

        public static string GetPackagePasteText(Guid sessionId)
        {
            return $"{{GINA:{sessionId}}}";
        }

        private static IEnumerable<Trigger> GetTriggersFromEQ(string filename, string triggerSetName)
        {
            var result = new List<Trigger>();
            if (!File.Exists(filename))
            {
                return result;
            }

            string text;
            using (var sr = new StreamReader(filename))
            {
                text = sr.ReadToEnd();
            }

            // Load optional metadata file
            var metadata = new List<EverquestMetadata>();
            var metadataPath = Path.ChangeExtension(filename, Configuration.EverquestMetadataExtension);
            try
            {
                var xdoc = new XmlDocument();
                xdoc.SafeLoad(metadataPath);
                foreach (var el in xdoc.SelectNodes("/Metadata/Triggers/Trigger").Cast<XmlElement>())
                {
                    metadata.Add(new EverquestMetadata
                    {
                        Name = el.GetElementValue("Name", null),
                        ID = el.GetElementValue("ID", 0),
                        Pattern = el.GetElementValue("Pattern", null)
                    });
                }
            }
            catch
            {
                // ignore missing/invalid metadata
            }

            foreach (Match m in EverquestAudioTriggerRegex.Matches(text))
            {
                var trig = new Trigger
                {
                    Name = m.Groups["pattern"].Value,
                    TriggerText = m.Groups["pattern"].Value,
                    MediaFileName = Path.Combine(Configuration.Current.EverquestFolder, "AudioTriggers", triggerSetName, m.Groups["sound"].Value) + ".wav",
                    PlayMediaFile = true
                };

                // Try to find friendly name from metadata by exact or unique contains match
                var meta = metadata.FirstOrDefault(o => string.Equals(o.Pattern, trig.TriggerText, StringComparison.OrdinalIgnoreCase));
                if (meta == null)
                {
                    meta = metadata
                        .Where(o => o.Pattern != null &&
                                    (o.Pattern.IndexOf(trig.TriggerText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                     trig.TriggerText.IndexOf(o.Pattern, StringComparison.OrdinalIgnoreCase) >= 0))
                        .FirstOrDefault(o => !EverquestAudioTriggerRegex.Matches(text).Cast<Match>()
                            .Any(n => n != m && string.Equals(n.Groups["pattern"].Value, o.Pattern, StringComparison.OrdinalIgnoreCase)));
                }

                if (meta != null)
                {
                    trig.Name = meta.Name;
                    _ = metadata.Remove(meta);
                }

                result.Add(trig);
            }

            return result;
        }

        private static void WriteEQMetadataFile(string filename, IEnumerable<EverquestMetadata> metadata)
        {
            var xdoc = new XmlDocument();
            _ = xdoc.AppendChild(xdoc.CreateXmlDeclaration("1.0", null, null));
            var root = xdoc.CreateElement("Metadata");
            _ = xdoc.AppendChild(root);
            _ = root.AppendChild(xdoc.NewElement("LastGINAWrite", DateTime.Now.ToString()));

            var triggersEl = xdoc.CreateElement("Triggers");
            _ = root.AppendChild(triggersEl);

            foreach (var md in metadata)
            {
                var t = xdoc.CreateElement("Trigger");
                _ = t.AppendChild(xdoc.NewElement("Name", md.Name));
                _ = t.AppendChild(xdoc.NewElement("ID", md.ID));
                _ = t.AppendChild(xdoc.NewElement("Pattern", md.Pattern));
                _ = triggersEl.AppendChild(t);
            }

            xdoc.Save(Path.ChangeExtension(filename, Configuration.EverquestMetadataExtension));
        }

        public static Package OpenPackageFromEQTriggers(string filename, string triggerSetName)
        {
            var pkg = new Package();
            var group = pkg.RootGroup.AddGroup(triggerSetName, null);
            foreach (var trig in GetTriggersFromEQ(filename, triggerSetName))
            {
                _ = group.AddTrigger(trig, null);
            }

            return pkg;
        }

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

        public static Package OpenPackageFromGamTextTriggers(string filename)
        {
            var pkg = new Package();
            var group = pkg.RootGroup.AddGroup("GamTextTriggers", null);
            var soundRoot = Path.Combine(Path.GetDirectoryName(filename) ?? string.Empty, "Sound Files");

            foreach (var line in File.ReadAllLines(filename))
            {
                var trig = Trigger.CreateFromGamTextTriggerString(line);
                if (trig == null)
                {
                    continue;
                }

                trig.Name = group.GetUniqueTriggerName(trig);
                _ = group.AddTrigger(trig, null);

                try
                {
                    if (trig.PlayMediaFile && !string.IsNullOrWhiteSpace(trig.MediaFileName))
                    {
                        var src = Path.Combine(soundRoot, trig.MediaFileName);
                        var dest = Path.Combine(Configuration.Current.ImportedMediaFileFolder, trig.MediaFileName);

                        if (File.Exists(src) && !File.Exists(dest))
                        {
                            _ = Directory.CreateDirectory(Path.GetDirectoryName(dest));
                            File.Copy(src, dest);
                        }
                        if (File.Exists(dest))
                        {
                            trig.MediaFileName = dest;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Configuration.LogDebug("Error copying files from GamTextTriggers folder: {0}", ex.ToString());
                }
            }

            return pkg;
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

        public static void AddDetectedShare(ShareDetectedEventArgs args)
        {
            if (!SharesDetected.Any(o => o.SessionId == args.SessionId)
                && !SharesProcessed.Any(o => o.SessionId == args.SessionId)
                && !SharesCreated.Any(o => o == args.SessionId))
            {
                SharesDetected.Add(args);
            }
        }

        public static void AddCreatedShare(Guid sessionId)
        {
            if (!SharesCreated.Contains(sessionId))
            {
                SharesCreated.Add(sessionId);
            }
        }

        public static void MarkProcessedShare(ShareDetectedEventArgs args)
        {
            if (args.ShareType != PackageShareType.Repository
                && args.SessionId != Guid.Empty
                && !SharesProcessed.Any(o => o.SessionId == args.SessionId))
            {
                SharesProcessed.Add(args);
            }
            _ = SharesDetected.Remove(args);
        }

        public static void DiscardShare(Guid sessionId)
        {
            var found = SharesDetected.FirstOrDefault(o => o.SessionId == sessionId);
            if (found != null)
            {
                _ = SharesDetected.Remove(found);
            }
        }

        public static void OpenRepository()
        {
            SharesDetected.Insert(0, new ShareDetectedEventArgs(PackageShareType.Repository, Guid.Empty, null, null));
        }

        public static void OpenFilePackage(string filename)
        {
            SharesDetected.Insert(0, new ShareDetectedEventArgs(PackageShareType.GINAPackageFile, Guid.NewGuid(), null, filename));
        }

        public static void OpenGamTextTriggerFile(string filename)
        {
            SharesDetected.Insert(0, new ShareDetectedEventArgs(PackageShareType.GamTextTriggersFile, Guid.NewGuid(), null, filename));
        }
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

        #region Nested types
        public class PrerecordedFile
        {
            public string FullPath { get; set; }
            public string Filename { get; set; }
            public int FileId { get; set; }
            public Guid? OriginalPackageId { get; set; }
            public byte[] Data { get; set; }
        }

        public class EQCharacterTriggerSet
        {
            public string Server { get; set; }
            public string Character { get; set; }
            public string TriggerSet { get; set; }
        }

        private class EverquestMetadata
        {
            public string Name { get; set; }
            public int ID { get; set; }
            public string Pattern { get; set; }
        }
        #endregion
    }
}
