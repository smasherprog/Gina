using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GimaSoft.Business.GINA
{
	// Token: 0x02000025 RID: 37
	public class LogSearcher
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060001C0 RID: 448 RVA: 0x00009618 File Offset: 0x00007818
		// (remove) Token: 0x060001C1 RID: 449 RVA: 0x00009650 File Offset: 0x00007850
		public event LogSearcher.FileRangeLocatedHandler FileRangeLocated = delegate(object o, EventArgs e)
		{
		};

		// Token: 0x060001C2 RID: 450 RVA: 0x00009685 File Offset: 0x00007885
		public void OnFileRangeLocated()
		{
			this.FileRangeLocated(this, new EventArgs());
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060001C3 RID: 451 RVA: 0x00009698 File Offset: 0x00007898
		// (remove) Token: 0x060001C4 RID: 452 RVA: 0x000096D0 File Offset: 0x000078D0
		public event LogSearcher.SearchCompletedHandler SearchCompleted = delegate(object o, EventArgs e)
		{
		};

		// Token: 0x060001C5 RID: 453 RVA: 0x00009705 File Offset: 0x00007905
		private void OnSearchCompleted()
		{
			this.SearchCompleted(this, new EventArgs());
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060001C6 RID: 454 RVA: 0x00009718 File Offset: 0x00007918
		// (remove) Token: 0x060001C7 RID: 455 RVA: 0x00009750 File Offset: 0x00007950
		public event LogSearcher.SearchCancelledHandler SearchCancelled = delegate(object o, EventArgs e)
		{
		};

		// Token: 0x060001C8 RID: 456 RVA: 0x00009785 File Offset: 0x00007985
		private void OnSearchCancelled()
		{
			this.SearchCancelled(this, new EventArgs());
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060001C9 RID: 457 RVA: 0x00009798 File Offset: 0x00007998
		// (remove) Token: 0x060001CA RID: 458 RVA: 0x000097D0 File Offset: 0x000079D0
		public event LogSearcher.SearchFailedHandler SearchFailed = delegate(object o, LogSearcher.SearchFailedEventArgs e)
		{
		};

		// Token: 0x060001CB RID: 459 RVA: 0x00009805 File Offset: 0x00007A05
		private void OnSearchFailed(LogSearcher.SearchFailures reason)
		{
			this.SearchFailed(this, new LogSearcher.SearchFailedEventArgs(reason));
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060001CC RID: 460 RVA: 0x0000981C File Offset: 0x00007A1C
		// (remove) Token: 0x060001CD RID: 461 RVA: 0x00009854 File Offset: 0x00007A54
		public event LogSearcher.MatchFoundHandler MatchFound = delegate(object o, LogSearcher.LogSearchMatchedEventArgs e)
		{
		};

		// Token: 0x060001CE RID: 462 RVA: 0x00009889 File Offset: 0x00007A89
		private void OnMatchFound(LogMatch match)
		{
			this.MatchFound(this, new LogSearcher.LogSearchMatchedEventArgs(match));
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060001CF RID: 463 RVA: 0x000098A0 File Offset: 0x00007AA0
		// (remove) Token: 0x060001D0 RID: 464 RVA: 0x000098D8 File Offset: 0x00007AD8
		public event LogSearcher.MatchProgressChangedHandler MatchProgressChanged = delegate(object o, LogSearcher.MatchProgressChangedEventArgs e)
		{
		};

		// Token: 0x060001D1 RID: 465 RVA: 0x0000990D File Offset: 0x00007B0D
		public void OnMatchProgressChanged(int progress, long linesRead, DateTime startTime)
		{
			this.MatchProgressChanged(this, new LogSearcher.MatchProgressChangedEventArgs(progress, linesRead, startTime));
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00009930 File Offset: 0x00007B30
		public LogSearcher(GINACharacter character, LogSearchTypes searchType, string searchText, DateTime startDate, DateTime endDate)
		{
			this.Character = character;
			this._SearchType = searchType;
			this.SearchText = searchText;
			this.StartDate = startDate;
			this.EndDate = endDate;
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00009A3A File Offset: 0x00007C3A
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00009A42 File Offset: 0x00007C42
		public GINACharacter Character { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00009A4B File Offset: 0x00007C4B
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00009A53 File Offset: 0x00007C53
		public string SearchText { get; set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00009A5C File Offset: 0x00007C5C
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00009A64 File Offset: 0x00007C64
		public DateTime StartDate { get; set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00009A6D File Offset: 0x00007C6D
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00009A75 File Offset: 0x00007C75
		public DateTime EndDate { get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00009A7E File Offset: 0x00007C7E
		// (set) Token: 0x060001DC RID: 476 RVA: 0x00009A86 File Offset: 0x00007C86
		public bool IncludeArchives { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00009A8F File Offset: 0x00007C8F
		public bool Cancelling
		{
			get
			{
				return this._Task != null && this._Task.IsBusy && this._Task.CancellationPending;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00009AD4 File Offset: 0x00007CD4
		public List<LogSearcher.ArchiveFileInfo> ArchiveFileDateRanges
		{
			get
			{
				List<LogSearcher.ArchiveFileInfo> list = new List<LogSearcher.ArchiveFileInfo>();
				IEnumerable<LogSearcher.FileSearchInfo> enumerable = this.Character.ArchivedLogFiles.Select((string o) => new LogSearcher.FileSearchInfo(this, o, DateTime.MinValue, DateTime.MaxValue, LogSearchTypes.Text, this.Character, string.Empty));
				foreach (LogSearcher.FileSearchInfo fileSearchInfo in enumerable)
				{
					LogSearcher.SearchFailures searchFailures = fileSearchInfo.PrepareSearch();
					if (searchFailures == LogSearcher.SearchFailures.Okay)
					{
						list.Add(new LogSearcher.ArchiveFileInfo(fileSearchInfo.FileName, fileSearchInfo.FileStartDate, fileSearchInfo.FileEndDate));
					}
					fileSearchInfo.Dispose();
				}
				return list;
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00009EF0 File Offset: 0x000080F0
		public void Search()
		{
			if (this._Task != null)
			{
				this.Cancel();
			}
			this._Task = new BackgroundWorker();
			this._Task.WorkerSupportsCancellation = true;
			this._Task.DoWork += delegate(object o, DoWorkEventArgs e)
			{
				if (this.Character == null || string.IsNullOrWhiteSpace(this.Character.LogFilePath))
				{
					return;
				}
				List<string> list = new List<string> { this.Character.LogFilePath };
				if (this.IncludeArchives)
				{
					list.AddRange(this.Character.ArchivedLogFiles);
				}
				int lastProgress = 0;
				int progress = 0;
				DateTime searchStartDT = DateTime.Now;
				this.OnMatchProgressChanged(0, 0L, searchStartDT);
				List<LogSearcher.FileSearchInfo> list2 = new List<LogSearcher.FileSearchInfo>();
				List<LogSearcher.SearchFailures> list3 = new List<LogSearcher.SearchFailures>();
				foreach (string text in list)
				{
					LogSearcher.FileSearchInfo fileSearchInfo = new LogSearcher.FileSearchInfo(this, text, this.StartDate, this.EndDate, this._SearchType, this.Character, this.SearchText);
					LogSearcher.SearchFailures searchFailures = fileSearchInfo.PrepareSearch();
					list3.Add(searchFailures);
					if (searchFailures == LogSearcher.SearchFailures.Okay)
					{
						fileSearchInfo.MatchFound += delegate(object io, LogSearcher.LogSearchMatchedEventArgs ie)
						{
							this.OnMatchFound(ie.Match);
						};
						fileSearchInfo.FileProgressChanged += delegate(object io, LogSearcher.FileProgressChangedEventsArgs ie)
						{
							this.BytesProcessed += ie.BytesProcessed;
							this.LinesRead += ie.LinesRead;
							progress = (int)((double)this.BytesProcessed / (double)this.BytesToProcess * 100.0);
							if (progress != lastProgress)
							{
								this.OnMatchProgressChanged(lastProgress = progress, this.LinesRead, searchStartDT);
							}
						};
						list2.Add(fileSearchInfo);
					}
				}
				if (list3.All((LogSearcher.SearchFailures n) => n == LogSearcher.SearchFailures.NoFile))
				{
					this.OnSearchFailed(LogSearcher.SearchFailures.NoFile);
					return;
				}
				if (list3.Where((LogSearcher.SearchFailures n) => n != LogSearcher.SearchFailures.NoFile).All((LogSearcher.SearchFailures n) => n == LogSearcher.SearchFailures.NoDataInRange))
				{
					this.OnSearchFailed(LogSearcher.SearchFailures.NoDataInRange);
					return;
				}
				this.BytesToProcess = list2.Sum((LogSearcher.FileSearchInfo n) => n.TotalBytesToSearch);
				foreach (LogSearcher.FileSearchInfo fileSearchInfo2 in list2.OrderBy((LogSearcher.FileSearchInfo n) => n.FileStartDate))
				{
					fileSearchInfo2.Search();
					fileSearchInfo2.Dispose();
				}
				this.OnMatchProgressChanged(0, this.LinesRead, searchStartDT);
				if (((BackgroundWorker)e.Argument).CancellationPending)
				{
					this.OnSearchCancelled();
				}
				this.OnSearchCompleted();
			};
			this._Task.RunWorkerAsync(this._Task);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009F4C File Offset: 0x0000814C
		public void Cancel()
		{
			try
			{
				if (this._Task != null && this._Task.IsBusy)
				{
					this._Task.CancelAsync();
				}
			}
			catch
			{
			}
		}

		// Token: 0x040000C4 RID: 196
		private LogSearchTypes _SearchType;

		// Token: 0x040000C5 RID: 197
		private BackgroundWorker _Task;

		// Token: 0x040000C6 RID: 198
		private long BytesProcessed;

		// Token: 0x040000C7 RID: 199
		private long BytesToProcess;

		// Token: 0x040000C8 RID: 200
		private long LinesRead;

		// Token: 0x02000026 RID: 38
		// (Invoke) Token: 0x060001F0 RID: 496
		public delegate void FileRangeLocatedHandler(object sender, EventArgs e);

		// Token: 0x02000027 RID: 39
		// (Invoke) Token: 0x060001F4 RID: 500
		public delegate void SearchCompletedHandler(object sender, EventArgs e);

		// Token: 0x02000028 RID: 40
		// (Invoke) Token: 0x060001F8 RID: 504
		public delegate void SearchCancelledHandler(object sender, EventArgs e);

		// Token: 0x02000029 RID: 41
		public enum SearchFailures
		{
			// Token: 0x040000DA RID: 218
			NoFile,
			// Token: 0x040000DB RID: 219
			FileAccess,
			// Token: 0x040000DC RID: 220
			NoDataInRange,
			// Token: 0x040000DD RID: 221
			Okay
		}

		// Token: 0x0200002A RID: 42
		public class SearchFailedEventArgs : EventArgs
		{
			// Token: 0x060001FB RID: 507 RVA: 0x00009F90 File Offset: 0x00008190
			public SearchFailedEventArgs(LogSearcher.SearchFailures reason)
			{
				this.Reason = reason;
			}

			// Token: 0x170000BA RID: 186
			// (get) Token: 0x060001FC RID: 508 RVA: 0x00009F9F File Offset: 0x0000819F
			// (set) Token: 0x060001FD RID: 509 RVA: 0x00009FA7 File Offset: 0x000081A7
			public LogSearcher.SearchFailures Reason { get; set; }
		}

		// Token: 0x0200002B RID: 43
		// (Invoke) Token: 0x060001FF RID: 511
		public delegate void SearchFailedHandler(object sender, LogSearcher.SearchFailedEventArgs e);

		// Token: 0x0200002C RID: 44
		public class LogSearchMatchedEventArgs : EventArgs
		{
			// Token: 0x06000202 RID: 514 RVA: 0x00009FB0 File Offset: 0x000081B0
			public LogSearchMatchedEventArgs(LogMatch match)
			{
				this.Match = match;
			}

			// Token: 0x170000BB RID: 187
			// (get) Token: 0x06000203 RID: 515 RVA: 0x00009FBF File Offset: 0x000081BF
			// (set) Token: 0x06000204 RID: 516 RVA: 0x00009FC7 File Offset: 0x000081C7
			public LogMatch Match { get; set; }
		}

		// Token: 0x0200002D RID: 45
		// (Invoke) Token: 0x06000206 RID: 518
		public delegate void MatchFoundHandler(object sender, LogSearcher.LogSearchMatchedEventArgs e);

		// Token: 0x0200002E RID: 46
		public class FileProgressChangedEventsArgs : EventArgs
		{
			// Token: 0x06000209 RID: 521 RVA: 0x00009FD0 File Offset: 0x000081D0
			public FileProgressChangedEventsArgs(long bytesProcessed, long linesRead)
			{
				this.BytesProcessed = bytesProcessed;
				this.LinesRead = linesRead;
			}

			// Token: 0x170000BC RID: 188
			// (get) Token: 0x0600020A RID: 522 RVA: 0x00009FE6 File Offset: 0x000081E6
			// (set) Token: 0x0600020B RID: 523 RVA: 0x00009FEE File Offset: 0x000081EE
			public long BytesProcessed { get; set; }

			// Token: 0x170000BD RID: 189
			// (get) Token: 0x0600020C RID: 524 RVA: 0x00009FF7 File Offset: 0x000081F7
			// (set) Token: 0x0600020D RID: 525 RVA: 0x00009FFF File Offset: 0x000081FF
			public long LinesRead { get; set; }
		}

		// Token: 0x0200002F RID: 47
		// (Invoke) Token: 0x0600020F RID: 527
		public delegate void FileProgressChangedHandler(object sender, LogSearcher.FileProgressChangedEventsArgs e);

		// Token: 0x02000030 RID: 48
		public class MatchProgressChangedEventArgs : EventArgs
		{
			// Token: 0x06000212 RID: 530 RVA: 0x0000A008 File Offset: 0x00008208
			public MatchProgressChangedEventArgs(int newProgress, long linesRead, DateTime startTime)
			{
				this.Progress = newProgress;
				this.LinesRead = linesRead;
				double totalSeconds = (DateTime.Now - startTime).TotalSeconds;
				this.LinesPerSecond = ((totalSeconds == 0.0) ? this.LinesRead : ((long)((double)linesRead / totalSeconds)));
			}

			// Token: 0x170000BE RID: 190
			// (get) Token: 0x06000213 RID: 531 RVA: 0x0000A05C File Offset: 0x0000825C
			// (set) Token: 0x06000214 RID: 532 RVA: 0x0000A064 File Offset: 0x00008264
			public int Progress { get; set; }

			// Token: 0x170000BF RID: 191
			// (get) Token: 0x06000215 RID: 533 RVA: 0x0000A06D File Offset: 0x0000826D
			// (set) Token: 0x06000216 RID: 534 RVA: 0x0000A075 File Offset: 0x00008275
			public long LinesPerSecond { get; set; }

			// Token: 0x170000C0 RID: 192
			// (get) Token: 0x06000217 RID: 535 RVA: 0x0000A07E File Offset: 0x0000827E
			// (set) Token: 0x06000218 RID: 536 RVA: 0x0000A086 File Offset: 0x00008286
			public long LinesRead { get; set; }
		}

		// Token: 0x02000031 RID: 49
		// (Invoke) Token: 0x0600021A RID: 538
		public delegate void MatchProgressChangedHandler(object sender, LogSearcher.MatchProgressChangedEventArgs e);

		// Token: 0x02000032 RID: 50
		private class FileSearchInfo : IDisposable
		{
			// Token: 0x0600021D RID: 541 RVA: 0x0000A094 File Offset: 0x00008294
			public FileSearchInfo(LogSearcher searcher, string file, DateTime startDate, DateTime endDate, LogSearchTypes searchType, GINACharacter character, string searchText)
			{
				this.LogSearcher = searcher;
				this.StartDate = startDate;
				this.EndDate = endDate;
				this.FileName = file;
				this._SearchType = searchType;
				this.Character = character;
				this.SearchText = searchText;
			}

			// Token: 0x1400000A RID: 10
			// (add) Token: 0x0600021E RID: 542 RVA: 0x0000A16C File Offset: 0x0000836C
			// (remove) Token: 0x0600021F RID: 543 RVA: 0x0000A1A4 File Offset: 0x000083A4
			public event LogSearcher.MatchFoundHandler MatchFound = delegate(object o, LogSearcher.LogSearchMatchedEventArgs e)
			{
			};

			// Token: 0x06000220 RID: 544 RVA: 0x0000A1D9 File Offset: 0x000083D9
			private void OnMatchFound(LogMatch match)
			{
				this.MatchFound(this, new LogSearcher.LogSearchMatchedEventArgs(match));
			}

			// Token: 0x1400000B RID: 11
			// (add) Token: 0x06000221 RID: 545 RVA: 0x0000A1F0 File Offset: 0x000083F0
			// (remove) Token: 0x06000222 RID: 546 RVA: 0x0000A228 File Offset: 0x00008428
			public event LogSearcher.FileProgressChangedHandler FileProgressChanged = delegate(object o, LogSearcher.FileProgressChangedEventsArgs e)
			{
			};

			// Token: 0x06000223 RID: 547 RVA: 0x0000A25D File Offset: 0x0000845D
			private void OnFileProgressChanged(long bytesProcessed, long linesRead)
			{
				this.FileProgressChanged(this, new LogSearcher.FileProgressChangedEventsArgs(bytesProcessed, linesRead));
			}

			// Token: 0x170000C1 RID: 193
			// (get) Token: 0x06000224 RID: 548 RVA: 0x0000A272 File Offset: 0x00008472
			// (set) Token: 0x06000225 RID: 549 RVA: 0x0000A27A File Offset: 0x0000847A
			public string FileName { get; set; }

			// Token: 0x170000C2 RID: 194
			// (get) Token: 0x06000226 RID: 550 RVA: 0x0000A283 File Offset: 0x00008483
			public long TotalBytesToSearch
			{
				get
				{
					return this._EndPosition - this._StartPosition;
				}
			}

			// Token: 0x170000C3 RID: 195
			// (get) Token: 0x06000227 RID: 551 RVA: 0x0000A292 File Offset: 0x00008492
			// (set) Token: 0x06000228 RID: 552 RVA: 0x0000A29A File Offset: 0x0000849A
			public DateTime? FileStartDate { get; set; }

			// Token: 0x170000C4 RID: 196
			// (get) Token: 0x06000229 RID: 553 RVA: 0x0000A2A3 File Offset: 0x000084A3
			// (set) Token: 0x0600022A RID: 554 RVA: 0x0000A2AB File Offset: 0x000084AB
			public DateTime? FileEndDate { get; set; }

			// Token: 0x0600022B RID: 555 RVA: 0x0000A2B4 File Offset: 0x000084B4
			private void SetPosition(long position)
			{
				if (this._Reader == null)
				{
					return;
				}
				this._Reader.BaseStream.Position = position;
				this._Reader.DiscardBufferedData();
			}

			// Token: 0x0600022C RID: 556 RVA: 0x0000A2DC File Offset: 0x000084DC
			private string GetLookbackString(long fromPosition)
			{
				long num = fromPosition - Math.Min((long)this._BufferSize, fromPosition);
				long num2 = fromPosition - num;
				StringBuilder stringBuilder = new StringBuilder();
				byte[] array = new byte[num2];
				this.SetPosition(num);
				this._Reader.BaseStream.Read(array, 0, (int)num2);
				stringBuilder.Append(Encoding.ASCII.GetString(array).ToCharArray());
				string[] array2 = stringBuilder.ToString().Split(this.EOL, StringSplitOptions.None);
				return array2.Last<string>();
			}

			// Token: 0x0600022D RID: 557 RVA: 0x0000A35C File Offset: 0x0000855C
			private long? GetMidPositionForStart(long startPosition, long endPosition)
			{
				long num = (startPosition + endPosition) / 2L;
				this.SetPosition(num - 1L);
				int num2 = this._Reader.BaseStream.ReadByte();
				if (num2 != 10)
				{
					string lookbackString = this.GetLookbackString(num);
					num -= (long)lookbackString.Length;
				}
				return new long?(num);
			}

			// Token: 0x0600022E RID: 558 RVA: 0x0000A3A8 File Offset: 0x000085A8
			private long? GetMidPositionForEnd(long startPosition, long endPosition)
			{
				long num = (startPosition + endPosition) / 2L;
				if (num > 0L)
				{
					this.SetPosition(num - 1L);
					int num2 = this._Reader.BaseStream.ReadByte();
					if (num2 == 10)
					{
						return new long?(num);
					}
				}
				this.SetPosition(num);
				if (this._Reader.BaseStream.ReadByte() == 10)
				{
					return new long?(num + 1L);
				}
				string text = this._Reader.ReadLine();
				num = Math.Min(endPosition, num + (long)text.Length + 2L);
				return new long?(num);
			}

			// Token: 0x0600022F RID: 559 RVA: 0x0000A434 File Offset: 0x00008634
			private long? FindDate(DateTime dateTime, bool findLast = false)
			{
				long num = this._StartPosition;
				long num2 = this._EndPosition;
				long? num3 = null;
				bool flag = false;
				while (num < num2 && !flag)
				{
					num3 = (findLast ? this.GetMidPositionForEnd(num, num2) : this.GetMidPositionForStart(num, num2));
					if (num3 == null)
					{
						return null;
					}
					if (findLast && num3 == num2)
					{
						return new long?(num);
					}
					if (!findLast && num3 == num)
					{
						return new long?(num2 + 1L);
					}
					string text;
					if (findLast)
					{
						text = this.GetLookbackString(num3.Value - 1L);
					}
					else
					{
						this.SetPosition(num3.Value);
						text = this._Reader.ReadLine();
					}
					DateTime? dateTime2 = this.LoggedEntry(text).Item1;
					if (dateTime2 == null)
					{
						return null;
					}
					if (dateTime2 > dateTime && num2 != num3.Value)
					{
						num2 = num3.Value - 1L;
					}
					else if (dateTime2 < dateTime && num != num3.Value)
					{
						num = num3.Value;
					}
					else
					{
						if (dateTime2 == dateTime)
						{
							switch (findLast)
							{
							case false:
								while (num3 > num + 2L)
								{
									if (!(dateTime2 == dateTime))
									{
										break;
									}
									text = this.GetLookbackString(num3.Value - 2L);
									dateTime2 = this.LoggedEntry(text).Item1;
									if (dateTime2 == dateTime)
									{
										num3 -= (long)(text.Length + 2);
									}
								}
								break;
							case true:
								if (num3 < num2 + 1L)
								{
									this.SetPosition(num3.Value + 1L);
								}
								while (num3 < num2 + 1L && dateTime2 == dateTime)
								{
									text = this._Reader.ReadLine();
									dateTime2 = this.LoggedEntry(text).Item1;
									if (dateTime2 == dateTime)
									{
										num3 += (long)(text.Length + 2);
									}
								}
								break;
							}
						}
						flag = true;
					}
				}
				return num3;
			}

			// Token: 0x06000230 RID: 560 RVA: 0x0000A7A0 File Offset: 0x000089A0
			private bool GetRange()
			{
				this.SetPosition(this._StartPosition);
				string text = this._Reader.ReadLine();
				if (text == null)
				{
					return false;
				}
				this.FileStartDate = this.LoggedEntry(text).Item1;
				text = this.GetLookbackString(Math.Max(this._StartPosition, this._EndPosition - 1L));
				this.FileEndDate = this.LoggedEntry(text).Item1;
				if (this.FileStartDate == null || this.EndDate < this.FileStartDate)
				{
					return false;
				}
				if (this.FileEndDate == null || this.StartDate > this.FileEndDate)
				{
					return false;
				}
				this._StartPosition = this.FindDate(this.StartDate, false) ?? this._StartPosition;
				this._EndPosition = this.FindDate(this.EndDate, true) ?? this._EndPosition;
				return true;
			}

			// Token: 0x06000231 RID: 561 RVA: 0x0000A8E0 File Offset: 0x00008AE0
			private Tuple<DateTime?, string> LoggedEntry(string text)
			{
				Match match = this.LogFormat.Match(text);
				if (!match.Success)
				{
					return new Tuple<DateTime?, string>(null, text);
				}
				DateTime dateTime = DateTime.Parse(string.Format("{0} {1} {2} {3}:{4}:{5}", new object[]
				{
					match.Groups["month"].Value,
					match.Groups["date"].Value,
					match.Groups["year"].Value,
					match.Groups["hour"].Value,
					match.Groups["minute"].Value,
					match.Groups["second"].Value
				}));
				return new Tuple<DateTime?, string>(new DateTime?(dateTime), match.Groups["text"].Value);
			}

			// Token: 0x06000232 RID: 562 RVA: 0x0000A9DC File Offset: 0x00008BDC
			public LogSearcher.SearchFailures PrepareSearch()
			{
				if (string.IsNullOrWhiteSpace(this.FileName) || !File.Exists(this.FileName))
				{
					return LogSearcher.SearchFailures.NoFile;
				}
				try
				{
					FileStream fileStream = File.Open(this.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					FileInfo fileInfo = new FileInfo(this.FileName);
					this._StartPosition = 0L;
					this._EndPosition = fileInfo.Length;
					try
					{
						this._Reader = new StreamReader(fileStream);
					}
					catch
					{
						if (fileStream != null)
						{
							fileStream.Dispose();
						}
						return LogSearcher.SearchFailures.FileAccess;
					}
				}
				catch
				{
					return LogSearcher.SearchFailures.FileAccess;
				}
				if (this._Reader == null)
				{
					return LogSearcher.SearchFailures.FileAccess;
				}
				if (!this.GetRange())
				{
					return LogSearcher.SearchFailures.NoDataInRange;
				}
				return LogSearcher.SearchFailures.Okay;
			}

			// Token: 0x06000233 RID: 563 RVA: 0x0000AAF4 File Offset: 0x00008CF4
			public void Search()
			{
				long num = this._StartPosition;
				bool? flag = null;
				this.SetPosition(this._StartPosition);
				TriggerFilter triggerFilter = null;
				if (this._SearchType == LogSearchTypes.Text)
				{
					triggerFilter = new TriggerFilter(this.Character, new Trigger
					{
						TriggerText = (string.IsNullOrEmpty(this.SearchText) ? ".+" : this.SearchText),
						EnableRegex = true
					});
				}
				List<TriggerFilter> list = null;
				if (this._SearchType == LogSearchTypes.AllTriggers)
				{
					list = (from oi in TriggerGroup.RootGroup.DescendantTree.SelectMany((TriggerGroup oi) => oi.Triggers)
						where !this.Character.Filters.Any((TriggerFilter ni) => ni.Trigger == oi)
						select new TriggerFilter(this.Character, oi)).ToList<TriggerFilter>();
				}
				string text;
				while (num < this._EndPosition && (text = this._Reader.ReadLine()) != null)
				{
					if (this.LogSearcher.Cancelling)
					{
						num = this._EndPosition + 1L;
					}
					else if (!string.IsNullOrEmpty(text))
					{
						int num2 = 0;
						if (flag == true)
						{
							num2 = Configuration.EverquestFastSearchSkip;
						}
						else if (flag == null)
						{
							flag = new bool?(this.Character.GetIsEverquestCharacterFromLog(text));
							if (flag == true)
							{
								num2 = Configuration.EverquestFastSearchSkip;
							}
						}
						num += (long)(text.Length + 2);
						if (this._SearchType != LogSearchTypes.Text || triggerFilter.IsMatch(text.Substring(num2)))
						{
							Tuple<DateTime?, string> tuple = this.LoggedEntry(text);
							LogMatch logMatch = new LogMatch(this.Character, tuple.Item2, tuple.Item1, list);
							if (logMatch.MatchedTrigger != null || this._SearchType == LogSearchTypes.Text)
							{
								this.OnMatchFound(logMatch);
							}
						}
						this.OnFileProgressChanged((long)(text.Length + 2), 1L);
					}
				}
			}

			// Token: 0x06000234 RID: 564 RVA: 0x0000ACE5 File Offset: 0x00008EE5
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06000235 RID: 565 RVA: 0x0000ACF4 File Offset: 0x00008EF4
			public virtual void Dispose(bool disposing)
			{
				if (!this._Disposed)
				{
					this._Disposed = true;
					if (disposing)
					{
						if (this._Reader != null && this._Reader.BaseStream != null)
						{
							this._Reader.BaseStream.Dispose();
						}
						if (this._Reader != null)
						{
							this._Reader.Dispose();
						}
					}
				}
			}

			// Token: 0x06000236 RID: 566 RVA: 0x0000AD4C File Offset: 0x00008F4C
			~FileSearchInfo()
			{
				this.Dispose(false);
			}

			// Token: 0x040000E7 RID: 231
			private long _StartPosition;

			// Token: 0x040000E8 RID: 232
			private long _EndPosition;

			// Token: 0x040000E9 RID: 233
			private DateTime StartDate;

			// Token: 0x040000EA RID: 234
			private DateTime EndDate;

			// Token: 0x040000EB RID: 235
			private LogSearchTypes _SearchType;

			// Token: 0x040000EC RID: 236
			private GINACharacter Character;

			// Token: 0x040000ED RID: 237
			private string SearchText;

			// Token: 0x040000EE RID: 238
			private StreamReader _Reader;

			// Token: 0x040000EF RID: 239
			private Regex LogFormat = new Regex(Configuration.EverquestLogLineFormat, RegexOptions.Compiled);

			// Token: 0x040000F0 RID: 240
			private int _BufferSize = 900;

			// Token: 0x040000F1 RID: 241
			private LogSearcher LogSearcher;

			// Token: 0x040000F2 RID: 242
			private string[] EOL = new string[] { "\r\n" };

			// Token: 0x040000F3 RID: 243
			private int[] EOLChars = new int[] { 13, 10 };

			// Token: 0x040000F4 RID: 244
			protected bool _Disposed;
		}

		// Token: 0x02000033 RID: 51
		public class ArchiveFileInfo
		{
			// Token: 0x0600023C RID: 572 RVA: 0x0000AD7C File Offset: 0x00008F7C
			public ArchiveFileInfo(string filename, DateTime? startDate, DateTime? endDate)
			{
				this.FileName = filename;
				this.StartDate = startDate;
				this.EndDate = endDate;
			}

			// Token: 0x170000C5 RID: 197
			// (get) Token: 0x0600023D RID: 573 RVA: 0x0000AD99 File Offset: 0x00008F99
			// (set) Token: 0x0600023E RID: 574 RVA: 0x0000ADA1 File Offset: 0x00008FA1
			public string FileName { get; set; }

			// Token: 0x170000C6 RID: 198
			// (get) Token: 0x0600023F RID: 575 RVA: 0x0000ADAA File Offset: 0x00008FAA
			// (set) Token: 0x06000240 RID: 576 RVA: 0x0000ADB2 File Offset: 0x00008FB2
			public DateTime? StartDate { get; set; }

			// Token: 0x170000C7 RID: 199
			// (get) Token: 0x06000241 RID: 577 RVA: 0x0000ADBB File Offset: 0x00008FBB
			// (set) Token: 0x06000242 RID: 578 RVA: 0x0000ADC3 File Offset: 0x00008FC3
			public DateTime? EndDate { get; set; }
		}
	}
}
