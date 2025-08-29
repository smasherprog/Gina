using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using BusinessShared;
using GimaSoft.Business.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x02000012 RID: 18
	public class PhoneticDictionaryEditor : BindableControl
	{
		// Token: 0x06000275 RID: 629 RVA: 0x000089D5 File Offset: 0x00006BD5
		static PhoneticDictionaryEditor()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PhoneticDictionaryEditor), new FrameworkPropertyMetadata(typeof(PhoneticDictionaryEditor)));
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000276 RID: 630 RVA: 0x000089FA File Offset: 0x00006BFA
		public App App
		{
			get
			{
				return (App)Application.Current;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00008A06 File Offset: 0x00006C06
		// (set) Token: 0x06000278 RID: 632 RVA: 0x00008A13 File Offset: 0x00006C13
		public string SearchText
		{
			get
			{
				return base.Get<string>("SearchText");
			}
			set
			{
				base.Set("SearchText", value);
				this.UpdateFilter();
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00008A27 File Offset: 0x00006C27
		// (set) Token: 0x0600027A RID: 634 RVA: 0x00008A34 File Offset: 0x00006C34
		public string NewActualWord
		{
			get
			{
				return base.Get<string>("NewActualWord");
			}
			set
			{
				base.Set("NewActualWord", value);
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00008A42 File Offset: 0x00006C42
		// (set) Token: 0x0600027C RID: 636 RVA: 0x00008A4F File Offset: 0x00006C4F
		public string NewPhoneticWord
		{
			get
			{
				return base.Get<string>("NewPhoneticWord");
			}
			set
			{
				base.Set("NewPhoneticWord", value);
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600027D RID: 637 RVA: 0x00008A5D File Offset: 0x00006C5D
		// (set) Token: 0x0600027E RID: 638 RVA: 0x00008A6A File Offset: 0x00006C6A
		public CharacterViewModel SelectedVoiceCharacter
		{
			get
			{
				return base.Get<CharacterViewModel>("SelectedVoiceCharacter");
			}
			set
			{
				base.Set("SelectedVoiceCharacter", value);
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600027F RID: 639 RVA: 0x00008A78 File Offset: 0x00006C78
		// (set) Token: 0x06000280 RID: 640 RVA: 0x00008A85 File Offset: 0x00006C85
		public ICollectionView Transforms
		{
			get
			{
				return base.Get<ICollectionView>("Transforms");
			}
			set
			{
				base.Set("Transforms", value);
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000281 RID: 641 RVA: 0x00008A93 File Offset: 0x00006C93
		// (set) Token: 0x06000282 RID: 642 RVA: 0x00008AA0 File Offset: 0x00006CA0
		public bool ShowNewTransformDialog
		{
			get
			{
				return base.Get<bool>("ShowNewTransformDialog");
			}
			set
			{
				base.Set("ShowNewTransformDialog", value);
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000283 RID: 643 RVA: 0x00008AB3 File Offset: 0x00006CB3
		// (set) Token: 0x06000284 RID: 644 RVA: 0x00008ABB File Offset: 0x00006CBB
		private bool ChangesMade { get; set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000285 RID: 645 RVA: 0x00008AC4 File Offset: 0x00006CC4
		// (set) Token: 0x06000286 RID: 646 RVA: 0x00008ACC File Offset: 0x00006CCC
		private PhoneticTransform TransformBeingEdited { get; set; }

		// Token: 0x06000287 RID: 647 RVA: 0x00008AD5 File Offset: 0x00006CD5
		private void UpdateView()
		{
			this.Transforms = CollectionViewSource.GetDefaultView(Configuration.Current.PhoneticDictionary);
			this.UpdateFilter();
			this.Transforms.SortDescriptions.Add(new SortDescription("ActualWord", ListSortDirection.Ascending));
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00008B4C File Offset: 0x00006D4C
		private void UpdateFilter()
		{
			if (this.Transforms != null)
			{
				ICollectionView transforms = this.Transforms;
				transforms.Filter = (Predicate<object>)Delegate.Combine(transforms.Filter, new Predicate<object>(delegate(object o)
				{
					PhoneticTransform phoneticTransform = o as PhoneticTransform;
					return phoneticTransform.ActualWord.Contains(this.SearchText, true) || phoneticTransform.PhoneticWord.Contains(this.SearchText, true);
				}));
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000289 RID: 649 RVA: 0x00008BE0 File Offset: 0x00006DE0
		public GenericCommand VisibilityChangedCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if ((Visibility)p == Visibility.Visible)
						{
							this.SearchText = string.Empty;
							this.ChangesMade = false;
							this.UpdateView();
							if (this.SelectedVoiceCharacter == null)
							{
								this.SelectedVoiceCharacter = this.App.Data.Characters.FirstOrDefault<CharacterViewModel>();
							}
						}
					}
				};
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600028A RID: 650 RVA: 0x00008C48 File Offset: 0x00006E48
		public GenericCommand PlayTransformCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						CharacterViewModel characterViewModel = this.SelectedVoiceCharacter ?? this.App.Data.Characters.FirstOrDefault<CharacterViewModel>();
						if (characterViewModel == null)
						{
							return;
						}
						characterViewModel.SayText(p as string);
					}
				};
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00008CA6 File Offset: 0x00006EA6
		public GenericCommand RemoveTransformCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					PhoneticTransform phoneticTransform = p as PhoneticTransform;
					if (phoneticTransform == null)
					{
						return;
					}
					Configuration.Current.PhoneticDictionary.Remove(phoneticTransform);
					this.UpdateView();
					this.ChangesMade = true;
				});
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600028C RID: 652 RVA: 0x00008CFA File Offset: 0x00006EFA
		public GenericCommand EditTransformCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					PhoneticTransform phoneticTransform = p as PhoneticTransform;
					if (phoneticTransform == null)
					{
						return;
					}
					this.TransformBeingEdited = phoneticTransform;
					this.NewActualWord = phoneticTransform.ActualWord;
					this.NewPhoneticWord = phoneticTransform.PhoneticWord;
					this.ShowNewTransformDialog = true;
				});
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00008D38 File Offset: 0x00006F38
		public GenericCommand NewTransformCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						this.TransformBeingEdited = null;
						this.NewActualWord = this.SearchText;
						this.NewPhoneticWord = this.SearchText;
						this.ShowNewTransformDialog = true;
					}
				};
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600028E RID: 654 RVA: 0x00008E2C File Offset: 0x0000702C
		public GenericCommand AddTransformCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if (string.IsNullOrWhiteSpace(this.NewActualWord) || string.IsNullOrWhiteSpace(this.NewPhoneticWord))
						{
							return;
						}
						PhoneticTransform phoneticTransform = Configuration.Current.PhoneticDictionary.FirstOrDefault((PhoneticTransform o) => o.ActualWord.ToUpper() == this.NewActualWord.ToUpper());
						if (phoneticTransform == null)
						{
							Configuration.Current.PhoneticDictionary.Add(new PhoneticTransform(this.NewActualWord, this.NewPhoneticWord));
						}
						else
						{
							phoneticTransform.PhoneticWord = this.NewPhoneticWord;
						}
						if (this.TransformBeingEdited != null && this.TransformBeingEdited != phoneticTransform)
						{
							Configuration.Current.PhoneticDictionary.Remove(this.TransformBeingEdited);
						}
						this.ChangesMade = true;
						this.UpdateView();
						this.ShowNewTransformDialog = false;
					}
				};
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600028F RID: 655 RVA: 0x00008E5B File Offset: 0x0000705B
		public GenericCommand CancelTransformCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					this.ShowNewTransformDialog = false;
				});
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00008E85 File Offset: 0x00007085
		public GenericCommand CloseCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					if (this.ChangesMade)
					{
						Configuration.SaveConfiguration(false);
					}
					base.Visibility = Visibility.Collapsed;
				});
			}
		}
	}
}
