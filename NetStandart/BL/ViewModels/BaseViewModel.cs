﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UnblockHackNET.Helpers;
using UnblockHackNET.UI;
using Plugin.Connectivity;
using Xamarin.Forms;
using BlockChainNET.UI;

namespace UnblockHackNET.BL.ViewModels
{
    public class BaseViewModel : Bindable, IDisposable
    {
        readonly string _classShortName;
        readonly CancellationTokenSource _networkTokenSource = new CancellationTokenSource();
        readonly ConcurrentDictionary<string, ICommand> _cachedCommands = new ConcurrentDictionary<string, ICommand>();

        public NavigationMode NavigationMode { get; set; }

        protected Dictionary<string, object> NavigationParams
        {
            get { return Get<Dictionary<string, object>>(); }
            private set { Set(value); }
        }

        public PageState State
        {
            get => Get(PageState.Clean);
            set => Set(value);
        }

        public bool IsConnected => !CrossConnectivity.IsSupported || CrossConnectivity.IsSupported && CrossConnectivity.Current.IsConnected;
        public CancellationToken CancellationToken => _networkTokenSource?.Token ?? CancellationToken.None;

        public ICommand GoBackCommand => MakeCommand(GoBackCommandExecute);

        public BaseViewModel()
        {
            _classShortName = GetType().Name.Replace(@"ViewModel", "");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BaseViewModel()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            ClearDialogs();
            CancelNetworkRequests();
        }

        public void SetNavigationParams(Dictionary<string, object> navParams)
        {
            NavigationParams = navParams;
        }

        public void CancelNetworkRequests()
        {
            _networkTokenSource.Cancel();
        }

        public virtual Task OnPageAppearing()
        {
            return Task.FromResult(0);
        }

        public virtual Task OnPageDissapearing()
        {
            return Task.FromResult(0);
        }

        protected void NavigateTo(object toName,
            NavigationMode mode = NavigationMode.Normal,
            string toTitle = null,
            Dictionary<string, object> navParams = null,
            bool newNavigationStack = false,
            bool withAnimation = true,
            bool withBackButton = false,
            object fromName = null)
        {
            NavigateTo(toName, fromName ?? _classShortName, mode, toTitle, navParams, newNavigationStack, withAnimation, withBackButton);
        }

        protected static void NavigateTo(object toName,
            object fromName,
            NavigationMode mode = NavigationMode.Normal,
            string toTitle = null,
            Dictionary<string, object> dataToLoad = null,
            bool newNavigationStack = false,
            bool withAnimation = true,
            bool withBackButton = false)
        {

            if (toName == null) return;

            MessageBus.SendMessage(Consts.DialogHideLoadingMessage);

            MessageBus.SendMessage(Consts.NavigationPushMessage,
                new NavigationPushInfo
                {
                    To = toName.ToString(),
                    From = fromName?.ToString(),
                    Mode = mode,
                    NavigationParams = dataToLoad,
                    ToTitle = toTitle,
                    NewNavigationStack = newNavigationStack,
                    WithAnimation = withAnimation,
                    WithBackButton = withBackButton
                });
        }

        protected static ICommand MakeNavigateToCommand(object toName,
            NavigationMode mode = NavigationMode.Normal,
            string toTitle = null,
            bool newNavigationStack = false,
            bool withAnimation = true,
            bool withBackButton = true,
            Dictionary<string, object> navParams = null)
        {
            return new Command(() => NavigateTo(toName, null, mode, toTitle, navParams, newNavigationStack, withAnimation, withBackButton));
        }

        protected ICommand MakeCommand(Action commandAction, [CallerMemberName] string propertyName = null)
        {
            return GetCommand(propertyName) ?? SaveCommand(new Command(commandAction), propertyName);
        }

        protected ICommand MakeCommand(Action<object> commandAction, [CallerMemberName] string propertyName = null)
        {
            return GetCommand(propertyName) ?? SaveCommand(new Command(commandAction), propertyName);
        }

        protected void NavigateBack(NavigationMode mode = NavigationMode.Normal, bool withAnimation = true, bool force = false)
        {
            ClearDialogs();

            MessageBus.SendMessage(Consts.NavigationPopMessage, new NavigationPopInfo
            {
                Mode = mode,
                WithAnimation = withAnimation,
                Force = force
            });
        }

        public void ClearDialogs()
        {
            HideLoading();
        }

        void GoBackCommandExecute()
        {
            NavigateBack(NavigationMode);
        }

        protected void ShowLoading(string message = null, bool useDelay = true)
        {
            MessageBus.SendMessage(Consts.DialogShowLoadingMessage, message);
        }

        protected void HideLoading()
        {
            MessageBus.SendMessage(Consts.DialogHideLoadingMessage);
        }

        protected static Task ShowAlert(string title, string message, string cancel)
        {
            var tcs = new TaskCompletionSource<bool>();
            MessageBus.SendMessage(Consts.DialogAlertMessage,
                new DialogAlertInfo
                {
                    Title = title,
                    Message = message,
                    Cancel = cancel,
                    OnCompleted = () => tcs.SetResult(true)
                });
            return tcs.Task;
        }

        protected static Task<string> ShowSheet(string title, string cancel, string destruction, string[] items)
        {
            var tcs = new TaskCompletionSource<string>();
            MessageBus.SendMessage(Consts.DialogSheetMessage,
                new DialogSheetInfo
                {
                    Title = title,
                    Cancel = cancel,
                    Destruction = destruction,
                    Items = items,
                    OnCompleted = s => tcs.SetResult(s)
                });
            return tcs.Task;
        }

        protected static Task<bool> ShowQuestion(string title, string question, string positive, string negative)
        {
            var tcs = new TaskCompletionSource<bool>();
            MessageBus.SendMessage(Consts.DialogQuestionMessage,
                new DialogQuestionInfo
                {
                    Title = title,
                    Question = question,
                    Positive = positive,
                    Negative = negative,
                    OnCompleted = b => tcs.SetResult(b)
                });
            return tcs.Task;
        }

        protected static Task<string> ShowEntryAlert(string title, string message, string cancel, string ok, string placeholder)
        {
            var tcs = new TaskCompletionSource<string>();
            MessageBus.SendMessage(Consts.DialogEntryMessage,
                new DialogEntryInfo
                {
                    Title = title,
                    Message = message,
                    Cancel = cancel,
                    Ok = ok,
                    Placeholder = placeholder,
                    OnCompleted = s => tcs.SetResult(s),
                    OnCancelled = () => tcs.SetResult(null)
                });
            return tcs.Task;
        }

        protected static void ShowToast(string text, bool isLongTime = false, bool isCenter = false)
        {
            MessageBus.SendMessage(Consts.DialogToastMessage,
                new DialogToastInfo
                {
                    Text = text,
                    IsCenter = isCenter,
                    IsLongTime = isLongTime
                });
        }

        ICommand SaveCommand(ICommand command, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (!_cachedCommands.ContainsKey(propertyName))
                _cachedCommands.TryAdd(propertyName, command);

            return command;
        }

        ICommand GetCommand(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            return _cachedCommands.TryGetValue(propertyName, out var cachedCommand)
                ? cachedCommand
                : null;
        }
    }
}
