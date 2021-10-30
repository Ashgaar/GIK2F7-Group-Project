using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;

namespace Wpf
{   
    /// <summary>
    /// Handles Data
    /// </summary>
    class DataHandler
    {
        private ObservableCollection<Game> games = new();
        ConnectionHandler conHandler = new();

        public string defaultImage = ("iVBORw0KGgoAAAANSUhEUgAAAWoAAAF1CAYAAADBWKCtAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAb6SURBVHhe7dzBqVxHFEXRb6eiRJSL0pJyUSKKxS7MH4ozEQW78Fqj14Mz3XQ3vPvX9+/f//ny5cvH8vXr18+n3/v58+fn0+/Z2y/29sv/ff/r16+Pvz+fAYgSaoC4//76+Pbt2+fH3/PTxH6xt1/s/2z/48cP36gB6oQaIE6oAeKEGiBOqAHihBogTqgB4rxCftjbL/b2y+29V8gBHiDUAHFCDRAn1ABxQg0QJ9QAcUINEOce9WFvv9jbL7f37lEDPECoAeKEGiBOqAHihBogTqgB4oQaIM496sPefrG3X27v3aMGeIBQA8QJNUCcUAPECTVAnFADxAk1QJx71Ie9/WJvv9zeu0cN8AChBogTaoA4oQaIE2qAOKEGiBNqgDj3qA97+8Xefrm9d48a4AFCDRAn1ABxQg0QJ9QAcUINECfUAHHuUR/29ou9/XJ77x41wAOEGiBOqAHihBogTqgB4oQaIE6oAeLcoz7s7Rd7++X23j1qgAcINUCcUAPECTVAnFADxAk1QJxQA8S5R33Y2y/29svtvXvUAA8QaoA4oQaIE2qAOKEGiBNqgDihBohzj/qwt1/s7Zfbe/eoAR4g1ABxQg0QJ9QAcUINECfUAHFCDRDnHvVhb7/Y2y+39+5RAzxAqAHihBogTqgB4oQaIE6oAeKEGiDOPerD3n6xt19u792jBniAUAPECTVAnFADxAk1QJxQA8QJNUCce9SHvf1ib7/c3rtHDfAAoQaIE2qAOKEGiBNqgDihBogTaoA496gPe/vF3n65vXePGuABQg0QJ9QAcUINECfUAHFCDRAn1ABx7lEf9vaLvf1ye+8eNcADhBogTqgB4oQaIE6oAeKEGiBOqAHi3KM+7O0Xe/vl9t49aoAHCDVAnFADxAk1QJxQA8QJNUCcUAPEuUd92Nsv9vbL7b171AAPEGqAOKEGiBNqgDihBogTaoA4oQaIc4/6sLdf7O2X23v3qAEeINQAcUINECfUAHFCDRAn1ABxQg0Q5x71YW+/2Nsvt/fuUQM8QKgB4oQaIE6oAeKEGiBOqAHihBogzj3qw95+sbdfbu/dowZ4gFADxAk1QJxQA8QJNUCcUAPECTVAnHvUh739Ym+/3N67Rw3wAKEGiBNqgDihBogTaoA4oQaIE2qAOPeoD3v7xd5+ub13jxrgAUINECfUAHFCDRAn1ABxQg0QJ9QAce5RH/b2i739cnvvHjXAA4QaIE6oAeKEGiBOqAHihBogTqgB4tyjPuztF3v75fbePWqABwg1QJxQA8QJNUCcUAPECTVAnFADxLlHfdjbL/b2y+29e9QADxBqgDihBogTaoA4oQaIE2qAOKEGiHOP+rC3X+ztl9t796gBHiDUAHFCDRAn1ABxQg0QJ9QAcUINEOce9WFvv9jbL7f37lEDPECoAeKEGiBOqAHihBogTqgB4oQaIM496sPefrG3X27v3aMGeIBQA8QJNUCcUAPECTVAnFADxAk1QJx71Ie9/WJvv9zeu0cN8AChBogTaoA4oQaIE2qAOKEGiBNqgDj3qA97+8Xefrm9d48a4AFCDRAn1ABxQg0QJ9QAcUINECfUAHHuUR/29ou9/XJ77x41wAOEGiBOqAHihBogTqgB4oQaIE6oAeLcoz7s7Rd7++X23j1qgAcINUCcUAPECTVAnFADxAk1QJxQA8S5R33Y2y/29svtvXvUAA8QaoA4oQaIE2qAOKEGiBNqgDihBohzj/qwt1/s7Zfbe/eoAR4g1ABxQg0QJ9QAcUINECfUAHFCDRDnHvVhb7/Y2y+39+5RAzxAqAHihBogTqgB4oQaIE6oAeKEGiDOPerD3n6xt19u792jBniAUAPECTVAnFADxAk1QJxQA8QJNUCce9SHvf1ib7/c3rtHDfAAoQaIE2qAOKEGiBNqgDihBogTaoA496gPe/vF3n65vXePGuABQg0QJ9QAcUINECfUAHFCDRAn1ABx7lEf9vaLvf1ye+8eNcADhBogTqgB4oQaIE6oAeKEGiBOqAHi3KM+7O0Xe/vl9t49aoAHCDVAnFADxAk1QJxQA8QJNUCcUAPEuUd92Nsv9vbL7b171AAPEGqAOKEGiBNqgDihBogTaoA4oQaIc4/6sLdf7O2X23v3qAEeINQAcUINECfUAHFCDRAn1ABxQg0Q5x71YW+/2Nsvt/fuUQM8QKgB4oQaIE6oAeKEGiBOqAHihBogzj3qw95+sbdfbu/dowZ4gFADxAk1QJxQA8QJNUCcUAPECTVAnHvUh739Ym+/3N67Rw3wAKEGiBNqgDihBogTaoA4oQaIE2qAOPeoD3v7xd5+ub13jxrgAUINECfUAHFCDRAn1ABxQg0QJ9QAce5RH/b2i739cnvvHjVA3sfHvzewew5LQJ8RAAAAAElFTkSuQmCC");

        /// <summary>
        /// Gets data from the API via Connection handler.
        /// </summary>
        /// <returns>Game object</returns>
        public async Task<ObservableCollection<Game>> GetData()
        {
            games = await conHandler.LoadGames();
            
            return games;
        }

        /// <summary>
        /// Adds game to the database and to the local object.
        /// </summary>
        /// <param name="pTitle">Title of the game</param>
        /// <param name="pDescription">Description of the game</param>
        /// <param name="pRating">Rating of the game</param>
        /// <param name="pImage">Image to the game</param>
        /// <returns>Game object</returns>
        public async Task<ObservableCollection<Game>> AddGameAsync(string pTitle, string pDescription, float pRating, string pImage)
        {
            Game game = new Game()
            {
                name = pTitle,
                description = pDescription,
                rating = pRating,
                image = pImage
            };
            string post = SerializeMessage(game);

            bool response = await conHandler.AddGame(post);

            if (response)
            {
                games.Add(game);
                return games;
            }
            else
            {
                return games;
            }
        }

        /// <summary>
        /// Updates game in the local object and in the database
        /// </summary>
        /// <param name="pListId">Object index id</param>
        /// <param name="pId">Database entry id</param>
        /// <param name="pTitle">Title of the game</param>
        /// <param name="pDescription">Description of the game</param>
        /// <param name="pRating">Rating of the game</param>
        /// <param name="pImage">Image to the game</param>
        /// <returns>Game object</returns>
        public async Task<ObservableCollection<Game>> UpdateGameAsync(int pListId, int pId, string pTitle, string pDescription, float pRating, string pImage)
        {
            Game game = new Game()
            {
                id = pId,
                name = pTitle,
                description = pDescription,
                rating = pRating,
                image = pImage
            };
            string post = SerializeMessage(game);
            bool response = await conHandler.UpdateGame(pId, post);
            if(response)
            {
                games[pListId] = game;

                return games;
            }
            else
            {
                return games;
            }
            

        }

        /// <summary>
        /// Deletes game from local object and database
        /// </summary>
        /// <param name="pListId">Object index id</param>
        /// <param name="pId">Database entry id</param>
        /// <returns>Game object</returns>
        public async Task<ObservableCollection<Game>> DeleteGameAsync(int pListId, int pId)
        {
            bool result = await conHandler.DeleteGame(pId);
            if (result)
            {
                games.RemoveAt(pListId);
                return games;
            }
            else
            {
                return games;
            }
        }

        /// https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to
        /// <summary>
        /// To object(s) using System.Text.Json
        /// </summary>
        /// <param name="pJson"></param>
        /// <returns></returns>
        public static List<Game> DeserializeMessage(string pJson)
        {
            var obj = System.Text.Json.JsonSerializer.Deserialize<List<Game>>(pJson);

            // if we have problems with Json parsing to our model, with PropertyNameCaseInsensitive we can force the behaviour
            // https://stackoverflow.com/questions/58879190/system-text-json-doesnt-deserialize-what-newtonsoft-does
            /*
            var obj = System.Text.Json.JsonSerializer.Deserialize<List<Game>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // or false
            });
            */
            return obj;
        }


        /// <summary>
        /// To Json using System.Text.Json
        /// </summary>
        /// <param name="pObj"></param>
        /// <returns></returns>
        public static string SerializeMessage(Game pObj)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(pObj);
            return json;
        }

        public static string SerializeMessage(List<Game> pObjList)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(pObjList);
            return json;
        }


        /// <summary>
        /// To object(s) using Newtonsoft.Json />
        /// </summary>
        /// <param name="pJson"></param>
        /// <returns></returns>
        public static List<Game> DeserializeMessageNs(string pJson)
        {
            var obj = JsonConvert.DeserializeObject<List<Game>>(pJson);
            return obj;
        }

        /// <summary>
        /// To Json using Newtonsoft.Json />
        /// </summary>
        /// <param name="pObj"></param>
        /// <returns></returns>
        public static string SerializeMessageNs(Game pObj)
        {
            var json = JsonConvert.SerializeObject(pObj);
            return json;
        }

        /// <summary>
        /// Converts Bitmap to BitmapImage
        /// </summary>
        /// <param name="pBitmap">Bitmap variable</param>
        /// <returns>BitmapImage</returns>
        //https://stackoverflow.com/questions/26260654/wpf-converting-bitmap-to-imagesource
        public BitmapImage BitmapToImageSource(Bitmap pBitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                pBitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        /// <summary>
        /// Converts BitmapImage to base64 string
        /// </summary>
        /// <param name="pBi">BitmapImage</param>
        /// <returns>BitmapImage in string variable</returns>
        public string BitmapToBase64(BitmapImage pBi)
        {
            MemoryStream ms = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(pBi));
            encoder.Save(ms);
            byte[] bitmapdata = ms.ToArray();

            return Convert.ToBase64String(bitmapdata);
        }

        /// <summary>
        /// Converts base64 string to BitmapImage
        /// </summary>
        /// <param name="pBase64String">string to convert to BitmapImage</param>
        /// <returns>BitmapImage</returns>
        //https://stackoverflow.com/questions/23624865/convert-base64-string-to-bitmapimage-c-sharp-windows-phone
        public BitmapImage Base64StringToBitmapImage(string pBase64String)
        {
            Bitmap bmpTemp = null;
            byte[] byteBuffer = Convert.FromBase64String(pBase64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);
            memoryStream.Position = 0;
            bmpTemp = (Bitmap)Bitmap.FromStream(memoryStream);
            memoryStream.Close();
            memoryStream = null;
            byteBuffer = null;
            BitmapImage bmpImage = BitmapToImageSource(bmpTemp);

            return bmpImage;
        }

        /// <summary>
        /// Opens image from file dialog and saves to variable.
        /// </summary>
        /// <param name="pImageBase64">Image in base64 string</param>
        /// <returns>File name</returns>
        public string AddImage(out string pImageBase64)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.bmp;*.png;*.jpg)|*.bmp;*.png;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage image = new BitmapImage(new Uri(openFileDialog.FileName));
                pImageBase64 = BitmapToBase64(image);
                return openFileDialog.SafeFileName;
            }
            else
            {
                //MessageBox.Show("Please choose a file.");
                pImageBase64 = null;
                return null;
            }
        }
    }
}
