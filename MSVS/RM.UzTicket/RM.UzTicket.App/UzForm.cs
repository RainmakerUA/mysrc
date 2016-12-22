using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RM.UzTicket.App.Controls;
using RM.UzTicket.Lib;

namespace RM.UzTicket.App
{
	public partial class UzForm : Form
	{
		private readonly UzTicketClient _client;

		public UzForm()
		{
			InitializeComponent();

			_client = new UzTicketClient();
		}

		private void completableTextBoxFromStation_Complete(object sender, Controls.CompleteAsyncEventArgs e)
		{
			e.SuggestionsAsync = _client.GetStationsAsync(e.Request).ContinueWith<IReadOnlyList<string>>(st => Array.ConvertAll(st.Result, s => s.Title));
		}

		private async void buttonTrain_Click(object sender, EventArgs e)
		{
			try
			{
				var source = await _client.GetFirstStationAsync(completableTextBoxFromStation.Value);
				var dest = await _client.GetFirstStationAsync(completableTextBoxDestination.Value);

				if (source != null && dest != null)
				{
					var trains = await _client.GetTrainsAsync(dateTimePickerDate.Value.Date, source, dest);
					var trainNum = ItemSelectForm.SelectItem(Array.ConvertAll(trains, tr => $"{tr.DepartureTime.DateTime:HH:mm} {tr.Number} {tr.SourceStation.Title} - {tr.DestinationStation.Title}"), "Select train:");

					if (trainNum != null)
					{
						trainNum = trainNum.Split('\u0020')[1].Trim();

						var train = trains.FirstOrDefault(tr => tr.Number == trainNum);
						textBoxTrain.Text = train?.GetInfo() ?? String.Empty;
					}
				}
			}
			catch (Exception exc)
			{
				MessageBox.Show(exc.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
