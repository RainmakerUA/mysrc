using System;
using System.Net.Sockets;
using System.Threading;

namespace RM.Net.Receiver
{
	internal sealed class UdpReceiver
	{
		public class ReceivedEventArgs : EventArgs
		{
			public ReceivedEventArgs(byte[] data)
			{
				Data = data;
			}

			public ReceivedEventArgs(string error)
			{
				Error = error;
			}

			public byte[] Data { get; }

			public string Error { get; }
		}

		private const int _defaultPort = 8888;
		private readonly UdpClient _client;
		private bool _active;
		private CancellationTokenSource _cancelSource;

		public UdpReceiver() : this(0)
		{
		}

		public UdpReceiver(int port)
		{
			_client = new UdpClient(port == 0 ? _defaultPort : port);
		}

		public bool Active => _active;

		public event EventHandler<ReceivedEventArgs> Received;

		public void Start()
		{
			try
			{
				_cancelSource = new CancellationTokenSource();
				_active = true;
				ReceiveLoop();
			}
			catch (Exception e)
			{
				RaiseReceived(new ReceivedEventArgs(e.Message));
			}
		}

		public void Stop()
		{
			_active = false;
			_cancelSource.Cancel();
		}

		private void RaiseReceived(ReceivedEventArgs e)
		{
			Received?.Invoke(this, e);
		}

		private async void ReceiveLoop()
		{
			var cancelToken = _cancelSource.Token;

			while (_active && !cancelToken.IsCancellationRequested)
			{
				try
				{
					var result = await _client.ReceiveAsync();
					RaiseReceived(new ReceivedEventArgs(result.Buffer));
				}
				catch (Exception e)
				{
					RaiseReceived(new ReceivedEventArgs(e.Message));
				}

			}
		}
	}
}
