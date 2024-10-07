// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using BaseApp.Core.Utils;
using BaseApp.Face.Services;
using BaseApp.Views;
using Microsoft.Extensions.Hosting;
using System.Windows;


namespace BaseApp.Services
{
    /// <summary>
    /// Managed host of the application.
    /// </summary>
    public class ApplicationHostService(IServiceProvider serviceProvider) : IHostedService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        private MainWindow? _mainWindow;

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await HandleActivationAsync();
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            CameraService.Dispose();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Creates main window during activation.
        /// </summary>
        private async Task HandleActivationAsync()
        {
            _ = Task.Run(() => CameraService.ConnectCamera());
            _ = Task.Run(() => FaceRecognitionService.LoadData());

            ServiceProviderUtil.SetServiceProvider(_serviceProvider);

            if (!Application.Current.Windows.OfType<MainWindow>().Any())
            {
                _mainWindow = (
                    _serviceProvider.GetService(typeof(MainWindow)) as MainWindow
                )!;
                _mainWindow!.ShowWindow();
            }

            await Task.CompletedTask;
        }
    }
}
