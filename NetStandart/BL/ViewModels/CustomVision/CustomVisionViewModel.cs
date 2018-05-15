using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Collections.Generic;

namespace UnblockHackNET.BL.ViewModels.CustomVision
{
    public class CustomVisionViewModel : BaseViewModel
    {
        public CustomVisionViewModel()
        {
            MaxPropPrediction = new Prediction()
            {
                Probability = 0.001
            };
            GoToCartoPage = new Command(Navigate);
        }

        #region Fields
        private ImageSource _source;
        public ImageSource ImageSource
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                OnPropertyChanged();
            }
        }

        public string ResultText
        {
            get => Get("");
            set => Set(value);
        }
        public bool ResultIsVisible
        {
            get => Get(false);
            set => Set(value);
        }
        public bool IndicatorIsRunning
        {
            get => Get(false);
            set => Set(value);
        }
        public int ResultFontSize
        {
            get => Get(16);
            set => Set(value);
        }
        #endregion

        public ICommand GetImageAndRunCommand => MakeCommand(GetImageAndRun);


        public ICommand SetImageAndRunCommand => MakeCommand(SetImageAndRun);

        private async void SetImageAndRun()
        {
            try
            {
                ResultIsVisible = false;
                IndicatorIsRunning = true;
                ResultFontSize = 16;
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (cameraStatus != PermissionStatus.Granted)
                {
                    var semaphore = new SemaphoreSlim(1, 1);
                    semaphore.Wait();
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    cameraStatus = results[Permission.Camera];
                    semaphore.Release();
                }
                if (cameraStatus == PermissionStatus.Granted)
                {
                    await CrossMedia.Current.Initialize();
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        ResultText = ":( No camera available.";
                    }
                    else
                    {
                        var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                        {
                            Directory = "SampleDirectory",
                            Name = "test.jpg"
                        });
                        if (file == null)
                            return;

                        ImageSource = ImageSource.FromFile(file.Path);
                        var temp = await MakePredictionRequest(file);

                        SetPropsAfterPredictin(temp);

                        file.Dispose();
                    }
                }
                ResultIsVisible = true;
                IndicatorIsRunning = false;
            }
            catch (Exception ex)
            {
                ResultIsVisible = true;
                IndicatorIsRunning = false;
                ResultText = ex.ToString();
            }
        }

        private async void GetImageAndRun()
        {
            try
            {
                ResultIsVisible = false;
                IndicatorIsRunning = true;
                ResultFontSize = 16;
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (storageStatus != PermissionStatus.Granted)
                {
                    var semaphore = new SemaphoreSlim(1, 1);
                    semaphore.Wait();
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    storageStatus = results[Permission.Storage];
                    semaphore.Release();
                }
                if (storageStatus == PermissionStatus.Granted)
                {
                    await CrossMedia.Current.Initialize();

                    var file = await CrossMedia.Current.PickPhotoAsync();
                    if (file == null)
                        return;

                    ImageSource = ImageSource.FromFile(file.Path);
                    var temp = await MakePredictionRequest(file);

                    file.Dispose();
                    SetPropsAfterPredictin(temp);
                }
                ResultIsVisible = true;
                IndicatorIsRunning = false;
            }
            catch (Exception ex)
            {
                ResultIsVisible = true;
                IndicatorIsRunning = false;
                ResultText = ex.ToString();
            }
        }

        private Prediction MaxPropPrediction { get; set; }

        private void SetPropsAfterPredictin(Model temp)
        {
            if (temp != null)
            {
                MaxPropPrediction = new Prediction()
                {
                    Probability = 0.001
                };

                foreach (var item in temp?.Predictions)
                {
                    if (item.Probability > MaxPropPrediction.Probability)
                    {
                        MaxPropPrediction = item;
                    }
                }

                if (string.IsNullOrEmpty(MaxPropPrediction.TagId))
                {
                    ResultText = "Non Response";
                }
                else
                {
                    IsPredictSet = true;
                    ResultText = $"Результат: {MaxPropPrediction.TagName} {Math.Round(MaxPropPrediction.Probability * 100)}%";
                }
            }
        }

        public bool IsPredictSet
        {
            get => Get(false);
            set => Set(value);
        }

        public ICommand GoToCartoPage { get; set; }

        private void Navigate()
        {
            var tmp = new Dictionary<string, object>();
            tmp.Add("AnimalLabel", MaxPropPrediction.TagName);

            NavigateTo(Pages.Carto,
                       NavigationMode.Normal,
                       withAnimation: true,
                       newNavigationStack: false,
                       navParams: tmp);

        }
        private static byte[] GetImageAsByteArray(MediaFile file)
        {
            byte[] result;
            using (Stream fileStream = file.GetStream())
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                result = binaryReader.ReadBytes((int) fileStream.Length);
            }
            return result;
        }
        static async Task<Model> MakePredictionRequest(MediaFile imageFilePath)
        {
            var client = new HttpClient();
            string result = "";
            client.DefaultRequestHeaders.Add("Prediction-Key", "Custom Vision KEY");
            string url = "Custom Vision URL";

            HttpResponseMessage response;
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);
                result = await response.Content.ReadAsStringAsync();
            }
            Model myObj = JsonConvert.DeserializeObject<Model>(result);
            return myObj;
        }


        public string PetuchText
        {
            get => Get("");
            set => Set(value);
        }

        public ICommand OnApperCommand => MakeCommand(OnApper);

        private void OnApper()
        {
            object text = "";
            base.NavigationParams.TryGetValue("Text", out text);
            PetuchText = text as string;
        }
    }

    public class Model
    {
        public string Id { get; set; }
        public string Project { get; set; }
        public string Iteration { get; set; }
        public string Created { get; set; }
        public List<Prediction> Predictions { get; set; }
    }

    public class Prediction
    {
        public string TagId { get; set; }
        public string TagName { get; set; }
        public double Probability { get; set; }
    }
}
