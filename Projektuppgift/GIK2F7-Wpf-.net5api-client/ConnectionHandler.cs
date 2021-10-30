using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Diagnostics;

namespace Wpf
{
    /// <summary>
    /// Handles connections to the API
    /// </summary>
    class ConnectionHandler
    {

        public static string EndPoint;
        public static HttpClient ApiClient { get; set; }

        /// <summary>
        /// Constructor of the Connection handler
        /// </summary>
        public ConnectionHandler()
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            EndPoint = ConfigManager.GetAppConf("ProductsRestApiSsl");
        }

        /// <summary>
        /// Load games from the api
        /// </summary>
        /// <returns>Object list of games</returns>
        public async Task<ObservableCollection<Game>> LoadGames()
        {
            ObservableCollection<Game> game = new();

            try
            {
                HttpResponseMessage response = await ApiClient.GetAsync(EndPoint);
                string jsonData = await response.Content.ReadAsStringAsync();

                List<Game> deData = DataHandler.DeserializeMessage(jsonData);

                foreach (var x in deData)
                {
                    game.Add(new Game { id = x.id, name = x.name, description = x.description, rating = x.rating, image = x.image});
                };

                return game;
            }
            catch(HttpRequestException rqst)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("log.log", "listener"));
                MessageBox.Show($"{rqst.Message}");
                Trace.TraceInformation(rqst.Message);
                Trace.Flush();
                return game;
            }
        }
        /// <summary>
        /// Adds game to the database via the API
        /// </summary>
        /// <param name="pData">Game object data</param>
        /// <returns>bool</returns>
        public async Task<bool> AddGame(string pData)
        {
            try
            {
                StringContent sendData = new StringContent(pData, Encoding.UTF8, "application/json");
                HttpResponseMessage respons = await ApiClient.PostAsync(EndPoint, sendData);
                return respons.IsSuccessStatusCode;

            }
            catch (HttpRequestException rqst)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("log.log", "listener"));
                MessageBox.Show($"{rqst.Message}");
                Trace.TraceInformation(rqst.Message);
                Trace.Flush();
                return false;
            }
        }
        /// <summary>
        /// Updates game object data inside the database via the API
        /// </summary>
        /// <param name="pId">Id the database entry</param>
        /// <param name="pData">Game object data</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateGame(int pId, string pData)
        {
            try
            {
                StringContent sendData = new StringContent(pData, Encoding.UTF8, "application/json");
                HttpResponseMessage respons = await ApiClient.PutAsync(EndPoint + pId, sendData);
                return respons.IsSuccessStatusCode;
            }
            catch (HttpRequestException rqst)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("log.log", "listener"));
                MessageBox.Show($"{rqst.Message}");
                Trace.TraceInformation(rqst.Message);
                Trace.Flush();
                return false;
            }
        }

        /// <summary>
        /// Deletes entry in the database via the API
        /// </summary>
        /// <param name="pId">Id the database entry</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteGame(int pId)
        {
            try
            {
                HttpResponseMessage respons = await ApiClient.DeleteAsync(EndPoint + pId);
                return respons.IsSuccessStatusCode;
            }
            catch (HttpRequestException rqst)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("log.log", "listener"));
                MessageBox.Show($"{rqst.Message}");
                Trace.TraceInformation(rqst.Message);
                Trace.Flush();
                return false;
            }
        }
    }
}
