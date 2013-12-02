using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using MTProto.Core.ApiLayer;
using MTProto.Core.Helpers;
using System.Collections.Generic;

namespace RDavey.Net.No
{
	public class DataDownloadedEventArgs : EventArgs
	{
		public byte[] Data { get; set; }
		public TimeSpan ElapsedTime { get; set; }

		public DataDownloadedEventArgs (byte[] data)
		{
			this.Data = data;
		}
	}

	public class AsyncTcpClient : IDisposable
	{
		private IPAddress[] addresses;
		private int port;
		private WaitHandle addressesSet;
		private TcpClient tcpClient;
		private int failedConnectionCount;

		public string ClientName { get; set; }

		private ManualResetEvent _connectedEvent = new ManualResetEvent(false);

		/// <summary>
		/// Construct a new client from a known IP Address
		/// </summary>
		/// <param name="address">The IP Address of the server</param>
		/// <param name="port">The port of the server</param>
		public AsyncTcpClient(IPAddress address, int port)
			: this(new[] { address }, port)
		{
		}

		/// <summary>
		/// Construct a new client where multiple IP Addresses for
		/// the same client are known.
		/// </summary>
		/// <param name="addresses">The array of known IP Addresses</param>
		/// <param name="port">The port of the server</param>
		public AsyncTcpClient(IPAddress[] addresses, int port)
			: this(port)
		{
			this.addresses = addresses;
		}

		/// <summary>
		/// Construct a new client where the address or host name of
		/// the server is known.
		/// </summary>
		/// <param name="hostNameOrAddress">The host name or address of the server</param>
		/// <param name="port">The port of the server</param>
		public AsyncTcpClient(string hostNameOrAddress, int port)
			: this(port)
		{
			addressesSet = new AutoResetEvent(false);
			addresses = Dns.GetHostAddresses(hostNameOrAddress.Trim());
			//Signal the addresses are now set
			((AutoResetEvent)addressesSet).Set();  
		}

		/// <summary>
		/// Private constuctor called by other constuctors
		/// for common operations.
		/// </summary>
		/// <param name="port"></param>
		private AsyncTcpClient(int port)
		{
			if (port < 0)
				throw new ArgumentException();
			this.port = port;
			this.tcpClient = new TcpClient();
			this.Encoding = Encoding.Default;
		}

		/// <summary>
		/// The endoding used to encode/decode string when sending and receiving.
		/// </summary>
		public Encoding Encoding { get; set; }

		/// <summary>
		/// Attempts to connect to one of the specified IP Addresses
		/// </summary>
		public async Task ConnectAsync()
		{
			if (addressesSet != null)
				//Wait for the addresses value to be set
				addressesSet.WaitOne(); 

			//Set the failed connection count to 0
			//Interlocked.Exchange(ref failedConnectionCount, 0);
		
			try
			{
				//Start the async connect operation
				await tcpClient.ConnectAsync(addresses, port).ConfigureAwait(false);
				tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

				_connectedEvent.Set(); // сигнализация об успешном подключении
			}
			catch
			{
				//Increment the failed connection count in a thread safe way
				//Interlocked.Increment(ref failedConnectionCount);
				if (failedConnectionCount >= addresses.Length)
				{
					//We have failed to connect to all the IP Addresses
					//connection has failed overall.
					return;
				}
			}
		}

		public async Task<byte[]> ExchangeWithServer(byte[] clientRequestBytes) 
		{
			NetworkStream networkStream = tcpClient.GetStream();

			//// Запрос
			//byte[] clientRequestBytes = tt.Serialize();
			Debug.WriteLine("[Client] Writing request {0}\n{1}", clientRequestBytes.Length, BitConverter.ToString(clientRequestBytes));
			await networkStream.WriteAsync(clientRequestBytes, 0, clientRequestBytes.Length).ConfigureAwait(false);

			// Ответ
			var buffer = new byte[4096];
			var byteCount = await networkStream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
			Debug.WriteLine("[Client] Server response was {0}\n{1}", byteCount, BitConverter.ToString(buffer));
			if (byteCount == 0) return null;

			//return new TcpTransport(buffer);
			return buffer;
		}

		int _reconnectTries = 0;

		/// <summary>
		/// Асинхронное чтение из сокета
		/// </summary>
		/// <returns></returns>
		public async Task<byte[]> ReadAsync(CancellationToken cancellationToken)
		{
			_connectedEvent.WaitOne(); // ожидание подключения

			try
			{
				NetworkStream networkStream = tcpClient.GetStream();

				byte[] buffer = new byte[tcpClient.ReceiveBufferSize];

				int readed = await networkStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);

				Debug.WriteLine("[Client] Recieved bytes: " + readed.ToString());
				Debug.WriteLine("[Client] readed: " + BinaryHelper.ByteToHexBitFiddle(buffer, readed));

				// data readed???
				if (readed == 0)
				{
					if (_reconnectTries == 3)
					{
						throw new Exception("TOO MANY CONNECT TRIES!!!");
					}
					_reconnectTries++;

					// конекшн дроппед - реконнект
					tcpClient = new TcpClient();
					await tcpClient.ConnectAsync(addresses, port);
					buffer = await ReadAsync(cancellationToken);
				}

				return buffer;
			}
			catch (System.InvalidOperationException ex)
			{
				Debug.WriteLine("[Client] InvalidOperationException: " + ex.ToString());
				//throw new Exception()
				return new byte[] { };
			}
			catch (SocketException sex)
			{
				Debug.WriteLine("[Client] SocketException: " + sex.ToString());
				throw sex;
			}
			catch (Exception eex)
			{
				throw eex;
			}

		}


		/*
			if(myNetworkStream.CanRead){
				byte[] myReadBuffer = new byte[1024];
				StringBuilder myCompleteMessage = new StringBuilder();
				int numberOfBytesRead = 0;

				// Incoming message may be larger than the buffer size.
				do{
					 numberOfBytesRead = myNetworkStream.Read(myReadBuffer, 0, myReadBuffer.Length);

					 myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));
									
				}
				while(myNetworkStream.DataAvailable);

		 */


		/// <summary>
		/// Cинхронное чтение из сокета
		/// </summary>
		/// <returns></returns>
		public byte[] Read()
		{
			_connectedEvent.WaitOne(); // ожидание подключения
			try
			{
				NetworkStream networkStream = tcpClient.GetStream();

				byte[] buffer = new byte[0];

				if (networkStream.CanRead)
				{
					byte[] myReadBuffer = new byte[1024];
					int batchReadCount = 0;
					// Incoming message may be larger than the buffer size.

					while (networkStream.DataAvailable)
					{
						batchReadCount = networkStream.Read(myReadBuffer, 0, myReadBuffer.Length);
						//System.Buffer.BlockCopy(myReadBuffer, 0, buffer, readed, batchReadCount);

						if (batchReadCount != 0)
						{
							Array.Resize(ref buffer, buffer.Length + batchReadCount);
							System.Buffer.BlockCopy(myReadBuffer, 0, buffer, buffer.Length - batchReadCount, batchReadCount);
						}
					}
				}
				// data readed???
				if (buffer.Length != 0)
				{
					Debug.WriteLine("[Client] Recieved bytes: " + buffer.Length);
					Debug.WriteLine("[Client] readed: " + BinaryHelper.ByteToHexBitFiddle(buffer,  buffer.Length));
				}
				else
				{
					if (!tcpClient.Connected)
					{
						Debug.WriteLine("[Client] disconnected");
					}
				}

				return buffer;
			}
			catch (System.InvalidOperationException ex)
			{
				Debug.WriteLine("[Client] InvalidOperationException: " + ex.ToString());
				//throw new Exception()
				return new byte[] { };
			}
			catch (SocketException sex)
			{
				Debug.WriteLine("[Client] SocketException: " + sex.ToString());
				throw sex;
			}
			catch (Exception eex)
			{
				throw eex;
			}

		}
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		/// <summary>
		/// Writes a string to the network using the defualt encoding.
		/// </summary>
		/// <param name="data">The string to write</param>
		/// <returns>A WaitHandle that can be used to detect
		/// when the write operation has completed.</returns>
		public void Write(string data)
		{
			byte[] bytes = Encoding.GetBytes(data);
			WriteAsync(bytes);
		}

		/// <summary>
		/// Writes an array of bytes to the network.
		/// </summary>
		/// <param name="bytes">The array to write</param>
		/// <returns>A WaitHandle that can be used to detect
		/// when the write operation has completed.</returns>
		public Task WriteAsync(byte[] bytes)
		{
			_connectedEvent.WaitOne();

			try
			{
				NetworkStream networkStream = tcpClient.GetStream();
				Debug.WriteLine("[Client] Writing request {0}\n{1}", bytes.Length, BitConverter.ToString(bytes));
				return networkStream.WriteAsync(bytes, 0, bytes.Length);
			}
			catch (SocketException sex)
			{
				Debug.WriteLine("[Client] SocketException: " + sex.ToString());
				throw sex;
			}
			catch (InvalidOperationException ex)
			{
				Debug.WriteLine("[Client] InvalidOperationException: " + ex.ToString());
				throw ex;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("[Client] Exception: " + ex.ToString());
				throw ex;
			}
		}        

		/// <summary>
		/// Callback for Get Host Addresses operation
		/// </summary>
		/// <param name="result">The AsyncResult object</param>
		private void GetHostAddressesCallback(IAsyncResult result)
		{
			addresses = Dns.EndGetHostAddresses(result);
			//Signal the addresses are now set
			((AutoResetEvent)addressesSet).Set();
		}

		internal void CloseAsync()
		{
			cancellationTokenSource.Cancel();
			tcpClient.Close();
		}

		public void Dispose()
		{
			if (_connectedEvent != null)
				_connectedEvent.Dispose();

			if (addressesSet != null)
				addressesSet.Dispose();

			if (cancellationTokenSource != null)
				cancellationTokenSource.Dispose();

			if (tcpClient.Connected)
				this.tcpClient.Close();
		}
	}
}
