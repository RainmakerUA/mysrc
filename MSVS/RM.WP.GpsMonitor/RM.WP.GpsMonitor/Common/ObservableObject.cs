using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RM.WP.GpsMonitor.Common
{
	public abstract class ObservableObject : INotifyPropertyChanged
	{
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			try
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e);
				throw;
			}
		}

		protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			if (propertyExpression == null)
			{
				throw new ArgumentNullException(nameof(propertyExpression));
			}

			var body = propertyExpression.Body as MemberExpression;

			var property = body?.Member as PropertyInfo;

			if (property != null)
			{
				// ReSharper disable once ExplicitCallerInfoArgument
				OnPropertyChanged(property.Name);
			}

			throw new ArgumentException("Argument is not a property expression!", nameof(propertyExpression));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
