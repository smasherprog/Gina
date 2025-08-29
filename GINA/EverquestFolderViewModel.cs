using GimaSoft.Business.GINA;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GimaSoft.GINA
{
    // Token: 0x0200001B RID: 27
    public class EverquestFolderViewModel : GINAViewModel
    {
        // Token: 0x06000325 RID: 805 RVA: 0x0000AD3C File Offset: 0x00008F3C
        public static IEnumerable<EverquestFolderViewModel> GetCharacterSets()
        {
            var list = new List<EverquestFolderViewModel>();
            var files = Directory.GetFiles(Path.Combine(Configuration.Current.EverquestFolder, Configuration.EverquestUserDataFolder), Configuration.EverquestATFileMask);
            foreach (var text in files)
            {
                if (EverquestFolderViewModel.ATFileRegex.Match(text).Success)
                {
                    list.Add(new EverquestFolderViewModel(text));
                }
            }
            return (from o in
                        (from o in
                             (from o in list
                              group o by new { o.Server, o.Character }).Select(a =>
        {
            var everquestFolderViewModel = new EverquestFolderViewModel(a.Key.Server, a.Key.Character)
            {
                Children = a.OrderBy((EverquestFolderViewModel n) => n.TriggerSet).ToList<EverquestFolderViewModel>()
            };
            return everquestFolderViewModel;
        })
                         group o by o.Server).Select(delegate (IGrouping<string, EverquestFolderViewModel> o)
                     {
                         var everquestFolderViewModel2 = new EverquestFolderViewModel(o.Key, null)
                         {
                             Children = o.OrderBy((EverquestFolderViewModel n) => n.Character).ToList<EverquestFolderViewModel>()
                         };
                         return everquestFolderViewModel2;
                     })
                    orderby o.Server
                    select o).ToList<EverquestFolderViewModel>();
        }

        // Token: 0x06000326 RID: 806 RVA: 0x0000AEBC File Offset: 0x000090BC
        public static IEnumerable<EverquestFolderViewModel> GetCharacters()
        {
            var list = new List<EverquestFolderViewModel>();
            var files = Directory.GetFiles(Configuration.Current.EverquestFolder, Configuration.EverquestUIFileMask);
            foreach (var text in files)
            {
                var match = EverquestFolderViewModel.UIFileRegex.Match(text);
                if (match.Success)
                {
                    list.Add(new EverquestFolderViewModel(match.Groups["server"].Value, match.Groups["char"].Value));
                }
            }
            return (from o in (from o in list
                               group o by o.Server).Select(delegate (IGrouping<string, EverquestFolderViewModel> o)
                           {
                               var everquestFolderViewModel = new EverquestFolderViewModel(o.Key, null)
                               {
                                   Children = o.OrderBy((EverquestFolderViewModel n) => n.Character).ToList<EverquestFolderViewModel>()
                               };
                               return everquestFolderViewModel;
                           })
                    orderby o.Server
                    select o).ToList<EverquestFolderViewModel>();
        }

        // Token: 0x06000327 RID: 807 RVA: 0x0000AFE8 File Offset: 0x000091E8
        public static IEnumerable<string> GetTriggerSets()
        {
            return (from o in Directory.GetDirectories(Path.Combine(Configuration.Current.EverquestFolder, Configuration.EverquestAudioTriggersFolder))
                    select Path.GetFileName(o) into o
                    where o.ToLower() != "shared" && o.ToLower() != "_gina"
                    orderby o
                    select o).ToList<string>();
        }

        // Token: 0x06000328 RID: 808 RVA: 0x0000B07C File Offset: 0x0000927C
        public EverquestFolderViewModel(string filename)
        {
            Filename = filename;
            var match = EverquestFolderViewModel.ATFileRegex.Match(filename);
            if (match.Success)
            {
                Server = match.Groups["server"].Value;
                Character = match.Groups["char"].Value;
                TriggerSet = match.Groups["triggerSet"].Value;
            }
        }

        // Token: 0x06000329 RID: 809 RVA: 0x0000B0FB File Offset: 0x000092FB
        public EverquestFolderViewModel(string server, string character)
        {
            Server = server;
            Character = character;
        }

        // Token: 0x0600032A RID: 810 RVA: 0x0000B111 File Offset: 0x00009311
        public static string GetAudioTriggerFilename(string server, string character, string triggerSet)
        {
            return string.Format(Configuration.EverquestATFileFormat, triggerSet, character, server);
        }

        // Token: 0x17000135 RID: 309
        // (get) Token: 0x0600032B RID: 811 RVA: 0x0000B120 File Offset: 0x00009320
        // (set) Token: 0x0600032C RID: 812 RVA: 0x0000B12D File Offset: 0x0000932D
        public string Filename
        {
            get => base.Get<string>("Filename"); set => base.Set("Filename", value);
        }

        // Token: 0x17000136 RID: 310
        // (get) Token: 0x0600032D RID: 813 RVA: 0x0000B13B File Offset: 0x0000933B
        // (set) Token: 0x0600032E RID: 814 RVA: 0x0000B148 File Offset: 0x00009348
        public string Server
        {
            get => base.Get<string>("Server"); set => base.Set("Server", value);
        }

        // Token: 0x17000137 RID: 311
        // (get) Token: 0x0600032F RID: 815 RVA: 0x0000B156 File Offset: 0x00009356
        // (set) Token: 0x06000330 RID: 816 RVA: 0x0000B163 File Offset: 0x00009363
        public string Character
        {
            get => base.Get<string>("Character"); set => base.Set("Character", value);
        }

        // Token: 0x17000138 RID: 312
        // (get) Token: 0x06000331 RID: 817 RVA: 0x0000B171 File Offset: 0x00009371
        // (set) Token: 0x06000332 RID: 818 RVA: 0x0000B17E File Offset: 0x0000937E
        public string TriggerSet
        {
            get => base.Get<string>("TriggerSet"); set => base.Set("TriggerSet", value);
        }

        // Token: 0x17000139 RID: 313
        // (get) Token: 0x06000333 RID: 819 RVA: 0x0000B18C File Offset: 0x0000938C
        // (set) Token: 0x06000334 RID: 820 RVA: 0x0000B199 File Offset: 0x00009399
        public bool IsSelected
        {
            get => base.Get<bool>("IsSelected"); set => base.Set("IsSelected", value);
        }

        // Token: 0x1700013A RID: 314
        // (get) Token: 0x06000335 RID: 821 RVA: 0x0000B1AC File Offset: 0x000093AC
        // (set) Token: 0x06000336 RID: 822 RVA: 0x0000B1B9 File Offset: 0x000093B9
        public IEnumerable<EverquestFolderViewModel> Children
        {
            get => base.Get<IEnumerable<EverquestFolderViewModel>>("Children"); set => base.Set("Children", value);
        }

        // Token: 0x1700013B RID: 315
        // (get) Token: 0x06000337 RID: 823 RVA: 0x0000B1C7 File Offset: 0x000093C7
        public bool IsServer => string.IsNullOrWhiteSpace(Character);

        // Token: 0x1700013C RID: 316
        // (get) Token: 0x06000338 RID: 824 RVA: 0x0000B1D4 File Offset: 0x000093D4
        public bool IsCharacter => string.IsNullOrWhiteSpace(TriggerSet) && !string.IsNullOrWhiteSpace(Character);

        // Token: 0x1700013D RID: 317
        // (get) Token: 0x06000339 RID: 825 RVA: 0x0000B1F3 File Offset: 0x000093F3
        public bool IsTriggerSet => !string.IsNullOrWhiteSpace(TriggerSet);

        // Token: 0x1700013E RID: 318
        // (get) Token: 0x0600033A RID: 826 RVA: 0x0000B203 File Offset: 0x00009403
        public string DisplayName
        {
            get
            {
                if (IsServer)
                {
                    return Server;
                }
                if (IsCharacter)
                {
                    return Character;
                }
                return TriggerSet;
            }
        }

        // Token: 0x0400009A RID: 154
        private static readonly Regex ATFileRegex = new Regex(Configuration.EverquestATFileRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // Token: 0x0400009B RID: 155
        private static readonly Regex UIFileRegex = new Regex(Configuration.EverquestUIFileRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}
