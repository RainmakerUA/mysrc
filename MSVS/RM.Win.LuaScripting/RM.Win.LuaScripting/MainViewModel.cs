using Mvvm;
using Mvvm.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace RM.Win.LuaScripting
{
	public sealed class MainViewModel : BindableBase
	{
		private string _proxyUrl;
		private string _proxyRegex;
		private string _proxyParseLua;
		private string _proxyList;
		private string _errorMessage;

		public MainViewModel()
		{
			GetProxiesCommand = new DelegateCommand(GetProxies);
			CloseCommand = new DelegateCommand<ICloseable>(Close);
			CopyResultCommand = new DelegateCommand(CopyResults);
			CloseErrorCommand = new DelegateCommand(CloseError);
		}

		public string ProxyUrl
		{
			get => _proxyUrl;
			set => SetProperty(ref _proxyUrl, value);
		}

		public string ProxyRegex
		{
			get => _proxyRegex;
			set => SetProperty(ref _proxyRegex, value);
		}

		public string ProxyParseLua
		{
			get => _proxyParseLua;
			set => SetProperty(ref _proxyParseLua, value);
		}

		public string ProxyList
		{
			get => _proxyList;
			set => SetProperty(ref _proxyList, value);
		}

		public string ErrorMessage
		{
			get => _errorMessage;
			set
			{
				SetProperty(ref _errorMessage, value);
				OnPropertyChanged(() => HasError);
			}
		}

		public bool HasError => !String.IsNullOrEmpty(_errorMessage);

		public ICommand GetProxiesCommand { get; }

		public ICommand CloseCommand { get; }

		public ICommand CopyResultCommand { get; }

		public ICommand CloseErrorCommand { get; }

		private void GetProxies()
		{
			try
			{
				ProxyList = String.Join(Environment.NewLine, ProxyParser.GetProxies(ProxyUrl, ProxyRegex, ProxyParseLua));
			}
			catch (Exception e)
			{
				ErrorMessage = e.Message;
			}
		}

		private void Close(ICloseable closeable)
		{
			closeable?.Close();
		}

		private void CopyResults()
		{
			var text = ProxyList;

			if (!String.IsNullOrEmpty(text))
			{
				Clipboard.SetText(text, TextDataFormat.Text);
			}
		}

		private void CloseError()
		{
			ErrorMessage = null;
		}
	}
}
