using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace RM.Net.Receiver
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const int _pbCount = 256;
		private readonly UdpReceiver _receiver;
		private readonly List<ProgressBar> _progressBars;
		private readonly Dictionary<byte, ulong> _counts;
		private double _pbScale = 1.0;

		public MainWindow()
		{
			_progressBars = new List<ProgressBar>();
			_counts = new Dictionary<byte, ulong>();

			InitializeComponent();

			InitializeGrid();
			InitializeProgressBars();

			_receiver = new UdpReceiver(9999);
			_receiver.Received += OnReceived;
		}

		protected override void OnContentRendered(EventArgs e)
		{
			base.OnContentRendered(e);
			//_receiver.Start();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (_receiver.Active)
			{
				_receiver.Stop();
			}

			base.OnClosing(e);
		}

		private void OnReceiverToggle(object sender, RoutedEventArgs e)
		{
			var isActive = _receiver.Active;

			if (isActive)
			{
				_receiver.Stop();
			}
			else
			{
				_receiver.Start();
			}

			buttonStart.IsEnabled = isActive;
			buttonStop.IsEnabled = !isActive;
		}

		private void OnReceived(object sender, UdpReceiver.ReceivedEventArgs e)
		{
			if (e.Data != null)
			{
				foreach (var b in e.Data)
				{
					SafeIncrement(_counts, b);
				}

				Dispatcher.Invoke(UpdateProgressBars);
			}
			else
			{
				// TODO: Error handling
			}
		}

		private void UpdateProgressBars()
		{
			var pbCount = _progressBars.Count;
			var step = (Byte.MaxValue + 1) / pbCount;
			var upScale = false;

			for (int i = 0; i < pbCount; i++)
			{
				var pb = _progressBars[i];
				var countForPb = 0UL;

				for (int j = 0; j < step; j++)
				{
					countForPb += SafeGetValue(_counts, (byte)(i * step + j), 0UL);
				}

				var newValue = countForPb / _pbScale;
				pb.IsIndeterminate = newValue < Double.Epsilon;
				pb.Value = newValue;

				if (newValue > pb.Maximum)
				{
					upScale = true;
				}
			}

			if (upScale)
			{
				_pbScale *= 1.2;
				UpdateProgressBars();
			}
		}

		private static string FormatBytes(byte[] bytes)
		{
			return String.Join("\u0020", Array.ConvertAll(bytes, b => b.ToString("X2")));
		}

		private static TV SafeGetValue<TK, TV>(IDictionary<TK, TV> dict, TK key, TV defValue)
		{
			return dict.ContainsKey(key) ? dict[key] : defValue;
		}

		private static void SafeIncrement<TK>(IDictionary<TK, ulong> dict, TK key)
		{
			ulong oldCount;
			dict[key] = dict.TryGetValue(key, out oldCount) ? oldCount + 1 : 1;
		}

		private void InitializeGrid()
		{
			for (int i = 0; i < _pbCount; i++)
			{
				gridRoot.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			}
		}

		private void InitializeProgressBars()
		{
			for (int i = 0; i < _pbCount; i++)
			{
				var bar = CreateProgressBar(i);
				_progressBars.Insert(i, bar);
				gridRoot.Children.Add(bar);
			}
		}

		private static ProgressBar CreateProgressBar(int index)
		{
			var step = (Byte.MaxValue + 1) / _pbCount;
			var bar = new ProgressBar
							{
								IsIndeterminate = true,
								//Margin = new Thickness(0, 10, 0, 0),
								Height = 2,
								ToolTip = $"Bytes {step * index} .. {step * (index + 1) - 1}",
							};
			bar.SetValue(Grid.RowProperty, index + 1);
			return bar;
		}
	}
}
