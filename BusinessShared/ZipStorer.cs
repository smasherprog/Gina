using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace BusinessShared
{
	// Token: 0x02000005 RID: 5
	public class ZipStorer : IDisposable
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002434 File Offset: 0x00000634
		static ZipStorer()
		{
			for (int i = 0; i < ZipStorer.CrcTable.Length; i++)
			{
				uint num = (uint)i;
				for (int j = 0; j < 8; j++)
				{
					if ((num & 1U) != 0U)
					{
						num = 3988292384U ^ (num >> 1);
					}
					else
					{
						num >>= 1;
					}
				}
				ZipStorer.CrcTable[i] = num;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000024A4 File Offset: 0x000006A4
		public static ZipStorer Create(string _filename, string _comment)
		{
			Stream stream = new FileStream(_filename, FileMode.Create, FileAccess.ReadWrite);
			ZipStorer zipStorer = ZipStorer.Create(stream, _comment);
			zipStorer.Comment = _comment;
			zipStorer.FileName = _filename;
			return zipStorer;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000024D4 File Offset: 0x000006D4
		public static ZipStorer Create(Stream _stream, string _comment)
		{
			return new ZipStorer
			{
				Comment = _comment,
				ZipFileStream = _stream,
				Access = FileAccess.Write
			};
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002500 File Offset: 0x00000700
		public static ZipStorer Open(string _filename, FileAccess _access)
		{
			Stream stream = new FileStream(_filename, FileMode.Open, (_access == FileAccess.Read) ? FileAccess.Read : FileAccess.ReadWrite);
			ZipStorer zipStorer = ZipStorer.Open(stream, _access);
			zipStorer.FileName = _filename;
			return zipStorer;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002530 File Offset: 0x00000730
		public static ZipStorer Open(Stream _stream, FileAccess _access)
		{
			if (!_stream.CanSeek && _access != FileAccess.Read)
			{
				throw new InvalidOperationException("Stream cannot seek");
			}
			ZipStorer zipStorer = new ZipStorer();
			zipStorer.ZipFileStream = _stream;
			zipStorer.Access = _access;
			if (zipStorer.ReadFileInfo())
			{
				return zipStorer;
			}
			throw new InvalidDataException();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002578 File Offset: 0x00000778
		public void AddFile(ZipStorer.Compression _method, string _pathname, string _filenameInZip, string _comment)
		{
			if (this.Access == FileAccess.Read)
			{
				throw new InvalidOperationException("Writing is not alowed");
			}
			FileStream fileStream = new FileStream(_pathname, FileMode.Open, FileAccess.Read);
			this.AddStream(_method, _filenameInZip, fileStream, File.GetLastWriteTime(_pathname), _comment);
			fileStream.Close();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000025BC File Offset: 0x000007BC
		public void AddStream(ZipStorer.Compression _method, string _filenameInZip, Stream _source, DateTime _modTime, string _comment)
		{
			if (this.Access == FileAccess.Read)
			{
				throw new InvalidOperationException("Writing is not alowed");
			}
			if (this.Files.Count != 0)
			{
				ZipStorer.ZipFileEntry zipFileEntry = this.Files[this.Files.Count - 1];
			}
			ZipStorer.ZipFileEntry zipFileEntry2 = default(ZipStorer.ZipFileEntry);
			zipFileEntry2.Method = _method;
			zipFileEntry2.EncodeUTF8 = this.EncodeUTF8;
			zipFileEntry2.FilenameInZip = this.NormalizedFilename(_filenameInZip);
			zipFileEntry2.Comment = ((_comment == null) ? "" : _comment);
			zipFileEntry2.Crc32 = 0U;
			zipFileEntry2.HeaderOffset = (uint)this.ZipFileStream.Position;
			zipFileEntry2.ModifyTime = _modTime;
			this.WriteLocalHeader(ref zipFileEntry2);
			zipFileEntry2.FileOffset = (uint)this.ZipFileStream.Position;
			this.Store(ref zipFileEntry2, _source);
			_source.Close();
			this.UpdateCrcAndSizes(ref zipFileEntry2);
			this.Files.Add(zipFileEntry2);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000026A4 File Offset: 0x000008A4
		public void Close()
		{
			if (this.Access != FileAccess.Read)
			{
				uint num = (uint)this.ZipFileStream.Position;
				uint num2 = 0U;
				if (this.CentralDirImage != null)
				{
					this.ZipFileStream.Write(this.CentralDirImage, 0, this.CentralDirImage.Length);
				}
				for (int i = 0; i < this.Files.Count; i++)
				{
					long position = this.ZipFileStream.Position;
					this.WriteCentralDirRecord(this.Files[i]);
					num2 += (uint)(this.ZipFileStream.Position - position);
				}
				if (this.CentralDirImage != null)
				{
					this.WriteEndRecord(num2 + (uint)this.CentralDirImage.Length, num);
				}
				else
				{
					this.WriteEndRecord(num2, num);
				}
			}
			if (this.ZipFileStream != null)
			{
				this.ZipFileStream.Flush();
				this.ZipFileStream.Dispose();
				this.ZipFileStream = null;
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000277C File Offset: 0x0000097C
		public List<ZipStorer.ZipFileEntry> ReadCentralDir()
		{
			if (this.CentralDirImage == null)
			{
				throw new InvalidOperationException("Central directory currently does not exist");
			}
			List<ZipStorer.ZipFileEntry> list = new List<ZipStorer.ZipFileEntry>();
			ushort num7;
			ushort num8;
			ushort num9;
			for (int i = 0; i < this.CentralDirImage.Length; i += (int)(46 + num7 + num8 + num9))
			{
				uint num = BitConverter.ToUInt32(this.CentralDirImage, i);
				if (num != 33639248U)
				{
					break;
				}
				bool flag = (BitConverter.ToUInt16(this.CentralDirImage, i + 8) & 2048) != 0;
				ushort num2 = BitConverter.ToUInt16(this.CentralDirImage, i + 10);
				uint num3 = BitConverter.ToUInt32(this.CentralDirImage, i + 12);
				uint num4 = BitConverter.ToUInt32(this.CentralDirImage, i + 16);
				uint num5 = BitConverter.ToUInt32(this.CentralDirImage, i + 20);
				uint num6 = BitConverter.ToUInt32(this.CentralDirImage, i + 24);
				num7 = BitConverter.ToUInt16(this.CentralDirImage, i + 28);
				num8 = BitConverter.ToUInt16(this.CentralDirImage, i + 30);
				num9 = BitConverter.ToUInt16(this.CentralDirImage, i + 32);
				uint num10 = BitConverter.ToUInt32(this.CentralDirImage, i + 42);
				uint num11 = (uint)(46 + num7 + num8 + num9);
				Encoding encoding = (flag ? Encoding.UTF8 : ZipStorer.DefaultEncoding);
				ZipStorer.ZipFileEntry zipFileEntry = default(ZipStorer.ZipFileEntry);
				zipFileEntry.Method = (ZipStorer.Compression)num2;
				zipFileEntry.FilenameInZip = encoding.GetString(this.CentralDirImage, i + 46, (int)num7);
				zipFileEntry.FileOffset = this.GetFileOffset(num10);
				zipFileEntry.FileSize = num6;
				zipFileEntry.CompressedSize = num5;
				zipFileEntry.HeaderOffset = num10;
				zipFileEntry.HeaderSize = num11;
				zipFileEntry.Crc32 = num4;
				zipFileEntry.ModifyTime = this.DosTimeToDateTime(num3);
				if (num9 > 0)
				{
					zipFileEntry.Comment = encoding.GetString(this.CentralDirImage, i + 46 + (int)num7 + (int)num8, (int)num9);
				}
				list.Add(zipFileEntry);
			}
			return list;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002954 File Offset: 0x00000B54
		public bool ExtractFile(ZipStorer.ZipFileEntry _zfe, string _filename)
		{
			string directoryName = Path.GetDirectoryName(_filename);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			if (Directory.Exists(_filename))
			{
				return true;
			}
			Stream stream = new FileStream(_filename, FileMode.Create, FileAccess.Write);
			bool flag = this.ExtractFile(_zfe, stream);
			if (flag)
			{
				stream.Close();
			}
			File.SetCreationTime(_filename, _zfe.ModifyTime);
			File.SetLastWriteTime(_filename, _zfe.ModifyTime);
			return flag;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000029B8 File Offset: 0x00000BB8
		public bool ExtractFile(ZipStorer.ZipFileEntry _zfe, Stream _stream)
		{
			if (!_stream.CanWrite)
			{
				throw new InvalidOperationException("Stream cannot be written");
			}
			byte[] array = new byte[4];
			this.ZipFileStream.Seek((long)((ulong)_zfe.HeaderOffset), SeekOrigin.Begin);
			this.ZipFileStream.Read(array, 0, 4);
			if (BitConverter.ToUInt32(array, 0) != 67324752U)
			{
				return false;
			}
			Stream stream;
			if (_zfe.Method == ZipStorer.Compression.Store)
			{
				stream = this.ZipFileStream;
			}
			else
			{
				if (_zfe.Method != ZipStorer.Compression.Deflate)
				{
					return false;
				}
				stream = new DeflateStream(this.ZipFileStream, CompressionMode.Decompress, true);
			}
			byte[] array2 = new byte[16384];
			this.ZipFileStream.Seek((long)((ulong)_zfe.FileOffset), SeekOrigin.Begin);
			int num2;
			for (uint num = _zfe.FileSize; num > 0U; num -= (uint)num2)
			{
				num2 = stream.Read(array2, 0, (int)Math.Min((long)((ulong)num), (long)array2.Length));
				_stream.Write(array2, 0, num2);
			}
			_stream.Flush();
			if (_zfe.Method == ZipStorer.Compression.Deflate)
			{
				stream.Dispose();
			}
			return true;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002AAC File Offset: 0x00000CAC
		public static bool RemoveEntries(ref ZipStorer _zip, List<ZipStorer.ZipFileEntry> _zfes)
		{
			if (!(_zip.ZipFileStream is FileStream))
			{
				throw new InvalidOperationException("RemoveEntries is allowed just over streams of type FileStream");
			}
			List<ZipStorer.ZipFileEntry> list = _zip.ReadCentralDir();
			string tempFileName = Path.GetTempFileName();
			string tempFileName2 = Path.GetTempFileName();
			try
			{
				ZipStorer zipStorer = ZipStorer.Create(tempFileName, string.Empty);
				foreach (ZipStorer.ZipFileEntry zipFileEntry in list)
				{
					if (!_zfes.Contains(zipFileEntry) && _zip.ExtractFile(zipFileEntry, tempFileName2))
					{
						zipStorer.AddFile(zipFileEntry.Method, tempFileName2, zipFileEntry.FilenameInZip, zipFileEntry.Comment);
					}
				}
				_zip.Close();
				zipStorer.Close();
				File.Delete(_zip.FileName);
				File.Move(tempFileName, _zip.FileName);
				_zip = ZipStorer.Open(_zip.FileName, _zip.Access);
			}
			catch
			{
				return false;
			}
			finally
			{
				if (File.Exists(tempFileName))
				{
					File.Delete(tempFileName);
				}
				if (File.Exists(tempFileName2))
				{
					File.Delete(tempFileName2);
				}
			}
			return true;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002BE0 File Offset: 0x00000DE0
		private uint GetFileOffset(uint _headerOffset)
		{
			byte[] array = new byte[2];
			this.ZipFileStream.Seek((long)((ulong)(_headerOffset + 26U)), SeekOrigin.Begin);
			this.ZipFileStream.Read(array, 0, 2);
			ushort num = BitConverter.ToUInt16(array, 0);
			this.ZipFileStream.Read(array, 0, 2);
			ushort num2 = BitConverter.ToUInt16(array, 0);
			return (uint)((long)(30 + num + num2) + (long)((ulong)_headerOffset));
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002C48 File Offset: 0x00000E48
		private void WriteLocalHeader(ref ZipStorer.ZipFileEntry _zfe)
		{
			long position = this.ZipFileStream.Position;
			Encoding encoding = (_zfe.EncodeUTF8 ? Encoding.UTF8 : ZipStorer.DefaultEncoding);
			byte[] bytes = encoding.GetBytes(_zfe.FilenameInZip);
			this.ZipFileStream.Write(new byte[] { 80, 75, 3, 4, 20, 0 }, 0, 6);
			this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.EncodeUTF8 ? 2048 : 0), 0, 2);
			this.ZipFileStream.Write(BitConverter.GetBytes((ushort)_zfe.Method), 0, 2);
			this.ZipFileStream.Write(BitConverter.GetBytes(this.DateTimeToDosTime(_zfe.ModifyTime)), 0, 4);
			Stream zipFileStream = this.ZipFileStream;
			byte[] array = new byte[12];
			zipFileStream.Write(array, 0, 12);
			this.ZipFileStream.Write(BitConverter.GetBytes((ushort)bytes.Length), 0, 2);
			this.ZipFileStream.Write(BitConverter.GetBytes(0), 0, 2);
			this.ZipFileStream.Write(bytes, 0, bytes.Length);
			_zfe.HeaderSize = (uint)(this.ZipFileStream.Position - position);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002D68 File Offset: 0x00000F68
		private void WriteCentralDirRecord(ZipStorer.ZipFileEntry _zfe)
		{
			Encoding encoding = (_zfe.EncodeUTF8 ? Encoding.UTF8 : ZipStorer.DefaultEncoding);
			byte[] bytes = encoding.GetBytes(_zfe.FilenameInZip);
			byte[] bytes2 = encoding.GetBytes(_zfe.Comment);
			this.ZipFileStream.Write(new byte[] { 80, 75, 1, 2, 23, 11, 20, 0 }, 0, 8);
			this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.EncodeUTF8 ? 2048 : 0), 0, 2);
			this.ZipFileStream.Write(BitConverter.GetBytes((ushort)_zfe.Method), 0, 2);
			this.ZipFileStream.Write(BitConverter.GetBytes(this.DateTimeToDosTime(_zfe.ModifyTime)), 0, 4);
			this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.Crc32), 0, 4);
			this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.CompressedSize), 0, 4);
			this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.FileSize), 0, 4);
			this.ZipFileStream.Write(BitConverter.GetBytes((ushort)bytes.Length), 0, 2);
			this.ZipFileStream.Write(BitConverter.GetBytes(0), 0, 2);
			this.ZipFileStream.Write(BitConverter.GetBytes((ushort)bytes2.Length), 0, 2);
			this.ZipFileStream.Write(BitConverter.GetBytes(0), 0, 2);
			this.ZipFileStream.Write(BitConverter.GetBytes(0), 0, 2);
			this.ZipFileStream.Write(BitConverter.GetBytes(0), 0, 2);
			this.ZipFileStream.Write(BitConverter.GetBytes(33024), 0, 2);
			this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.HeaderOffset), 0, 4);
			this.ZipFileStream.Write(bytes, 0, bytes.Length);
			this.ZipFileStream.Write(bytes2, 0, bytes2.Length);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002F40 File Offset: 0x00001140
		private void WriteEndRecord(uint _size, uint _offset)
		{
			Encoding encoding = (this.EncodeUTF8 ? Encoding.UTF8 : ZipStorer.DefaultEncoding);
			byte[] bytes = encoding.GetBytes(this.Comment);
			this.ZipFileStream.Write(new byte[] { 80, 75, 5, 6, 0, 0, 0, 0 }, 0, 8);
			this.ZipFileStream.Write(BitConverter.GetBytes((int)((ushort)this.Files.Count + this.ExistingFiles)), 0, 2);
			this.ZipFileStream.Write(BitConverter.GetBytes((int)((ushort)this.Files.Count + this.ExistingFiles)), 0, 2);
			this.ZipFileStream.Write(BitConverter.GetBytes(_size), 0, 4);
			this.ZipFileStream.Write(BitConverter.GetBytes(_offset), 0, 4);
			this.ZipFileStream.Write(BitConverter.GetBytes((ushort)bytes.Length), 0, 2);
			this.ZipFileStream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00003024 File Offset: 0x00001224
		private void Store(ref ZipStorer.ZipFileEntry _zfe, Stream _source)
		{
			byte[] array = new byte[16384];
			uint num = 0U;
			long position = this.ZipFileStream.Position;
			long position2 = _source.Position;
			Stream stream;
			if (_zfe.Method == ZipStorer.Compression.Store)
			{
				stream = this.ZipFileStream;
			}
			else
			{
				stream = new DeflateStream(this.ZipFileStream, CompressionMode.Compress, true);
			}
			_zfe.Crc32 = uint.MaxValue;
			int num2;
			do
			{
				num2 = _source.Read(array, 0, array.Length);
				num += (uint)num2;
				if (num2 > 0)
				{
					stream.Write(array, 0, num2);
					uint num3 = 0U;
					while ((ulong)num3 < (ulong)((long)num2))
					{
						_zfe.Crc32 = ZipStorer.CrcTable[(int)((UIntPtr)((_zfe.Crc32 ^ (uint)array[(int)((UIntPtr)num3)]) & 255U))] ^ (_zfe.Crc32 >> 8);
						num3 += 1U;
					}
				}
			}
			while (num2 == array.Length);
			stream.Flush();
			if (_zfe.Method == ZipStorer.Compression.Deflate)
			{
				stream.Dispose();
			}
			_zfe.Crc32 ^= uint.MaxValue;
			_zfe.FileSize = num;
			_zfe.CompressedSize = (uint)(this.ZipFileStream.Position - position);
			if (_zfe.Method == ZipStorer.Compression.Deflate && !this.ForceDeflating && _source.CanSeek && _zfe.CompressedSize > _zfe.FileSize)
			{
				_zfe.Method = ZipStorer.Compression.Store;
				this.ZipFileStream.Position = position;
				this.ZipFileStream.SetLength(position);
				_source.Position = position2;
				this.Store(ref _zfe, _source);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003170 File Offset: 0x00001370
		private uint DateTimeToDosTime(DateTime _dt)
		{
			return (uint)((_dt.Second / 2) | (_dt.Minute << 5) | (_dt.Hour << 11) | (_dt.Day << 16) | (_dt.Month << 21) | (_dt.Year - 1980 << 25));
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000031C2 File Offset: 0x000013C2
		private DateTime DosTimeToDateTime(uint _dt)
		{
			return new DateTime((int)((_dt >> 25) + 1980U), (int)((_dt >> 21) & 15U), (int)((_dt >> 16) & 31U), (int)((_dt >> 11) & 31U), (int)((_dt >> 5) & 63U), (int)((_dt & 31U) * 2U));
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000031F4 File Offset: 0x000013F4
		private void UpdateCrcAndSizes(ref ZipStorer.ZipFileEntry _zfe)
		{
			long position = this.ZipFileStream.Position;
			this.ZipFileStream.Position = (long)((ulong)(_zfe.HeaderOffset + 8U));
			this.ZipFileStream.Write(BitConverter.GetBytes((ushort)_zfe.Method), 0, 2);
			this.ZipFileStream.Position = (long)((ulong)(_zfe.HeaderOffset + 14U));
			this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.Crc32), 0, 4);
			this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.CompressedSize), 0, 4);
			this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.FileSize), 0, 4);
			this.ZipFileStream.Position = position;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000032A4 File Offset: 0x000014A4
		private string NormalizedFilename(string _filename)
		{
			string text = _filename.Replace('\\', '/');
			int num = text.IndexOf(':');
			if (num >= 0)
			{
				text = text.Remove(0, num + 1);
			}
			return text.Trim(new char[] { '/' });
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000032E8 File Offset: 0x000014E8
		private bool ReadFileInfo()
		{
			if (this.ZipFileStream.Length < 22L)
			{
				return false;
			}
			try
			{
				this.ZipFileStream.Seek(-17L, SeekOrigin.End);
				BinaryReader binaryReader = new BinaryReader(this.ZipFileStream);
				for (;;)
				{
					this.ZipFileStream.Seek(-5L, SeekOrigin.Current);
					uint num = binaryReader.ReadUInt32();
					if (num == 101010256U)
					{
						break;
					}
					if (this.ZipFileStream.Position <= 0L)
					{
						goto Block_5;
					}
				}
				this.ZipFileStream.Seek(6L, SeekOrigin.Current);
				ushort num2 = binaryReader.ReadUInt16();
				int num3 = binaryReader.ReadInt32();
				uint num4 = binaryReader.ReadUInt32();
				ushort num5 = binaryReader.ReadUInt16();
				if (this.ZipFileStream.Position + (long)((ulong)num5) != this.ZipFileStream.Length)
				{
					return false;
				}
				this.ExistingFiles = num2;
				this.CentralDirImage = new byte[num3];
				this.ZipFileStream.Seek((long)((ulong)num4), SeekOrigin.Begin);
				this.ZipFileStream.Read(this.CentralDirImage, 0, num3);
				this.ZipFileStream.Seek((long)((ulong)num4), SeekOrigin.Begin);
				return true;
				Block_5:;
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000340C File Offset: 0x0000160C
		public void Dispose()
		{
			this.Close();
		}

		// Token: 0x04000001 RID: 1
		public bool EncodeUTF8;

		// Token: 0x04000002 RID: 2
		public bool ForceDeflating;

		// Token: 0x04000003 RID: 3
		private List<ZipStorer.ZipFileEntry> Files = new List<ZipStorer.ZipFileEntry>();

		// Token: 0x04000004 RID: 4
		private string FileName;

		// Token: 0x04000005 RID: 5
		private Stream ZipFileStream;

		// Token: 0x04000006 RID: 6
		private string Comment = "";

		// Token: 0x04000007 RID: 7
		private byte[] CentralDirImage;

		// Token: 0x04000008 RID: 8
		private ushort ExistingFiles;

		// Token: 0x04000009 RID: 9
		private FileAccess Access;

		// Token: 0x0400000A RID: 10
		private static uint[] CrcTable = new uint[256];

		// Token: 0x0400000B RID: 11
		private static Encoding DefaultEncoding = Encoding.GetEncoding(437);

		// Token: 0x02000006 RID: 6
		public enum Compression : ushort
		{
			// Token: 0x0400000D RID: 13
			Store,
			// Token: 0x0400000E RID: 14
			Deflate = 8
		}

		// Token: 0x02000007 RID: 7
		public struct ZipFileEntry
		{
			// Token: 0x06000022 RID: 34 RVA: 0x00003432 File Offset: 0x00001632
			public override string ToString()
			{
				return this.FilenameInZip;
			}

			// Token: 0x0400000F RID: 15
			public ZipStorer.Compression Method;

			// Token: 0x04000010 RID: 16
			public string FilenameInZip;

			// Token: 0x04000011 RID: 17
			public uint FileSize;

			// Token: 0x04000012 RID: 18
			public uint CompressedSize;

			// Token: 0x04000013 RID: 19
			public uint HeaderOffset;

			// Token: 0x04000014 RID: 20
			public uint FileOffset;

			// Token: 0x04000015 RID: 21
			public uint HeaderSize;

			// Token: 0x04000016 RID: 22
			public uint Crc32;

			// Token: 0x04000017 RID: 23
			public DateTime ModifyTime;

			// Token: 0x04000018 RID: 24
			public string Comment;

			// Token: 0x04000019 RID: 25
			public bool EncodeUTF8;
		}
	}
}
