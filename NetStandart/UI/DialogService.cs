﻿using System;
using Acr.UserDialogs;
using UnblockHackNET;
using UnblockHackNET.Helpers;
using Xamarin.Forms;

namespace BlockChainNET.UI
{
    public class DialogService
    {
        static readonly Lazy<DialogService> LazyInstance = new Lazy<DialogService>(() => new DialogService(), true);
        Application _app;

        DialogService()
        {
            MessagingCenter.Subscribe<MessageBus, DialogAlertInfo>(this, Consts.DialogAlertMessage, DialogAlertCallback);
            MessagingCenter.Subscribe<MessageBus, DialogSheetInfo>(this, Consts.DialogSheetMessage, DialogSheetCallback);
            MessagingCenter.Subscribe<MessageBus, DialogQuestionInfo>(this, Consts.DialogQuestionMessage, DialogQuestionCallback);
            MessagingCenter.Subscribe<MessageBus, DialogEntryInfo>(this, Consts.DialogEntryMessage, DialogEntryCallback);

            MessagingCenter.Subscribe<MessageBus, string>(this, Consts.DialogShowLoadingMessage, DialogShowLoadingCallback);

            MessagingCenter.Subscribe<MessageBus>(this, Consts.DialogHideLoadingMessage, DialogHideLoadingCallback);

            MessagingCenter.Subscribe<MessageBus, DialogToastInfo>(this, Consts.DialogToastMessage, DialogToastCallback);
        }

        public static void Init(Application app)
        {
            LazyInstance.Value.SetApp(app);
        }

        void SetApp(Application app)
        {
            _app = app;
        }

        void DialogEntryCallback(MessageBus bus, DialogEntryInfo entryInfo)
        {
            if (entryInfo == null) throw new ArgumentNullException(nameof(entryInfo));
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await UserDialogs.Instance.PromptAsync(new PromptConfig
                {
                    Message = entryInfo.Message,
                    Title = entryInfo.Title,
                    OkText = entryInfo.Ok ?? PromptConfig.DefaultOkText,
                    CancelText = entryInfo.Cancel ?? PromptConfig.DefaultCancelText,
                    Placeholder = entryInfo.Placeholder,
                    Text = entryInfo.Text,
                    InputType = InputType.Name
                });
                if (result.Ok) entryInfo.OnCompleted?.Invoke(result.Text ?? string.Empty);
                else entryInfo.OnCancelled?.Invoke();
            });
        }

        void DialogToastCallback(MessageBus bus, DialogToastInfo toastInfo)
        {
            if (toastInfo == null) throw new ArgumentNullException(nameof(toastInfo));
            Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.Toast(new ToastConfig(toastInfo.Text)
            {
                Duration = TimeSpan.FromSeconds(toastInfo.IsLongTime? 2 : 1)
            }));
        }

        void DialogHideLoadingCallback(MessageBus bus)
        {
            Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.HideLoading());
        }

        void DialogShowLoadingCallback(MessageBus bus, string message)
        {
            Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.ShowLoading(message, MaskType.Black));
        }

        void DialogQuestionCallback(MessageBus bus, DialogQuestionInfo questionInfo)
        {
            if (questionInfo == null) throw new ArgumentNullException(nameof(questionInfo));

            if (_app?.MainPage == null)
                throw new FieldAccessException(@"App property not set or App Main Page is not set. Use Init() before using dialogs");

            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await _app.MainPage.DisplayAlert(questionInfo.Title, questionInfo.Question,
                                                              questionInfo.Positive, questionInfo.Negative);
                questionInfo.OnCompleted?.Invoke(result);
            });
        }

        void DialogSheetCallback(MessageBus bus, DialogSheetInfo sheetInfo)
        {
            if (sheetInfo == null) throw new ArgumentNullException(nameof(sheetInfo));
            if (_app?.MainPage == null)
                throw new FieldAccessException(@"App property not set or App Main Page is not set. Use Init() before using dialogs");

            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await _app.MainPage.DisplayActionSheet(sheetInfo.Title, sheetInfo.Cancel,
                                                                    sheetInfo.Destruction, sheetInfo.Items);
                sheetInfo.OnCompleted?.Invoke(result);
            });
        }

        void DialogAlertCallback(MessageBus bus, DialogAlertInfo alertInfo)
        {
            if (alertInfo == null) throw new ArgumentNullException(nameof(alertInfo));
            if (_app?.MainPage == null)
                throw new FieldAccessException(@"App property not set or App Main Page is not set. Use Init() before using dialogs");

            Device.BeginInvokeOnMainThread(async () =>
            {
                await _app.MainPage.DisplayAlert(alertInfo.Title, alertInfo.Message, alertInfo.Cancel);
                alertInfo.OnCompleted?.Invoke();
            });
        }
    }

    public class DialogAlertInfo
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Cancel { get; set; }
        public Action OnCompleted { get; set; }
    }

    public class DialogEntryInfo
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Placeholder { get; set; }
        public string Cancel { get; set; }
        public string Ok { get; set; }
        public string Text { get; set; }
        public Action<string> OnCompleted { get; set; }
        public Action OnCancelled { get; set; }
    }

    public class DialogSheetInfo
    {
        public string Title { get; set; }
        public string Cancel { get; set; }
        public string Destruction { get; set; }
        public string[] Items { get; set; }
        public Action<string> OnCompleted { get; set; }
    }

    public class DialogQuestionInfo
    {
        public string Title { get; set; }
        public string Question { get; set; }
        public string Positive { get; set; }
        public string Negative { get; set; }
        public Action<bool> OnCompleted { get; set; }
    }

    public class DialogToastInfo
    {
        public string Text { get; set; }
        public bool IsLongTime { get; set; }
        public bool IsCenter { get; set; }
    }
}
