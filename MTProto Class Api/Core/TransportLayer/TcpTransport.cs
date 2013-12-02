using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;
using MTProto.Core.ApiLayer.Attributes;

namespace MTProto.Core
{
	internal class TcpTransport : ISerializable
	{
		public ExpectedAnswer Answer { get; set; }

		/// <summary>
		/// 4 байта длины
		/// </summary>
		/// <remarks>
		/// включая длину, порядковый номер и CRC32; всегда делится на четыре
		/// </remarks>
		
		private Int32 PacketLength { get; set; }

		/// <summary>
		/// 4 байта с порядковым номером пакета внутри данного tcp-соединения 
		/// </summary>
		/// <remarks>
		/// (первый отправленный пакет помечается 0, следующий - 1 и т.д.)
		/// </remarks>
		public Int32 PacketNumber { get; set; }

		/// <summary>
		/// Полезная нагрузка
		/// </summary>
		public byte[] Payload { get; set; }

		/// <summary>
		/// 4 байта CRC32 
		/// </summary>
		/// <remarks>
		/// длины, порядкового номера и полезной нагрузки вместе
		/// </remarks>
		private UInt32 CRC32 { get; set; }
        
        /// <summary>
        /// Идентификатор сообщения внутри транспорта
        /// </summary>
        public long MessageId { get; set; }

		public TcpTransport(Int32 packetNumber, byte[] payload)
		{
			this.PacketLength = payload.Length + 12;
			this.PacketNumber = packetNumber;
			this.Payload = payload;
			this.Answer = null;
		}

		public TcpTransport(Int32 packetNumber, byte[] payload, long msgId, string expectedAnswer)
		{
			this.PacketLength = payload.Length + 12;
			this.PacketNumber = packetNumber;
			this.Payload = payload;
			this.Answer = new ExpectedAnswer(msgId, expectedAnswer);
		}
		
		public TcpTransport(Int32 packetNumber, byte[] payload, ExpectedAnswer exAn)
		{
			this.PacketLength = payload.Length + 12;
			this.PacketNumber = packetNumber;
			this.Payload = payload;
			this.Answer = exAn;
		}

		public TcpTransport(byte[] byteArray)
		{
			using (BinaryReader br = new BinaryReader(new MemoryStream(byteArray)))
			{
				this.PacketLength = br.ReadInt32();
				this.PacketNumber = br.ReadInt32();
				this.Payload = br.ReadBytes(this.PacketLength - 12);
				this.CRC32 = br.ReadUInt32();
			}
		}

		public TcpTransport(MemoryStream ms)
		{
			BinaryReader br = new BinaryReader(ms);
			this.PacketLength = br.ReadInt32();
			this.PacketNumber = br.ReadInt32();
			this.Payload = br.ReadBytes(this.PacketLength - 12);
			this.CRC32 = br.ReadUInt32();            
		}

		public byte[] Serialize()
		{
			using(MemoryStream ms = new MemoryStream())
			{
#if MEASURESPEED
				Stopwatch sw = new Stopwatch();
				sw.Start();
#endif
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					Crc32 crc = new Crc32();
					this.PacketLength = this.Length;
					bw.Write(this.PacketLength);
					bw.Write(this.PacketNumber);
					bw.Write(this.Payload, 0, this.Payload.Length);
					bw.Write(crc.ComputeHash(ms.ToArray(), 0, 4 + 4 + this.Payload.Length).Reverse().ToArray());   
				}
#if MEASURESPEED
				sw.Stop();
				Debug.WriteLine("Tcp transport serialize elapsed = {0} ms", sw.Elapsed.TotalMilliseconds);
#endif
				return ms.ToArray();
			}
		   

			/*byte[] result = new byte[4 + 4 + this.Payload.Length + 4];
			this.PacketLength = this.Length;
			Buffer.BlockCopy(BitConverter.GetBytes(this.PacketLength), 0, result, 0, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(this.PacketNumber), 0, result, 4, 4);
			Buffer.BlockCopy(this.Payload, 0, result, 8, this.Payload.Length);

			Crc32 crc = new Crc32();
			Buffer.BlockCopy(crc.ComputeHash(result, 0, 4 + 4 + this.Payload.Length).Reverse().ToArray(), 0, result, result.Length - 4, 4);
			*/			
		}

		public int Length
		{
			get { return this.Payload.Length + 12; }
		}


        public System.Threading.Tasks.Task<byte[]> SerializeAsync()
        {
            return System.Threading.Tasks.Task.FromResult(Serialize());
        }
    }


}
