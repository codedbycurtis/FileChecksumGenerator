using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ChecksumGenerator.ViewModels.Framework;

namespace ChecksumGenerator.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Members

        private string _filePath;
        private string _computedHash;
        private string _status;
        private int _selectedAlgorithmIndex;
        private SolidColorBrush _statusColor;

        #endregion

        #region Properties

        public int SelectedAlgorithmIndex
        {
            get => _selectedAlgorithmIndex;
            set => SetProperty(ref _selectedAlgorithmIndex, value);
        }

        public string FilePath
        {
            get => _filePath;
            set => SetProperty(ref _filePath, value);
        }

        public string ComputedHash
        {
            get => _computedHash;
            set => SetProperty(ref _computedHash, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public SolidColorBrush StatusColor
        {
            get => _statusColor;
            set => SetProperty(ref _statusColor, value);
        }

        #endregion

        #region Commands

        public ICommand OpenFileDialogCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand GenerateCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of <see cref="MainViewModel"/>.
        /// </summary>
        public MainViewModel()
        {
            StatusColor = new SolidColorBrush(Colors.Black);
            Status = "Inactive";

            OpenFileDialogCommand = new RelayCommand(() =>
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog()
                {
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Title = "Open",
                    ValidateNames = true,
                };

                if (openFileDialog.ShowDialog() is true)
                {
                    FilePath = openFileDialog.FileName;
                }
            });

            ExitCommand = new RelayCommand(() =>
            {
                Application.Current.Shutdown();
            });

            GenerateCommand = new RelayCommand(() =>
            {
                Status = "In progress...";

                try
                {
                    // Creates a BufferedStream on top of the underlying FileStream with
                    // a buffer size of ~1mb
                    using var fileStream = File.OpenRead(FilePath);
                    using var bufferedStream = new BufferedStream(fileStream, 1048576);

                    switch (SelectedAlgorithmIndex)
                    {
                        // MD5
                        case 0:
                            {
                                using var md5Instance = MD5.Create();
                                var md5Hash = md5Instance.ComputeHash(bufferedStream);
                                ComputedHash = BitConverter.ToString(md5Hash).Replace("-", "").ToLower();
                                break;
                            }                           

                        // SHA1
                        case 1:
                            {
                                using var sha1Instance = SHA1.Create();
                                var sha1Hash = sha1Instance.ComputeHash(bufferedStream);
                                ComputedHash = BitConverter.ToString(sha1Hash).Replace("-", "").ToLower();
                                break;
                            }

                        // SHA256
                        case 2:
                            {
                                using var sha256Instance = SHA256.Create();
                                var sha256Hash = sha256Instance.ComputeHash(bufferedStream);
                                ComputedHash = BitConverter.ToString(sha256Hash).Replace("-", "").ToLower();
                                break;
                            }

                        // SHA384
                        case 3:
                            {
                                using var sha384Instance = SHA384.Create();
                                var sha384Hash = sha384Instance.ComputeHash(bufferedStream);
                                ComputedHash = BitConverter.ToString(sha384Hash).Replace("-", "").ToLower();
                                break;
                            }

                        // SHA512
                        case 4:
                            {
                                using var sha512Instance = SHA512.Create();
                                var sha512Hash = sha512Instance.ComputeHash(bufferedStream);
                                ComputedHash = BitConverter.ToString(sha512Hash).Replace("-", "").ToLower();
                                break;
                            }
                    }

                    StatusColor = new SolidColorBrush(Colors.LimeGreen);
                    Status = "Success!";
                }

                catch (Exception e)
                {
                    StatusColor = new SolidColorBrush(Colors.Red);
                    Status = "Failed!";
                    MessageBox.Show(e.Message, "An error occurred");
                }
            });
        }

        #endregion
    }
}
