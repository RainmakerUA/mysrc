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
				var handler = PropertyChanged;
				if (handler != null)
				{
					handler(this, new PropertyChangedEventArgs(propertyName));
				}
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
				throw new ArgumentNullException("propertyExpression");
			}

			var body = propertyExpression.Body as MemberExpression;

			if (body != null)
			{
				var property = body.Member as PropertyInfo;

				if (property != null)
				{
					OnPropertyChanged(property.Name);
				}
			}

			throw new ArgumentException("Argument is not a property expression!", "propertyExpression");
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
