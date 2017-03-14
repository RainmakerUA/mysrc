using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using RM.Win.BossKey.Win32;
using WpfWindow = System.Windows.Window;

namespace RM.Win.BossKey.ViewModel
{
	public class SelectViewModel : ViewModelBase
	{
		private readonly DelegateCommand _okCommand;
		private readonly DelegateCommand _cancelCommand;
		private readonly DelegateCommand _clearFilterCommand;

		private Window[] _windows;
		private Window[] _windowView;
		private Window _selectedWindow;
		private bool _isLoading;
		private bool _visibleOnly;
		private string _filterText;

		public SelectViewModel()
		{
			_okCommand = new DelegateCommand(OnOkExecute, OnOkCanExecute);
			_cancelCommand = new DelegateCommand(OnCancelExecute);
			_clearFilterCommand = new DelegateCommand(OnClearFilterExecute);
		}

		public Window[] Windows
		{
			get { return _windows; }
			set
			{
				_windows = value;
				OnPropertyChanged();

				UpdateWindowView();
				OnPropertyChanged(nameof(WindowView));
			}
		}

		public Window[] WindowView => _windowView;

		public Window SelectedWindow
		{
			get { return _selectedWindow; }
			set
			{
				_selectedWindow = value;
				OnPropertyChanged();
				_okCommand?.OnCanExecuteChanged(this);
			}
		}

		public bool IsLoading
		{
			get { return _isLoading; }
			set
			{
				_isLoading = value;
				OnPropertyChanged();
			}
		}

		public bool VisibleOnly
		{
			get { return _visibleOnly; }
			set
			{
				_visibleOnly = value;
				OnPropertyChanged();

				UpdateWindowView();
				OnPropertyChanged(nameof(WindowView));
			}
		}

		public string FilterText
		{
			get { return _filterText; }
			set
			{
				_filterText = value;
				OnPropertyChanged();

				UpdateWindowView();
				OnPropertyChanged(nameof(WindowView));
			}
		}

		public ICommand OkCommand => _okCommand;

		public ICommand CancelCommand => _cancelCommand;

		public ICommand ClearFilterCommand => _clearFilterCommand;

		public async void LoadWindows()
		{
			IsLoading = true;
			Windows = await Task.Run(new Func<Window[]>(GetWindows)).ConfigureAwait(false);

			IsLoading = false;
		}

		private void UpdateWindowView()
		{
			_windowView = GetWindowsView(_windows, this);
		}

		private bool FilterWindow(Window win)
		{
			var visibleOnly = VisibleOnly;
			var filterText = FilterText;

			return (!visibleOnly || win.IsVisible)
					&& (String.IsNullOrWhiteSpace(filterText) || win.Title.IndexOf(filterText, StringComparison.InvariantCultureIgnoreCase) >= 0
																|| win.ExeName.IndexOf(filterText, StringComparison.InvariantCultureIgnoreCase) >= 0);
		}

		private void OnClearFilterExecute(object parameter)
		{
			_visibleOnly = false;
			_filterText = String.Empty;
			OnPropertyChanged(nameof(VisibleOnly));
			OnPropertyChanged(nameof(FilterText));
			UpdateWindowView();
			OnPropertyChanged(nameof(WindowView));
		}

		private void OnOkExecute(object parameter)
		{
			if (parameter is WpfWindow w)
			{
				w.DialogResult = true;
			}
		}

		private bool OnOkCanExecute(object parameter)
		{
			return SelectedWindow != null;
		}

		private void OnCancelExecute(object parameter)
		{
			if (parameter is WpfWindow w)
			{
				w.DialogResult = false;
			}
		}

		private static Window[] GetWindows()
		{
			var currentProcessID = Process.GetCurrentProcess().Id;
			return NativeWindow.FindAll(nw => nw.ProcessID != currentProcessID)
								.OrderBy(nw => nw.ExeName).ThenBy(nw => nw.Title)
								.Select(ToVmWindow).ToArray();
		}

		private static Window ToVmWindow(NativeWindow win)
		{
			return Window.Create(win.Handle.ToInt64(), win.Title, win.Class, win.ExeName, win.Icon, win.IsVisible);
		}

		private static Window[] GetWindowsView(Window[] windows, SelectViewModel viewModel)
		{
			return Array.FindAll(windows, viewModel.FilterWindow);
		}
	}
}
